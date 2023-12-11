﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITCommunityCRM.Data;
using ITCommunityCRM.Data.Models;
using ITCommunityCRM.Models.View.Extensions;
using ITCommunityCRM.Models.View.Events;
using ITCommunityCRM.Servises;

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
            var iTCommunityCRMDbContext = _context.Events.Include(x => x.NotificationType);
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
                .Include(x => x.NotificationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xevent == null)
            {
                return NotFound();
            }

            return View(xevent);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Type");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NotificationTypeId")] CreateEventViewModel xevent)
        {
            if (ModelState.IsValid)
            {
                var e = xevent.ConvertToEvent();
                _context.Add(e);
                await _context.SaveChangesAsync();

                await _notificationService.NotificateAsync(e.Id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Id", xevent.NotificationTypeId);
            return View(xevent);
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
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Type", xevent.NotificationTypeId);
            return View(xevent.ConvertToEditEventViewModel());
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NotificationTypeId")] EditEventViewModel xevent)
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
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Type", xevent.NotificationTypeId);
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
                .Include(x => x.NotificationType)
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