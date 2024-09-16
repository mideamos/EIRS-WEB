using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
    public class NotificationViewModel
    {
        public int NotificaationID { get; set; }

        public string NotificationRefNo { get; set; }

        public DateTime NotificationDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Notification Method")]
        [Display(Name = "Notification Method")]
        public int[] NotificationMethodId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Notification Type")]
        [Display(Name = "Notification Type")]
        public int NotificationTypeID { get; set; }

        public string EventRefNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Tax Payer Type")]
        [Display(Name = "Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Tax Type")]
        [Display(Name = "Tax Type")]
        public int TaxPayerID { get; set; }

        public string TaxPayerName { get; set; }

        public int NotificationModeID { get; set; }

        public bool NotificationStatus { get; set; }

        public string NotificationContent { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
