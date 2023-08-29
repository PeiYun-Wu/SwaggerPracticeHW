using CommonModule;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PennyTest2.DataBase;
using PennyTest2.Helper;
using PennyTest2.Helpers;
using PennyTest2.Models.Api;
using PennyTest2.PdfHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PennyTest2.Services
{
    public class QueryService
    {
        public static List<GetAllTaskList> GetTaskList(GetTaskListRes data) //使用GetTaskListRes的屬性
        {

            List<GetAllTaskList> TaskList = new List<GetAllTaskList>(); //建構(model)GetAllTaskList函數
            //當回傳值,所以swagger的responseBody會有GetAllTaskList欄位
            var sqlParameters = new List<SqlParameter>(); //建立參數list

            var query = "select * from task A where 1=1 ";  //因為taskId是pk不會有空值,不需要if
                                                            //sqlParameters.Add(new SqlParameter("TASKID", data.TASKId));
            if (!string.IsNullOrWhiteSpace(data.TASKId))
            {
                query += @" AND A.TASKID=@TASKID "; //動態查詢@
                sqlParameters.Add(new SqlParameter("TASKID", data.TASKId));
            }

            if (!string.IsNullOrWhiteSpace(data.State))
            {
                query += @" AND A.STATE=@STATE"; //動態查詢@
                sqlParameters.Add(new SqlParameter("STATE", data.State));
            }
            if (!string.IsNullOrWhiteSpace(data.EMPNo))
            {
                query += @" AND A.EMPNO=@EMPNO";
                sqlParameters.Add(new SqlParameter("EMPNO", data.EMPNo));
            }//設定三個讓使用者選的類別


            using (var elSrv = new PennyTest_Entities())//連線
            {
                TaskList = elSrv.Database.SqlQuery<GetAllTaskList>(query, sqlParameters.ToArray()).ToList();//放入要回傳的

            }
            return TaskList; //回傳他,responseBody帶入GetAllTaskList
        }

        public static void PostMission(GetAllTaskList req)
        {
            using (var ecSrv = new PennyTest_Entities())
            {

                var query = new Task();
                query.TASKID = req.TASKId;
                query.EMPNO = req.EMPNo;
                query.MEMO = req.MEMO;
                query.STATE = req.State;

                ecSrv.task.Add(query);
                ecSrv.SaveChanges();
            }
        }

        public static Task CheckUserIsExist(GetAllTaskList data) //TASK裡面的EMPNO是否存在
        {
            var query = new Task();
            using (var ecSrv = new PennyTest_Entities())
            {
                query = ecSrv.task.Where(X => X.EMPNO == data.EMPNo).FirstOrDefault();
            }
            return query;
        }

        public static Task CheckMissionIsExist(GetTaskListRes data)//查看TaskID是否存在
        {
            var query = new Task();
            using (var ecSrv = new PennyTest_Entities())
            {
                query = ecSrv.task.Where(X => X.TASKID == data.TASKId).FirstOrDefault();
            }
            return query;
        }

        public static Task CheckReuse(GetAllTaskList data)//TASK裡面的TASKID是否有人在執行
        {
            var query = new Task();
            using (var ecSrv = new PennyTest_Entities())
            {
                query = ecSrv.task.Where(X => X.TASKID == data.TASKId).FirstOrDefault();
            }
            return query;
        }

        /// <summary>
        /// 取得Task單筆資料 excel報表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static List<GetTaskReportList> GetTaskReport(GetTaskReportRequest data)
        {
            List<GetTaskReportList> result = new List<GetTaskReportList>();
            var sqlParameter = new List<SqlParameter>();


            using (var peSrv = new PennyTest_Entities())
            {
                string command = $@" SELECT
                       TASKID, MEMO, STATE
                       FROM TASK t
                       WHERE t.EMPNO IS NOT NULL ";
                if (!string.IsNullOrWhiteSpace(data.EMPNo))
                {
                    command += @" AND t.EMPNO = @EMPNo ";
                    sqlParameter.Add(new SqlParameter("@EMPNo", data.EMPNo));
                }
                command += @"order by t.TASKID ";

                result = peSrv.Database.SqlQuery<GetTaskReportList>(command, sqlParameter.ToArray()).ToList();

            }
            return result;

        }

        /// <summary>
        /// 取得mission excel報表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static List<GetMissionQuery> GetMissionReport(GetMissionRequest data) //pdf與excel共享資料
        {
            List<GetMissionQuery> result = new List<GetMissionQuery>();
            List<SqlParameter> sqlParameter = new List<SqlParameter>(); //拼接,防串改

            string listStr = string.Concat("'", string.Join("','", data.TASKLIST), "'");

            using (var peSrv = new PennyTest_Entities())
            {
                string command = $@" select T.TASKID,T.EMPNO,E.NAME 
                        from task T 
                        LEFT JOIN EMPLOYEE E ON E.EMPNO = T.EMPNO
                       WHERE t.EMPNO IS NOT NULL AND t.TASKID in ({listStr})";
                command += @" order by t.TASKID";

                result = peSrv.Database.SqlQuery<GetMissionQuery>(command, sqlParameter.ToArray()).ToList();
            }
            return result;
        }


        #region MissionPDF
        /// <summary>
        /// 建立FLPDF
        /// </summary>
        /// <param name="elSrv"></param>
        /// <param name="data"></param>
        public static void CreateMissionPDF(List<GetMissionQuery> dataListM)
        {
            using (var elSrv = new PennyTest_Entities())
            {

                var domain = $"{CommUtility.GetConfig("PutPDF")}\\FL";
                var outputPath = Path.Combine($"{domain}\\", $"{DateFormateHelper.ToyyyyMMddString(DateTime.Now)}.pdf");
                var fileNo = "";
                fileNo = GetPDFSerialNo(domain, outputPath, fileNo);
                outputPath = Path.Combine($"{domain}\\", $"{fileNo}.pdf");


                LogHelper.WriteLog(LogLevel.Debug, $"outputPath :{outputPath}");
                if (!File.Exists(outputPath))
                {
                    LogHelper.WriteLog(LogLevel.Debug, $"outputPath safe");
                    LogHelper.WriteLog(LogLevel.Debug, $"MissionLIst :{dataListM}");

                    #region 資料來源

                    if (dataListM != null)
                    {
                        LogHelper.WriteLog(LogLevel.Debug, $"Crew safe");

                        #endregion

                        var stream = new MemoryStream();
                        var doc = new Document(PageSize.A4.Rotate());
                        var writer = PdfWriter.GetInstance(doc, stream);

                        #region 頁首、頁尾
                        writer.PageEvent = new ITextFooterEvents("Penny");

                        doc.Open();

                        var bfCh = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\msjh.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                        var fontTitle = new Font(bfCh, 24);
                        doc.Add(new Paragraph("TaskPDF", fontTitle)
                        {
                            Alignment = Element.ALIGN_CENTER
                        });

                        #endregion

                        #region 基本 Date設定
                        PdfPTable table1 = new PdfPTable(new float[] { 3, 7 })
                        {
                            //表格總寬
                            WidthPercentage = 35.7f,
                            HorizontalAlignment = Element.ALIGN_LEFT
                        };
                        PdfPCell cellTitle = new PdfPCell(new Phrase("CreateTime"));
                        cellTitle.BackgroundColor = new BaseColor(231, 230, 230);
                        cellTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellTitle.VerticalAlignment = Element.ALIGN_MIDDLE;

                        table1.AddCell(cellTitle);
                        PdfPCell cellValue = new PdfPCell(new Phrase(DateFormateHelper.ToyyyyMMddString(DateTime.Now)));
                        table1.AddCell(cellValue);
                        #endregion
                        #region 動態資料欄位


                        var table2 = new PdfPTable(new float[] { 3, 3, 3 })
                        {
                            //表格總寬
                            WidthPercentage = 100,
                            HorizontalAlignment = Element.ALIGN_LEFT
                        };
                        cellTitle.Phrase = new Phrase("TaskID");
                        table2.AddCell(cellTitle);
                        cellTitle.Phrase = new Phrase("EMPNo");
                        table2.AddCell(cellTitle);
                        cellTitle.Phrase = new Phrase("NAME");
                        table2.AddCell(cellTitle);

                        //for taskid
                        PdfPCell cellTaskID = new PdfPCell();

                        var table3 = new PdfPTable(new float[] { 3, 3, 3 })
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = Element.ALIGN_LEFT
                        };

                        var countList = dataListM.GroupBy(x => x.TASKId).Select(x => x.Count()).ToList(); //取比數
                        var distinctTaskid = dataListM.Select(x => x.TASKId).Distinct().ToList(); //取1/10

                        int Mergeindex = 0;

                        //var singleData = dataListM.Count();

                        for (int x = 0; x < countList.Count(); x++)//taskid會有兩筆
                        {
                            cellTaskID.Phrase = new Phrase(distinctTaskid[x]);
                            cellTaskID.Rowspan = countList[Mergeindex]; //超出範圍
                            Mergeindex++;
                            table3.AddCell(cellTaskID);

                            foreach (var item in dataListM)
                            {
                                if (item.TASKId == distinctTaskid[x])
                                {
                                    cellTaskID.Rowspan = 1;
                                    cellTaskID.Phrase = new Phrase(item.EMPNo);
                                    table3.AddCell(cellTaskID);
                                    cellTaskID.Phrase = new Phrase(item.NAME);
                                    table3.AddCell(cellTaskID);
                                }
                            }
                        }
                        #endregion
                        doc.Add(table1);
                        doc.Add(table2);
                        doc.Add(table3);
                     
                        doc.Close();
                        writer.Close();
                        CreatePDFFile(stream, domain, outputPath);
                        elSrv.SaveChanges();
                    }
                }
            }
        }
     
        #endregion

        #region 處理檔名後面的流水號
        /// <summary>
        /// 處理檔名後面的流水號
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="outputPath"></param>
        /// <param name="dataID"></param>
        /// <param name="dataNo"></param>
        /// <param name="fileNo"></param>
        /// <returns></returns>
        private static string GetPDFSerialNo(string domain, string outputPath, string fileNo)
        {
            if (File.Exists(outputPath))
            {
                bool fileFlag = true;
                int index = 1;
                do
                {
                    outputPath = Path.Combine($"{DateFormateHelper.ToyyyyMMddString(DateTime.Now)}_{index}.pdf");
                    if (!File.Exists(outputPath))
                    {
                        fileFlag = false;
                        fileNo = $"_{index}";
                    }
                    index++;
                } while (fileFlag);
            }
            return fileNo;
        }
        #endregion


        /// <summary>
        /// 產生PDF
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="Domain"></param>
        /// <param name="outputPath"></param>
        private static void CreatePDFFile(MemoryStream stream, string domain, string outputPath)
        {

            try
            {
                //建目錄
                var exists = Directory.Exists(domain);
                if (!exists)
                {
                    Directory.CreateDirectory(domain);
                }
            }
            catch { }

            //檔案上傳
            File.WriteAllBytes(outputPath, stream.ToArray());

        }

    }
}



