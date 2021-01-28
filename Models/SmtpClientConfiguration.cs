namespace Models
{
    /// <summary>
    /// Профиль электронного ящика для отправки писем.
    /// </summary>
    public class SmtpClientConfiguration
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
