using Newtonsoft.Json;

namespace CommonModule
{
    /// <summary>
    /// ToJsonString
    /// </summary>
    public class ToJsonString
    {
        /// <summary>
        /// 將資料物件轉換為 JavaScript 物件標記法 (JSON) 格式的字串
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}