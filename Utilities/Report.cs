using System;

namespace Utilities
{
    /// <summary>
    /// Отчет о посещаемости ребенка.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Полное имя ребенка.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Период.
        /// </summary>
        public Period Period { get; set; }
        /// <summary>
        /// Возрастная группа.
        /// </summary>
        public string AgeGroup { get; set; }
    }
}
