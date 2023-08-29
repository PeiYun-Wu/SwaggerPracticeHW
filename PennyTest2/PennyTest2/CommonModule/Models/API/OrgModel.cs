using System.Collections.Generic;

namespace ELogCMSGateway.Models.Api
{
    /// <summary>
    /// 2.6/2.9/2.13/2.15.1/2.27_取得基本資料
    /// </summary>
    public class CompDeptFunctionInfo
    {
        /// <summary>
        /// GetFunctionList
        /// </summary>
        public CompDeptFunctionInfo()
        {
            Department = new List<DeptInfo>();
        }

        /// <summary>
        /// 公司代碼
        /// </summary>
        public string ACompanyID { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string ADescription { get; set; }

        /// <summary>
        /// 部門列表
        /// </summary>
        public List<DeptInfo> Department { get; set; }
    }

    /// <summary>
    /// 2.6 功能列表
    /// </summary>
    public class DeptInfo
    {
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string BDepartmentID { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        public string BDescription { get; set; }
    }

    /// <summary>
    /// 2.9 getCompany 取得公司別
    /// </summary>
    public class CompInfo
    {
        /// <summary>
        /// 公司代碼
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 2.13 getLicenseAircraftType 取得證照行別代碼
    /// </summary>
    public class LicenseAircraftType
    {
        /// <summary>
        /// 證照飛機型別代碼
        /// </summary>
        public string LicenseAircraftTypeID { get; set; }

        /// <summary>
        /// 證照飛機型別名稱
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 公司別
        /// </summary>
        public string LicenseCompanyID { get; set; }
    }

    /// <summary>
    /// 2.15.1 getFleets 取得機隊
    /// </summary>
    public class GetFleets
    {
        /// <summary>
        /// 公司別
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// 機隊代碼
        /// </summary>
        public string FleetID { get; set; }

        /// <summary>
        /// 證照飛機型別名稱
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 啟用狀態
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// 2.27 getStation取得機場資料
    /// </summary>
    public class GetStation
    {
        /// <summary>
        /// 機場代碼
        /// </summary>
        public string AirportID { get; set; }

        /// <summary>
        /// 機場名稱
        /// </summary>
        public string Description { get; set; }
    }
}