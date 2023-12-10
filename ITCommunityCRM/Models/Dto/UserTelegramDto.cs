using System.Text;

namespace ITCommunityCRM.Models.Dto
{
    public class UserTelegramDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string AuthDate { get; set; }
        public string Hash { get; set; }

        public override string ToString()
        {
            StringBuilder dataStringBuilder = new StringBuilder(256);

            dataStringBuilder.Append("auth_date");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.AuthDate);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("first_name");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.FirstName);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("id");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.Id);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("usert_name");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.UserName);
            //     dataStringBuilder.Append('\n');

            return dataStringBuilder.ToString();
        }
    }
}
