using MimeKit.Text;

namespace ITCommunityCRM.Models.Configuration
{
    public class EmailNotificationSettings
    {
        public string EmailProvider { get; set; }
        public int Port { get; set; }
        public TextFormat Format { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
