using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
    public class TokenRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "grant_type is Required")]
        public string grant_type { get; set; }

    }
}
