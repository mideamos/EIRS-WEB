using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class TreasuryReceiptSignViewModel
    {
        public long ReceiptID { get; set; }

        public string AdditionalSignatureLocation { get; set; }

        public string SavedSignaturePath { get; set; }

        public float DocumentWidth { get; set; }

        public int SignSourceID { get; set; }

        [Display(Name = "Notes")]
        public string SignNotes { get; set; }

        public string ImgSrc { get; set; }
    }
}