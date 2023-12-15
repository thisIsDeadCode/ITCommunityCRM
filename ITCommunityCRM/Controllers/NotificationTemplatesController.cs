using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;

namespace ITCommunityCRM.Controllers
{
    public class NotificationTemplatesController : Controller
    {
        private readonly ITCommunityCRMDbContext _context;

        public NotificationTemplatesController(ITCommunityCRMDbContext context)
        {
            _context = context;
        }

        // GET: NotificationTemplates
        public async Task<IActionResult> Index()
        {
            var iTCommunityCRMDbContext = _context.NotificationMessageTemplates.Include(n => n.NotificationType);
            return View(await iTCommunityCRMDbContext.ToListAsync());
        }

        // GET: NotificationTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotificationMessageTemplates == null)
            {
                return NotFound();
            }

            var notificationTemplate = await _context.NotificationMessageTemplates
                .Include(n => n.NotificationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificationTemplate == null)
            {
                return NotFound();
            }

            return View(notificationTemplate);
        }

        // GET: NotificationTemplates/Create
        public IActionResult Create()
        {
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Id");
            return View();
        }

        // POST: NotificationTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,MessageTemplate,NotificationTypeId")] NotificationTemplate notificationTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Id", notificationTemplate.NotificationTypeId);
            return View(notificationTemplate);
        }

        // GET: NotificationTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotificationMessageTemplates == null)
            {
                return NotFound();
            }

            var notificationTemplate = await _context.NotificationMessageTemplates.FindAsync(id);
            if (notificationTemplate == null)
            {
                return NotFound();
            }
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Id", notificationTemplate.NotificationTypeId);
            return View(notificationTemplate);
        }

        // POST: NotificationTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,MessageTemplate,NotificationTypeId")] NotificationTemplate notificationTemplate)
        {
            if (id != notificationTemplate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationTemplateExists(notificationTemplate.Id))
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
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Id", notificationTemplate.NotificationTypeId);
            return View(notificationTemplate);
        }

        // GET: NotificationTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotificationMessageTemplates == null)
            {
                return NotFound();
            }

            var notificationTemplate = await _context.NotificationMessageTemplates
                .Include(n => n.NotificationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificationTemplate == null)
            {
                return NotFound();
            }

            return View(notificationTemplate);
        }

        // POST: NotificationTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotificationMessageTemplates == null)
            {
                return Problem("Entity set 'ITCommunityCRMDbContext.NotificationMessageTemplates'  is null.");
            }
            var notificationTemplate = await _context.NotificationMessageTemplates.FindAsync(id);
            if (notificationTemplate != null)
            {
                _context.NotificationMessageTemplates.Remove(notificationTemplate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationTemplateExists(int id)
        {
          return (_context.NotificationMessageTemplates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
