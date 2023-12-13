namespace ITCommunityCRM.Data.Models
{
    public class NotificationTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MessageTemplate { get; set; }

        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
