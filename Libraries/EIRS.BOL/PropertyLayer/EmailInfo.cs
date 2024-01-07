using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BOL
{
    public class EmailDetails
    {
        public int TaxPayerTypeID { get; set; }

        public string TaxPayerTypeName { get; set; }
        public string FirstSignerName { get; set; }
        public string FirstSignerEmail { get; set; }

        public int TaxPayerID { get; set; }
        public string CertificateID { get; set; }
        public string ExpiryDate { get; set; }
        public string TaxYearCovered { get; set; }

        public string TaxPayerName { get; set; }

        public string TaxPayerEmail { get; set; }

        public string TaxPayerRIN { get; set; }

        public string TaxPayerMobileNumber { get; set; }

        public string TaxPayerTIN { get; set; }

        public string ContactAddress { get; set; }

        public string AssetRIN { get; set; }

        public string AssetTypeName { get; set; }

        public string AssetName { get; set; }

        public string AssetLGA { get; set; }

        public string TaxPayerRoleName { get; set; }

        public string BillTypeName { get; set; }

        public string BillRefNo { get; set; }

        public string BillAmount { get; set; }

        public string BillPaidAmount { get; set; }

        public string ReceivedAmount { get; set; }

        public string BillOutstandingAmount { get; set; }

        public string BillStatusName { get; set; }

        public string SettlementRefNo { get; set; }
        public string PoARefNo { get; set; }

        public int? LoggedInUserID { get; set; }

        public string RuleNames { get; set; }

    }
}
