using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Класс для копирования.
    /// </summary>
    public static class OptionsCopier
    {
        /// <summary>
        /// Метод расширения для поверхностного копирования настроек 
        /// из одного объекта в другой.
        /// </summary>
        /// <param name="dst">Объект-приемник.</param>
        /// <param name="src">Объект-источник.</param>
        public static void CopyFrom(this Options dst, Options src)
        {
            dst.AgeGroups = src.AgeGroups;
            dst.Children = src.Children;
            dst.Reports = src.Reports;
        }
    }
}
