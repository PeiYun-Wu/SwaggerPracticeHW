using CommonModule;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PennyTest2.Models
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]

    public class PeResultStatus
    {
        #region 一般狀況

        // 成功狀態碼
        public static ResultStatus SuccessCode
        {
            //交易成功
            get { return new ResultStatus("C0000", "Successful"); }
        }

        // 程式未知錯誤態碼 資料異常，請稍後再試！
        public static ResultStatus UnknownRuntimeError
        {
            get { return new ResultStatus("C9999", "程式資料異常，請稍後再試"); }
        }

        #endregion 一般狀況

        #region 資料庫相關

        public static ResultStatus DbNoData
        {
            get { return new ResultStatus("D0000", "查無資料"); }
        }

        public static ResultStatus DbDataExisted
        {
            get { return new ResultStatus("D0010", "資料已存在，無法存取"); }
        }

        public static ResultStatus DbDataParameter
        {
            get { return new ResultStatus("D0100", "參數有誤"); }
        }

        public static ResultStatus DbDataErrorMsg
        {
            get { return new ResultStatus("D1000", "資料查詢有誤，請稍後再使用"); }
        }

        public static ResultStatus DbErrorMsg
        {
            get { return new ResultStatus("D9999", "資料庫資料異常，請稍後再使用"); }
        }

        #endregion 資料庫相關
    }
}