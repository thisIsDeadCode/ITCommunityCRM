using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Data.Models.Consts;
using Microsoft.EntityFrameworkCore;

namespace ITCommunityCRM.Services
{
    public class NotificationService
    {
        private readonly ITCommunityCRMDbContext _context;
        private TelegramNotification _telegramNotification;

        public NotificationService(TelegramNotification telegramNotification, ITCommunityCRMDbContext context)
        {
            _telegramNotification = telegramNotification;
            _context = context;
        }

        public async Task NotificateAsync(int eventId)
        {
            var e = await _context.Events
                .Include(x => x.NotificationType)
                .FirstOrDefaultAsync(x => x.Id == eventId);

            switch (e.NotificationType.Type)
            {
                case NotificationTypeConst.All:
                {
                    await _telegramNotification.NotificateAsync(e);
                    break;
                }
                case NotificationTypeConst.Email: break;
                case NotificationTypeConst.Telegram: 
                {
                    await _telegramNotification.NotificateAsync(e);
                    break;
                };
                default: break;  
            }
        }
    }
}
