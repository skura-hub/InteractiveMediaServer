
namespace Backend.Application.DTOs.Settings
{
    public class MailSettings
    {
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}