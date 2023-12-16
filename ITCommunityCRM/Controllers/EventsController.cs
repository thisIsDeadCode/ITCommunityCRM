using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Models.View.Events;
using ITCommunityCRM.Models.View.Extensions;
using ITCommunityCRM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ITCommunityCRM.Controllers
{
    public class EventsController : Controller
    {
        private readonly ITCommunityCRMDbContext _context;
        private NotificationService _notificationService;

        public EventsController(ITCommunityCRMDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var iTCommunityCRMDbContext = _context.Events
                .Include(x => x.NotificationTemplate)
                .ThenInclude(x => x.NotificationType);
            ViewBag.NotificationMessageTemplateId = new SelectList(_context.NotificationMessageTemplates, "Id", "Title");
            
            return View(await iTCommunityCRMDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var xevent = await _context.Events
                .Include(x => x.NotificationTemplate)
                .ThenInclude(x => x.NotificationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xevent == null)
            {
                return NotFound();
            }

            return View(xevent);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Registration(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var xevent = await _context.Events
                .Include(x => x.NotificationTemplate)
                .ThenInclude(x => x.NotificationType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (xevent == null)
            {
                return NotFound();
            }

            if (!HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                var userId = Guid.NewGuid().ToString();
                var user = new User(userId);
                user.IsAnonymousUser = true;

                var eventUser = new EventUser()
                {
                    EventId = xevent.Id,
                    Event = xevent,

                    UserId = userId,
                    User = user
                };

                _context.Users.Add(user);
                _context.EventUsers.Add(eventUser);
                await _context.SaveChangesAsync();

                HttpContext.Response.Cookies.Append("UserId", userId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                });
            }
            else
            {
                var isSuccsess = HttpContext.Request.Cookies.TryGetValue("UserId", out string userId);
                if (isSuccsess)
                {
                    var eventUser = _context.EventUsers
                        .Include(x => x.User)
                        .FirstOrDefault(x => x.EventId == xevent.Id && x.UserId == userId);

                    if (eventUser == null)
                    {
                        _context.EventUsers.Add(new EventUser()
                        {
                            EventId = xevent.Id,
                            UserId = userId
                        });
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return View(nameof(Details), xevent);
                }
            }


            return View(nameof(Details), xevent);
        }


        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["NotificationMessageTemplateId"] = new SelectList(_context.NotificationMessageTemplates, "Id", "Title");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,NotificationMessageTemplateId")] CreateEventViewModel xevent)
        {
            if (ModelState.IsValid)
            {
                var e = xevent.ConvertToEvent();
                _context.Add(e);
                await _context.SaveChangesAsync();

                await _notificationService.NotificateAsync(e.Id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationMessageTemplateId"] = new SelectList(_context.NotificationMessageTemplates, "Id", "Title");
            return RedirectToAction("Index");
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var xevent = await _context.Events.FindAsync(id);
            if (xevent == null)
            {
                return NotFound();
            }
            ViewData["NotificationMessageTemplateId"] = new SelectList(_context.NotificationMessageTemplates, "Id", "Title");
            return View(xevent.ConvertToEditEventViewModel());
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,NotificationMessageTemplateId")] EditEventViewModel xevent)
        {
            if (id != xevent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xevent.ConvertToEvent());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(xevent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationMessageTemplateId"] = new SelectList(_context.NotificationMessageTemplates, "Id", "Title");
            return View(xevent);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var xevent = await _context.Events
                .Include(x => x.NotificationTemplate)
                .ThenInclude(x => x.NotificationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xevent == null)
            {
                return NotFound();
            }

            return View(xevent);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ITCommunityCRMDbContext.Events'  is null.");
            }
            var xevent = await _context.Events.FindAsync(id);
            if (xevent != null)
            {
                _context.Events.Remove(xevent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
