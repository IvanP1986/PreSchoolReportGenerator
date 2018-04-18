using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace Utilities
{
    /// <summary>
    /// Настройки программы.
    /// </summary>
    [Serializable]
    public class Options
    {
        /// <summary>
        /// Возрастные группы.
        /// </summary>
        [XmlArrayItem("AgeGroup", IsNullable = true)]
        public string[] AgeGroups { get; set; }
        /// <summary>
        /// Дети.
        /// </summary>
        [XmlArrayItem("Child", IsNullable = true)]
        public string[] Children { get; set; }
        /// <summary>
        /// Отчеты.
        /// </summary>
        [XmlArrayItem(IsNullable = true)]
        public Report[] Reports { get; set; }

        public void LoadOptionsFromFile(string Path)
        {
            var serializer = new XmlSerializer(typeof(Options));
            using (var stream = File.Open(Path, System.IO.FileMode.Open))
            {
                var options = serializer.Deserialize(stream) as Options;
                this.CopyFrom(options);
            }
        }
        public void SaveOptionsToFile(string Path)
        {
            var serializer = new XmlSerializer(typeof(Options));
            using (var stream = File.Open(Path, System.IO.FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }
        public void CreateNewConfiguration()
        {
            this.AgeGroups = new string[]
            {
                "до 3-х лет", "3-х-5 лет", " 5-7л."
            };
            this.Children = new string[]
            {
                "Пустобаев Сергей Иванович",
                "Пустобаева Ольга Ивановна",
                "Пустобаева Анастасия Ивановна",
                "Пустобаева Дарья Ивановна"
            };
            this.Reports = new Report[]
            {
                new Report()
                {
                    AgeGroup = "до 3-х лет",
                    Children = new ObservableCollection<string>()
                    { "Пустобаева Анастасия Ивановна"}
                    ,Period = new Period() {Month=4, Year=2018}
                }
            };

        }
    }
}
