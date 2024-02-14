
using System;
using System.IO;

namespace EIRS.Repository
{
    public class NewErrorLog
    {
        public static string RootPath()
        {
            return "C:\\inetpub\\wwwroot\\api-new-new\\bin\\Payload";
        }

        public static void WriteFormModel(string payload, string location)
        {
            var line = Environment.NewLine + Environment.NewLine;
            try
            {

               // string filepath = $"C://Users//Temitayo Oyetunji//Desktop//FromXLaptop//NowNow//NewRepo//Presentation//EIRS.API//{location}//";
                string filepath = $"C://inetpub//wwwroot//api-new-new//bin//Payload//{location}//";

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MMM-yyyy") + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

               StreamWriter sw = File.AppendText(filepath);
                var date = DateTime.Now.ToString();
                sw.WriteLine($"--------------------------------*Start @ {date}*------------------------------------------");
                sw.WriteLine(payload);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.WriteLine(line);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

    }
}