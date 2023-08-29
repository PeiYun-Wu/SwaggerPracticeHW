#pragma warning disable 1591

using System.Diagnostics.CodeAnalysis;

namespace CommonModule
{
    /// <summary>
    /// 共用Response 結果狀態清單
    /// ## 代碼前綴字母說明
    /// [A] 權限/Token驗證碼相關            [C] 一般狀況              [D] 資料庫相關             [V] 資料驗證相關
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]
    public class ResultStatus
    {
        public readonly string Code;
        public readonly string Msg;

        public ResultStatus(string code, string msg)
        {
            Code = code;
            ResultStatusSingleton singleton = ResultStatusSingleton.GetInstance();
            Msg = singleton.GetResultMsg(code,msg);
        }

        public static ResultStatus ReturnCode
        {
            get { return new ResultStatus("returnCode", "returnMsg"); }
        }
        //Penny
        public static ResultStatus DoubleuseTaskID
        {
            get { return new ResultStatus("C9999", "該任務已有人在執行"); }
        }
        public static ResultStatus NoTaskID
        {
            get { return new ResultStatus("C9999", "此任務不存在"); }
        }
        public static ResultStatus NoAccountId
        {
            get { return new ResultStatus("C9999", "不存在使用者"); }
        }
        public static ResultStatus HadAccountId
        {
            get { return new ResultStatus("C9999", "存在使用者"); }
        }

        #region 一般狀況

        // 成功狀態碼
        public static ResultStatus SuccessCode
        {
            get { return new ResultStatus("C0000", "Success"); }
        }

        // 程式未知錯誤態碼 資料異常，請稍後再試！
        public static ResultStatus UnknownRuntimeError
        {
            get { return new ResultStatus("C9999", "Data error, please try again later"); }
        }

        public static ResultStatus UploadError
        {
            get { return new ResultStatus("C0004", "Data upload fail"); }
        }

        #endregion 一般狀況

        #region 資料庫相關

        public static ResultStatus DbNoData
        {
            get { return new ResultStatus("D0000", "No result"); }
        }

        public static ResultStatus DbDataExisted
        {
            get { return new ResultStatus("D0010", "Duplicated data, please check and retry"); }
        }

        public static ResultStatus DbCatoryStatus
        {
            get { return new ResultStatus("D0011", "請檢查Category是否被停用"); }
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
            get { return new ResultStatus("D9999", "Databese error, please try again later"); }
        }

        #endregion 資料庫相關

        #region 資料驗證相關

        // 欄位驗證錯誤
        public static ResultStatus FieldValidateError
        {
            get { return new ResultStatus("V0010", "欄位驗證有誤"); }
        }

        public static ResultStatus ImportFieldError
        {
            get { return new ResultStatus("V0020", "資料匯入格式有誤"); }
        }

        public static ResultStatus ImportFileError
        {
            get { return new ResultStatus("V0030", "匯入檔案與功能不符"); }
        }

        public static ResultStatus IsCanDeleteValidateError
        {
            get { return new ResultStatus("V0040", "此資料不可刪除"); }
        }

        public static ResultStatus AccountPwdError
        {
            get { return new ResultStatus("V0110", "Account or password error, please contact administractor"); }
        }

        public static ResultStatus AccountCannotUse
        {
            get { return new ResultStatus("V0120", "Account has been suspended, please contact administractor"); }
        }

        public static ResultStatus MemberExistError
        {
            get { return new ResultStatus("V0130", "此用戶已存在"); }
        }

        public static ResultStatus AccountBlack
        {
            get { return new ResultStatus("V0140", "您的帳戶為交易列管帳戶，不可進行交易,請聯絡系統管理員!"); }
        }

        public static ResultStatus PushAuthorityError
        {
            get { return new ResultStatus("V2000", "戶無權限讀取該筆推播明細。"); }
        }

        public static ResultStatus CaptchaError
        {
            get { return new ResultStatus("V0210", "圖形驗證碼有誤"); }
        }

        public static ResultStatus CaptchaTimeoutError
        {
            get { return new ResultStatus("V0220", "圖形驗證碼已逾時"); }
        }

        public static ResultStatus SecurityAnsError
        {
            get { return new ResultStatus("V0230", "安全性問題回答錯誤"); }
        }

        public static ResultStatus AccountError
        {
            get { return new ResultStatus("V0240", "舊站已開戶會員無法進行此步驟"); }
        }

        public static ResultStatus ApproveLevelError
        {
            get { return new ResultStatus("V0250", "審核層級不足"); }
        }

        public static ResultStatus TradeKeyError
        {
            get { return new ResultStatus("V0260", "很抱歉，交易密碼輸入錯誤，請重新輸入。"); }
        }

        public static ResultStatus TradeDuplicate
        {
            get { return new ResultStatus("V0300", "很抱歉，交易序號重複，請稍後或重新輸入交易。"); }
        }

        public static ResultStatus PermitValidateError
        {
            get { return new ResultStatus("V9999", "API parameter error, please contact administractor"); }
        }

        #endregion 資料驗證相關

        #region Token驗證碼相關

        // Token驗證相關
        public static ResultStatus TimeoutError
        {
            get { return new ResultStatus("A9999", "授權已逾時，請重新交易"); }
        }

        public static ResultStatus TokenError
        {
            get { return new ResultStatus("A0010", "Authorization error, please contact administractor"); }
        }

        public static ResultStatus TokenRqError
        {
            get { return new ResultStatus("A0020", "授權傳入異常"); }
        }

        #endregion Token驗證碼相關

        #region 權限相關

        // 權限相關異常
        public static ResultStatus AuthorityNotEnoughError
        {
            get { return new ResultStatus("A0110", "Authorization error, please contact administractor"); }
        }

        public static ResultStatus NoAuthorityError
        {
            get { return new ResultStatus("A0120", "此帳號無任何功能權限"); }
        }

        public static ResultStatus ActivateError
        {
            get { return new ResultStatus("A0130", "帳戶未開通"); }
        }

        #endregion 權限相關

        #region 交易安全驗證相關

        public static ResultStatus JWTTimeError
        {
            get { return new ResultStatus("E9995", "JWT expiration time"); }
        }

        public static ResultStatus TokenTimeoutError
        {
            get { return new ResultStatus("E9996", "Session time out"); }
        }

        public static ResultStatus AccountLogoutError
        {
            get { return new ResultStatus("E9997", "You have been denied due to login from other devices"); }
        }

        public static ResultStatus AuthenticationError
        {
            get { return new ResultStatus("E9998", "不合法的通訊驗證來源，例如無法滿足來源辨識原則。"); }
        }

        public static ResultStatus IntegrityError
        {
            get { return new ResultStatus("E9999", "通訊資料完整性驗證錯誤，例如無法滿足完整性原則。"); }
        }

        #endregion 交易安全驗證相關

        #region getLogin驗證使用者帳號密碼
        public static ResultStatus UserAccountError
        {
            //您的帳號密碼錯誤，請再確認或是聯絡管理人員
            get { return new ResultStatus("C0102", "Your account password is incorrect. Please confirm or contact the administrator."); }
        }

        public static ResultStatus UserStatusError
        {
            //您的帳號已被停用，請聯絡管理人員開通帳號
            get { return new ResultStatus("C0101", "Your account has been disabled. Please contact the administrator to open an account."); }
        }

        #endregion getLogin驗證使用者帳號密碼
    }
}