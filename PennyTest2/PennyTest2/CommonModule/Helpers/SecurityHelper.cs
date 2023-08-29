using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace CommonModule
{
    /// <summary>
    /// 安全性處理協助
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// AES加密
        /// skey傳入16字為AES128; 32字為AES256
        /// </summary>
        /// <param name="plainStr">待加密字串</param>
        /// <param name="skey">加密金鑰</param>
        /// <returns>加密結果</returns>
        public static string AESEncrypt(string plainStr, string skey)
        {
            string encrypt = string.Empty;
            try
            {
                byte[] bData = Encoding.UTF8.GetBytes(plainStr);
                byte[] bKey = Encoding.UTF8.GetBytes(skey);
                encrypt = AESEncrypt(bData, bKey);
            }
            catch (Exception ex)
            {
                string message = string.Format("AESEncrypt in String Error  : Str={0} | ErrorMessage={1}", plainStr, ex.Message);
                LogHelper.WriteLog(LogLevel.Warn, message);
            }

            return encrypt;
        }

        /// <summary>
        /// AES加密
        /// skey傳入16字為AES128; 32字為AES256
        /// </summary>
        /// <param name="bData">待加密字串</param>
        /// <param name="bKey">加密金鑰</param>
        /// <returns>加密結果</returns>
        public static string AESEncrypt(byte[] bData, byte[] bKey)
        {
            byte[] bIV = new byte[16];
            AesCryptoServiceProvider aes;
            string encrypt = string.Empty;
            try
            {
                aes = new AesCryptoServiceProvider();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = bKey;
                aes.IV = bIV;
                
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                cStream.Write(bData, 0, bData.Length);
                cStream.FlushFinalBlock();
                encrypt = Convert.ToBase64String(mStream.ToArray());

                mStream.Close();
                aes.Clear();
            }
            catch (Exception ex)
            {
                string plainStr = BitConverter.ToString(bData);
                string message = string.Format("AESEncrypt in Byte[] Error  : Str={0} | ErrorMessage={1}", plainStr, ex.Message);
                LogHelper.WriteLog(LogLevel.Warn, message);
            }

            return encrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">待解密字串</param>
        /// <param name="skey">加密金鑰</param>
        /// <returns>解密結果</returns>
        public static string AESDecrypt(string encryptStr, string skey)
        {
            string decrypt = string.Empty;
            try
            {
                byte[] bData = Convert.FromBase64String(encryptStr);
                byte[] bKey = Encoding.UTF8.GetBytes(skey);
                decrypt = AESDecrypt(bData, bKey);
            }
            catch (Exception ex)
            {
                string message = string.Format("AESDecrypt in String Error : Str={0} | ErrorMessage={1}", encryptStr, ex.Message);
                LogHelper.WriteLog(LogLevel.Warn, message);
            }

            return decrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="bData">待解密字串</param>
        /// <param name="bKey">加密金鑰</param>
        /// <returns>解密結果</returns>
        public static string AESDecrypt(byte[] bData, byte[] bKey)
        {
            byte[] bIV = new byte[16];
            AesCryptoServiceProvider aes;
            string decrypt = string.Empty;
            try
            {
                aes = new AesCryptoServiceProvider();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = bKey;
                aes.IV = bIV;
                
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                cStream.Write(bData, 0, bData.Length);
                cStream.FlushFinalBlock();
                decrypt = Encoding.UTF8.GetString(mStream.ToArray());

                mStream.Close();
                aes.Clear();
            }
            catch (Exception ex)
            {
                string encryptStr = BitConverter.ToString(bData);
                string message = string.Format("AESDecrypt in Byte[] Error : Str={0} | ErrorMessage={1}", encryptStr, ex.Message);
                LogHelper.WriteLog(LogLevel.Warn, message);
            }

            return decrypt;
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="plainStr">待加密字串 by string</param>
        /// <returns>加密結果</returns>
        public static string SHA256Encrypt(string plainStr)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainStr);
            return SHA256Encrypt(bytes);
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="bytes">待加密字串 by byte[]</param>
        /// <returns>加密結果</returns>
        public static string SHA256Encrypt(byte[] bytes)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash); // Base64 - 44碼
        }

        /// <summary>
        /// SHA256加密 For Signature
        /// </summary>
        /// <param name="plainStr">待加密字串 by string</param>
        /// <returns>加密結果</returns>
        public static string SHA256EncryptForSignature(string plainStr)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainStr);
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(bytes);
            string result = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower(); // 16進制
            byte[] bytes2 = Encoding.UTF8.GetBytes(result);
            return Convert.ToBase64String(bytes2);
        }

        /// <summary>
        /// 產生一個非負數且最大值 max 以下的亂數
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>隨機亂數數字</returns>
        public static int RNGCrypt(int max)
        {
            RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
            byte[] rb = new byte[4];
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0)
            {
                value = -value;
            }

            return value;
        }

        /// <summary>
        /// 產生一個非負數且最小值在 min 以上最大值在 max 以下的亂數
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>隨機亂數數字</returns>
        public static int RNGCrypt(int min, int max)
        {
            int value = RNGCrypt(max - min) + min;
            return value;
        }

        /// <summary>
        /// 將文字做Base64加密
        /// </summary>
        /// <param name="str">str</param>
        /// <returns>Base64字串</returns>
        public static string Base64Encode(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 將Base64字串轉回文字
        /// </summary>
        /// <param name="base64Str">base64Str</param>
        /// <returns>轉回結果</returns>
        public static string Base64Decode(string base64Str)
        {
            byte[] bytes = Convert.FromBase64String(base64Str);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 對JWT的header和payLoad 簽章(signature)
        /// </summary>
        /// <param name="header">JWT的header</param>
        /// <param name="payLoad">JWT的payLoad</param>
        /// <returns>Base64簽章</returns>
        public static string SignJWT(string header, string payLoad)
        {
            string plainText = header + '.' + payLoad;
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] jwtKeyBytes = Encoding.UTF8.GetBytes(KeyCode.JwtTokenKey);
            HMACSHA256 hasher = new HMACSHA256(jwtKeyBytes);
            byte[] signatureBytes = hasher.ComputeHash(plainTextBytes);
            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>
        /// 驗證時間戳記是否符合
        /// </summary>
        /// <param name="timeStamp">Base64 TimeStamp string</param>
        /// <returns>true:符合;false:不符合</returns>
        public static bool TimeStampValidate(string timeStamp)
        {
            int timeStampValidateMinutes = CommUtility.GetConfig("TimeStampValidateMinutes", 15);
            DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime utcTime = DateTime.UtcNow;
            TimeSpan sTimeSpan = utcTime.AddMinutes(-timeStampValidateMinutes) - baseTime;
            double sTime = Math.Round(sTimeSpan.TotalMilliseconds);
            TimeSpan eTimeSpan = utcTime.AddMinutes(timeStampValidateMinutes) - baseTime;
            double eTime = Math.Round(eTimeSpan.TotalMilliseconds);
            var timStampBytes = Convert.FromBase64String(timeStamp);
            string totalsecond = Encoding.UTF8.GetString(timStampBytes);
            double totalseconds = double.Parse(totalsecond);
            if (totalseconds > sTime && totalseconds < eTime)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 產生驗證用的Salt
        /// </summary>
        /// <param name="name">Route Method Name</param>
        /// <param name="timeStamp">時間戳記</param>
        /// <returns>驗證用的Salt</returns>
        public static string GenerateSalt(string name, string timeStamp)
        {
            string result = string.Empty;
            int nRound = name.Length % 2 == 0 ? name.Length / 2 : (name.Length / 2) + 1;
            int tsRound = timeStamp.Length % 2 == 0 ? timeStamp.Length / 2 : (timeStamp.Length / 2) + 1;
            int loopLength = timeStamp.Length > name.Length ? timeStamp.Length : name.Length;
            for (int i = 0; i < loopLength; i++)
            {
                if (i < nRound)
                {
                    result = result + name[i];
                }

                if (i < tsRound)
                {
                    result = result + timeStamp[i];
                }

                if (i < name.Length / 2)
                {
                    result = result + name[name.Length - 1 - i];
                }

                if (i < timeStamp.Length / 2)
                {
                    result = result + timeStamp[timeStamp.Length - 1 - i];
                }

                if (result.Length == 32)
                {
                    break;
                }
            }

            result = result.PadLeft(32, '*');
            return result;
        }

        /// <summary>
        /// 產生Aes Key
        /// </summary>
        /// <param name="factor">因子(Token)</param>
        /// <returns>Aes Key</returns>
        public static string GenerateAESKey(string factor)
        {
            string result = string.Empty;
            int midLength = factor.Length % 2 == 0 ? factor.Length / 2 : (factor.Length / 2) + 1;
            var factorA = factor.Substring(0, midLength).ToList();
            var factorB = factor.Substring(midLength, factor.Length - midLength).ToList();
            for (int i = 2; i <= midLength; i = i + 2)
            {
                var prefix = factorA.Skip(factorA.Count() - i).Take(2).ToList();
                result = result + string.Join(string.Empty, prefix);
                var postfix = factorB.Skip(factorB.Count() - i).Take(2).ToList();
                result = result + string.Join(string.Empty, postfix);
            }

            // 補齊
            if (factorA.Count > factorB.Count)
            {
                result = result + factorA.First();
            }
            else if (factorB.Count > factorA.Count)
            {
                result = result + factorB.First();
            }

            if (result.Length > 32)
            {
                result = result.Substring(0, 32);
            }

            if (result.Length < 32)
            {
                result = result.PadRight(32, '#');
            }

            return result;
        }

        /// <summary>
        /// 取得Request Body內容
        /// </summary>
        /// <returns>Request Body內容</returns>
        public static string GetRequestBody()
        {
            try
            {
                var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();
                return bodyText;
            }
            catch (Exception ex)
            {
                return $"Get Request Fail:{ex.Message}";
            }
        }

        /// <summary>
        /// 取得Response Body內容
        /// </summary>
        /// <param name="httpResponseMessage">httpResponseMessage</param>
        /// <returns>Response Body內容</returns>
        public static string GetResponseBody(HttpResponseMessage httpResponseMessage)
        {
            string responseData = string.Empty;
            ObjectContent content = httpResponseMessage.Content as ObjectContent;
            if (content != null && content.Value != null)
            {
                responseData = content.Value.ToString();
            }

            return responseData;
        }

        /// <summary>
        /// 取得Token有效時間(分鐘)
        /// 預設15分鐘
        /// </summary>
        /// <returns>Token有效時間</returns>
        public static int GetTokenValidMin()
        {
            int tokenValidMin = CommUtility.GetConfig("TokenValidMin", 15);
            return tokenValidMin;
        }
    }
}
