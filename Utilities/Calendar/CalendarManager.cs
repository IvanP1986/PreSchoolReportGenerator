using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Calendar
{
    /// <summary>
    /// Управляет календарем на месяц.
    /// </summary>
    public class CalendarManager
    {
        #region Fields
        /// <summary>
        /// Провайдер календаря.
        /// </summary>
        ICalendarProvider _calendarProvider;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Конструктор с параметры.
        /// </summary>
        /// <param name="calendarProvider">Провайдер календаря.</param>
        public CalendarManager(ICalendarProvider calendarProvider)
        {
            _calendarProvider = calendarProvider;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Возвращает список дней месяца.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="month">Порядковый номер месяца.</param>
        /// <returns></returns>
        public IEnumerable<Day> GetDayList(int year, int month)
        {
            return _calendarProvider.GetDayList(year, month);
        }
        #endregion Methods
    }
}
