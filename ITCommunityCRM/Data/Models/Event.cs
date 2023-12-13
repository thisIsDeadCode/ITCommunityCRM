namespace ITCommunityCRM.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int NotificationMessageTemplateId { get; set; }
        public NotificationTemplate NotificationTemplate { get; set; }
    }
}
