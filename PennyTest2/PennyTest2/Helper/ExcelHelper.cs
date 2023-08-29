using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using PennyTest2.Models.Api;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace PennyTest2.Helper
{
    /// <summary>
    /// ExcelHelper
    /// </summary>
    public class ExcelHelper
    {
        public static MemoryStream ExportTaskListExcel(string reportName, List<string> titleList, List<GetTaskReportList> dataList)
        {
            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                // 新增worksheet
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(reportName);
                DataTable dt = ToDataTable(dataList);
                dt.TableName = reportName;

                #region 各報表客製樣式
                ws.Cells[1, 1, 1, 3].Merge = true;
                ws.Cells["A1"].Value = reportName;
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A2"].Value = $"Print Time：{DateFormateHelper.ToyyyyMMddString(DateTime.Now)}";
                #endregion

                // 將DataTable資料塞到sheet中
                for (int i = 0; i < titleList.Count; i++)
                {
                    ws.Cells[3, i + 1].Value = titleList[i].ToString();
                }
                ws.Cells["A4"].LoadFromDataTable(dt, false);



                ws.Cells.AutoFitColumns();

                package.SaveAs(stream);
                stream.Position = 0;


                using (FileStream createStream = new FileStream(@"C:\output.xlsx", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    package.SaveAs(createStream);//存檔
                }
            }

            return stream;
        }

        public static MemoryStream ExportMissionExcel(string reportName, List<string> titleList, List<GetMissionQuery> dataList)
        {
            var stream = new MemoryStream();

            List<GetMissionQuery> MList = new List<GetMissionQuery>();
            var listStr = new List<string>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                // 新增worksheet
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(reportName);
                DataTable dt = ToDataTable(dataList);
                dt.TableName = reportName;

                #region 各報表客製樣式
                ws.Cells[1, 1, 1, 3].Merge = true;
                ws.Cells["A1"].Value = reportName;
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A2"].Value = $"Print Time：{DateFormateHelper.ToyyyyMMddString(DateTime.Now)}";
                #endregion

                // 將DataTable資料塞到sheet中
                for (int i = 0; i < titleList.Count; i++)
                {
                    ws.Cells[3, i + 1].Value = titleList[i].ToString();
                }
                ws.Cells["A4"].LoadFromDataTable(dt, false);

                //存取輸入的TaskID.GroupBy後的數量
                var countList = dataList.GroupBy(x => x.TASKId).Select(x => x.Count()).ToList();

                //行數
                int rowIndex = 4;

                foreach (var count in countList) 
                {
                    ws.Cells[rowIndex, 1, rowIndex + count-1, 1].Merge = true;
                    rowIndex += count; //新的一行開始算 
                }
                ws.Cells.AutoFitColumns();
                package.SaveAs(stream);
                stream.Position = 0;
                //讀寫檔案 繼承stream
                using (FileStream createStream = new FileStream(@"C:\Mergeoutput.xlsx", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    package.SaveAs(createStream);//存檔
                }
            }

            return stream;
        }


        /// <summary>
        /// List To DataTable
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="items">資料項目</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}