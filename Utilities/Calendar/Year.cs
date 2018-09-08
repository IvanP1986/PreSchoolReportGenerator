namespace Utilities.Calendar
{
    /// <summary>
    /// Год с месяцами.
    /// </summary>
    public class Year
    {
        /// <summary>
        /// Номер года.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Месяцы с днями.
        /// </summary>
        public Month[] Months { get; set; }
    }
}
