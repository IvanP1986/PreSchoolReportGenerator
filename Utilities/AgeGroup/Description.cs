using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary>
    /// Описание. Служит для создания подписи для enum, например.
    /// </summary>
    class Description : Attribute
    {
        /// <summary>
        /// Текст подписи.
        /// </summary>
        public string Text;
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="text">Текст.</param>
        public Description(string text)
        {
            Text = text;
        }
    }
}
