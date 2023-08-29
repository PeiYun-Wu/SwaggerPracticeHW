using CommonModule;
using PennyTest2.DataBase;
using PennyTest2.Helper;
using PennyTest2.Models.Api;
using PennyTest2.Models;
using PennyTest2.Services;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Linq;
using System;

namespace PennyTest2.Controllers
{
    public class QueryController : ApiController
    {
        /// <summary>
        /// 任務查詢
        /// </summary>
        [HttpPost]
        [Route("Query/GetTaskList")]
        public ResponseObj GetTaskList(RequestObj<GetTaskListRes> req)  //let swagger Parameter show GetTaskListRes (model)
        {
            GetTaskListRes data = req.RequestData;
            Commuser.Uid = data.EMPNo;
            //task ub = QueryService.CheckMissionIsExist(data); 
            //if (ub == null)
            //{
            //    throw new ErrInfoException(ResultStatus.NoTaskID); //任務不存在
            //}
            ResponseObj obj = new ResponseObj(QueryService.GetTaskList(req.RequestData));
            return obj;
        }
        /// <summary>
        /// 新增任務
        /// </summary>
        [HttpPost]
        [Route("Query/PostMission")]
        public ResponseObj PostMission(RequestObj<GetAllTaskList> req)
        {
            GetAllTaskList data = req.RequestData;
            Commuser.Uid = data.EMPNo;
            Task ub = QueryService.CheckUserIsExist(data);

            if (ub == null)
            {
                throw new ErrInfoException(ResultStatus.UserAccountError); //使用者不存在
            }

            Task ab = QueryService.CheckReuse(data);

            if (ab != null)
            {
                throw new ErrInfoException(ResultStatus.DoubleuseTaskID);//該任務已有人執行
            }


            //上面都過關 再開始跑
            QueryService.PostMission(req.RequestData);
            return new ResponseObj(ResultStatus.SuccessCode);

        }

        /// <summary>
        /// 取得任務資料excel
        /// </summary>
        [HttpPost]
        [Route("Query/GetTaskReport")]
        public HttpResponseMessage GetTaskReport(RequestObj<GetTaskReportRequest> req)
        {
            HttpResponseMessage response = null;
            response = Request.CreateResponse(HttpStatusCode.OK);

            var dataList = QueryService.GetTaskReport(req.RequestData);
            List<string> titleList = new List<string>() { "TASKID", "MEMO", "STATE", };

            using (MemoryStream ms = ExcelHelper.ExportTaskListExcel("EMPNo_" + req.RequestData.EMPNo, titleList, dataList))
            {
                response.Content = new ByteArrayContent(ms.ToArray());
            }

            InitExcelValue(response, "TaskReport");

            return response;
        }

        /// <summary>
        /// 取得多筆任務資料excel
        /// </summary>
        [HttpPost]
        [Route("Query/GetMissionReport")]
        public HttpResponseMessage GetMissionReport(RequestObj<GetMissionRequest> req)
        {
          
            HttpResponseMessage response = null;
            response = Request.CreateResponse(HttpStatusCode.OK);

            try
            {
                var dataList = QueryService.GetMissionReport(req.RequestData);
                List<string> titleList = new List<string>() { "TASKID", "EMPNO", "NAME", };

                //ms讀寫記憶體 陣列存取字串
                using (MemoryStream ms = ExcelHelper.ExportMissionExcel("多筆任務資料", titleList, dataList))
                {
                    response.Content = new ByteArrayContent(ms.ToArray());
                }

                InitExcelValue(response, "MissionReport");

              
            }
            catch (Exception ex)
            {
                throw new ErrInfoException(PeResultStatus.DbDataParameter);
            }

            return response;
        }


        /// <summary>
        /// 產生excel檔案
        /// </summary>
        /// <param name="response"></param>
        /// <param name="fileName"></param>
        public void InitExcelValue(HttpResponseMessage response, string fileName)
        {
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xlsx"
            };
        }

        /// <summary>
        /// 取得多筆任務資料pdf
        /// </summary>
        /// <param name="req">RequestObj</param>
        /// <returns>ResponseObj</returns>
        [HttpPost]
        [Route("Put/PutMissionPDF")]
        public ResponseObj PutMissionPDF(RequestObj<GetMissionRequest> req)
        {
            ResponseObj obj;
            try
            {
                var dataList = QueryService.GetMissionReport(req.RequestData);
                QueryService.CreateMissionPDF(dataList);

                obj = new ResponseObj(PeResultStatus.SuccessCode);
                return obj;
            }
            catch (Exception ex)
            {
                obj = new ResponseObj(PeResultStatus.DbDataParameter);
                return obj;
            }

           
        }

    }
}