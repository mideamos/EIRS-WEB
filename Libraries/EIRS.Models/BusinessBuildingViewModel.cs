using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class BusinessBuildingViewModel
    {
        public int BuildingID { get; set; }
        public string BuildingRIN { get; set; }
        public string BuildingName { get; set; }

        public int BusinessID { get; set; }
        public string BusinessRIN { get; set; }
        public string BusinessName { get; set; }

        public int BuildingUnitID { get; set; }

        public string UnitNumber { get; set; }
    }
}
