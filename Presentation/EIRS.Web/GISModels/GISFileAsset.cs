using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public partial class GISFileAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long FileId { get; set; }
        public string FileNumber { get; set; }
        public string PageNo { get; set; }
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
        public DateTime DateSaved { get; set; }
    }
}