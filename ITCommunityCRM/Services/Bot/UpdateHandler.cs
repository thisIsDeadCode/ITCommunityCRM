using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Data.Models.Consts;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;
using User = ITCommunityCRM.Data.Models.User;

namespace ITCommunityCRM.Services.Bot
{
    public class UpdateHandler : IUpdateHandler
    {
        private const string HELLO_MESSAGE = "Привет, дорогой друг. Добро пожаловать в наше сообщество.";
        private const string WRONG_EVENT_ID = "Информация о событии не найдена";
        private const string RESIGTER_BUTTON_TEXT = "Участвовать";
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly ITCommunityCRMDbContext _context;

        public UpdateHandler(UserManager<User> userManager, ILogger<UpdateHandler> logger, ITCommunityCRMDbContext context)
        {
            _userManager = userManager;
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
            var user = await TryCreateUser(message.From);
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
            await SendEventInvitation(bot, chatId, eventInfo, cancellationToken);
        }

        // TODO: should be move to another place, duplicate with externalauth controller
        private async Task<User> TryCreateUser(Telegram.Bot.Types.User? from)
        {
            if (from == null)
            {
                throw new InvalidDataException("Empty author message");
            }

            var userId = from.Id.ToString();
            var username = from.Username ?? Guid.NewGuid().ToString();
            var first_name = from.FirstName;
            var info = new UserLoginInfo(UserLoginInfoConst.TelegramLoginProvider, userId, UserLoginInfoConst.TelegramLoginProvider);
            var telegramUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (telegramUser == null)
            {
                if (string.IsNullOrEmpty(username))
                {
                    telegramUser = new User(new Guid().ToString(), first_name);
                }
                else
                {
                    telegramUser = new User(username, first_name);
                }
                await _userManager.CreateAsync(telegramUser);
            }

            await _userManager.AddLoginAsync(telegramUser, info);

            return telegramUser;
        }

        // TODO: need change button when already added to event
        private async Task SendEventInvitation(ITelegramBotClient bot, long chatId, Event @event, CancellationToken cancellationToken)
        {
            var eventInformation = $"{@event.Name}\r\n{@event.Description}";
            var registerToEventButton = new InlineKeyboardButton(RESIGTER_BUTTON_TEXT) { CallbackData = @event.Id.ToString() }; //{ Url = "https://google.com"};
            await bot.SendTextMessageAsync(
                chatId, 
                eventInformation,
                replyMarkup: new InlineKeyboardMarkup(registerToEventButton),
                cancellationToken: cancellationToken);
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
