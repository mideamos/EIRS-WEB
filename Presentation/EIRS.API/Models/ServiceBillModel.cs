using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EIRS.API.Models
{
    public class ServiceBillModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer Type is Required")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer is Required")]
        public int TaxPayerID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Notes is Required")]
        public string Notes { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "MDA Service is Required")]
        public int MDAServiceID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Year is Required")]
        public int TaxYear { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "MDA Service Items Required")]
        public IList<ServiceBillItemModel> LstServiceBillItem { get; set; }
    }

    public class ServiceBillItemModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "MDA Service Item is Required")]
        public int MDAServiceItemID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Service Base Amount is Required")]
        public decimal? ServiceBaseAmount { get; set; }
    }

    public class MDAServiceModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "MDA Service is Required")]
        public int MDAServiceID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Year is Required")]
        public int TaxYear { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "MDA Service Items Required")]
        public IList<ServiceBillItemModel> LstServiceBillItem { get; set; }

    }

    public class ServiceBillWithMultipleServiceModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer Type is Required")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer is Required")]
        public int TaxPayerID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Notes is Required")]
        public string Notes { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "MDA Service Required")]
        public IList<MDAServiceModel> LstMDAService { get; set; }

    }


}