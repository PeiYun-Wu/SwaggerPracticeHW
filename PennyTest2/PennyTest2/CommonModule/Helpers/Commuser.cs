using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonModule
{
    /// <summary>
    /// 會員資訊共用元件
    /// </summary>
    public class Commuser
    {
        /// <summary>
        /// Uid
        /// </summary>
        public static string Uid
        {
            get
            {
                if (CommUtility.GetContext("Uid") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("Uid");
            }
            set
            {
                CommUtility.SetContext("Uid", value);
            }
        }

        /// <summary>
        /// Token
        /// </summary>
        public static string Token
        {
            get
            {
                if (CommUtility.GetContext("Token") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("Token");
            }
            set
            {
                CommUtility.SetContext("Token", value);
            }
        }

        /// <summary>
        /// Jwt
        /// </summary>
        public static string Jwt
        {
            get
            {
                if (CommUtility.GetContext("Jwt") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("Jwt");
            }
            set
            {
                CommUtility.SetContext("Jwt", value);
            }
        }

        /// <summary>
        /// DBUser
        /// </summary>
        public static string DBUser
        {
            get
            {
                if (CommUtility.GetContext("DBUser") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("DBUser");
            }
            set
            {
                CommUtility.SetContext("DBUser", value);
            }
        }

        /// <summary>
        /// TimezoneOffset
        /// </summary>
        public static string TimezoneOffset
        {
            get
            {
                if (CommUtility.GetContext("timezoneOffset") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("timezoneOffset");
            }
            set
            {
                CommUtility.SetContext("timezoneOffset", value);
            }
        }

        /// <summary>
        /// RequestData
        /// </summary>
        public static string RequestData
        {
            get
            {
                if (CommUtility.GetContext("RequestData") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("RequestData");
            }
            set
            {
                CommUtility.SetContext("RequestData", value);
            }
        }

        /// <summary>
        /// AuthCode
        /// </summary>
        public static string AuthCode
        {
            get
            {
                if (CommUtility.GetContext("AuthCode") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("AuthCode");
            }
            set
            {
                CommUtility.SetContext("AuthCode", value);
            }
        }

        /// <summary>
        /// AppInfo
        /// </summary>
        public static string AppInfo
        {
            get
            {
                if (CommUtility.GetContext("AppInfo") == null)
                    return string.Empty;
                else
                    return CommUtility.GetContext("AppInfo");
            }
            set
            {
                CommUtility.SetContext("AppInfo", value);
            }
        }
    }
}