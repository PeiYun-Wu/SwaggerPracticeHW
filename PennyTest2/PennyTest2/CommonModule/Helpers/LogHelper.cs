using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using NLog;
using NLog.Targets;

namespace CommonModule
{
    /// <summary>
    /// Log等級
    /// </summary>
    public enum LogLevel
    {
        /// <summary>除錯</summary>
        Debug,

        /// <summary>資訊</summary>
        Info,

        /// <summary>警告</summary>
        Warn,

        /// <summary>錯誤</summary>
        Error
    }

    /// <summary>
    /// Log紀錄處理協助
    /// </summary>
    public static class LogHelper
    {
        private static Logger logRec = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 設定NLog 目標
        /// </summary>
        /// <param name="targetName">targetName</param>
        public static void SetLogger(string targetName)
        {
            logRec = LogManager.GetLogger(targetName);
        }

        // TODO ERROR 和 WARN需要另外寫入寄出Email

        /// <summary>
        /// 將訊息寫入Log檔
        /// (如果ex有給Log會寫入ex發生時的行數，否則會以lineNumber進行寫入，
        ///  lineNumber不給值會自動帶入呼叫程式的行數)
        /// </summary>
        /// <param name="level">Log等級</param>
        /// <param name="content">要記錄的內容</param>
        /// <param name="ex">Exception Instance</param>
        /// <param name="lineNumber">錯誤或者是寫入Log的程式所在的行數</param>
        /// <param name="className">類別名稱</param>
        public static void WriteLog(
            LogLevel level,
            string content,
            Exception ex = null,
            [CallerLineNumber] int lineNumber = 0,
            string className = null)
        {
            string line = ex != null ? GetExceptionLineNumber(ex).ToString() : lineNumber.ToString();
            string exMsg = ex != null ? string.Format("{0}ExMsg:{1}{2}", !string.IsNullOrEmpty(content) ? "> " : string.Empty, GetExceptionMessage(ex), GetEntityValidErrorMessage(ex)) : string.Empty;
            string method = GetSourceMethod(ex);
            string classMsg = className == null ? method.Split('.').Last() : className;
            string logMsg = string.Format("{0} | Line:{1} | {2} | {3} {4}", method, line, classMsg, content, exMsg);
            switch (level)
            {
                case LogLevel.Debug:
                    logRec.Debug("{0} | Line:{1} | {2} | {3} {4}", method, line, classMsg, content, exMsg);
                    break;

                case LogLevel.Info:
                    logRec.Info("{0} | Line:{1} | {2} | {3} {4}", method, line, classMsg, content, exMsg);
                    break;

                case LogLevel.Warn:
                    logRec.Warn("{0} | Line:{1} | {2} | {3} {4}", method, line, classMsg, content, exMsg);
                    SendLogMail(LogLevel.Warn, logMsg);
                    break;

                case LogLevel.Error:
                    logRec.Error("{0} | Line:{1} | {2} | {3} {4}", method, line, classMsg, content, exMsg);
                    if (content != "SmtpMailError" && content != "LogMailError")
                        SendLogMail(LogLevel.Error, logMsg);
                    break;
            }
        }

        /// <summary>
        /// 取得Exception發生時的程式所在行數
        /// </summary>
        /// <param name="ex">發生錯誤時的Exception</param>
        /// <returns>發生Exception所在行數(-1表示抓不到所在行數)</returns>
        private static int GetExceptionLineNumber(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var firstFrame = st.GetFrame(0);
            if (firstFrame == null)
            {
                return 0;
            }

            // 先取第一個堆疊物件看是否可以拿到錯誤所在行數
            if (firstFrame.GetFileLineNumber() > 0)
            {
                return firstFrame.GetFileLineNumber();
            }

            // 因為無法透過第一個堆疊物件取得所在行數，所以改從最後一個開始往前取看看是否可以取得，
            // 通常會有這種情況大都是呼叫第三方Dll時又不是透過throw exception的方式回拋時會遇到
            for (int i = st.GetFrames().Length - 1; i > 0; i--)
            {
                if (st.GetFrame(i).GetFileLineNumber() > 0)
                {
                    return st.GetFrame(i).GetFileLineNumber();
                }
            }

            return 0;
        }

        /// <summary>
        /// 取得Log檔完整路徑與檔案名稱
        /// </summary>
        /// <param name="targetName">目標名稱</param>
        /// <returns>Log完整路徑與檔案名稱</returns>
        public static string GetLogFullName(string targetName)
        {
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName(targetName);
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            return fileTarget.FileName.Render(logEventInfo);
        }

        /// <summary>
        /// 取得呼叫本Log Method的Method Name
        /// </summary>
        /// <param name="ex">例外物件，有傳遞會以例外物件進行來嘗試取得Mehtod Name</param>
        /// <returns>回傳Namespace.Name.Method Name(null:表示抓不到所在的Method Name)</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetSourceMethod(Exception ex = null)
        {
            if (ex == null)
            {
                // 需透過當前堆疊物件來取得，GetFrame(1)表示目前的Method Name，GetFrame(2)才是呼叫者的Method Name
                StackFrame frame = new StackTrace().GetFrame(2);
                return string.Format("{0}.{1}", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name);
            }

            var st = new StackTrace(ex, true);
            var firstFrame = st.GetFrame(0);

            if (firstFrame == null)
            {
                return string.Empty;
            }

            // 先嘗試抓取StackFrame(0)，並以是否可以拿到錯誤所在行數作為依據以判斷是否可以取得執行堆疊物件發生例外時的Method資訊
            if (firstFrame.GetFileLineNumber() > 0)
            {
                return string.Format(
                    "{0}.{1}",
                    firstFrame.GetMethod().DeclaringType.FullName,
                    firstFrame.GetMethod().Name);
            }

            // 因為無法透過第一個堆疊物件取得所在行數，所以改從最後一個開始往前取看看是否可以取得，
            // 通常會有這種情況大都是呼叫第三方Dll時又不是透過throw exception的方式回拋時會遇到
            for (int i = st.GetFrames().Length - 1; i > 0; i--)
            {
                if (st.GetFrame(i).GetFileLineNumber() > 0)
                {
                    return string.Format(
                        "{0}.{1}",
                        st.GetFrame(i).GetMethod().DeclaringType.FullName,
                        st.GetFrame(i).GetMethod().Name);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 取得Entity詳細錯誤訊息
        /// </summary>
        /// <param name="ex">Exception </param>
        /// <returns>錯誤訊息內容</returns>
        private static string GetEntityValidErrorMessage(Exception ex)
        {
            StringBuilder detailMessage = new StringBuilder();
            DbEntityValidationException dbex = ex as DbEntityValidationException;
            if (dbex != null)
            {
                foreach (var validationErrors in dbex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        detailMessage.Append(validationError.PropertyName + ":" + validationError.ErrorMessage);
                    }
                }
            }

            return detailMessage.ToString();
        }

        /// <summary>
        /// 取得StackTrace詳細錯誤訊息
        /// </summary>
        /// <param name="ex">Exception </param>
        /// <returns>錯誤訊息內容</returns>
        private static string GetStackTraceErrorMessage(Exception ex)
        {
            Type type = typeof(LogHelper);

            // 取得namespace字眼供塞選StackTrace字串用
            string[] nameSpaceStr = type.Namespace.Split('.');
            StringBuilder traceMsg = new StringBuilder();
            if (ex.StackTrace != null)
            {
                string[] traceList = ex.StackTrace.Replace("\n", string.Empty).Split('\r');
                foreach (var trace in traceList)
                {
                    if (trace.Contains(nameSpaceStr[0]))
                    {
                        traceMsg.AppendLine();
                        traceMsg.Append(trace);
                    }
                }
            }

            return traceMsg.ToString();
        }

        /// <summary>
        /// 取得最底層InnerExcption完整訊息
        /// </summary>
        /// <param name="ex">Exception </param>
        /// <returns>最底層InnerExcption完整訊息</returns>
        public static string GetInnerException(Exception ex)
        {
            string result = string.Empty;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 取得Excption擷取後訊息
        /// </summary>
        /// <param name="ex">Exception </param>
        /// <returns>Excption擷取後訊息</returns>
        public static string GetExceptionMessage(Exception ex)
        {
            string trace = ex.ToString();
            int idx = trace.IndexOf("   於 lambda_method(Closure , Object , Object[] )");
            if (idx > -1)
            {
                trace = trace.Substring(0, idx);
            }

            return trace;
        }

        /// <summary>
        /// 以SMTP寄送Warn、Error的Log信件
        /// </summary>
        /// <param name="level">Log Level</param>
        /// <param name="errorMsg">Error Message </param>
        public static void SendLogMail(LogLevel level, string errorMsg)
        {
            try
            {
                bool isSendLog = CommUtility.GetConfigWhenFailGetBaseConfig("SendLogMailMode", false);

                if (isSendLog)
                {
                    string mailTo = CommUtility.GetConfigWhenFailGetBaseConfig<string>("LogMailRecipient");
                    string[] mailToList = mailTo.Split(',');
                    string mailFrom = CommUtility.GetConfigWhenFailGetBaseConfig<string>("LogMailSender");

                    string methodName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                    string mailTitle = $"{Environment.MachineName}>{methodName}";

                    // 錯誤訊息加上時間、Log種類
                    errorMsg = $" {DateTime.Now.ToString("HH:mm:ss")} | {level.ToString()} | {errorMsg}";
                    SmtpMailHelper.SendSmtpMail(mailFrom, mailToList, mailTitle, errorMsg, null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogLevel.Error, "LogMailError", ex);
            }
        }
    }
}