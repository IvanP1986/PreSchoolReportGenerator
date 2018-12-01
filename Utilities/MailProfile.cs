namespace Utilities
{
    /// <summary>
    /// Профиль электронного ящика.
    /// </summary>
    public class MailProfile : ViewModelBase
    {
        /// <summary>
        /// Электронный адрес.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// SMTP host.
        /// </summary>
        public string SmtpHost { get; set; }
        /// <summary>
        /// SMTP port.
        /// </summary>
        public int SmtpPort { get; set; } 
    }
}
