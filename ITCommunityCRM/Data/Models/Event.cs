namespace ITCommunityCRM.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
