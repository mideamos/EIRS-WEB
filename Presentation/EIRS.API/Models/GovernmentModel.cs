﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EIRS.API.Models
{
    public class GovernmentModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Government name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string GovernmentName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Government Type")]
        public int GovernmentTypeID { get; set; }

        public string GovernmentTypeName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Contact Address")]
        public string ContactAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter mobile phone number of individual")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string MobileNumber1 { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailAddress1 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Office")]
        public int TaxOfficeID { get; set; }

        public int? NotificationMethodID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Amount is Required")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Method is Required")]
        public int PaymentMethodID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Date is Required")]
        public DateTime PaymentDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Transaction Ref No is Required")]
        public string TransactionRefNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Revenue Stream is Required")]
        public int RevenueStreamID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Revenue Sub Stream is Required")]
        public int RevenueSubStreamID { get; set; }

        public int? AgencyID { get; set; }

        public string Notes { get; set; }


    }
}