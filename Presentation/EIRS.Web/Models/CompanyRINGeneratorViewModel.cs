using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class RINGeneratorViewModel
    {
        [Display(Name = "Upload File")]
        [File(AllowedFileExtensions = new string[] { ".xlsx", ".xls", }, MaxContentLength = 1024 * 1024 * 1000, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase ExcelFile { get; set; }
    }

    public class FileAttribute : ValidationAttribute
    {

        public int MaxContentLength = int.MaxValue;
        public string[] AllowedFileExtensions;
        public string[] AllowedContentTypes;

        public override bool IsValid(object value)
        {

            var file = value as HttpPostedFileBase;

            //this should be handled by [Required]
            if (file == null)
            {
                return true;
            }

            if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = "File is too large, maximum allowed is: {0} KB";
                return false;
            }

            if (AllowedFileExtensions != null)
            {
                if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload file of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
            }

            if (AllowedContentTypes != null)
            {
                if (!AllowedContentTypes.Contains(file.ContentType))
                {
                    ErrorMessage = "Please upload file of type: " + string.Join(", ", AllowedContentTypes);
                    return false;
                }
            }

            return true;
        }

    }

    public class ExcelFormulaModel
    {
        public string Name { get; set; }
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public int ToRow { get; set; }
        public int ToCol { get; set; }
    }
}