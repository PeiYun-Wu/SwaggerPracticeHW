using System.Collections.Generic;

namespace PennyTest2.Models.Api
{
    public class GetAllTaskList
    {
        public string EMPNo { get; set; }
        public string TASKId { get;set; }

        public string MEMO { get; set; }
        public string State { get; set; }
    }

    public class GetTaskListRes
    {
        public string EMPNo { get; set; }
        public string TASKId { get; set; }
      
        public string State { get; set; }

      
    }

    #region Taskexcel單筆資料
    public class GetTaskReportRequest //input condition
    {
        public string EMPNo { get; set; }
       
    }
    public class GetTaskReportList //empno output
    {
      
        public string TASKId { get; set; }
        public string MEMO { get; set; }
        public string State { get; set; }
    }

    #endregion

    #region mission excel+pdf 多筆資料
    public class GetMissionRequest //input condition
    {
        public List<string> TASKLIST { get; set; }
    }

    public class GetMissionQuery //empno output
    {
        public string TASKId { get; set; }
        public string EMPNo { get; set; }
        public string NAME { get; set; }
       

    }

    #endregion
   



}