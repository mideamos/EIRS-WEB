using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using LicenseContext = OfficeOpenXml.LicenseContext;


namespace EIRS.Common
{
    public class CommUtil
    {
        public static readonly char[] Alphabets = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static readonly string[] ReferenceDataChild = new string[] { "size", "assettype", "governmenttype", "addresstype", "taxoffice", "ward", "town", "lga", "buildingtype", "buildingcompletion", "buildingoccupancy", "buildingpurpose", "unitpurpose", "buildingownership", "unitfunction", "taxpayertype", "economicactivities", "taxpayerrole", "title", "vehicletype", "vehiclepurpose", "vehicleownership", "vehiclefunction", "vehiclesubtype", "businesstype", "businessstructure", "businessoperation", "businesscategory", "businesssector", "businesssubsector", "landownership", "landpurpose", "notificationmethod", "notificationtype", "settlementstatus", "exceptiontype", "vehiclelicense", "vehicleinsurance", "landdevelopment", "landfunction", "landstreetcondition", "agencytype", "agency", "paymentoption", "paymentfrequency", "settlementmethod", "revenuestream", "directorate", "revenuesubstream", "assessmentitemcategory", "assessmentitemsubcategory", "assessmentgroup", "assessmentsubgroup", "assessmentitem", "profile", "assessmentrule", "mdaserviceitem", "mdaservice", "latecharge" };

        public static readonly string[] AssetChild = new string[] { "building", "vehicles", "business", "land", "buildingunit" };
        public static readonly string[] AP_Default = new string[] { "updatebuilding", "updatevehicles", "updatebusiness", "updateland", "updatebuildingunit" };

        public static readonly string[] TaxPayerChild = new string[] { "individual", "company", "government", "special" };

        public static readonly string[] RevenueDataChild = new string[] { "servicebill", "notification", "assessment", "settlement", "paymentaccount" };

        public static readonly string[] DataControlChild = new string[] { "profilewithoutrule", "taxpayerwithoutasset", "assessmentitemwithoutrule", "individualwithoutassessment", "companywithoutassessment" };

        public static string GenerateUniqueAphaNumeric()
        {
            string strAlphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strSmallAlphabets = "abcdefghijklmnopqrstuvwxyz";
            string strNumbers = "1234567890";

            string strCharacters = strAlphabets + strSmallAlphabets + strNumbers;

            int mIntLength = 10;
            string mStrAlphaNumeric = string.Empty;
            for (int i = 0; i < mIntLength; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, strCharacters.Length);
                    character = strCharacters.ToCharArray()[index].ToString();
                } while (mStrAlphaNumeric.IndexOf(character) != -1);
                mStrAlphaNumeric += character;
            }

            return mStrAlphaNumeric;
        }

        public static int GenerateUniqueNumber(int digits = 6)
        {
            DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long iteration = (long)(DateTime.UtcNow - UNIX_EPOCH).TotalSeconds / 30;

            //Here the system converts the iteration number to a byte[]
            byte[] iterationNumberByte = BitConverter.GetBytes(iteration);
            //To BigEndian (MSB LSB)
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(iterationNumberByte);
            }

            //Hash the userId by HMAC-SHA-1 (Hashed Message Authentication Code)
            byte[] userIdByte = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N"));
            HMACSHA1 userIdHMAC = new HMACSHA1(userIdByte, true);
            byte[] hash = userIdHMAC.ComputeHash(iterationNumberByte); //Hashing a message with a secret key

            //RFC4226 http://tools.ietf.org/html/rfc4226#section-5.4
            int offset = hash[hash.Length - 1] & 0xf; //0xf = 15d
            int binary =
                ((hash[offset] & 0x7f) << 24)      //0x7f = 127d
                | ((hash[offset + 1] & 0xff) << 16) //0xff = 255d
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);

            int intRandomKey = binary % (int)Math.Pow(10, digits); // Shrink: 6 digits
            return intRandomKey;
        }

        public static DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")); ;
        }

        public static string GetFormatedDate(DateTime? pDate)
        {
            return pDate == null ? "-" : pDate.Value.ToString("dd-MMM-yyyy");
        }

        public static string GetFormatedFullDate(DateTime? pDate)
        {
            return pDate == null ? "-" : pDate.Value.ToString("dd MMMM yyyy");
        }

        public static string GetFormatedDateTime(DateTime? pDate)
        {
            return pDate == null ? "-" : pDate.Value.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }

        public static string GetFormatedCurrency(decimal? dcAmount)
        {
            return dcAmount.GetValueOrDefault().ToString("C", CultureInfo.CreateSpecificCulture("en-NG"));
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }


        public static string RenderPartialToString(string viewName, object model, ControllerContext ControllerContext, ViewDataDictionary pObjViewData = null, TempDataDictionary pObjTempData = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewDataDictionary ViewData = pObjViewData ?? new ViewDataDictionary();
            TempDataDictionary TempData = pObjTempData ?? new TempDataDictionary();
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }

        }


        public static bool SendSMS(string pStrToNumber, string pStrBody)
        {
            string SmsSender = WebConfigurationManager.AppSettings["SmsSender"] ?? "";
            string SmsSenderUsername = WebConfigurationManager.AppSettings["SmsSenderUsername"] ?? "";
            string SmsSenderPassword = WebConfigurationManager.AppSettings["SmsSenderPassword"] ?? "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://app.multitexter.com/v2/app/sms");
            httpWebRequest.ContentType = "application/json"; httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string email = SmsSenderUsername;
                //  string email = "tytunji29@gmail.com";
                string password = SmsSenderPassword;
                //string password = "Omoiyatayo01";
                string message = pStrBody;
                string sender_name = "ERAS";
                string recipients = pStrToNumber;
                string forcednd = "1";
                string json = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"message\":\"" + message + "\",\"sender_name\":\"" + sender_name + "\",\"recipients\":\"" + recipients + "\",\"forcednd\":\"" + forcednd + "\"}";
                streamWriter.Write(json); streamWriter.Flush(); streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd(); Console.WriteLine(result);
            }

            return true;
        }
        //public static bool SendSMS(string pStrToNumber, string pStrBody)
        //{
        //    try
        //    {
        //        var client = new RestClient("https://api.infobip.com/sms/125/text/single");

        //        //var request = new RestRequest(Method.POST);
        //        //request.AddHeader("accept", "application/json");
        //        //request.AddHeader("content-type", "application/json");
        //        //request.AddHeader("authorization", "Basic RVNJUlM6cGEkJHcwcmQ=");
        //        //request.AddParameter("application/json", "{\"from\":\"EIRS\", \"to\":\"" + GlobalDefaultValues.CountryCode + pStrToNumber + "\",\"text\":\"" + pStrBody + "\"}", ParameterType.RequestBody);

        //        //IRestResponse response = client.Execute(request);

        //        //TwilioClient.Init(GlobalDefaultValues.Twilio_AccountSID, GlobalDefaultValues.Twilio_AuthToken);

        //        //MessageResource mObjCreateMessage = MessageResource.Create(
        //        //    body: pStrBody,
        //        //    from: new Twilio.Types.PhoneNumber(GlobalDefaultValues.Twilio_SenderID),
        //        //    to: new Twilio.Types.PhoneNumber(GlobalDefaultValues.CountryCode + pStrToNumber),
        //        //    pathAccountSid: GlobalDefaultValues.Twilio_AccountSID
        //        //);

        //        return true;
        //    }
        //    catch (Exception Ex)
        //    {
        //        return false;
        //    }
        //}

        public static void RenderReportNStoreInFile(string p_strExportFilePath, LocalReport p_localReport, string p_strReportType)
        {

        }

        public static void RenderReportNStoreInFile(string p_strExportFilePath, LocalReport p_localReport, string p_strReportType, double p_dblPageWidth, double p_dblPageHeight, double[] p_dblPageMargin, bool p_blnGetPageDefaultSetting)
        {

        }

        public static byte[] ExportToExcel<T>(string[] strColumName, string[] strDisplayColumnName, string[] strColumnDataType, string strTableName, IList<T> lstData)
        {
            return null;
        }
        public static void GenerateExcel(DataTable dataTable, string path)
        {

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            // create a excel app along side with workbook and worksheet and give a name to it
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Excel._Worksheet xlWorksheet = excelWorkBook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            foreach (DataTable table in dataSet.Tables)
            {
                //Add a new worksheet to workbook with the Datatable name
                Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = table.TableName;
                // add all the columns
                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }
                // add all the rows
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
            }
        }
        public static DataTable ConvertToDataTable<T>(IList<T> models)
        {
            // creating a data table instance and typed it as our incoming model   
            // as I make it generic, if you want, you can make it the model typed you want.  
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties of that model  
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Loop through all the properties              
            // Adding Column name to our datatable  
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names    
                dataTable.Columns.Add(prop.Name);
            }
            // Adding Row and its value to our dataTable  
            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows    
                    values[i] = Props[i].GetValue(item, null);
                }
                // Finally add value to datatable    
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static byte[] ConvertDataTableToExcel(DataTable dataTable)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Load the data from DataTable to Excel worksheet
                worksheet.Cells.LoadFromDataTable(dataTable, true);

                // Save the Excel package to a MemoryStream
                using (var memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        public static byte[] ExportToExcel2<T>(IList<T> lstData, MemberInfo[] pObjMemberInfo, bool blnShowTotal = false, string[] strTotalColumns = null)
        {
            return null;
        }
        public static byte[] ToExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            ExcelPackage.LicenseContext = 0;
            using (var pck = new ExcelPackage())
            {
                var excelWorkSheet = pck.Workbook.Worksheets.Add(sheetName);

                var headers = GetHeaders<T>();
                CreateHeader(headers, excelWorkSheet);

                var columnCount = headers.Length;
                var modelCells = excelWorkSheet.Cells["A1"];
                var infoResponseResult = data as T[] ?? data.ToArray();
                var modelRows = infoResponseResult.Length + 1;
                var modelRange = $"A1:{ColumnIndexToColumnLetter(columnCount)}{modelRows}";
                var modelTable = excelWorkSheet.Cells[modelRange];

                #region formatting headers:

                var modelRange2 = $"A1:{ColumnIndexToColumnLetter(columnCount)}{1}";
                var modelTableHeader = excelWorkSheet.Cells[modelRange2];
                var headerFont = modelTableHeader.Style.Font;
                headerFont.Bold = true;
                //headerFont.SetFromFont(new Font("Calibri", 12),true,false,false);
                headerFont.Color.SetColor(Color.White);

                modelTableHeader.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                modelTableHeader.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

                #endregion

                modelCells.LoadFromCollection(infoResponseResult, true);
                FormatCells(excelWorkSheet, headers, infoResponseResult.Length);
                modelTable.AutoFitColumns();
                return pck.GetAsByteArray();
            }


        }
        private static void FormatCells(ExcelWorksheet excelWorksheet, PropertyInfo[] headings, int rowCount)
        {
            for (int i = 0; i < headings.Length; i++)
            {
                var typeCode = Type.GetTypeCode(headings[i].PropertyType);
                switch (typeCode)
                {
                    case TypeCode.DateTime:
                        FormatCells(excelWorksheet, i, rowCount, "mm/dd/yyyy hh:mm:ss AM/PM");
                        break;

                    case TypeCode.Object:
                        if (headings[i].PropertyType == typeof(DateTime?))
                            FormatCells(excelWorksheet, i, rowCount, "mm/dd/yyyy hh:mm:ss AM/PM");
                        break;
                }
            }
        }
        private static void FormatCells(ExcelWorksheet excelWorksheet, int index, int rowCount, string cellFormat)
        {
            var cellRange = $"{ColumnIndexToColumnLetter(index + 1)}1:{ColumnIndexToColumnLetter(index + 1)}{rowCount + 1}";
            var formatRange = excelWorksheet.Cells[cellRange];
            formatRange.Style.Numberformat.Format = cellFormat;
        }
        private static string ColumnIndexToColumnLetter(int colIndex)
        {
            var div = colIndex;
            var colLetter = string.Empty;

            while (div > 0)
            {
                var mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (div - mod) / 26;
            }
            return colLetter;
        }
        private static void CreateHeader(PropertyInfo[] headings, ExcelWorksheet excelWorksheet)
        {
            for (int i = 0; i < headings.Length; i++)
            {
                excelWorksheet.Cells[1, i + 1].Value = headings[i].Name.ToUpper();
            }
        }
        public static Dictionary<String, Object> Dyn2Dict(dynamic dynObject)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObject))
            {
                object obj = propertyDescriptor.GetValue(dynObject);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }
        private static PropertyInfo[] GetHeaders<T>()
        {
            var t = typeof(T);
            return t.GetProperties();
        }

        public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
        {
            if (aListA == null || aListB == null || aListA.Count != aListB.Count)
                return false;
            if (aListA.Count == 0)
                return true;
            Dictionary<T, int> lookUp = new Dictionary<T, int>();
            // create index for the first list
            for (int i = 0; i < aListA.Count; i++)
            {
                int count = 0;
                if (!lookUp.TryGetValue(aListA[i], out count))
                {
                    lookUp.Add(aListA[i], 1);
                    continue;
                }
                lookUp[aListA[i]] = count + 1;
            }
            for (int i = 0; i < aListB.Count; i++)
            {
                int count = 0;
                if (!lookUp.TryGetValue(aListB[i], out count))
                {
                    // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                    return false;
                }
                count--;
                if (count <= 0)
                    lookUp.Remove(aListB[i]);
                else
                    lookUp[aListB[i]] = count;
            }
            // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
            return lookUp.Count == 0;
        }
    }

    public static class CommExtensions
    {

        public static DataTable ToDataTable(this ExcelWorksheet ws, bool hasHeaderRow = true)
        {

            var tbl = new DataTable();
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                tbl.Columns.Add(hasHeaderRow ? firstRowCell.Text.Trim() : string.Format("Column {0}", firstRowCell.Start.Column));
            }

            var startRow = hasHeaderRow ? 2 : 1;
            for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                var row = tbl.NewRow();
                bool hasValue = false;
                try
                {
                    foreach (var cell in wsRow)
                    {
                        if (!string.IsNullOrWhiteSpace(cell.Text))
                        {
                            hasValue = true;
                        }

                        row[cell.Start.Column - 1] = string.IsNullOrWhiteSpace(cell.Text) ? null : (cell.Text.ToLower() == "null" ? null : cell.Value);
                    }

                    if (hasValue)
                    {
                        tbl.Rows.Add(row);
                    }
                }
                catch (Exception Ex)
                {
                    break;
                }
            }
            return tbl;
        }
    }


}
