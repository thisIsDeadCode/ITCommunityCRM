using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Data.Models.Consts;
using Telegram.Bot;

namespace ITCommunityCRM.Services
{
    public class TelegramNotification
    {
        private ITelegramBotClient _telegramBotClient { get; set; }
        private readonly ITCommunityCRMDbContext _context;

        public TelegramNotification(ITelegramBotClient telegramBotClient, ITCommunityCRMDbContext context)
        {
            _telegramBotClient = telegramBotClient;
            _context = context;
        }

        public async Task NotificateAsync(Event e)
        {
            var tgUsers = _context.UserLogins.Where(x => x.LoginProvider == UserLoginInfoConst.TelegramLoginProvider).ToList();
            var template = _context.NotificationMessageTemplates.FirstOrDefault();

            foreach (var tgUser in tgUsers) {
                await _telegramBotClient.SendTextMessageAsync(tgUser.ProviderKey, template.MessageTemplate);
            }
        }
    }
}
