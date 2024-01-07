using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class BuildingLandViewModel
    {
        public int BuildingID { get; set; }
        public string BuildingRIN { get; set; }
        public string BuildingName { get; set; }

        public int LandID { get; set; }
        public string LandRIN { get; set; }
        public string LandName { get; set; }
    }
}
