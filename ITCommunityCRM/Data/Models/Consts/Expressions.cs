namespace ITCommunityCRM.Data.Models.Consts
{
    public static class Expressions
    {
        public const string UserName = "{UserName}";
        public const string UserFirstName = "{UserFirstName}";
        public const string UserEmail = "{UserEmail}";
        public const string EventName = "{EventName}";
        public const string DateEvent = "{DateEvent}";
        public const string EventLink = "{EventLink}";

        public static IReadOnlyList<string> List = new List<string>()
        {
            UserName, UserFirstName, UserEmail, EventName, DateEvent, EventLink
        };
    }
}
