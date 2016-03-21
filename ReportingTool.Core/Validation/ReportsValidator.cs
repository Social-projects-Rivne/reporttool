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
        /// <summary>
        /// User name validation
        /// </summary>
        /// <param name="userName">Login of specific user</param>
        /// <returns>Boolean value of validation result</returns>
        public static bool UserNameIsCorrect(string userName)
        {
            return !String.IsNullOrWhiteSpace(userName);  
        }

        /// <summary>
        /// Input dates validation
        /// </summary>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>Boolean value of validation result</returns>
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
