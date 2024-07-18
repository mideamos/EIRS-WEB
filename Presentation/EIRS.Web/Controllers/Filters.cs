using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EIRS.BOL;

namespace EIRS.Web.Controllers
{
    public class Filters
    {
        public class SessionTimeoutAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpContext ctx = HttpContext.Current;
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Login/Individual");
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
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
            First_Signature_Appended = 10,
            Second_Signature_Appended = 11,
            Last_Signature_Appended = 12,
            Sealed_eTCC = 13,
            Issued_eTCC = 14,
        }
        public enum NewTCCRequestStageForClient
        {
            Validate_Tax_Payer_Information = 1,
            Validate_Tax_Payer_Income = 2,
            Generate_TCC_Detail = 3,
            Prepare_TCC_Draft = 4,
            Generate_eTCC = 5,
            Waiting_For_First_Signature = 6,
            Waiting_For_Second_Signature = 7,
            Waiting_For_Last_Signature = 8,
            Issue_eTCC = 14


        }

        public enum NewTCCRequestStage
        {
            Validate_Tax_Payer_Information = 1,
            Validate_Tax_Payer_Income = 2,
            Generate_TCC_Detail = 3,
            Prepare_TCC_Draft = 4,
            Generate_eTCC = 5,
            Waiting_For_First_Signature = 6,
            Waiting_For_Second_Signature = 7,
            Waiting_For_Last_Signature = 8,
            Issue_eTCC = 14
        }

        public string GetEnumDescription(NewTCCRequestStage stage)
        {
            switch (stage)
            {
                case NewTCCRequestStage.Validate_Tax_Payer_Information:
                case NewTCCRequestStage.Validate_Tax_Payer_Income:
                    return "Your request is under information and income validation stage";
                case NewTCCRequestStage.Generate_TCC_Detail:
                    return "Your request is under TCC validation stage";
                case NewTCCRequestStage.Prepare_TCC_Draft:
                case NewTCCRequestStage.Generate_eTCC:
                    return "Your request is on approval stage";
                case NewTCCRequestStage.Waiting_For_First_Signature:
                case NewTCCRequestStage.Waiting_For_Second_Signature:
                    return "Your TCC is on approval Stage";
                case NewTCCRequestStage.Waiting_For_Last_Signature:
                    return "Your TCC is approved and ready for collection. Visit closest EIRS office to you.";
                case NewTCCRequestStage.Issue_eTCC:
                    return "Your TCC has been Issued";
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }
        }

        public enum TCCSigningStage
        {
            AwaitingFirstSigner = 10000,
            AwaitingSecondSigner = 10001,
            AwaitingThirdSigner = 10002,
            Done
        }
    }
}