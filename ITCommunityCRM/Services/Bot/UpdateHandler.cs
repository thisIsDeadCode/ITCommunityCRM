using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using MailKit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
        private const string HELLO_MESSAGE = "Привет, дорогой друг. Добро пожаловать в наше сообщество.";
        private const string WRONG_EVENT_ID = "Информация о событии не найдена";
        private readonly ILogger<UpdateHandler> _logger;
        private readonly ITCommunityCRMDbContext _context;

        public UpdateHandler(ILogger<UpdateHandler> logger, ITCommunityCRMDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(bot, message, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }

        private async Task BotOnMessageReceived(ITelegramBotClient bot, Message message, CancellationToken cancellationToken)
        {
            if (message.Type != MessageType.Text
                || message.Text?.StartsWith("/start", StringComparison.OrdinalIgnoreCase) != true)
            {
                return;
            }

            await TryCreateGroupAsync(message, cancellationToken);
            await SendResponseToStartCommand(bot, message, cancellationToken);
        }

        private async Task SendResponseToStartCommand(ITelegramBotClient bot, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var messageParts = message.Text?.Split(" ").ToArray() ?? Array.Empty<string>();

            if (messageParts.Length != 2)
            {
                _logger.LogInformation("Start command doesnt contains eventId parameters, send hello message.");
                await bot.SendTextMessageAsync(chatId, HELLO_MESSAGE, cancellationToken: cancellationToken);
                return;
            }


            var eventInfo = int.TryParse(messageParts[1], out var eventIdFromMessage)
                ? await _context.Events.FirstOrDefaultAsync(x => x.Id == eventIdFromMessage, cancellationToken)
                : null;

            if (eventInfo == null)
            {
                _logger.LogInformation("Empty eventId in start command.");
                await bot.SendTextMessageAsync(chatId, WRONG_EVENT_ID, cancellationToken: cancellationToken);
                return;
            }

            _logger.LogInformation("Find event, sending information to user");
            await bot.SendTextMessageAsync(chatId, GetMessageFromEvent(eventInfo), cancellationToken: cancellationToken);

        }

        private string GetMessageFromEvent(Event @event)
        {
            return $"{@event.Name}\r\n{@event.Description}";
        }

        // ?? it can be private a chat an user with the bot
        private async Task TryCreateGroupAsync(Message message, CancellationToken cancellationToken)
        {
            var isGroupAlreadyCreated = await _context.Groups.AnyAsync(x => x.GroupIdCode == message.Chat.Id, cancellationToken);
            if (!isGroupAlreadyCreated)
            {
                _context.Groups.Add(new Group()
                {
                    GroupIdCode = message.Chat.Id,
                    GroupName = message.Chat.Username ?? message.Chat.Title ?? string.Empty,
                    IsActive = true,
                });
                await _context.SaveChangesAsync(cancellationToken);
            }
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
