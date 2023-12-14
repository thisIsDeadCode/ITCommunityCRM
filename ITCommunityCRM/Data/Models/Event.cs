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


        public List<EventTag>? EventTags { get; set; } 
        public List<EventUser>? EventUsers { get; set; } 
        public List<EventSpeaker>? EventSpeakers { get; set; } 
        public List<EventGroup>? EventGroups { get; set; } 


        public int NotificationTemplateId { get; set; }
        public NotificationTemplate NotificationTemplate { get; set; }
    }
}
