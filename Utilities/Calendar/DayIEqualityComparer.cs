using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Calendar
{
    internal class DayIEqualityComparer : IEqualityComparer<Day>
    {
        public bool Equals(Day x, Day y)
        {
            return x.Number == y.Number;
        }

        public int GetHashCode(Day obj)
        {
            return obj.Number;
        }
    }
}
