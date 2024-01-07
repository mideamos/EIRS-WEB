using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public partial class GISFileAssessmentItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long AssessmentID { get; set; }
        public string FileNumber { get; set; }
        public string PageNo { get; set; }
        public string AssetNumber { get; set; }
        public string AssessmentAmount { get; set; }

        public DateTime DateSaved { get; set; }
        [NotMapped]
        public string AssessmentYear { get; set; } 
        [NotMapped]
        public decimal DecimalAssessmentAmount { get; set; }
    }

    public class ResultModel
    {
        public string StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public string FilesCount { get; set; }
        public string FilesTotalCount { get; set; }
        public List<File> File { get; set; }
    }
    public class ReturnObject
    {
        public string OwnerId { get; set; }
        public string OwnerFullName{ get; set; }
        public string OwnerPhoneNumber{ get; set; }
        public string AssessmentAmount{ get; set; }
        public string FileNumber{ get; set; }
        public string PageNo { get; internal set; }

        public decimal AssessmentAmountII { get; set; }
    }
    public class File
    {
        public string FileNumber { get; set; }
        public string CreationDate { get; set; }
        public string ModifiedDate { get; set; }
        public List<GISFilePartyR> Party { get; set; }
        public List<GISFileAssetR> Asset { get; set; }
        public List<GISFileAssessmentR> Assessment { get; set; }
        public List<GISFileInvoiceR> Invoice { get; set; }

    }
    public partial class GISFileInvoiceR
    {
       
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public string isReversal { get; set; }
        public string InvoiceID { get; set; }

        public List<GISFileInvoiceItemE> Items { get; set; }
    }

    public partial class GISFileInvoiceItemE
    {
     
        public long InvoiceID { get; set; }
        public string RevenueHeadId { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }

    }
    public partial class GISFileAssessmentR
    {
   
        public long AssessmentID { get; set; }
        public string AssessmentYear { get; set; }
        public List<GISFileAssessmentItemR> LsAssessments { get; set; }

    }
    public partial class GISFileAssessmentItemR
    {
        
        public string AssetNumber { get; set; }
        public string AssessmentAmount { get; set; }

        public DateTime DateSaved { get; set; }
    }
    public partial class GISFileAssetR
    {
       
        public string AssetNumber { get; set; }
        public string AssetName { get; set; }
        public string AssetType { get; set; }
        public string AssetSubType { get; set; }
        public string AssetLGA { get; set; }
        public string AssetDistrict { get; set; }
        public string AssetWard { get; set; }
        public string AssetBillingZone { get; set; }
        public string AssetSubzone { get; set; }
        public string AssetUse { get; set; }
        public string AssetPurpose { get; set; }
        public string AssetAddress { get; set; }
        public string AssetRoadName { get; set; }
        public string AssetOffStreet { get; set; }
        public string HoldingType { get; set; }
        public string AssetCofO { get; set; }
        public string TitleDocument { get; set; }
        public string SupportingDocument { get; set; }
        public string PartyID { get; set; }
        public string OccupierStatus { get; set; }
        public string AnyOccupants { get; set; }
        public string OccupancyType { get; set; }
        public string AssetModifiedDate { get; set; }
        public string AssetFootprintPresent { get; set; }
        public string AssetAge { get; set; }
        public string AssetCompletionYear { get; set; }
        public string AssetFurnished { get; set; }
        public string AssetSize { get; set; }
        public string AssetPerimeter { get; set; }
        public string NumberOfFloors { get; set; }
        public string AssetLatitude { get; set; }
        public string AssetLongitude { get; set; }
        public string StateOfRepair { get; set; }
        public string LevelOfCompletion { get; set; }
        public string HasGenerator { get; set; }
        public string HasSwimmingPool { get; set; }
        public string HasFence { get; set; }
        public string HasBuildings { get; set; }
        public string NumberOfBldgs { get; set; }
        public string WallMaterial { get; set; }
        public string RoofMaterial { get; set; }
        public string SewageAccess { get; set; }
        public string ElectricConnection { get; set; }
        public string WaterConnectionType { get; set; }
        public string SolidWasteCollectionType { get; set; }
    }
    public partial class GISFilePartyR
    {
        
        public string PartyExtID { get; set; }
        public string PartyID { get; set; }
        public string PartyTitle { get; set; }
        public string PartyFirstName { get; set; }
        public string PartyLastName { get; set; }
        public string PartyMiddleName { get; set; }
        public string PartyFullName { get; set; }
        public string PartyType { get; set; }
        public string PartyGender { get; set; }
        public string PartyDOB { get; set; }
        public string PartyTIN { get; set; }
        public string PartyNIN { get; set; }
        public string PartyPhone1 { get; set; }
        public string PartyPhone2 { get; set; }
        public string PartyEmail { get; set; }
        public string PartyNationality { get; set; }
        public string PartyMaritalStatus { get; set; }
        public string PartyOccupation { get; set; }
        public string ContactAddress { get; set; }
        public string PartyRelation { get; set; }
        public string AcquisitionDate { get; set; }
    }

}