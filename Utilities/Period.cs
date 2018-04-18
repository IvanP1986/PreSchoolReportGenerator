using System;

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
    }
}