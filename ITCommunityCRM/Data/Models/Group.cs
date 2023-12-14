namespace ITCommunityCRM.Data.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public long GroupIdCode { get; set; }
        public bool IsActive { get; set; }

        public int? TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
