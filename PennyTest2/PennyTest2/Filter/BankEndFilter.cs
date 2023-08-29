using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CommonModule;

namespace PennyTest2.Filter
{
    /// <summary>
    /// API驗證過濾器
    /// </summary>
    public class BankEndFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Action 執行前啟動
        /// </summary>
        /// <param name="filterContext">HttpAction input內容</param>
        public override void OnActionExecuting(HttpActionContext filterContext) //using System.Web.Http.Filters;
        {
            if (!filterContext.ActionDescriptor.GetCustomAttributes<IgnoreBankEndFilter>().Any())
            {
                HttpRequestMessage request = filterContext.Request; //using System.Net.Http;
                if (!IsFromSwaggerTest(request.Headers))
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    var timezone = context.Request.Headers.GetValues("timezoneOffset");
                    Commuser.TimezoneOffset = timezone[0];//取秒數
                }
                else
                {
                    Commuser.TimezoneOffset = "-480";
                }
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// For SwaggerTest
        /// </summary>
        /// <param name="httpHeader">Http header</param>
        /// <returns>true:SwaggerTest false: Not SwaggerTest</returns>
        private bool IsFromSwaggerTest(HttpRequestHeaders httpHeader)
        {
            if (CommUtility.GetBaseConfig("TestMode", false))
            {
                string swaggerToken = CommUtility.GetBaseConfig("SwaggerToken", string.Empty);
                if (string.IsNullOrEmpty(swaggerToken))
                {
                    return false;
                }

                IEnumerable<string> values = null;
                if (httpHeader.TryGetValues("swaggerToken", out values))
                {
                    if (swaggerToken == values.FirstOrDefault())
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Action 結束後(不一定需要實作)
        /// </summary>
        /// <param name="filterContext">HttpAction input內容</param>
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Filter排除的屬性
        /// </summary>
        public class IgnoreBankEndFilter : Attribute
        {
        }
    }
}