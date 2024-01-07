using EIRS.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web
{
    public class AdditionalSignature
    {
        public int PageNumber { get; set; }

        public float xPosition { get; set; }

        public float yPosition { get; set; }

        public float width { get; set; }

        public float height { get; set; }

        public string imgSrc { get; set; }
    }

    public partial class PDFTemplateModel
    {
        public Nullable<int> PDFTemplateID { get; set; }
        public string PDFTemplateName { get; set; }
        public string PDFTemplateRef { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> TemplateTypeID { get; set; }
        public string TemplateTypeName { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string BaseUrl { get; set; }
        public string TemplateFilePath { get; set; }
        public Nullable<bool> DocumentRequired { get; set; }
        public string DocumentRequiredText { get; set; }
        public Nullable<bool> IsStandardSecure { get; set; }
        public string StandardSecureText { get; set; }
        public Nullable<bool> IsQRSecure { get; set; }
        public string QRSecureText { get; set; }
        public Nullable<int> QRSecureTypeID { get; set; }
        public string QRSecureTypeName { get; set; }
        public Nullable<int> QRTemplateID { get; set; }
        public string QRTemplateName { get; set; }
        public Nullable<bool> IsCertificateSecure { get; set; }
        public string CertificateSecureText { get; set; }
        public string FieldIds { get; set; }
        public string FieldNames { get; set; }
        public string SignatureFilePath { get; set; }
        public Nullable<decimal> AbsoluteXCoordinate { get; set; }
        public Nullable<decimal> AbsoluteYCoordinate { get; set; }
        public Nullable<int> ProcessingTypeID { get; set; }
        public string ProcessinTypeName { get; set; }
        public Nullable<bool> isEditable { get; set; }
        public Nullable<bool> IsOverlayTemplate { get; set; }
        public string OverlayTemplateText { get; set; }
        public Nullable<int> OverlayTemplateID { get; set; }
        public string OverlayTemplateName { get; set; }
        public Nullable<int> SignTypeID { get; set; }
        public string SignTypeName { get; set; }
        public Nullable<bool> Active { get; set; }
        public string ActiveText { get; set; }
    }

    public partial class PDFTemplateFieldList
    {
        public int PFID { get; set; }
        public int FieldID { get; set; }
        public string FieldName { get; set; }
        public Nullable<int> FieldTypeID { get; set; }
        public string FieldComboValue { get; set; }
        public Nullable<bool> IsRequired { get; set; }
        public string ReportParameterName { get; set; }
        public string FieldValue { get; set; }
        public string ETX_OrderFieldName { get; set; }
    }

    public class DocumentViewModel
    {
        public int DocumentID { get; set; }

        public int PDFTemplateID { get; set; }

        public int OrganizationID { get; set; }

        public string Reason { get; set; }

        public string Location { get; set; }

        public bool IsExpirable { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public IList<DocumentFieldModel> FieldList { get; set; }
    }

    public class DocumentFieldModel
    {
        public int FieldID { get; set; }

        public object FieldValue { get; set; }
    }

    public static class SEDEFunction
    {
        public static PDFTemplateModel PDFTemplateDetail(int pIntPDFTemplateID)
        {
            PDFTemplateModel mObjPDFTemplateModel = new PDFTemplateModel();
            IDictionary<string, object> dcParameter = new Dictionary<string, object>();
            dcParameter["PDFTemplateID"] = pIntPDFTemplateID;

            IDictionary<string, object> dcAPIResponse = APICall.GetData(GlobalDefaultValues.SEDE_API_PDFTemplateDetailUrl, dcParameter);

            if (TrynParse.parseBool(dcAPIResponse["success"]))
            {
                mObjPDFTemplateModel = JsonConvert.DeserializeObject<PDFTemplateModel>(TrynParse.parseString(dcAPIResponse["result"]));
            }

            return mObjPDFTemplateModel;
        }

        public static IList<PDFTemplateFieldList> PDFTemplateFieldList(int pIntPDFTemplateID, int pIntDocumentID = 0)
        {
            IList<PDFTemplateFieldList> lstPDFTemplateField = new List<PDFTemplateFieldList>();
            IDictionary<string, object> dcParameter = new Dictionary<string, object>();
            dcParameter["PDFTemplateID"] = pIntPDFTemplateID;
            dcParameter["DocumentID"] = pIntDocumentID;

            IDictionary<string, object> dcAPIResponse = APICall.GetData(GlobalDefaultValues.SEDE_API_PDFTemplateFieldUrl, dcParameter);

            if (TrynParse.parseBool(dcAPIResponse["success"]))
            {
                lstPDFTemplateField = JsonConvert.DeserializeObject<IList<PDFTemplateFieldList>>(TrynParse.parseString(dcAPIResponse["result"]));
            }

            return lstPDFTemplateField;
        }
    }
}