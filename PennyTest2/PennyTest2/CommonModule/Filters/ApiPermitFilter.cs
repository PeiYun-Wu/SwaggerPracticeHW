using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CommonModule
{
    /// <summary>
    /// API權限過濾器
    /// </summary>
    public class ApiPermitFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">Token驗證服務</param>
        public ApiPermitFilter(ITokenValidate service)
        {
            tokenValidateSrv = service;
        }

        /// <summary>
        /// Token驗證服務
        /// </summary>
        private ITokenValidate tokenValidateSrv;

        /// <summary>
        /// Action 執行前啟動
        /// </summary>
        /// <param name="filterContext">HttpAction input內容</param>
        public override void OnActionExecuting(HttpActionContext filterContext)//登入驗證流程
        {
            if (CommUtility.GetBaseConfig("TestMode", false) && 
                CommUtility.GetBaseConfig("UseSwaggerToken", false))
            {
                // 使用SwaggerToken時，不驗證Token 有效性
                base.OnActionExecuting(filterContext);
                return;
            }

            if (!filterContext.ActionDescriptor.GetCustomAttributes<IgnoreApiPermit>().Any())
            {
                string token = Commuser.Token;
                if (string.IsNullOrEmpty(token))
                    throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "IsNullOrEmpty(token)" };

                //驗證在儲存體中存在的Token是否合法
                if (!tokenValidateSrv.ValidateDBToken(token))
                    throw new ErrInfoException(ResultStatus.PermitValidateError) { LogMsg = "Token不合法" };

                //驗證在儲存體中存在的Token是否被登出
                if (!tokenValidateSrv.ValidateDBTokenLogout(token))
                    throw new ErrInfoException(ResultStatus.AccountLogoutError) { LogMsg = "Token被登出" };

                //驗證在儲存體中存在的Token是否過時
                if (!tokenValidateSrv.ValidateDBTokenEffective(token))
                    throw new ErrInfoException(ResultStatus.TokenTimeoutError) { LogMsg = "Token過時" };
            }
            base.OnActionExecuting(filterContext);
        }
    }

    /// <summary>
    /// Filter排除的屬性
    /// </summary>
    public class IgnoreApiPermit : Attribute
    {
    }
}