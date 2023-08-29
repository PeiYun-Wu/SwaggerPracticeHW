namespace CommonModule
{
    /// <summary>
    /// Token 驗證介面
    /// </summary>
    public interface ITokenValidate
    {
        /// <summary>
        /// 驗證Token(在儲存體DB)是否合法
        /// </summary>
        /// <param name="toekn">Token</param>
        /// <returns>結果</returns>
        bool ValidateDBToken(string toekn);

        /// <summary>
        /// 驗證Token(在儲存體DB)是否被登出
        /// </summary>
        /// <param name="toekn">Token</param>
        /// <returns>結果</returns>
        bool ValidateDBTokenLogout(string toekn);

        /// <summary>
        /// 驗證Token(在儲存體DB)是否過時
        /// </summary>
        /// <param name="toekn">Token</param>
        /// <returns>結果</returns>
        bool ValidateDBTokenEffective(string toekn);
    }
}