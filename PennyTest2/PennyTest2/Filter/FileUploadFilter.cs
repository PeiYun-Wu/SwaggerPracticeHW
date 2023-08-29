using CommonModule;
using Newtonsoft.Json;
using PennyTest2.Models.Api;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PennyTest2.Filter
{
    /// <summary>
    /// 檔案上傳過濾器
    /// </summary>
    public class FileUploadFilter : ActionFilterAttribute
    {

        /// <summary>
        /// Action 執行前啟動
        /// </summary>
        /// <param name="filterContext">HttpAction input內容</param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (!filterContext.ActionDescriptor.GetCustomAttributes<IsFileUplaod>().Any() && 
                !filterContext.ActionDescriptor.GetCustomAttributes<IsNTCUplaod>().Any() &&
                !filterContext.ActionDescriptor.GetCustomAttributes<IsMaintCertUplaod>().Any())
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                ResponseObj errorResult = null;
                HttpRequestMessage request = filterContext.Request;
                string url = request.RequestUri.PathAndQuery.TrimStart('/');
                string method = request.Method.ToString();
                string jwtToken = string.Empty;
                CommUtility.SetContext("Url", url);
                CommUtility.SetContext("Method", method);
                CommUtility.SetContext("JwtToken", jwtToken);
                string requestData = string.Empty;
                try
                {
                    #region 取得FormData Params寫入requestData
                    var httpRequest = HttpContext.Current.Request;

                    if (filterContext.ActionDescriptor.GetCustomAttributes<IsNTCUplaod>().Any())
                    {
                        #region PutNTC
                        var rqModel = new RequestObj<PutNTCRequest>();
                        rqModel.RequestData = new PutNTCRequest();
                        var rqData = rqModel.RequestData;
                        rqData.AircraftNo = httpRequest.Params["AircraftNo"].ToString();
                        rqData.EONo = httpRequest.Params["EONo"].ToString();
                        rqData.NTCNo = httpRequest.Params["NTCNo"].ToString();
                        rqData.StartDateUTC = httpRequest.Params["StartDateUTC"].ToString();
                        rqData.EndDateUTC = httpRequest.Params["EndDateUTC"].ToString();
                        rqData.EffPeriod = httpRequest.Params["EffPeriod"].ToString();
                        rqData.Subject = httpRequest.Params["Subject"].ToString();
                        rqData.FileName = httpRequest.Params["FileName"].ToString();
                        rqData.FileSize = httpRequest.Params["FileSize"].ToString();
                        rqData.UploadDateTime = httpRequest.Params["UploadDateTime"].ToString();
                        rqData.OptionType = httpRequest.Params["OptionType"].ToString();
                        requestData = JsonConvert.SerializeObject(rqModel);
                        #endregion
                    }
                    else if (filterContext.ActionDescriptor.GetCustomAttributes<IsMaintCertUplaod>().Any())
                    {
                        #region PutMaintCert
                        var rqModel = new RequestObj<PutMainCertRequest>();
                        rqModel.RequestData = new PutMainCertRequest();
                        var rqData = rqModel.RequestData;
                        rqData.AircraftNo = httpRequest.Params["AircraftNo"].ToString();
                        rqData.CertKey = httpRequest.Params["CertKey"].ToString();
                        rqData.CertDate = httpRequest.Params["CertDate"].ToString();
                        rqData.FileName = httpRequest.Params["FileName"].ToString();
                        rqData.FileSize = httpRequest.Params["FileSize"].ToString();
                        rqData.UploadDateTime = httpRequest.Params["UploadDateTime"].ToString();
                        rqData.OptionType = httpRequest.Params["OptionType"].ToString();
                        requestData = JsonConvert.SerializeObject(rqModel);
                        #endregion
                    }
                    else
                    {
                        #region PutEntrySheet
                        var rqModel = new RequestObj<PutEntrySheetAttachRequest>();
                        rqModel.RequestData = new PutEntrySheetAttachRequest();
                        var rqData = rqModel.RequestData;
                        rqData.EntrySheetKey = httpRequest.Params["EntrySheetKey"].ToString();
                        rqData.FileID = httpRequest.Params["FileID"].ToString();
                        rqData.FileName = httpRequest.Params["FileName"].ToString();
                        rqData.FileType = httpRequest.Params["FileType"].ToString();
                        rqData.FileSize = httpRequest.Params["FileSize"].ToString();
                        rqData.PartReplItemID = httpRequest.Params["PartReplItemID"].ToString();
                        rqData.InSheetType = httpRequest.Params["InSheetType"].ToString();
                        rqData.OptionType = httpRequest.Params["OptionType"].ToString();
                        requestData = JsonConvert.SerializeObject(rqModel);
                        #endregion
                    }

                    Commuser.RequestData = requestData;

                    var authSrv = new AuthService();
                    authSrv.BindRequestData(request.Headers, requestData);
                    jwtToken = authSrv.RqHeader.TokenID;
                    authSrv.CreateAESKey(jwtToken);
                    //authSrv.ValidateJsonWebToken();
                    //authSrv.ValidateSequence();
                    //authSrv.ValidateVID();
                    //authSrv.ValidateDataSignature(string.Empty);

                    #endregion

                    //紀錄Request Raw Data
                    LogHelper.WriteLog(LogLevel.Debug, $"{method} Request: {url} : {requestData} : JWT TOKEN={jwtToken}");
                }
                catch (Exception ex)
                {
                    //紀錄Request Raw Data
                    LogHelper.WriteLog(LogLevel.Debug, $"{method} Request: {url} : {requestData} : JWT TOKEN={jwtToken}");
                    errorResult = ExceptionHelper.Exception(ex);
                    //紀錄Response Raw Data
                    LogHelper.WriteLog(LogLevel.Debug, $"{method} Response: {url} : {JsonConvert.SerializeObject(errorResult)} : JWT TOKEN={jwtToken}");
                    filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.OK, errorResult);
                }
            }
        }

        /// <summary>
        /// Action 執行後啟動
        /// </summary>
        /// <param name="filterContext">HttpAction output內容</param>
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }

    /// <summary>
    /// 是否為檔案上傳
    /// </summary>
    public class IsFileUplaod : Attribute
    {
    }

    /// <summary>
    /// 是否為NTC上傳
    /// </summary>
    public class IsNTCUplaod : Attribute
    {
    }

    /// <summary>
    /// 是否為MainCert上傳
    /// </summary>
    public class IsMaintCertUplaod : Attribute
    {
    }
}