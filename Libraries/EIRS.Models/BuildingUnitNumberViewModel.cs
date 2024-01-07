using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class BuildingUnitNumberViewModel
    {
        public int BuildingID { get; set; }
        public string BuildingRIN { get; set; }
        public string BuildingName { get; set; }

        public int UnitID { get; set; }

        public string BuildingUnitIds { get; set; }
        public string UnitNumber { get; set; }
    }
}
