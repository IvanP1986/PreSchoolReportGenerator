using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Calendar
{
    /// <summary>
    /// Интерфейс провайдера календаря.
    /// </summary>
    public interface ICalendarProvider
    {
        /// <summary>
        /// Возвращает список дней месяца.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="month">Порядковый номер месяца.</param>
        /// <returns>Результат выполнения операции.</returns>
        IEnumerable<Day> GetDayList(int year, int month);
    }
}
