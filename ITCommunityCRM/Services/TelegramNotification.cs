using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Data.Models.Consts;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace ITCommunityCRM.Services
{
    public class TelegramNotification
    {
        private ITelegramBotClient _telegramBotClient { get; set; }
        private readonly ITCommunityCRMDbContext _context;
        private readonly TemplateServise _templateServise;

        public TelegramNotification(ITelegramBotClient telegramBotClient, ITCommunityCRMDbContext context, 
            TemplateServise templateServise)
        {
            _telegramBotClient = telegramBotClient;
            _context = context;
            _templateServise = templateServise;
        }

        public async Task NotificateAsync(Event e)
        {
            var eventUsers = _context.EventUsers.Where(x => x.Id == e.Id);
            var userLogins = _context.UserLogins;

            var template = e.NotificationTemplate.MessageTemplate;


            foreach (var userEvent in eventUsers)
            {
                var tgUser = userLogins.FirstOrDefault(x => x.LoginProvider == UserLoginInfoConst.TelegramLoginProvider && x.UserId == userEvent.UserId);

                if(tgUser != null)
                {
                    await _telegramBotClient.SendTextMessageAsync(tgUser.ProviderKey, template);
                }
            }
        }
    }
}
