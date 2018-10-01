using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Calendar
{
    /// <summary>
    /// Реализация интерфейса простого календаря без учета праздников.
    /// </summary>
    public class SimpleCalendarProvider : ICalendarProvider
    {
        /// <summary>
        /// Возвращает список дней месяца.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="month">Порядковый номер месяца.</param>
        /// <returns>Результат выполнения операции.</returns>
        public IEnumerable<Day> GetDayList(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            List<Day> days = new List<Day>();
            {
                Day day = new Day()
                {
                    Number = date.Day,
                    IsWorkDay = (date.DayOfWeek != DayOfWeek.Saturday &&
                        date.DayOfWeek != DayOfWeek.Sunday)
                };

                days.Add(day);
                date.AddDays(1);

            }
            while (date.Month == month) ;

            return days;
        }
    }
}
