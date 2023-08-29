using System.Configuration;

namespace CommonModule
{
    /// <summary>
    /// Key管理
    /// </summary>
    public class KeyCode
    {
        /// <summary>
        /// JWT 用 KEY
        /// </summary>
        public static string JwtTokenKey
        {
            get
            {
                string setStr = CommUtility.GetConfig("JwtTokenKey");
                return SecurityHelper.Base64Decode(setStr);
            }
        }

        /// <summary>
        /// AES 用 KEY
        /// </summary>
        public static string PushAesKey
        {
            get
            {
                string setStr = CommUtility.GetConfig("PushAesKey");
                return SecurityHelper.Base64Decode(setStr);
            }
        }
    }
}