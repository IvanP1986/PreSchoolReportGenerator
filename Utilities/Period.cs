using System;
using System.Globalization;

namespace Utilities
{
    /// <summary>
    /// Период отчетности.
    /// </summary>
    [Serializable]
    public class Period
    {
        /// <summary>
        /// Год.
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Месяц.
        /// </summary>
        public int Month { get; set; }

        public override string ToString()
        {
            string fullMonthName = new DateTime(Year, Month, 1)
                .ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("ru"));

            return fullMonthName;
        }
    }
}