using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Models.Configuration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ITCommunityCRM.Services;

public class EmailNotification : IEmailSender
{
    private readonly ITCommunityCRMDbContext _context;
    private readonly EmailNotificationSettings _emailNotificationSettings;
    private readonly TemplateServise _templateServise;

    public EmailNotification(IOptions<EmailNotificationSettings> emailNotificationSettings, ITCommunityCRMDbContext context,
        TemplateServise templateServise)
    {
        _emailNotificationSettings = emailNotificationSettings.Value;
        _context = context;
        _templateServise = templateServise;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_emailNotificationSettings.Name, _emailNotificationSettings.Email));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(_emailNotificationSettings.Format)
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_emailNotificationSettings.EmailProvider, _emailNotificationSettings.Port, true);
            await client.AuthenticateAsync(_emailNotificationSettings.Email, _emailNotificationSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
    
    public async Task NotificateAsync(Event e)
    {
        var eventUsers = _context.EventUsers
            .Include(x => x.User)
            .Where(x => x.User.EmailConfirmed).ToList();
        var title = e.NotificationTemplate.Title;
        var template = e.NotificationTemplate.MessageTemplate;

        foreach (var eventUser in eventUsers)
        {
            var message = _templateServise.GetEmailMessage(template, eventUser.User, e);
            await SendEmailAsync(eventUser.User.Email, title, message);
        }
    }
}