using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Http.Controllers;
using CommonModule;

namespace PennyTest2.Helper
{
    public class DateFormateHelper
    {
        /// <summary>
        /// 將日期時間轉為MMM/dd/yyyy字串(May/12/2019)
        /// </summary>
        /// <param name="dt">傳入物件</param>
        /// <returns>字串</returns>
        public static string ToMMMddyyyString(DateTime? dt)
        {
            string result = string.Empty;
            if (dt != null)
            {
                DateTime q = (DateTime)dt;
                result = q.ToString("MMM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
            }
            return result;
        }

        /// <summary>
        /// 將日期時間轉為MMM/dd/yyyy HH:mm字串(May/12/2019 18:30)
        /// </summary>
        /// <param name="dt">傳入物件</param>
        /// <returns>字串</returns>
        public static string ToMMMddyyyHHmmString(DateTime? dt)
        {
            string result = string.Empty;
            if (dt != null)
            {
                DateTime q = (DateTime)dt;
                result = q.ToString("MMM/dd/yyyy HH:mm", CultureInfo.CreateSpecificCulture("en-GB"));
            }
            return result;
        }

        /// <summary>
        /// 將日期時間轉為MMM/dd/yyyy HH:mm:ss字串(May/12/2019 18:30:20)
        /// </summary>
        /// <param name="dt">傳入物件</param>
        /// <returns>字串</returns>
        public static string ToMMMddyyyHHmmssString(DateTime? dt)
        {
            string result = string.Empty;
            if (dt != null)
            {
                DateTime q = (DateTime)dt;
                result = q.ToString("MMM/dd/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-GB"));
            }
            return result;
        }

        /// <summary>
        /// 將日期時間轉為yyyy/MM/dd HH:mm:ss字串(2019/05/12 18:30:20)
        /// </summary>
        /// <param name="dt">傳入物件</param>
        /// <returns>字串</returns>
        public static string ToyyyyMMddString(DateTime? dt)
        {
            string result = string.Empty;
            if (dt != null)
            {
                DateTime q = (DateTime)dt;
                result = q.ToString("yyyy/MM/dd HH:mm:ss");
            }
            return result;
        }

        /// <summary>
        /// 將日期時間轉為yyyy/MM/dd HH:mm:ss字串(2019/05/12 18:30:20)
        /// </summary>
        /// <param name="dt">傳入物件</param>
        /// <returns>字串</returns>
        public static string ToyyyyMMddString(DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 將日期時間轉為yyyy/MM/dd HH:mm:ss字串(2019/05/12 18:30:20)
        /// </summary>
        /// <param name="dt">傳入物件</param>
        /// <returns>字串</returns>
        public static string ToyyyyMMddmmssString(DateTime? dt)
        {
            string result = string.Empty;
            if (dt != null)
            {
                DateTime q = (DateTime)dt;
                result = q.ToString("yyyyMMddHHmmss");
            }
            return result;
        }

        /// <summary>
        /// 將日期時間轉為yyMMdd字串(190512)
        /// </summary>
        /// <param name="dt">傳入物件</param>
        /// <returns>字串</returns>
        public static string ToyyMMddString(DateTime dt)
        {
            return dt.ToString("yyMMdd");
        }

        /// <summary>
        /// 將字串轉為日期
        /// </summary>
        /// <param name="inputDateTime">傳入物件</param>
        /// <param name="parseFormate">傳入格式</param>
        /// <returns>字串</returns>
        public static DateTime? StringToDateTime(string inputDateTime, string parseFormate)
        {
            DateTime outputDateTime;
            DateTime? result = (DateTime?)null;
            if (DateTime.TryParseExact(inputDateTime, parseFormate, CultureInfo.InvariantCulture, DateTimeStyles.None, out outputDateTime))
            {
                result = outputDateTime;
            }

            return result;
        }
    }
}
