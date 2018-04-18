﻿using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Utilities
{
    /// <summary>
    /// Отчет о посещаемости ребенка.
    /// </summary>
    [Serializable]
    public class Report
    {
        /// <summary>
        /// Дети.
        /// </summary>
        [XmlArrayItem("Child")]
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
