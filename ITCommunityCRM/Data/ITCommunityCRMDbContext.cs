using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Data.Models.Consts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITCommunityCRM.Data
{
    public class ITCommunityCRMDbContext : IdentityDbContext<User>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationTemplate> NotificationMessageTemplates { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Tag> Tags { get; set; }


        public ITCommunityCRMDbContext(DbContextOptions<ITCommunityCRMDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotificationType>()
            .HasData(
            new NotificationType[]
            {
                new NotificationType { Id = 1, Type = NotificationTypeConst.All},
                new NotificationType { Id = 2, Type = NotificationTypeConst.Email},
                new NotificationType { Id = 3, Type = NotificationTypeConst.Telegram},
            });

            modelBuilder.Entity<NotificationTemplate>()
            .HasData(
            new NotificationTemplate[]
            {
                new NotificationTemplate { Id = 1, MessageTemplate = "Hi", Title = "Title", NotificationTypeId = 3},
            });

            modelBuilder.Entity<Tag>()
            .HasData(
            new Tag[]
            {
                new Tag { Id = 1, Name = ".Net"},
                new Tag { Id = 2, Name = ".C#"},
                new Tag { Id = 3, Name = "Java"},
                new Tag { Id = 4, Name = "JS"},
                new Tag { Id = 5, Name = "TS"},
                new Tag { Id = 6, Name = "Angular"},
                new Tag { Id = 7, Name = "Vue"},
                new Tag { Id = 8, Name = "React"},
                new Tag { Id = 9, Name = "Pyton"},
                new Tag { Id = 10, Name = "HR"},
                new Tag { Id = 11, Name = "Crypto"},
                new Tag { Id = 12, Name = "Security"},
                new Tag { Id = 13, Name = "Vacancies"},
                new Tag { Id = 14, Name = "Flood"},
                new Tag { Id = 15, Name = "etc"},
            });
        }
    }
}
