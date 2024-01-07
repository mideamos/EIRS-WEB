using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BOL
{
    partial class Payment_Account
    {
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string strPaymentDate { get; set; }
        public string strAmount { get; set; }
        public string RevenueStreamName { get; set; }
        public string RevenueSubStreamName { get; set; }
        public string AgencyName { get; set; }
        public string SettlementMethodName { get; set; }
        public string SettlementStatusName { get; set; }

        public bool ValidateDuplicateCheck { get; set; } = true;
    }
}
