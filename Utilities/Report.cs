using System;
using System.Collections.ObjectModel;

namespace Utilities
{
    /// <summary>
    /// Отчет о посещаемости ребенка.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Дети.
        /// </summary>
        public ObservableCollection<string> Children { get; set; }
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
