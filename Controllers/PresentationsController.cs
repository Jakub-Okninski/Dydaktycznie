﻿using Dydaktycznie.Data;
using Dydaktycznie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dydaktycznie.Controllers
{
    public class PresentationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PresentationsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(string? sortOrder, string? searchString, DateTime? startDate, DateTime? endDate, int? categoryId, Status? status, int? slidesCount, string currentFilter, int? pageNumber)
        {
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["SlidesCountSortParam"] = sortOrder == "slides_asc" ? "slides_desc" : "slides_asc";
            ViewData["ViewCountSortParam"] = sortOrder == "view_asc" ? "view_desc" : "view_asc";
            ViewData["NameSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["StatusSortParam"] = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["StartDateFilter"] = startDate;
            ViewData["EndDateFilter"] = endDate;
            ViewData["CategoryIdFilter"] = categoryId;
            if (status != null)
                ViewData["StatusFilter"] = ((int)status).ToString();
            ViewData["SlidesCountFilter"] = slidesCount;

            IQueryable<Presentation> presentations = _context.Presentations.Include(p => p.Category);

            if (!String.IsNullOrEmpty(searchString))
            {
                presentations = presentations.Where(p => p.Title.Contains(searchString));
            }

            if (startDate != null)
            {
                presentations = presentations.Where(p => p.CreationDate >= startDate);
            }

            if (endDate != null)
            {
                presentations = presentations.Where(p => p.CreationDate <= endDate);
            }

            if (categoryId != null && categoryId != -1)
            {
                presentations = presentations.Where(p => p.CategoryID == categoryId);
            }

            if (status != null)
            {
                presentations = presentations.Where(p => p.status == status);
            }

            if (slidesCount != null)
            {
                presentations = presentations.Where(p => p.SlidesCount <= slidesCount);
            }

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
            ViewData["Categories"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["Statuses"] = new SelectList(Enum.GetValues(typeof(Status))
                                                    .Cast<Status>()
                                                    .Select(s => new SelectListItem
                                                    {
                                                        Text = s.ToString(),
                                                        Value = ((int)s).ToString()
                                                    }), "Value", "Text");

            int pageSize = 3;
            return View(await PaginatedList<Presentation>.CreateAsync(presentations.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentation = await _context.Presentations
                .Include(p => p.Category)
                .Include(q => q.Author)
                .FirstOrDefaultAsync(m => m.PresentationID == id);
            if (presentation == null)
            {
                return NotFound();
            }

            return View(presentation);
        }
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            Presentation presentation = new Presentation();
            presentation.Description = "1";
            presentation.FileName = "lalal";
            presentation.CreationDate = DateTime.Now;
            presentation.SlidesCount = 1;
            presentation.FileName = "AuthorID";
            presentation.ViewCount = 1;
            presentation.status = Status.draft;
            return View(presentation);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PresentationID,Title,Description,CreationDate,SlidesCount,ViewCount,CategoryID,status,FileName,AuthorID")] Presentation presentation, IFormFile presentationFile)
        {

            presentation.Description = "Profesional Presentation about " + presentation.Title;
            presentation.CreationDate = DateTime.Now;
            presentation.SlidesCount = new Random().Next(1, 100);
            presentation.ViewCount = new Random().Next(0, 100);

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                presentation.AuthorID = userId;
                if (presentationFile != null && presentationFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(presentationFile.FileName);
                    presentation.FileName = uniqueFileName;

                    string imgPath = Path.Combine(_env.WebRootPath, "img");

                    if (!Directory.Exists(imgPath))
                    {
                        Directory.CreateDirectory(imgPath);
                    }

                    var filePath = Path.Combine(imgPath, uniqueFileName); 

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await presentationFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(presentation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Categories"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status)));
            presentation.FileName = "lalal";

            return View(presentation);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PresentationID,Title,Description,CreationDate,SlidesCount,ViewCount,CategoryID,status,FileName,AuthorID")] Presentation presentation, IFormFile? presentationFile)
        {
            if (id != presentation.PresentationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (presentationFile != null && presentationFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(presentationFile.FileName);
                        string imgPath = Path.Combine(_env.WebRootPath, "img");
                        var newFilePath = Path.Combine(imgPath, uniqueFileName);
                        using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await presentationFile.CopyToAsync(fileStream);
                        }

                        string imgPath2 = Path.Combine(_env.WebRootPath, "img");
                        var filePath2 = Path.Combine(imgPath, presentation.FileName);
                        if (System.IO.File.Exists(filePath2))
                        {
                            System.IO.File.Delete(filePath2);
                        }
                        presentation.FileName = uniqueFileName;
                    }
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
            ViewData["Categories"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status)));
            return View(presentation);
        }
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation != null)
            {
                string imgPath = Path.Combine(_env.WebRootPath, "img");
                var filePath = Path.Combine(imgPath, presentation.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

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
