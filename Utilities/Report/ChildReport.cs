using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Utilities.Report
{
    /// <summary>
    /// Отчет о посещаемости ребенка.
    /// </summary>
    [Serializable]
    public class ChildReport
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
        public AgeGroupType AgeGroup { get; set; }
        /// <summary>
        /// Имя воспитателя/учителя.
        /// </summary>
        public string TeacherName { get; set; }
        [XmlIgnore]
        public string FilePath { get; set; }
        /// <summary>
        /// Признак того, что элемент выбран.
        /// </summary>
        [XmlIgnore]
        public bool IsSelectef { get; set; }

        public override string ToString()
        {
            string children = string.Join(",", Children.Select(_GetShortName));
            string value = $"Отчет за {Period} по {children}";
            return value;
        }

        private string _GetShortName(string c)
        {
            string fio = Regex.Replace(c, @"^(\w+)\s(\w)\w+\s(\w)\w+$", "$1 $2. $3.");
            return fio;
        }
    }
}
