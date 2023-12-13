using ITCommunityCRM.Services.Bot.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ITCommunityCRM.Services.Bot
{
    // Compose Receiver and UpdateHandler implementation
    public class ReceiverService : ReceiverServiceBase<UpdateHandler>
    {
        public ReceiverService(
            ITelegramBotClient botClient,
            UpdateHandler updateHandler,
            ILogger<ReceiverServiceBase<UpdateHandler>> logger)
            : base(botClient, updateHandler, logger)
        {


        }
    }
}
