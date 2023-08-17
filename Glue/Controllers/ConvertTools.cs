using System;
using System.Globalization;
using System.Data;

namespace Glue.Controllers
{
    public class ConvertTools
    {
        public static DateTime? StringConvertToDate(string dateString)
        {
            string[] formats = { "yyyy-M-d", "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss.fffZ", "yyyy-M-dTHH:mm:ss.fffZ" };
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
    }
}
