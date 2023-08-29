using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CommonModule;

namespace CommonModule
{
    /// <summary>
    /// ConfigHelper(從Config讀取參數)
    /// </summary>
    public class ConfigHelper : IConfig
    {
        /// <summary>
        /// 讀取config 參數
        /// </summary>
        /// <typeparam name="T">變數類型</typeparam>
        /// <param name="name">變數名稱</param>
        /// <param name="defValue">預設參數</param>
        /// <returns>T型態變數內容</returns>
        public T GetConfig<T>(string name, T defValue)
        {
            string value = ConfigurationManager.AppSettings[name];
            if (value == null)
            {
                return defValue;
            }
            else
            {
                return CommUtility.ConvertValue(value, defValue);
            }
        }

        /// <summary>
        /// 讀取config 參數
        /// </summary>
        /// <typeparam name="T">變數類型</typeparam>
        /// <param name="name">變數名稱</param>
        /// <returns>T型態變數內容</returns>
        public T GetConfig<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];
            if (value == null)
            {
                throw new ApplicationException($"參數{name}讀取錯誤。");
            }
            else
            {
                T result = default(T);
                if (CommUtility.TryConvertValue(value, out result))
                    return result;
                else
                    throw new ApplicationException($"參數{name}轉換錯誤，{value}無法轉為{typeof(T).Name}型態。");
            }
        }
    }
}