using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class NinValidationViewModel
    {
        public List<NINIndividual> NINIndividuals { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
    public class NINIndividual
    {
        public string IndividualRIN { get; set; }   
        public string IndividualName { get; set; }  
        public string Tin { get; set; }        
        public string NIN { get; set; }        
        public string ContactAddress { get; set; } 
        public string NINStatus { get; set; }      
    }
}