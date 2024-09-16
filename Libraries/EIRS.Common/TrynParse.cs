using System;
using System.Linq;

namespace EIRS.Common
{
    public class TrynParse
    {
        public static string parseString(object p_objParseValue)
        {
            try { return p_objParseValue?.ToString(); }
            catch (NullReferenceException strnullexp) { return string.Empty; }
            catch (FormatException strfmtexp) { return string.Empty; }
        }

        public static string parseStringForExcel(object p_objParseValue)
        {
            try { return p_objParseValue.ToString(); }
            catch (NullReferenceException strnullexp) { return null; }
            catch (FormatException strfmtexp) { return string.Empty; }
        }



        public static bool parseBool(object p_objParseValue)
        {
            bool blnOutput = false;
            bool.TryParse(parseString(p_objParseValue), out blnOutput);
            return blnOutput;
        }

        public static int parseInt(object p_objParseValue)
        {
            int intOutput = 0;
            int.TryParse(parseString(p_objParseValue), out intOutput);
            return intOutput;
        }

        public static int? parseNullableInt(object p_objParseValue)
        {
            int intOutput = 0;
            if (int.TryParse(parseString(p_objParseValue), out intOutput)) return intOutput;
            return null;
        }

        public static double parseDouble(object p_objParseValue)
        {
            double dblOutput = 0d;
            double.TryParse(parseString(p_objParseValue), out dblOutput);
            return dblOutput;
        }

        public static float parseFloat(object p_objParseValue)
        {
            float fltOutput = 0f;
            float.TryParse(parseString(p_objParseValue), out fltOutput);
            return fltOutput;
        }

        public static decimal parseDecimal(object p_objParseValue)
        {
            decimal decOutput = 0m;
            decimal.TryParse(parseString(p_objParseValue), out decOutput);
            return decOutput;
        }

        public static DateTime parseDatetime(object p_objParseValue)
        {
            DateTime dtOutput;
            DateTime.TryParse(TrynParse.parseString(p_objParseValue), out dtOutput);
            return dtOutput;
        }

        public static TimeSpan parseTimeSpan(object p_objParseValue)
        {
            TimeSpan intOutput = TimeSpan.MinValue;
            TimeSpan.TryParse(parseString(p_objParseValue), out intOutput);
            return intOutput;
        }

        public static TimeSpan? parseNullableTimeSpan(object p_objParseValue)
        {
            if (parseString(p_objParseValue) == "")
            {
                return null;
            }
            else
            {
                TimeSpan intOutput;
                TimeSpan.TryParse(parseString(p_objParseValue), out intOutput);
                return intOutput;
            }
        }

        public static DateTime? parseNullableDate(object p_objParseValue)
        {
            if (parseString(p_objParseValue) == "")
            {
                return null;
            }
            else
            {
                DateTime intOutput;
                DateTime.TryParse(parseString(p_objParseValue), out intOutput);
                return intOutput;
            }
        }

        public static long parseLong(object p_objParseValue)
        {
            long lngOutput = 0;
            long.TryParse(parseString(p_objParseValue), out lngOutput);
            return lngOutput;
        }

        public static int?[] parseNullableIntArray(object p_objParseValue)
        {
            int?[] intOutput = new int?[] { };

            if (!string.IsNullOrWhiteSpace(parseString(p_objParseValue)))
            {
                intOutput = parseString(p_objParseValue).Split(',').Select(n => parseNullableInt(n)).ToArray();
            }

            return intOutput;
        }

        public static int[] parseIntArray(object p_objParseValue)
        {
            int[] intOutput = new int[] { };

            if (!string.IsNullOrWhiteSpace(parseString(p_objParseValue)))
            {
                intOutput = parseString(p_objParseValue).Split(',').Select(n => parseInt(n)).ToArray();
            }

            return intOutput;
        }
    }
}
