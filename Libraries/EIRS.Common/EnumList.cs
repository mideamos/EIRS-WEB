using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Common
{
    public class EnumList
    {
        public enum Track { INSERT = 1, UPDATE = 2, DELETE = 3, EXISTING = 4 }

        public enum TemplateProcessingType { Design = 1, Direct = 2 }

        public enum FieldType { Text = 1, Number = 2, Combo = 3, Date = 4, Memo = 5, FileUpload = 6 }

        public enum OperationType { Deposit = 1, Transfer = 2, Withdraw = 3 }

        public enum AssetTypes { Building = 1, Vehicles = 2, Business = 3, Land = 4 }

        public enum TaxPayerType { Individual = 1, Companies = 2, Special = 3, Government = 4 }

        public enum UserType { Admin = 1, Staff = 2, Partner = 3 }

        public enum SettlementStatus { Notified = 2, Assessed = 1, Partial = 3, Settled = 4, Unsettled = 5, PendingApproval=6, Disapproved=7, PartialWithPendingApproval=8 }

        public enum PaymentStatus { Due = 1, Partial = 2, Paid = 3, }

        public enum PageList { About = 1, TaxPayers = 2, TaxAssets = 3, Partnership = 4, TaxTypes = 5, Support = 6 }

        public enum ClaimTypes { UserName, EmailAddress, UserID }

        public enum NotificationType { Tax_Payer_Capture = 1, Asset_Profile_Linked = 2, Bill_Generated = 3, Settlement_Received = 4, POA = 5 }

        public enum NotificationMethod { Email = 1, SMS = 2, Print = 3 }

        public enum NotificationMode { Auto = 1, Manual = 2 }

        public enum ReveneueStream
        {
            Business_Premises = 1,
            Capital_Gains_Taxes = 2,
            Consumption_Taxes = 3,
            Direct_Assessment = 4,
            Land_Use_Charge = 5,
            Lottery_Gaming_Regulation = 6,
            MDA_Services = 7,
            Pay_As_You_Earn = 8,
            Stamp_Duty = 9,
            Vehicle_License = 10,
            Withholding_Taxes = 11,
            Waste_Management = 12,
            State_Government = 13,
            Development_Levy = 14
        }

        public enum RegisterationStatus
        {
            Not_Started = 1,
            Partial = 2,
            Completed = 3
        }

        public enum TCCRequestStatus
        {
            Started = 1,
            In_Progess = 2,
            Awaiting_for_Payment = 3,
            Paid = 4,
            Validated_Information = 5,
            Validated_Income = 6,
            Generated_TCC_Details = 7,
            Prepared_TCC_Draft = 8,
            Generated_eTCC = 9,
            Validated_eTCC = 10,
            Signed_eTCC_Visible = 11,
            Signed_eTCC_Digital = 12,
            Sealed_eTCC = 13,
            Issued_eTCC = 14,
        }

        public enum TCCRequestStage
        {
            Validate_Tax_Payer_Information = 1,
            Validate_Tax_Payer_Income = 2,
            Generate_TCC_Detail = 3,
            Prepare_TCC_Draft = 4,
            Generate_eTCC = 5,
            Validate_eTCC = 6,
            Sign_eTCC_Visble = 7,
            Sign_eTCC_Dgital = 8,
            Seal_eTCC = 9,
            Issue_eTCC = 10
        }

        public enum CertificateStatus
        {
            Started = 1,
            Created = 2,
            Generated = 3,
            Validated = 4,
            Signed_Visible = 5,
            Signed_Digital = 6,
            Sealed = 7,
            Issued = 8,
        }

        public enum CertificateStage
        {
            Generate = 1,
            Validate = 2,
            Sign_Visible = 3,
            Sign_Digital = 4,
            Seal = 5,
            Issue = 6
        }

        public enum SystemUserRole
        {
            Admin = 1,
            Admin_RDM = 2,
            Admin_Master = 3
        }

        public enum ALScreen
        {
            Capture_Individual_Add = 1,
            Admin_Users_Add = 2,
            Admin_Users_Edit = 3,
            Capture_Individual_Edit_Assessment = 4,
            Capture_Corporate_Edit_Assessment = 5,
            Capture_Government_Edit_Assessment = 6,
            Admin_Central_Menu_Add = 7,
            Operation_Manager_PoA_Transfer = 8,
            Operation_Manager_PoA_Settlement = 9,
            Settle_Treasury_Receipt_Add = 10,
        }
    }
}
