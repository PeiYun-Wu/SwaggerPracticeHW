using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModule
{
    /// <summary>
    /// 自訂義回應文案
    /// </summary>
    public interface IResultStatus
    {
        /// <summary>
        /// 自訂義回應文案
        /// </summary>
        /// <returns>回應文案對應對應檔</returns>
        Dictionary<string, string> SetCustomResultMsg();
    }
}