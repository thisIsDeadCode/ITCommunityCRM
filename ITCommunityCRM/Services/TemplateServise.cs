using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Data.Models.Consts;
using ITCommunityCRM.Models.Configuration;
using Microsoft.Extensions.Options;

namespace ITCommunityCRM.Services
{
    public class TemplateServise
    {
        private readonly AppSettings _appSettings;

        public TemplateServise(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GetEmailMessage(string template, User user, Event e)
        {
            foreach (var expressoin in Expressions.List)
            {
                switch (expressoin)
                {
                    case Expressions.UserName:
                    {
                        template.Replace(Expressions.UserName, user.UserName);
                        break;
                    }
                    case Expressions.UserFirstName:
                    {
                        template.Replace(Expressions.UserFirstName, user.FirstName);
                        break;
                    }
                    case Expressions.UserEmail:
                    {
                        template.Replace(Expressions.UserEmail, user.Email);
                        break;
                    }
                    case Expressions.EventName:
                    {
                        template.Replace(Expressions.EventName, e.Name);
                        break;
                    }
                    case Expressions.DateEvent:
                    {
                        template.Replace(Expressions.DateEvent, $"{e.StartDate} - {e.EndDate}");
                        break;
                    }
                    case Expressions.EventLink:
                    {
                        template.Replace(Expressions.DateEvent, $"{_appSettings.DomainLink}/Events/{e.Id}");
                        break;
                    }

                    default: break;
                }
            }
            return template;
        }
    }
}
