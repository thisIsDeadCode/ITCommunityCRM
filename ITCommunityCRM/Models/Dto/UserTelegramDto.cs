using System.Text;

namespace ITCommunityCRM.Models.Dto
{
    public class UserTelegramDto
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string user_name { get; set; }
        public string auth_date { get; set; }
        public string hash { get; set; }

        public override string ToString()
        {
            StringBuilder dataStringBuilder = new StringBuilder(256);

            dataStringBuilder.Append("auth_date");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.first_name);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("first_name");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.user_name);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("id");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.id);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("usert_name");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(this.auth_date);
            //     dataStringBuilder.Append('\n');

            return dataStringBuilder.ToString();
        }
    }
}
