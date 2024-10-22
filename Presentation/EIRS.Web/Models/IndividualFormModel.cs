using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class IndividualFormModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public string Gender { get; set; }

        public string NIN { get; set; }
        public string Rin { get; set; }
        public string MobileNumber  { get; set; }
        public string NINStatus { get; set; }

        public string ContactAddress { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

}