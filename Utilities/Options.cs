using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Настройки программы.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Возрастные группы.
        /// </summary>
        public string[] AgeGroups { get; set; }
        /// <summary>
        /// Дети.
        /// </summary>
        public string[] Children { get; set; }
        /// <summary>
        /// Отчеты.
        /// </summary>
        public Report Reports { get; set; }

        public void LoadOptionsFromFile(string Path)
        {

        }
    }
}
