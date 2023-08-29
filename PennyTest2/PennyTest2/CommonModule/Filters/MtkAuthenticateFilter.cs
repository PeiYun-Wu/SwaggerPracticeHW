using Newtonsoft.Json;
using PennyTest2.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CommonModule
{
    /// <summary>
    /// 安全性認證過濾器
    /// </summary>
    public class MtkAuthenticateFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Action 執行前啟動
        /// </summary>
        /// <param name="filterContext">HttpAction input內容</param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            ResponseObj errorResult = null;
            HttpRequestMessage request = filterContext.Request;
            string url = request.RequestUri.PathAndQuery.TrimStart('/');
            string method = request.Method.ToString();
            string jwtToken = string.Empty;
            CommUtility.SetContext("Url", url);  //連到httpContext
            CommUtility.SetContext("Method", method);
            CommUtility.SetContext("JwtToken", jwtToken);

            string requestData = string.Empty;
            try
            {

                using (var stream = new StreamReader(request.Content.ReadAsStreamAsync().Result))
                {
                    stream.BaseStream.Position = 0;
                    requestData = stream.ReadToEnd();
                    Commuser.RequestData = requestData;
                }


                if (!filterContext.ActionDescriptor.GetCustomAttributes<IgnoreMtkAuthenticate>().Any()) //
                {//假如沒有mtk就會跑進來<ignore...>
                    var authSrv = new AuthService();
                    authSrv.BindRequestData(request.Headers, requestData);
                    jwtToken = authSrv.RqHeader.TokenID;
                    authSrv.CreateAESKey(jwtToken);//解密
                    authSrv.ValidateJsonWebToken();//token驗證
                    if (!IsFromSwaggerTest(request.Headers)) //判斷從swagger近來
                    {//三種驗證確保資料安全
                        authSrv.ValidateSequence();
                        authSrv.ValidateVID();
                        authSrv.ValidateDataSignature(requestData);
                    }
                }
                //紀錄Request Raw Data //記log以方便後續處理
                LogHelper.WriteLog(LogLevel.Debug, $"{method} Request: {url} : {requestData} : JWT TOKEN={jwtToken}");
            }
            catch (Exception ex) //後台紀錄哪裡出問題
            {
                //紀錄Request Raw Data
                LogHelper.WriteLog(LogLevel.Debug, $"{method} Request: {url} : {requestData} : JWT TOKEN={jwtToken}");
                errorResult = ExceptionHelper.Exception(ex);
                //紀錄Response Raw Data
                LogHelper.WriteLog(LogLevel.Debug, $"{method} Response: {url} : {JsonConvert.SerializeObject(errorResult)} : JWT TOKEN={jwtToken}");
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.OK, errorResult);
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Action 執行後啟動
        /// </summary>
        /// <param name="filterContext">HttpAction output內容</param>
        public override void OnActionExecuted(HttpActionExecutedContext filterContext) //Controller Response之後
        {
            string url = CommUtility.GetContext("Url");
            string method = CommUtility.GetContext("Method");
            //取得jwt
            string jwtToken = Commuser.Jwt;
            // 發生Exception,紀錄全域的Exception
            if (filterContext.Exception != null)
            {
                var exceptionResponse = ExceptionHelper.Exception(filterContext.Exception);
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.OK, exceptionResponse);
            }

            string responseData = "No Response";
            // 紀錄輸出給前端Response
            if (filterContext.Response != null)
            {
                if (filterContext.Response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    ObjectContent content = filterContext.Response.Content as ObjectContent;
                    if (content != null && content.Value != null)
                        responseData = JsonConvert.SerializeObject(content.Value);
                }
                else
                    responseData = filterContext.Response.Content.Headers.ContentType.MediaType;
            }
            //紀錄Response Raw Data
            LogHelper.WriteLog(LogLevel.Debug, $"{method} Response: {url} : {responseData} : JWT TOKEN={jwtToken}");
            //給前端使用
            filterContext.Response.Headers.Add("Access-Control-Expose-Headers", "tokenID");
            filterContext.Response.Headers.Add("tokenID", jwtToken); //每做一次改變,token更新傳給前端
            base.OnActionExecuted(filterContext); //防止jwt被竄改
        }

        /// <summary>
        /// For SwaggerTest
        /// </summary>
        /// <param name="httpHeader">Http header</param>
        /// <returns>true:SwaggerTest false: Not SwaggerTest</returns>
        private bool IsFromSwaggerTest(HttpRequestHeaders httpHeader) //
        {
            if (CommUtility.GetBaseConfig("TestMode", false)) //true才跑
            {
                string swaggerToken = CommUtility.GetBaseConfig("SwaggerToken", string.Empty);
                if (string.IsNullOrEmpty(swaggerToken))
                    return false;

                IEnumerable<string> values = null;
                if (httpHeader.TryGetValues("swaggerToken", out values))
                {
                    if (swaggerToken == values.FirstOrDefault())
                    {
                        if (CommUtility.GetBaseConfig("UseSwaggerToken", false))
                        {
                            Commuser.Token = swaggerToken;
                        }
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
    }

    /// <summary>
    /// Filter排除的屬性
    /// </summary>
    public class IgnoreMtkAuthenticate : Attribute
    {
    }
}