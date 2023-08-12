using System;
using System.Globalization;
namespace Glue.Controllers
{
    public class ConvertTools
    {
        public static DateTime? ConvertToDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss.fffZ", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
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
