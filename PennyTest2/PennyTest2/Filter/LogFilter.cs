using CommonModule;
using PennyTest2.DataBase;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace PennyTest2.Filter
{
    /// <summary>
    /// 電文 LogFilter
    /// </summary>
    public class LogFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Action 結束後
        /// </summary>
        /// <param name="filterContext">HttpAction input內容</param>
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            if (!filterContext.Request.GetActionDescriptor().GetCustomAttributes<IgnoreLog>().Any())
            {
                string responseData = string.Empty;
                // 紀錄輸出給前端Response

                using (var ecSrv = new PennytTest_Entities())
                {
                    var query = new TRANSACTIONLOGBACKEND();
                    query.BUSERID = Commuser.Uid;
                    query.QUERYDATETIMEUTC = DateTime.UtcNow;
                    query.QUERYATTIMEZONE = Convert.ToDecimal(Commuser.TimezoneOffset);
                    query.TXNNO = filterContext.ActionContext.ActionDescriptor.ActionName;
                    query.SYSTEMTYPE = "ECHECK";
                    query.UUID = Guid.NewGuid().ToString("N");

                    string requestData = Commuser.RequestData;
                    if (!string.IsNullOrWhiteSpace(requestData))
                    {
                        // 資料庫過濾參數 FileContent
                        JObject jo = JObject.Parse(requestData);
                        if (jo.ContainsKey("FileContent"))
                        {
                            jo.Property("FileContent").Remove();
                        } 
                        requestData = jo.ToString();
                    }
                    //20200224 STATEMENTJSON 改為空值或不寫入
                    //query.STATEMENTJSON = requestData; //(電文REQUEST BODY 的DATA的 JSON)
                    query.BROSWERVERSION = HttpContext.Current.Request.UserAgent;
                    query.IP = CommUtility.GetClientIPInfo();

                    // 發生Exception,紀錄全域的Exception
                    if (filterContext.Exception != null)
                    {
                        query.TXNSTATUSCODE = "N"; //(Y-成功,N-失敗,T:TIME OUT)
                        //20200224 EXCEPTIONMESSAGE 改為空值或不寫入
                        //query.EXCEPTIONMESSAGE = filterContext.Exception.ToString();
                    }

                    if (filterContext.Response != null)
                    {
                        if (filterContext.Response.Content.Headers.ContentType.MediaType == "application/json")
                        {
                            ResponseObj content = (filterContext.Response.Content as ObjectContent).Value as ResponseObj;
                            if (content.ReturnCode != "C0000")
                            {
                                query.TXNSTATUSCODE = "N"; //(Y-成功,N-失敗,T:TIME OUT)
                                //20200224 EXCEPTIONMESSAGE 改為空值或不寫入
                                //query.EXCEPTIONMESSAGE = "{ \"Code\":\"" + content.ReturnCode + "\",\"Msg\":\"" + content.ReturnMsg + "\"}";
                            }
                            else
                            {
                                query.TXNSTATUSCODE = "Y";
                            }
                        }
                    }

                    ecSrv.TRANSACTIONLOGBACKEND.Add(query);
                    ecSrv.SaveChanges();
                }
            }
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Filter排除的屬性
        /// </summary>
        public class IgnoreLog : Attribute
        {
        }
    }
}