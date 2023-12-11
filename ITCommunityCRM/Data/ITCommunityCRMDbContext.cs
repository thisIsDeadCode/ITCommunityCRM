using ITCommunityCRM.Models;
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


        public DbSet<NotificationMessageTemplate> NotificationMessageTemplates { get; set; }


        public ITCommunityCRMDbContext(DbContextOptions<ITCommunityCRMDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotificationType>().HasData(
            new NotificationType[]
            {
                new NotificationType { Id = 1, Type = NotificationTypeConst.All},
                new NotificationType { Id = 2, Type = NotificationTypeConst.Email},
                new NotificationType { Id = 3, Type = NotificationTypeConst.Telegram},
            });

            modelBuilder.Entity<NotificationMessageTemplate>().HasData(
            new NotificationMessageTemplate[]
            {
                new NotificationMessageTemplate { Id = 1, MessageTemplate = "Hi", Title = "Title"},
            });
        }
    }
}
