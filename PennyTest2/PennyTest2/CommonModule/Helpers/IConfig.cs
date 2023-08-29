using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModule
{
    /// <summary>
    /// Config 介面
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 讀出T型態參數
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <param name="defValue">預設值</param>
        /// <returns>T型態參數內容</returns>
        T GetConfig<T>(string name, T defValue);

        /// <summary>
        /// 讀出T型態參數
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <returns>T型態參數內容</returns>
        T GetConfig<T>(string name);
    }
}
