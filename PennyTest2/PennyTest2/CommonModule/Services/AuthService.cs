using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace CommonModule
{
    /// <summary>
    /// 資料驗證服務
    /// </summary>
    public class AuthService : ToJsonString
    {
        /// <summary>
        /// 電文加解密金鑰
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// 上行電文Http檔頭
        /// </summary>
        public RequestHeader RqHeader { get; set; }

        /// <summary>
        /// 驗證時效性
        /// </summary>
        public void ValidateSequence()
        {
            if (string.IsNullOrEmpty(RqHeader.Sequence))
                throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "IsNullOrEmpty(RqHeader.Sequence)" };

            int effectiveSeconds = CommUtility.GetConfig("EffectiveSeconds", 300);
            DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime utcTime = DateTime.UtcNow;
            TimeSpan sTimeSpan = utcTime.AddSeconds(-effectiveSeconds) - baseTime;
            double sTime = Math.Round(sTimeSpan.TotalMilliseconds);
            TimeSpan eTimeSpan = utcTime.AddSeconds(effectiveSeconds) - baseTime;
            double eTime = Math.Round(eTimeSpan.TotalMilliseconds);
            var timStampBytes = Convert.FromBase64String(RqHeader.Sequence);
            string totalmillisecond = Encoding.UTF8.GetString(timStampBytes);
            double totalmilliseconds = double.Parse(totalmillisecond);
            if (!(totalmilliseconds > sTime && totalmilliseconds < eTime))
                throw new ErrInfoException(ResultStatus.JWTTimeError) { LogMsg = $"驗證時效性失敗!({totalmilliseconds} > sTime{sTime} && {totalmilliseconds} < eTime{eTime})" };
        }

        /// <summary>
        /// 驗證來源正確性(VID)
        /// </summary>
        public void ValidateVID()
        {
            if (string.IsNullOrEmpty(RqHeader.VID))
                throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "IsNullOrEmpty(RqHeader.VID)" };

            string preSeq = RqHeader.Sequence.Substring(0, 6);
            string postSeq = RqHeader.Sequence.Substring(RqHeader.Sequence.Length - 6);
            string vid = $"{preSeq}{RqHeader.AppInfo}{RqHeader.DevType}{RqHeader.DevID}{postSeq}";
            string plainVID = SecurityHelper.AESDecrypt(RqHeader.VID, AuthKey);
            if (plainVID != vid)
                throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "驗證來源正確性(VID)失敗" };
        }

        /// <summary>
        /// 驗證上行電文內容正確性
        /// </summary>
        /// <param name="rqData">上行電文</param>
        public void ValidateDataSignature(string rqData)
        {
            if (string.IsNullOrEmpty(RqHeader.Signature))
                throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "IsNullOrEmpty(RqHeader.Signature)" };

            if (string.IsNullOrEmpty(rqData))
                rqData = RqHeader.Sequence;

            byte[] rqBytesData = Encoding.UTF8.GetBytes(rqData);
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(rqBytesData);
            string plainText = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            string cryptText = SecurityHelper.AESEncrypt(plainText, AuthKey);
            if (cryptText != RqHeader.Signature)
                throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "驗證上行電文內容正確性失敗" };
        }

        /// <summary>
        /// 驗證Json Web Token
        /// </summary>
        /// <returns>Jwt Jti</returns>
        public string ValidateJsonWebToken()
        {
            string[] jwtArray = RqHeader.TokenID.Split('.');
            string header = jwtArray[0];
            string payLoad = jwtArray[1];
            string signature = jwtArray[2];
            //驗簽章
            if (signature != SignJsonWebToken(header, payLoad))
                throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "ValidateJsonWebToken驗簽章失敗" };

            string strPayLoad = Encoding.UTF8.GetString(Convert.FromBase64String(payLoad));
            var jwtPayload = JsonConvert.DeserializeObject<JWTPayload>(strPayLoad);
            //驗有效期
            DateTime jwtExp = DateTime.Parse(jwtPayload.Exp);
            if (DateTime.Now > jwtExp)
                throw new ErrInfoException(ResultStatus.JWTTimeError) { LogMsg = "ValidateJsonWebToken驗有效期失敗" };

            Commuser.Token = jwtPayload.Jti;
            Commuser.Uid = jwtPayload.Uid;
            return jwtPayload.Jti;
        }

        /// <summary>
        /// 產生Json Web Token
        /// </summary>
        /// <param name="userId">使用者代號</param>
        /// <param name="uptContext">是否更新暫存</param>
        /// <returns>Json Web Token</returns>
        public string CreateJsonWebToken(string userId, bool uptContext = true)  //長榮規格第七~八頁
        {
            string header = SecurityHelper.Base64Encode(new JWTHeader().ToString()); //header設定加密方式和交易格式
            string token = Guid.NewGuid().ToString();
            JWTPayload jwtPayload = new JWTPayload()
            {
                Jti = token, //guid
                Exp = DateTime.Now.AddHours(24).ToString("yyyy/MM/dd HH:mm:ss"), //過期時間
                Uid = userId  //使用者代號
            };
            string payLoad = SecurityHelper.Base64Encode(jwtPayload.ToString());
            string signature = SignJsonWebToken(header, payLoad); //簽章起來
            string jwt = header + "." + payLoad + "." + signature;  //三組base64字串組成合併為jwt
            if (uptContext)
            {
                Commuser.Jwt = jwt;
                Commuser.Token = token;
            }
            return jwt;
        }

        /// <summary>
        /// 產生Json Web Token簽章(signature)
        /// </summary>
        /// <param name="header">JWT的header</param>
        /// <param name="payLoad">JWT的payLoad</param>
        /// <returns>Base64簽章</returns>
        public string SignJsonWebToken(string header, string payLoad)
        {
            string plainText = header + '.' + payLoad;
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] jwtKeyBytes = Encoding.UTF8.GetBytes(KeyCode.JwtTokenKey);
            HMACSHA256 hasher = new HMACSHA256(jwtKeyBytes);
            byte[] signatureBytes = hasher.ComputeHash(plainTextBytes);
            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>
        /// 產生Aes Key
        /// </summary>
        /// <param name="factor">產生因子</param>
        /// <returns>AES Key</returns>
        public string CreateAESKey(string factor)
        {
            if (factor.Length < 32)
                factor = factor.PadRight(32, '*');

            string[] jwtArray = factor.Split('.');
            string factorA = jwtArray[1];
            string factorB = jwtArray[2];
            int midLength = factorA.Length;
            if (midLength > factorB.Length)
                midLength = factorB.Length;

            midLength = midLength % 2 == 0 ? midLength : midLength + 1;
            AuthKey = string.Empty;
            for (int i = 0; i <= midLength; i = i + 2)
            {
                var prefixA = factorA.Skip(i).Take(2).ToList();
                AuthKey = AuthKey + string.Join(string.Empty, prefixA);
                var prefixB = factorB.Skip(i).Take(2).ToList();
                AuthKey = AuthKey + string.Join(string.Empty, prefixB);
            }
            if (AuthKey.Length > 32)
                AuthKey = AuthKey.Substring(0, 32);
            Commuser.AuthCode = AuthKey;
            return AuthKey;
        }

        /// <summary>
        /// Bind上行電文資料
        /// </summary>
        /// <param name="httpHeader">Http Header</param>
        /// <param name="httpBody">Http Body</param>
        public void BindRequestData(HttpRequestHeaders httpHeader, string httpBody)
        {
            //Bind user define http Header
            RqHeader = new RequestHeader();
            var properties = typeof(RequestHeader).GetProperties();
            foreach (var property in properties)
            {
                string headKey = property.Name;
                IEnumerable<string> values = null;
                if (httpHeader.TryGetValues(headKey, out values))
                    property.SetValue(RqHeader, values.FirstOrDefault(), null);
            }
            //儲存Jwt
            Commuser.Jwt = RqHeader.TokenID;
            //儲存App版本
            Commuser.AppInfo = RqHeader.AppInfo;
        }
    }
}