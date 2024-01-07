using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace EIRS.Common
{
    public static class StringHelpers
    {
        public static string ToSeoUrl(this string pstrURL)
        {
            if (pstrURL != null)
            {
                string encodedUrl = pstrURL;

                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

                encodedUrl = encodedUrl.Replace("'", "");

                encodedUrl = Regex.Replace(encodedUrl, @"[^A-Za-z0-9]", "-");

                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

                // trim leading & trailing characters

                encodedUrl = encodedUrl.Trim('-');

                return encodedUrl;
            }
            else
                return pstrURL;
        }

        /// <summary>
        ///     Removes dashes ("-") from the given object value represented as a string and returns an empty string ("")
        ///     when the instance type could not be represented as a string.
        ///     <para>
        ///         Note: This will return the type name of given isntance if the runtime type of the given isntance is not a
        ///         string!
        ///     </para>
        /// </summary>
        /// <param name="value">The object instance to undash when represented as its string value.</param>
        /// <returns></returns>
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        /// <summary>
        ///     Removes dashes ("-") from the given string value.
        /// </summary>
        /// <param name="value">The string value that optionally contains dashes.</param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }

        public static string ToShortDateForExcel(this DateTime value)
        {
            return value.ToString("dd_MM_yyy");
        }

        public static string ToExcelName(this RouteData routeData, string AppendExcelName = "")
        {
            return $"{AppendExcelName}{TrynParse.parseString(routeData.Values["action"]).Replace("ExportToExcel","") }_{DateTime.Now.ToShortDateForExcel()}.xlsx";
        }
    }
}
