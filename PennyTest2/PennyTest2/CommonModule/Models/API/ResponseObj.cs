using System;
using System.Runtime.Serialization;
using System.Text;

namespace CommonModule
{
    /// <summary>
    /// 回傳元件
    /// </summary>
    [DataContract]
    public class ResponseObj : ToJsonString
    {
        /// <summary>
        /// 回應代碼
        /// </summary>
        [DataMember(Order = 1)]
        public string ReturnCode { get; set; }

        /// <summary>
        /// 回應訊息
        /// </summary>
        [DataMember(Order = 2)]
        public string ReturnMsg { get; set; }

        /// <summary>
        /// 系統時間
        /// </summary>
        [DataMember(Order = 3)]
        public string Sequence { get; set; }

        /// <summary>
        /// JWT
        /// </summary>
        [DataMember(Order = 4)]
        public object ReturnData { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public ResponseObj()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="returnCode">回傳代碼</param>
        /// <param name="returnMsg">回傳訊息</param>
        public ResponseObj(string returnCode, string returnMsg)
        {
            var timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
            byte[] tmpData = Encoding.UTF8.GetBytes(timeStamp.ToString());
            Sequence = Convert.ToBase64String(tmpData);
            ReturnCode = returnCode;
            ReturnMsg = returnMsg;
            ReturnData = new object();
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="data">回傳資料</param>
        public ResponseObj(ResultStatus data)
            : this(data.Code, data.Msg)
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="data">回傳資料</param>
        public ResponseObj(object data)
            : this(ResultStatus.SuccessCode)
        {
            ReturnData = data;
        }
    }
}