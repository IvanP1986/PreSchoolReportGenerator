using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Calendar
{
    /// <summary>
    /// Месяц.
    /// </summary>
    public class Month
    {
        /// <summary>
        /// Список дней месяца.
        /// </summary>
        public List<Day> Days { get; set; }

        /// <summary>
        /// Год.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Номер месяца.
        /// </summary>
        public int Number { get; set; }
    }
}
