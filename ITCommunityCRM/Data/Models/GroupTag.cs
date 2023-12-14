namespace ITCommunityCRM.Data.Models
{
    public class GroupTag
    {
        public int Id { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
