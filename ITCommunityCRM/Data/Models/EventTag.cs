﻿namespace ITCommunityCRM.Data.Models
{
    public class EventTag
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
