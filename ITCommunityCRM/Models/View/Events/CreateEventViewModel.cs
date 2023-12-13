namespace ITCommunityCRM.Models.View.Events
{
    public class CreateEventViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NotificationMessageTemplateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
