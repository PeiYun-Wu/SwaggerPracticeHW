
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PennyTest2.Models.Api
{
    #region PutCorrectDetail

    public class PutSheetDetailRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string EntrySheetKey { get; set; }
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
        public string EntrySheetType { get; set; }
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

    #endregion

    #region PutEntrySheetAttachRequest

    public class PutEntrySheetAttachRequest
    {
        public string EntrySheetKey { get; set; }
        public string FileID { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string InSheetType { get; set; }
        public string PartReplItemID { get; set; }
        public string FileSize { get; set; }
        public string OptionType { get; set; }
        /// <summary>
        /// FileData
        /// </summary>
        [Required]
        public string FileData { get; set; }

    }
    #endregion

    #region PutEntryStatus
    public class PutEntryStatusRequest
    {
        public string EntrySheetKey { get; set; }
        public decimal EntryStatus { get; set; }
    }

    #endregion

    #region PutNTC
    public class PutNTCRequest
    {
        public string AircraftNo { get; set; }
        public string EONo { get; set; }
        public string NTCNo { get; set; }
        public string StartDateUTC { get; set; }
        public string EndDateUTC { get; set; }
        public string EffPeriod { get; set; }
        public string Subject { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        [Required]
        public string FileData { get; set; }
        public string UploadDateTime { get; set; }
        public string OptionType { get; set; }

    }

    #endregion

    #region PutNTC
    public class PutMainCertRequest
    {
        public string AircraftNo { get; set; }
        public string CertKey { get; set; }
        public string CertDate { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string UploadDateTime { get; set; }
        public string OptionType { get; set; }
        /// <summary>
        /// FileData
        /// </summary>
        [Required]
        public string FileData { get; set; }

    }
    #endregion

    #region sheetJson

    public class SheetData
    {
        public string entrySheetID { get; set; }
        public string entrySheetType { get; set; }
        public string entryStatus { get; set; }
        public string aircraftNo { get; set; }
        public string formType { get; set; }
        public string formKey { get; set; }
        public string formId { get; set; }
        public string formSerial { get; set; }
        public List<EntrySheetFile> entrySheetFile { get; set; }
        public SheetJson sheetJson { get; set; }
    }

    public class SheetJson
    {
        public SupddInfo supDDInfo { get; set; }
        public LogOriginal logOriginal { get; set; }
        public LogOriginal logCorrection { get; set; }
        public DdOriginal ddOriginal { get; set; }
        public DdOriginal ddCorrection { get; set; }
        public FltOriginal fltOriginal { get; set; }
        public FltOriginal fltCorrection { get; set; }
        public ArOriginal arOriginal { get; set; }
        public ArOriginal arCorrection { get; set; }
    }

    public class SupddInfo
    {
        public string supDDStation { get; set; }
        public string supDDBy { get; set; }
        public string supDDDate { get; set; }
        public string supDDByLic { get; set; }
        public string supDDId { get; set; }
        public string supDDSerial { get; set; }
    }

    public class LogOriginal
    {
        public string findingKey { get; set; }
        public string releaseTime { get; set; }
        public string flightNo { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string findingContent { get; set; }
        public string frmCode { get; set; }
        public string findingSigner { get; set; }
        public List<FindingImage> findingImage { get; set; }
        public string findingFrom { get; set; }
        public decimal? maintType { get; set; }
        public string previousMDDKey { get; set; }
        public string originalFindingKey { get; set; }
        public string originalFindingDate { get; set; }
        public string ataChpter { get; set; }
        public string workorder { get; set; }
        public string man { get; set; }
        public string hour { get; set; }
        public List<Action> action { get; set; }
        public List<Component> component { get; set; }
        public List<Pme> pme { get; set; }
        public string apuKey { get; set; }
        public string engOilKey { get; set; }
        public string hydKey { get; set; }
        public string apuHour { get; set; }
        public string apuCyc { get; set; }
        public string apuArrival { get; set; }
        public string apuAdd { get; set; }
        public string engCount { get; set; }
        public List<EngOil> engOil { get; set; }
        public string hyd1 { get; set; }
        public string hyd2 { get; set; }
        public string hyd3 { get; set; }
        public string hyd4 { get; set; }
        public string relSigner { get; set; }
        public string relDate { get; set; }
        public string relByLic { get; set; }
        public string findingDate { get; set; }
    }

    public class FindingImage
    {
        public string option { get; set; }
        public string fileID { get; set; }
        public string filePath { get; set; }
    }

    public class Action
    {
        public string actionKey { get; set; }
        public string actionContent { get; set; }
        public string actionSigner { get; set; }
        public string actionSignTime { get; set; }
        public string actionSignLic { get; set; }
        public string engSigner { get; set; }
        public string engSignTime { get; set; }
        public string engSignLic { get; set; }
        public string ndtSigner { get; set; }
        public string ndtSignTime { get; set; }
        public string ndtSignLic { get; set; }
        public string bsiSigner { get; set; }
        public string bsiSignTime { get; set; }
        public string bsiSignLic { get; set; }
        public string riiSigner { get; set; }
        public string riiSignLic { get; set; }
        public string riiSignTime { get; set; }
    }

    public class Component
    {
        public string componentKey { get; set; }
        public string lable { get; set; }
        public string onPN { get; set; }
        public string onSN { get; set; }
        public decimal? onQTY { get; set; }
        public string onPOS { get; set; }
        public string onExpiry { get; set; }
        public string offPN { get; set; }
        public string offSN { get; set; }
        public decimal? offQTY { get; set; }
        public string offPOS { get; set; }
        public string offExpiry { get; set; }
        public string software { get; set; }
        public string creator { get; set; }
        public List<ComponentImage> componentImage { get; set; }
    }

    public class ComponentImage
    {
        public string option { get; set; }
        public string fileID { get; set; }
        public string filePath { get; set; }
    }

    public class Pme
    {
        public string pmeKey { get; set; }
        public string toolNo { get; set; }
        public string idNo { get; set; }
        public string creator { get; set; }
    }

    public class LogCorrection
    {
        public string findingKey { get; set; }
        public string releaseTime { get; set; }
        public string flightNo { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string findingContent { get; set; }
        public string frmCode { get; set; }
        public string findingSigner { get; set; }
        public List<FindingImage> findingImage { get; set; }
        public string ataChpter { get; set; }
        public string workorder { get; set; }
        public string man { get; set; }
        public string hour { get; set; }
        public List<Action> action { get; set; }
        public List<Component> component { get; set; }
        public List<Pme> pme { get; set; }
        public string apuHour { get; set; }
        public string apuCyc { get; set; }
        public string apuArrival { get; set; }
        public string apuAdd { get; set; }
        public string engCount { get; set; }
        public List<EngOil> engOil { get; set; }
        public string hyd1 { get; set; }
        public string hyd2 { get; set; }
        public string hyd3 { get; set; }
        public string hyd4 { get; set; }
    }

    public class DdOriginal
    {
        public string ddKey { get; set; }
        public string findingKey { get; set; }
        public string originalFindingKey { get; set; }
        public string originalFindingDate { get; set; }
        public string ddType { get; set; }
        public string ddTypeNo { get; set; }
        public string ddOther { get; set; }
        public string category { get; set; }
        public string plaCardNo { get; set; }
        public string reptInterval { get; set; }
        public string reptOther { get; set; }
        public string dueAfterHour { get; set; }
        public string dueAfterCyc { get; set; }
        public string dueAtDay { get; set; }
        public string dueAtTAC { get; set; }
        public string dueAtTAH { get; set; }
        public string dueAtNote { get; set; }
        public string description { get; set; }
    }

    public class DdCorrection
    {
        public string category { get; set; }
        public string description { get; set; }
        public string ddKey { get; set; }
        public string ddType { get; set; }
        public string ddTypeNo { get; set; }
        public string ddOther { get; set; }
        public string plaCardNo { get; set; }
        public string reptInterval { get; set; }
        public string reptOther { get; set; }
        public string dueAfterHour { get; set; }
        public string dueAfterCyc { get; set; }
        public string dueAtDay { get; set; }
        public string dueAtTAC { get; set; }
        public string dueAtTAH { get; set; }
        public string dueAtNote { get; set; }
    }

    public class FltOriginal
    {
        public string fltLogKey { get; set; }
        public string flightKey { get; set; }
        public string flightNo { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string blockOut { get; set; }
        public string takeOff { get; set; }
        public string landing { get; set; }
        public string blockin { get; set; }
        public string ldgCyc { get; set; }
        public string localTraining { get; set; }
        public List<Crew> crew { get; set; }
        public string autolandRecordID { get; set; }
        public string autolandPerform { get; set; }
        public string autolandResult { get; set; }
        public string autolandRemark { get; set; }
        public string autolandNote { get; set; }
        public string apuPerform { get; set; }
        public string apuResult { get; set; }
        public string apuTime { get; set; }
        public string apuTOCTime { get; set; }
        public decimal? apuFlightLevel { get; set; }
        public string fullthrst { get; set; }
        public List<DeIce> deIce { get; set; }
        public string fixedUser { get; set; }
        public string fixedDate { get; set; }
    }

    public class EngOil
    {
        /// <summary>
        /// 
        /// </summary>
        public string EngID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EngNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EngArrival { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EngAdd { get; set; }
    }

    public class Crew
    {
        public decimal? rank { get; set; }
        public decimal? takeoffPos { get; set; }
        public decimal? takeoffRole { get; set; }
        public decimal? takeoffDivide { get; set; }
        public decimal? landingPos { get; set; }
        public decimal? landingRole { get; set; }
        public decimal? landingDivide { get; set; }
        public string ad { get; set; }
        public string memberName { get; set; }
        public string liceneceNum { get; set; }
    }

    public class DeIce
    {
        public string deicerecordID { get; set; }
        public string wheatherCond { get; set; }
        public string oat { get; set; }
        public string deiceType { get; set; }
        public string companyName { get; set; }
        public string fluidName { get; set; }
        public string dilution { get; set; }
        public string deIceMethod { get; set; }
        public string beginningTime { get; set; }
        public string holdoverTime { get; set; }
        public string multiText { get; set; }
    }

    public class FltCorrection
    {
        public string fltLogKey { get; set; }
        public string flightKey { get; set; }
        public string flightNo { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string _out { get; set; }
        public string off { get; set; }
        public string on { get; set; }
        public string _in { get; set; }
        public string ldgCyc { get; set; }
        public string localTraining { get; set; }
        public List<Crew> crew { get; set; }
        public string autolandRecordID { get; set; }
        public string autolandPerform { get; set; }
        public string autolandResult { get; set; }
        public string autolandRemark { get; set; }
        public string autolandNote { get; set; }
        public string apuPerform { get; set; }
        public string apuResult { get; set; }
        public string apuTime { get; set; }
        public string apuTOCTime { get; set; }
        public string apuFlightLevel { get; set; }
        public string fullthrst { get; set; }
        public List<DeIce> deIce { get; set; }
    }

    public class ArOriginal
    {
        public string arKey { get; set; }
        public string arSigner { get; set; }
        public string arSigntime { get; set; }
        public string arSignlic { get; set; }
        public string certAGT { get; set; }
        public string station { get; set; }
        public string etopsStatus { get; set; }
        public string caaStation { get; set; }
        public string picSigner { get; set; }
        public string picSignTime { get; set; }
        public string picSignLic { get; set; }
    }

    public class ArCorrection
    {
        public string arKey { get; set; }
        public string arSigner { get; set; }
        public string arSigntime { get; set; }
        public string arSignlic { get; set; }
        public string certAGT { get; set; }
        public string station { get; set; }
        public string etopsStatus { get; set; }
        public string caaStation { get; set; }
        public string picSigner { get; set; }
        public string picSignTime { get; set; }
        public string picSignLic { get; set; }
    }
    #endregion

    #region FLPDF
    /// <summary>
    /// 
    /// </summary>
    public class FlightCrewListModel
    {
        public string Rank { get; set; }
        public string NAME { get; set; }
        public string LicenceNo { get; set; }
        public string POS { get; set; }
        public string Role { get; set; }
        public string LandingPOS { get; set; }
        public string LandingRole { get; set; }
        public decimal TakeOffDivide { get; set; }

    }

    public class FlightLogQuery
    {
        public string CompanyID { get; set; }
        public string FleetID { get; set; }
        public string AircraftNO { get; set; }
        public string EntryDateTimeUTC { get; set; }
        public string FlightNO { get; set; }
        public string FromStation { get; set; }
        public string TOStation { get; set; }
        public string BlockOut { get; set; }
        public string TakeOff { get; set; }
        public string Landing { get; set; }
        public string BlockIn { get; set; }
        public string PICSignature { get; set; }
        public string PICLicenseNo { get; set; }
        public string FormNo { get; set; }
        public List<FlightCrewListModel> CrewList { get; set; }
    }
    #endregion

    /// <summary>
    /// PDF飛機資訊
    /// </summary>
    public class FormAircraftInfo
    {
        public string CompanyID { get; set; }
        public string FleetID { get; set; }
        public string LicenseAircraftTypeID { get; set; }

    }
}