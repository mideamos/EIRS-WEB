using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class NotificationMethodViewModel
    {
        public int NotificationMethodID { get; set; }

        [Display(Name = "Notification Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Notification Method")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string NotificationMethodName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
