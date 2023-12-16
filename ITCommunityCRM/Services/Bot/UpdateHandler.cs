using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Data.Models.Consts;
using ITCommunityCRM.Models.Configuration;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private const string OPEN_EVENT_BUTTON_TEXT = "Уже участвую";
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly ITCommunityCRMDbContext _context;
        private readonly EventService eventService;

        public UpdateHandler(
            UserManager<User> userManager,
            ILogger<UpdateHandler> logger,
            ITCommunityCRMDbContext context,
            EventService eventService)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            this.eventService = eventService;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(bot, message, cancellationToken),
                { CallbackQuery: { } callBackQuery } => BotOnKeyboardPress(bot, callBackQuery, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }

        private async Task BotOnKeyboardPress(ITelegramBotClient bot, CallbackQuery callBackQuery, CancellationToken cancellationToken)
        {
            var message = callBackQuery?.Message;
            var messageButtons = message?.ReplyMarkup?.InlineKeyboard.SelectMany(x => x).ToArray()
                ?? Array.Empty<InlineKeyboardButton>();

            if (message == null
                || messageButtons.Length != 1
                || messageButtons[0].Text != RESIGTER_BUTTON_TEXT
                || !long.TryParse(callBackQuery?.Data, out var eventId))
            {
                await bot.AnswerCallbackQueryAsync(callBackQuery!.Id, "Unknow command", cancellationToken: cancellationToken);
                return;
            }

            var user = await TryCreateUser(callBackQuery!.From);
            var @event = await _context.Events.FirstAsync(x => x.Id == eventId, cancellationToken);
            await eventService.RegisterUser(@event, user, cancellationToken);

            var button = await GetButtonForEventMessageAsync(@event, user, cancellationToken);
            await bot.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, new InlineKeyboardMarkup(button),cancellationToken);
            //await SendEventInvitation(bot, message.Chat.Id, @event, user, cancellationToken);
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
            await SendResponseToStartCommand(bot, user, message, cancellationToken);
        }

        private async Task SendResponseToStartCommand(ITelegramBotClient bot, User user, Message message, CancellationToken cancellationToken)
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
            await SendEventInvitation(bot, chatId, eventInfo, user, cancellationToken);
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
        private async Task SendEventInvitation(ITelegramBotClient bot, long chatId, Event @event, User user, CancellationToken cancellationToken)
        {
            var eventInformation = $"{@event.Name}\r\n{@event.Description}";
            var registerToEventButton = await GetButtonForEventMessageAsync(@event, user, cancellationToken);
            await bot.SendTextMessageAsync(
                chatId,
                eventInformation,
                replyMarkup: new InlineKeyboardMarkup(registerToEventButton),
                cancellationToken: cancellationToken);
        }

        private async Task<InlineKeyboardButton> GetButtonForEventMessageAsync(Event @event, User user, CancellationToken cancellationToken)
        {
            var isRegistered = await eventService.IsUserAlreadyRegister(@event, user, cancellationToken);
            if (!isRegistered)
            {
                return new InlineKeyboardButton(RESIGTER_BUTTON_TEXT) { CallbackData = @event.Id.ToString() };
            }

            var link = eventService.GetLink(@event);
            return new InlineKeyboardButton(OPEN_EVENT_BUTTON_TEXT) { Url = link };
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
