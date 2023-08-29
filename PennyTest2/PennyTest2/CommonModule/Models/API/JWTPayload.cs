namespace CommonModule
{
    /// <summary>
    /// JWTPayload
    /// </summary>
    public class JWTPayload: ToJsonString
    {
        /// <summary>
        /// 登入伺服端識別碼
        /// </summary>
        public string Jti { get; set; }

        /// <summary>
        /// 到期時間
        /// </summary>
        public string Exp { get; set; }

        /// <summary>
        /// 使用者代號
        /// </summary>
        public string Uid { get; set; }
    }
}