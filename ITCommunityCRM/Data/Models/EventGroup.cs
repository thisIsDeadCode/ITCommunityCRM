namespace ITCommunityCRM.Data.Models
{
    public class EventGroup
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
