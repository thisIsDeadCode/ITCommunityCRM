using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Models.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ITCommunityCRM.Services
{
    public class EventService
    {
        private readonly UserManager<User> userManager;
        private readonly ITCommunityCRMDbContext context;
        private readonly IOptions<AppSettings> appSettings;

        public EventService(
            UserManager<User> userManager,
            ITCommunityCRMDbContext context,
            IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.context = context;
            this.appSettings = appSettings;
        }

        public async Task<bool> IsUserAlreadyRegister(Event @event, User user, CancellationToken cancellationToken)
        {
            return await context.Events.Where(e => e == @event)
                .Where(x => x.EventUsers != null && x.EventUsers.Any(y => y.User == user))
                .AnyAsync(cancellationToken);
        }

        public async Task RegisterUser(Event @event, User user, CancellationToken cancellationToken)
        {
            if (await IsUserAlreadyRegister(@event, user, cancellationToken))
            {
                return;
            }

            var eventUser = new EventUser() { Event = @event, EventId = @event.Id, User = user, UserId = user.Id };
            await context.AddAsync(eventUser, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        internal string GetLink(Event @event)
        {
            return $"{appSettings.Value.DomainLink}events/{@event.Id}";
        }
    }
}
