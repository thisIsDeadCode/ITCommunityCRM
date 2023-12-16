using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Models.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace ITCommunityCRM.Services;

public class RedirectLinkService
{
    private readonly ITCommunityCRMDbContext dbContext;
    private readonly IOptions<AppSettings> appSettings;
    private const string USER_ID_PARAM_NAME = "userId";
    private const string EVENT_ID_PARAM_NAME = "eventId";
    private const string LINK_TO_PARAM_NAME = "linkTo";

    public RedirectLinkService(ITCommunityCRMDbContext dbContext, IOptions<AppSettings> appSettings)
    {
        this.dbContext = dbContext;
        this.appSettings = appSettings;
    }

    public string CreateUserLinkForEvent(User user, Event @event, string linkTo)
    {
        var domain = appSettings.Value.DomainLink;
        var url = $"{domain}Redirect";
        var parameters = new Dictionary<string, string>
        {
            { USER_ID_PARAM_NAME, user.Id},
            { EVENT_ID_PARAM_NAME, @event.Id.ToString() },
            { LINK_TO_PARAM_NAME, linkTo},
        };

        return QueryHelpers.AddQueryString(url, parameters);
    }

    public string GetOriginalUrl(string link)
    {
        var parameters = QueryHelpers.ParseQuery(link);
        return parameters.GetValueOrDefault(LINK_TO_PARAM_NAME).First();
    }

    private EventUser GetUserEventFromLink(string link)
    {
        var parameters = QueryHelpers.ParseQuery(link);
        parameters.TryGetValue(USER_ID_PARAM_NAME, out var userIds);
        parameters.TryGetValue(EVENT_ID_PARAM_NAME, out var eventIds);
        var userId = userIds.First();
        var eventId = int.Parse(eventIds.First());

        return dbContext.Events.Where(x => x.Id == eventId).SelectMany(x => x.EventUsers).First(x => x.UserId == userId);
    }

    internal void TrackRedirect(string queries)
    {
        var userEvent = GetUserEventFromLink(queries);
        //userEvent
    }
}
