using System;
using System.Collections.Generic;
using System.Linq;
using CommonModule;
using PennyTest2.DataBase;
using PennyTest2.Models.Api;

namespace PennyTest2.Helpers
{
    /// <summary>
    /// FunctionHelper
    /// </summary>
    public static class FunctionHelper
    {
        /// <summary>
        /// GetTIMEZONE
        /// </summary>
        /// <param name="req">req</param>
        /// <returns>decimal</returns>
        public static decimal GetTIMEZONE(string req)
        {
            var result = Convert.ToDecimal(Commuser.TimezoneOffset);
            if (!string.IsNullOrEmpty(req))
            {
                result = Convert.ToDecimal(req);
            }
            return result;
        }

        /// <summary>
        /// StringToDecimal
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>decimal</returns>
        public static decimal? StringToDecimal(object value)
        {
            decimal? rtnValue = null;
            if (!string.IsNullOrWhiteSpace(value.ToString()))
            {
                rtnValue = Convert.ToDecimal(value.ToString());
            }

            return rtnValue;
        }

        /// <summary>
        /// 產生表單流水號，格式3碼，前面補零
        /// </summary>
        /// <param name="maxSerial"></param>
        /// <returns></returns>
        public static string GetInsertSerialNo(int maxSerial)
        {
            maxSerial++;
            return maxSerial.ToString().PadLeft(3, '0');
        }
    }
}