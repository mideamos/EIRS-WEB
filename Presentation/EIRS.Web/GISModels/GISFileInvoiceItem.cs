using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public partial class GISFileInvoiceItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long InvoiceID { get; set; }
        public string RevenueHeadId { get; set; }
        public string FileNumber { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public DateTime DateSaved { get; set; }
        public string PageNo { get; internal set; }
        [NotMapped]
        public string InvoiceDate { get; set; }
         [NotMapped]
        public decimal DecimalAmount { get; set; }
    }
}