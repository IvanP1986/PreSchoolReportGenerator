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
        Task<IEnumerable<Day>> GetDayListAsync(int year, int month);
    }
}
