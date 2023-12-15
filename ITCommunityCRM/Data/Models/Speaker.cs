namespace ITCommunityCRM.Data.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public List<EventSpeaker>? EventSpeakers { get; set; }
        public List<SpeakerTag>? SpeakerTags { get; set; }
    }
}
