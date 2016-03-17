using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Core.Validation
{
    public static class ReportsValidator
    {
        public static bool UserNameIsCorrect(string userName)
        {
            return !String.IsNullOrWhiteSpace(userName);  
        }

        public static bool DatesAreCorrect(string dateFrom, string dateTo)
        {
            DateTime dateFromResult;
            DateTime dateToResult;
            CultureInfo provider = CultureInfo.InvariantCulture;
            var format = "yyyy-MM-dd";

            try
            {
                dateFromResult = DateTime.ParseExact(dateFrom, format, provider);
                dateToResult = DateTime.ParseExact(dateTo, format, provider);
            }
            catch (FormatException)
            {
                return false;
            }

            return (DateTime.Compare(dateFromResult, dateToResult) <= 0);
        }
    }
}
