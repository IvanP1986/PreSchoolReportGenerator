using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Calendar
{
    /// <summary>
    /// Реализация интерфейса календаря.
    /// </summary>
    public class CalendarProvider : ICalendarProvider
    {
        ///inheritdoc
        public async Task<List<Day>> GetDayListAsync(int year, int month)
        {
            List<Day> days = await _MarkGeneralWorkAndRestDays(year, month);
            days = await _MarkExceptions(days, year, month);

            return days;    
        }

        private async Task<List<Day>> _MarkGeneralWorkAndRestDays(int year, int month)
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

        private async Task<List<Day>> _MarkExceptions(List<Day> days, int year, int month)
        {
            //TODO: подключить питон например и свянуть данные с сайта например https://hh.ru/article/calendar2018
            return days;
        }
    }
}
