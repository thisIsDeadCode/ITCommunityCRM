namespace ITCommunityCRM.Data.Models
{
    public class Speaker
    {
        public int Id { get; set; }


        public int? TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
