using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PennyTest2.Models.Api
{
    #region GetLogList/GetFltLogList
    /// <summary>
    /// 2.7 Request GetLogList
    /// </summary>
    public class GetLogListRequest
    {
        /// <summary>
        /// 公司別代碼
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// 飛機機型
        /// </summary>
        public string AircraftType { get; set; }

        /// <summary>
        /// 飛機機號
        /// </summary>
        public string AircraftNo { get; set; }

        /// <summary>
        /// 查詢起日
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 查詢迄日
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 起飛站
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 降落站
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 角色類型
        /// </summary>
        public string LogType { get; set; }
    }

    public class GetLogListResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string FormType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AircraftNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormSerial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalFindingKey { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string OriginalFindingDate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string FindingContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PdfUrl { get; set; }
    }

    public class GetFltLogListRequest
    {
        /// <summary>
        /// 公司別代碼
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// 角色群組代碼
        /// </summary>
        public string AircraftType { get; set; }

        /// <summary>
        /// 員工編號
        /// </summary>
        public string AircraftNo { get; set; }

        /// <summary>
        /// 角色群組編號
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 角色群組編號
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 角色群組編號
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 角色群組編號
        /// </summary>
        public string CrewID { get; set; }

        /// <summary>
        /// 角色群組編號
        /// </summary>
        public string LicNo { get; set; }
    }

    public class GetFltLogListResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string FromType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AircraftNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormSerial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FlightNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReleaseSigner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SignerLic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PdfUrl { get; set; }
    }

    #endregion

    #region GetEntrySheetList
    public class GetEntrySheetListRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string EntryType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AircraftNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EntryStatus { get; set; }
    }

    public class GetEntrySheetListResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string EntrySheetKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EntrySheetID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EntryStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AircraftNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormSerial { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FindingContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReleaseDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateDate { get; set; }
    }
    #endregion

    #region GetCorrectDetail
    public class GetSheetDetailRequest
    {
        /// <summary>
        /// ES+裝置ID+時戳
        /// </summary>
        public string EntrySheetKey { get; set; }
    }

    public class GetSheetDetailResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string FormType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormSerial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EntrySheetID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? EntrySheetType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EntryStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AircraftNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<EntrySheetFile> EntrySheetFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SheetJson { get; set; }
    }

    public class EntrySheetFile
    {
        /// <summary>
        /// 
        /// </summary>
        public string fileType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PartReplItemID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fileID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fileSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inSheetType { get; set; }

    }

    #endregion

    #region GetOriginalData
    public class GetOriginalDataRequest
    {
        /// <summary>
        /// 表單類型
        /// </summary>
        public string FormType { get; set; }

        /// <summary>
        /// 表單Key值
        /// </summary>
        public string FormKey { get; set; }
    }

    public class GetOriginalDataResponse
    {
        /// <summary>
        /// 表單類型
        /// </summary>
        public string FormType { get; set; }

        /// <summary>
        /// 表單Key值
        /// </summary>
        public string FormKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FormSerial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LogOriginal LogOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DdOriginal DdOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FltOriginal FltOrignal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ArOriginal ArOriginal { get; set; }
    }

    #endregion

    #region GetMaintItemReport
    public class GetMaintItemReportRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LogType { get; set; }
        public string MainType { get; set; }
        public string Ata { get; set; }
    }

    public class GetMainItemReportQuery
    {
        public string EntryDateTimeUTC { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string STNFROM { get; set; }
        public string STNTO { get; set; }
        public string ATACHAPTER { get; set; }
        public string WORKORDER { get; set; }
        public string FINDING { get; set; }
        public string ACTION { get; set; }
        public string PNOn { get; set; }
        public string SNOn { get; set; }
        public decimal? QTYOn { get; set; }
        public string POSOn { get; set; }
        public string PNOff { get; set; }
        public string SNOff { get; set; }
        public decimal? QTYOff { get; set; }
        public string POSOff { get; set; }
        public string Software { get; set; }
    }
    public class GetCabinItemReportQuery
    {
        public string EntryDateTimeUTC { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string STNFROM { get; set; }
        public string STNTO { get; set; }
        public string ATACHAPTER { get; set; }
        public string WORKORDER { get; set; }
        public string FINDING { get; set; }
        public string ACTION { get; set; }
        public string ITEMTYPE { get; set; }
        public string TYPE1 { get; set; }
        public string TYPE2 { get; set; }
        public string PNOn { get; set; }
        public string SNOn { get; set; }
        public decimal? QTYOn { get; set; }
        public string POSOn { get; set; }
        public string PNOff { get; set; }
        public string SNOff { get; set; }
        public decimal? QTYOff { get; set; }
        public string POSOff { get; set; }
        public string Software { get; set; }
    }
    #endregion

    #region GetDDItemReport
    public class GetDDItemReportRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class GetDDItemReportQuery
    {
        public string Status { get; set; }
        public string STNDeparture { get; set; }
        public string AircraftNO { get; set; }
        public string DDTypeNO { get; set; }
        public string EntryDateTimeUTC { get; set; }
        public string FromMNT { get; set; }
        public string Contents { get; set; }
        public string DueAtDay { get; set; }
        public string BackMNT { get; set; }
        public string WorkorderNo { get; set; }
    }
    #endregion

    #region GetCISReport
    public class GetCISReportRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string UploadStartDate { get; set; }
        public string UploadEndDate { get; set; }
        public string FlightNo { get; set; }
        public string STNDeparture { get; set; }
        public string STNArrival { get; set; }
        public string FlightStartDate { get; set; }
        public string FlightEndDate { get; set; }
        public string Airline { get; set; }
        public string ItemType { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
    }

    public class GetCISReportQuery
    {
        public string EntryDateTimeUTC { get; set; }
        public string ACTDate { get; set; }
        public string FISAircraftType { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string STNDeparture { get; set; }
        public string STNArrival { get; set; }
        public string WAL_AIRLINE { get; set; }
        public string ItemType { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        public string Description { get; set; }
        public string CorrectiveAction { get; set; }
        public string FlightKey { get; set; }
    }
    #endregion

    #region GetAutolandReport
    public class GetAutolandReportRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class GetAutolandReportQuery
    {
        public string AircraftType { get; set; }
        public string EntryDateTimeUTC { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string AutolandResult { get; set; }
        public string Note { get; set; }
    }
    #endregion

    #region GetAPUReport
    public class GetAPUReportRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class GetAPUReportQuery
    {
        public string AircraftType { get; set; }
        public string EntryDateTimeUTC { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string APUResult { get; set; }
        public string APUTOCTime { get; set; }
        public string APUTime { get; set; }
        public decimal? APUFlightLevel { get; set; }
    }
    #endregion

    #region GetFullThrustReport
    public class GetFullThrustReportRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string Year { get; set; }
        public string Quarterly { get; set; }
    }

    public class GetFullThrustReportQuery
    {
        public string AircraftType { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string EntryDateTimeUTC { get; set; }
    }
    #endregion

    #region GetMaintData
    public class GetMaintDataRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string MaintType { get; set; }
        public string Pn { get; set; }
        public string Sn { get; set; }
        public string Software { get; set; }
    }

    public class GetMaintDataQuery
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string STNDeparture { get; set; }
        public string STNArrival { get; set; }
        public decimal? MaintType { get; set; }
        public string FindingContent { get; set; }
        public string ATAChapterNo { get; set; }
        public string ActionContent { get; set; }
        public string RIISigner { get; set; }
        public string BSISigner { get; set; }
        public string NDTSigner { get; set; }
        public string ENGSigner { get; set; }
        public string ReleaseSigner { get; set; }
        public string PNOn { get; set; }
        public string SNOn { get; set; }
        public decimal? QTYOn { get; set; }
        public string POSOn { get; set; }
        public string LLPDateOn { get; set; }
        public string PNOff { get; set; }
        public string SNOff { get; set; }
        public decimal? QTYOff { get; set; }
        public string POSOff { get; set; }
        public string LLPDateOff { get; set; }
        public string ToolNo { get; set; }
        public string IDNo { get; set; }
        public string Creator { get; set; }
    }
    #endregion

    #region GetFlightData
    public class GetFlightDataRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
    }

    public class GetFlightDataQuery
    {
        public string AircraftType { get; set; }
        public string AircraftNO { get; set; }
        public string FlightNo { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string TakeDate { get; set; }
        public string TakeTime { get; set; }
        public string LandingDate { get; set; }
        public string LandingTime { get; set; }
        public string DuringHRS { get; set; }
        public decimal Ldgcyc { get; set; }
        public string Tah { get; set; }
        public string Tac { get; set; }
    }
    #endregion

    #region GetOilReport
    public class GetOilReportRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class GetOilReportQuery
    {
        public string DateTime { get; set; }
        public string FlightNo { get; set; }
        public string AircraftNO { get; set; }
        public string STNDeparture { get; set; }
        public string STNArrival { get; set; }
        public string E1Oil { get; set; }
        public string E2Oil { get; set; }
        public string APUOil { get; set; }
        public string HYD1 { get; set; }
        public string HYD2 { get; set; }
        public string HYD3 { get; set; }
        public string APUHours { get; set; }
        public string APUCycles { get; set; }
    }
    #endregion

    #region GetNTCList
    public class GetNTCListRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EONo { get; set; }
        public string NTCNo { get; set; }
    }

    public class GetNTCListQuery
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string EONo { get; set; }
        public string NTCNo { get; set; }
        public string EffPeriod { get; set; }
        public string Read { get; set; }
        public string Subject { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string UploadDateTime { get; set; }
        public string StartDateUTC { get; set; }
        public string EndDateUTC { get; set; }


    }
    #endregion

    #region GetMaintCertList
    public class GetMaintCertListRequest
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Type { get; set; }
    }

    public class GetMaintCertListQuery
    {
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftNo { get; set; }
        public string Type { get; set; }
        public string CertDate { get; set; }
        public string FileName { get; set; }
        public string UploadDateTime { get; set; }
        public string FileSize { get; set; }
        public string CertKey { get; set; }
    }
    #endregion

}