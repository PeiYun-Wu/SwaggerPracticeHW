using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonModule
{
    /// <summary>
    /// 回應狀態說明Singleton
    /// </summary>
    public sealed class ResultStatusSingleton
    {
        // 設為 static，載入時，即 new 一個實例
        private static readonly ResultStatusSingleton Instance = new ResultStatusSingleton();
        
        private Dictionary<string, string> msgMap;

        // 設為 private，外界不能 new
        private ResultStatusSingleton()
        {
            msgMap = new Dictionary<string, string>();
        }

        /// <summary>
        /// 使用靜態方法取得實例，因為載入時就 new 一個實例，所以不用考慮多執行緒的問題
        /// </summary>
        /// <returns>Instance</returns>
        public static ResultStatusSingleton GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 設定自訂義文案及取得實例
        /// </summary>
        /// <param name="srv">自訂義文案服務</param>
        /// <returns>Instance</returns>
        public static ResultStatusSingleton GetInstance(IResultStatus srv)
        {
            Instance.msgMap = srv.SetCustomResultMsg();
            return Instance;
        }

        /// <summary>
        /// 取得回應文案
        /// </summary>
        /// <param name="resultCode">狀態碼</param>
        /// <param name="defMsg">預設狀態碼文案</param>
        /// <returns>狀態碼文案</returns>
        public string GetResultMsg(string resultCode, string defMsg)
        {
            if (msgMap.ContainsKey(resultCode))
            {
                return msgMap[resultCode];
            }
            return defMsg ?? string.Empty;
        }
    }
}