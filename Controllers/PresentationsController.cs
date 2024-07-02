﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dydaktycznie.Data.Dydaktycznie.Models;
using Dydaktycznie.Models;

namespace Dydaktycznie.Controllers
{
    public class PresentationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PresentationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Presentations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Presentations.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Presentations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentation = await _context.Presentations
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PresentationID == id);
            if (presentation == null)
            {
                return NotFound();
            }

            return View(presentation);
        }

        // GET: Presentations/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            Presentation presentation = new Presentation();
            presentation.Description = "1";
            presentation.CreationDate = DateTime.Now;
            presentation.SlidesCount = 1;
            presentation.ViewCount = 1;
            presentation.status = Status.draft;
            return View(presentation);

        }


        // POST: Presentations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PresentationID,Title,Description,CreationDate,SlidesCount,ViewCount,CategoryID,status")] Presentation presentation)
        {

            System.Diagnostics.Debug.WriteLine($"super prezentacja");
            presentation.Description = "Profesional Presentation about " + presentation.Title;
            presentation.CreationDate= DateTime.Now;
            presentation.SlidesCount = new Random().Next(1, 100);
            presentation.ViewCount = new Random().Next(0, 100);
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("walidaca");

                _context.Add(presentation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["Categories"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status)));

            return View(presentation);
        }

        // GET: Presentations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation == null)
            {
                return NotFound();
            }
        
            ViewData["Categories"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status)));
            return View(presentation);
        }

        // POST: Presentations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PresentationID,Title,Description,CreationDate,SlidesCount,ViewCount,CategoryID,status")] Presentation presentation)
        {

          
                if (id != presentation.PresentationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presentation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresentationExists(presentation.PresentationID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "CategoryID", presentation.CategoryID);
            return View(presentation);
        }

        // GET: Presentations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentation = await _context.Presentations
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PresentationID == id);
            if (presentation == null)
            {
                return NotFound();
            }

            return View(presentation);
        }

        // POST: Presentations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation != null)
            {
                _context.Presentations.Remove(presentation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresentationExists(int id)
        {
            return _context.Presentations.Any(e => e.PresentationID == id);
        }
    }
}