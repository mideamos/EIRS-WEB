using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.API.Models
{
    public class ApiRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; }
    }
}