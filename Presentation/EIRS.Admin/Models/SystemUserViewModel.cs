using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EIRS.Admin.Models
{
    public class SystemUserViewModel
    {
        public int SystemUserID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(500, ErrorMessage = "Only 50 characters allowed.")]
        [Display(Name = "Name")]
        public string SystemUserName { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role")]
        public int SystemRoleID { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [MaxLength(50, ErrorMessage = "Only 50 characters allowed.")]
        [Display(Name = "Email Address")]
        public string UserLogin { get; set; }

        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(UserPassword), ErrorMessage = "Password And Confirm Password Do Not Match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }



    }
}