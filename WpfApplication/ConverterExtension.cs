using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication
{
    public static class ConverterExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(
            this IEnumerable<T> src)
        {
            return new ObservableCollection<T>(src);
        }
    }
}
