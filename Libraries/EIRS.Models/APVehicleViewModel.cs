using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
  public  class APVehicleViewModel
    {
      
        public int VehicleID { get; set; }

        [Display(Name = "VehicleRIN")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter VehicleRIN associated with Vehicle")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleRIN { get; set; }

        [Display(Name = "VIN")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter VIN associated with Vehicle")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string VIN { get; set; }

        [Display(Name = "Vehicle Reg Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter VehicleRegNumber associated with Vehicle")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleRegNumber { get; set; }
        

        [Display(Name = "Vehicle Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Land is primarily used for")]
        public int VehicleTypeID { get; set; }

        public string VehicleTypeName { get; set; }


        [Display(Name = "Vehicle Sub Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Land is primarily used for")]
        public int VehicleSubTypeID { get; set; }

        public string VehicleSubTypeName { get; set; }


        [Display(Name = "Vehicle Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Vehicle Purpose is primarily used for")]
        public int VehiclePurposeID { get; set; }

        public string VehiclePurposeName { get; set; }

        [Display(Name = "Vehicle Functione")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Vehicle Function is primarily used for")]
        public int VehicleFunctionID { get; set; }

        public string VehicleFunctionName { get; set; }


        [Display(Name = "Vehicle Ownership ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Vehicle Ownership  is primarily used for")]
        public int VehicleOwnershipID { get; set; }

        public string VehicleOwnershipName { get; set; }

        [Display(Name = "Vehicle Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Vehicle Description associated with Vehicle")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleDescription { get; set; }
        
        

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }
        [Display(Name = "LGA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select LGA which building is located")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }



    }
}
