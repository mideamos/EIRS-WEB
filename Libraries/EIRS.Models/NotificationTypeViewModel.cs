using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
    public class NotificationTypeViewModel
    {
        public int NotificationTypeID { get; set; }

        [Display(Name = "Notification Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Notification Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string NotificationTypeName { get; set; }

        [Display(Name = "Type Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Type Description")]
        public string TypeDescription { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
