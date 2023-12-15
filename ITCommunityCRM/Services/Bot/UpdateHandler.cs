using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = Telegram.Bot.Types.Message;

namespace ITCommunityCRM.Services.Bot
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly ITCommunityCRMDbContext _context;

        public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, IServiceProvider serviceProvider,
            ITCommunityCRMDbContext context)
        {
            _botClient = botClient;
            _logger = logger;
            _context = context;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }

        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            if (message.Type == MessageType.Text && message.Text == "/Start")
            {
                var group = _context.Groups.FirstOrDefault(x => x.GroupIdCode == message.Chat.Id);
                if(group == null)
                {
                    _context.Groups.Add(new Group()
                    {
                        GroupIdCode = message.Chat.Id,
                        GroupName = message.Chat.Username ?? message.Chat.Title,
                        IsActive = true,
                    });
                }
            }

            return;
        }

        private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}
