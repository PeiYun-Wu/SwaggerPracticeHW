using System;

namespace CommonModule
{
    /// <summary>
    /// ErrMark Model
    /// </summary>
    public class ErrInfoException : Exception
    {
        /// <summary>
        /// nErrNo
        /// </summary>
        public string ErrNo;

        /// <summary>
        /// strErrMsg
        /// </summary>
        public string ErrMsg;

        /// <summary>
        /// LogMsg
        /// </summary>
        public string LogMsg { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrInfoException"/> class.
        /// </summary>
        /// <param name="errNo">錯誤代碼</param>
        /// <param name="errMsg">錯誤內容</param>
        public ErrInfoException(string errNo, string errMsg)
            : base(errNo + ":" + errMsg)
        {
            ErrNo = errNo;
            ErrMsg = errMsg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrInfoException"/> class.
        /// </summary>
        /// <param name="resultStatus">結果狀態</param>
        public ErrInfoException(ResultStatus resultStatus)
            : this(resultStatus.Code, resultStatus.Msg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrInfoException"/> class.
        /// </summary>
        /// <param name="strPrefix">錯誤訊息前置字串</param>
        /// <param name="resultStatus">結果狀態</param>
        public ErrInfoException(string strPrefix, ResultStatus resultStatus)
            : this(resultStatus.Code, strPrefix + resultStatus.Msg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrInfoException"/> class.
        /// </summary>
        /// <param name="resultStatus">結果狀態</param>
        /// <param name="strPostfix">錯誤訊息後置字串</param>
        public ErrInfoException(ResultStatus resultStatus, string strPostfix)
            : this(resultStatus.Code, resultStatus.Msg + strPostfix)
        {
        }
    }

    /// <summary>
    /// ExceptionHelper
    /// </summary>
    public class ExceptionHelper
    {
        /// <summary>
        /// 錯誤訊息處理
        /// </summary>
        /// <param name="ex">異常資訊</param>
        /// <returns>ResponseObj</returns>
        public static ResponseObj ErrInfo(ErrInfoException ex)
        {
            return new ResponseObj(ex.ErrNo, ex.ErrMsg);
        }

        /// <summary>
        /// 錯誤訊息處理
        /// </summary>
        /// <param name="exception">異常資訊</param>
        /// <returns>ResponseObj</returns>
        public static ResponseObj Exception(Exception exception)
        {
            ErrInfoException errInfoException = exception as ErrInfoException;
            if (errInfoException != null)
            {
                if (!string.IsNullOrWhiteSpace(errInfoException.LogMsg))
                {
                    LogHelper.WriteLog(LogLevel.Debug, errInfoException.LogMsg);
                }
                return ErrInfo(errInfoException);
            }
            else
            {
                LogHelper.WriteLog(LogLevel.Warn, string.Empty, exception);
                return new ResponseObj(ResultStatus.UnknownRuntimeError);
            }
        }
    }
}
