using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Calendar
{
    /// <summary>
    /// День месяца.
    /// </summary>
    public class Day
    {
        /// <summary>
        /// Номер дня.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Признак того, кто день является рабочим.
        /// </summary>
        public bool IsWorkDay { get; set; }
    }
}
