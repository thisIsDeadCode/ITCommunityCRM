namespace ITCommunityCRM.Data.Models
{
    public class UserTag
    {
        public int Id { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public string UserId {  get; set; } 
        public User User { get; set; } 
    }
}
