 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public class TccDownloaded
    {
        public string TccRefNo { get; set; }
        public string IndividualRIn { get; set; }
        public string FullName { get; set; }
        public DateTime? DateIssued { get; set; }
        public long TccId { get; set; }
    }
}