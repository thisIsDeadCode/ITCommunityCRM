namespace ITCommunityCRM.Data.Models
{
    public class SpeakerTag
    {
        public int Id { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
    }
}
