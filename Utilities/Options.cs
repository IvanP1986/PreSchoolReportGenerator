using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Report;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;

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
        [XmlArrayItem("AgeGroup")]
        public AgeGroupType[] AgeGroups { get; set; }
        /// <summary>
        /// Дети.
        /// </summary>
        [XmlArrayItem("Child", IsNullable = true)]
        public string[] Children { get; set; }
        /// <summary>
        /// Отчеты.
        /// </summary>
        [XmlArrayItem(IsNullable = true)]
        public ChildReport[] Reports { get; set; }

        private ObservableCollection<Letter> letters;

        public ObservableCollection<Letter> Letters
        {
            get { return letters; }
            set { letters = value; }
        }

        public string[] TeacherNames { get; set; }

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
            this.AgeGroups = new [] {
                AgeGroupType.BeforeThree
                ,AgeGroupType.BetweenThreeAndFive
                ,AgeGroupType.UpperFive
            };
            this.Children = new string[]
            {
                "Пустобаев Сергей Иванович",
                "Пустобаева Ольга Ивановна",
                "Пустобаева Анастасия Ивановна",
                "Пустобаева Дарья Ивановна"
            };

            this.TeacherNames = new string[]
            {
                "Пустобаева И.Б."
            };

            Period period = new Period() { Year = DateTime.Now.Year, Month = DateTime.Now.Month };
            this.Reports = new ChildReport[]
            {
                new ChildReport()
                {
                    AgeGroup = AgeGroupType.BeforeThree,
                    Children = new ObservableCollection<string>()
                    { this.Children.First(n => n.Contains("Дарья")) }
                    ,Period = period
                    ,TeacherName = TeacherNames.First()
                },
                new ChildReport()
                {
                    AgeGroup = AgeGroupType.BetweenThreeAndFive,
                    Children = new ObservableCollection<string>()
                    { this.Children.First(n => n.Contains("Анастасия")) }
                    ,Period = period
                    ,TeacherName = TeacherNames.First()
                },
                new ChildReport()
                {
                    AgeGroup = AgeGroupType.UpperFive,
                    Children = new ObservableCollection<string>()
                    { this.Children.First(n => n.Contains("Ольга")) }
                    ,Period = period
                    ,TeacherName = TeacherNames.First()
                }
            };

        }
    }
}
