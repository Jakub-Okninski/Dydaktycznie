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

      
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["SlidesCountSortParam"] = sortOrder == "slides_asc" ? "slides_desc" : "slides_asc";
            ViewData["ViewCountSortParam"] = sortOrder == "view_asc" ? "view_desc" : "view_asc";
            ViewData["NameSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["StatusSortParam"] = sortOrder == "status_asc" ? "status_desc" : "status_asc";

            IQueryable<Presentation> presentations = _context.Presentations.Include(p => p.Category);

            if (sortOrder == "title_desc")
            {
                presentations = presentations.OrderByDescending(p => p.Title);
            }
            else if (sortOrder == "title_asc")
            {
                presentations = presentations.OrderBy(p => p.Title);
            }
            else if (sortOrder == "date_asc")
            {
                presentations = presentations.OrderBy(p => p.CreationDate);
            }
            else if (sortOrder == "date_desc")
            {
                presentations = presentations.OrderByDescending(p => p.CreationDate);
            }
            else if (sortOrder == "slides_asc")
            {
                presentations = presentations.OrderBy(p => p.SlidesCount);
            }
            else if (sortOrder == "slides_desc")
            {
                presentations = presentations.OrderByDescending(p => p.SlidesCount);
            }
            else if (sortOrder == "view_asc")
            {
                presentations = presentations.OrderBy(p => p.ViewCount);
            }
            else if (sortOrder == "view_desc")
            {
                presentations = presentations.OrderByDescending(p => p.ViewCount);
            }
            else if (sortOrder == "name_asc")
            {
                presentations = presentations.OrderBy(p => p.Category.Name);
            }
            else if (sortOrder == "name_desc")
            {
                presentations = presentations.OrderByDescending(p => p.Category.Name);
            }
            else if (sortOrder == "status_asc")
            {
                presentations = presentations.OrderBy(p => p.status);
            }
            else if (sortOrder == "status_desc")
            {
                presentations = presentations.OrderByDescending(p => p.status);
            }
            else
            {
                presentations = presentations.OrderBy(p => p.Title);
            }

            return View(await presentations.ToListAsync());
        }

        // Pozostałe metody kontrolera (Create, Edit, Details, Delete) pozostają bez zmian.

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
