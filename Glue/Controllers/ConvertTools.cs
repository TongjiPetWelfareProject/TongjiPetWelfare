using System;
using System.Globalization;
using System.Data;

namespace Glue.Controllers
{
    public class ConvertTools
    {
        public static DateTime? StringConvertToDate(string dateString)
        {
            string[] formats = { "yyyy-M-d", "yyyy-MM-dd",
                "yyyy-MM-ddTHH:mm:ss.fffZ", "yyyy-M-dTHH:mm:ss.fffZ",
                "yyyy/M/d H:mm:ss"};
            if (DateTime.TryParseExact(dateString, formats, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                // 'parsedDate' now holds the DateTime value
                //Console.WriteLine(parsedDate.ToString()); // Print the parsed date
                return parsedDate;
            }
            else
            {
                return null;
            }
        }
        /*
        public static string DbDateStringConvertToNormal(string dateString)
        {
            // Extract month, day, time, and timezone parts
            string dayPart = dateString.Substring(0, 2);
            string monthDayPart = dateString.Substring(3, dateString.IndexOf("月") - 3);
            int yearStart = dateString.IndexOf("-", 3) + 1;
            string yearPart = dateString.Substring(dateString.IndexOf("-", 3) + 1, 2);
            //tring timePart = dateString.Substring(10, 8);
            //string amOrpm = dateString.Substring(dateString.IndexOf("下午")

            // return convertedDateTime.ToString();
        }
        */
    }
}
