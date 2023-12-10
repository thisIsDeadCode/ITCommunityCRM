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
                NotificationTypeId = createEventViewModel.NotificationTypeId,
            };
        }

        public static Event ConvertToEvent(this EditEventViewModel editEventViewModel)
        {
            return new Event()
            {
                Id = editEventViewModel.Id,
                Name = editEventViewModel.Name,
                NotificationTypeId = editEventViewModel.NotificationTypeId,
            };
        }

        public static EditEventViewModel ConvertToEditEventViewModel(this Event e)
        {
            return new EditEventViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                NotificationTypeId = e.NotificationTypeId,
            };
        }
    }
}
