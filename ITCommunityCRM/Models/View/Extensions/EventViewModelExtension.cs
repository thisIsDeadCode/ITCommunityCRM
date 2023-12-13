using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Models.View.Events;

namespace ITCommunityCRM.Models.View.Extensions
{
    public static class EventViewModelExtension
    {
        public static Event ConvertToEvent(this CreateEventViewModel createEventViewModel)
        {
            return new Event()
            {
                Name = createEventViewModel.Name,
                Description = createEventViewModel.Description,
                StartDate = createEventViewModel.StartDate,
                EndDate = createEventViewModel.EndDate,
                CreatedDate = DateTime.Now,
                NotificationMessageTemplateId = createEventViewModel.NotificationMessageTemplateId
            };
        }

        public static Event ConvertToEvent(this EditEventViewModel editEventViewModel)
        {
            return new Event()
            {
                Id = editEventViewModel.Id,
                Name = editEventViewModel.Name,
                Description = editEventViewModel.Description,
                StartDate = editEventViewModel.StartDate,
                EndDate = editEventViewModel.EndDate,
                CreatedDate = DateTime.Now,
                NotificationMessageTemplateId = editEventViewModel.NotificationMessageTemplateId
            };
        }

        public static EditEventViewModel ConvertToEditEventViewModel(this Event e)
        {
            return new EditEventViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                EndDate = e.EndDate,
                StartDate = e.StartDate,
                NotificationMessageTemplateId = e.NotificationMessageTemplateId,
            };
        }
    }
}
