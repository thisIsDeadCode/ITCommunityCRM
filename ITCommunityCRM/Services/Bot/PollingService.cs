using ITCommunityCRM.Services.Bot.Abstract;
using Microsoft.Extensions.Logging;

namespace ITCommunityCRM.Services.Bot
{
    // Compose Polling and ReceiverService implementations
    public class PollingService : PollingServiceBase<ReceiverService>
    {
        public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
            : base(serviceProvider, logger)
        {

        }
    }
}
