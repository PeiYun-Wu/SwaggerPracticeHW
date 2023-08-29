using System.Runtime.Serialization;

namespace CommonModule
{
    /// <summary>
    /// JWTHeader
    /// </summary>
    [DataContract]
    public class JWTHeader: ToJsonString
    {
        /// <summary>
        /// 交易格式
        /// </summary>
        [DataMember(Order = 1)]
        public string Typ;

        /// <summary>
        /// 加密方式
        /// </summary>
        [DataMember(Order = 2)]
        public string Alg;

        /// <summary>
        /// Initializes a new instance of the <see cref="JWTHeader"/> class.
        /// </summary>
        public JWTHeader()
        {
            Typ = "JWT"; //交易格式
            Alg = "HS256"; //加密方式
        }
    }
}