namespace CommonModule
{
    /// <summary>
    /// 上行電文Http檔頭
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// 時間戳記
        /// </summary>
        public string Sequence { get; set; }

        /// <summary>
        /// App版本
        /// </summary>
        public string AppInfo { get; set; }

        /// <summary>
        /// 裝置版本
        /// </summary>
        public string DevType { get; set; }

        /// <summary>
        /// 裝置識別碼
        /// </summary>
        public string DevID { get; set; }

        /// <summary>
        /// Json Web Token
        /// </summary>
        public string TokenID { get; set; }

        /// <summary>
        /// 來源驗證
        /// </summary>
        public string VID { get; set; }

        /// <summary>
        /// 簽章
        /// </summary>
        public string Signature { get; set; }
    }

    /// <summary>
    /// 上傳電文元件
    /// </summary>
    /// <typeparam name="T">物件</typeparam>
    public class RequestObj<T>
    {
        /// <summary>
        /// 上傳資料
        /// </summary>
        public T RequestData { get; set; }
    }
}