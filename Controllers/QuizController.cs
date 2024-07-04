using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dydaktycznie.Models;
using Dydaktycznie.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dydaktycznie.Controllers
{
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quizs
        public async Task<IActionResult> Index()
        {

            // Pobierz identyfikator zalogowanego użytkownika
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Przyjmując, że identyfikatorem zalogowanego użytkownika jest nazwa użytkownika

            // Zapytanie EF Core: Pobierz quizy tylko jeśli są przypisane do zalogowanego użytkownika
            var quizzes = await _context.Quizzes
                .Where(q => q.AuthorID == loggedInUserId) // Filtruj quizy na podstawie nazwy użytkownika autora
                .ToListAsync();

            return View(quizzes);

        }

        // GET: Quizs/Details/5
        // GET: Quizs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                                  .Include(q => q.QuizQuestions)
                                      .ThenInclude(qq => qq.QuestionAnswers) // Include QuestionAnswers related to QuizQuestions
                                                                .Include(q => q.Author) // Dołączenie autora quizu (użytkownika)

                                  .FirstOrDefaultAsync(m => m.QuizID == id);


            if (quiz == null)
            {
                return NotFound();
            }

            // At this point, quiz.QuizQuestions should be loaded if there are any related to this quiz
            // You can safely access quiz.QuizQuestions.Count here

            return View(quiz);
        }

        // GET: Quizs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizID,Title,Description,AuthorID")] Quiz quiz, IFormFile photo)
        {
            System.Diagnostics.Debug.WriteLine($"elooooooooooooooooooooooooooooo");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            System.Diagnostics.Debug.WriteLine(userId);

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"eloooooooooooooooooooooooooooo2o222");

                try
                {
                    // Pobierz Id aktualnie zalogowanego użytkownika
                     userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    quiz.AuthorID = userId;

                    if (photo != null && photo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            quiz.Photo = memoryStream.ToArray(); // Konwersja strumienia na tablicę bajtów i przypisanie do quiz.Photo
                        }
                    }

                    _context.Add(quiz);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Możesz tutaj dodać obsługę błędów związanych z bazą danych, jeśli zajdzie taka potrzeba
                    ModelState.AddModelError("", "Nie udało się zapisać zmian. Spróbuj ponownie.");
                }
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
            return View(quiz);
        }


        // GET: Quizs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                                     .Include(q => q.QuizQuestions)
                                         .ThenInclude(qq => qq.QuestionAnswers) // Include QuestionAnswers related to QuizQuestions
                                         .Include(q => q.Author) // Dołączenie autora quizu (użytkownika)

                                     .FirstOrDefaultAsync(m => m.QuizID == id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }

        // POST: Quizs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizID,Title,Description,Photo,AuthorID")] Quiz quiz, IFormFile? photo)
        {
            if (id != quiz.QuizID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null && photo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            quiz.Photo = memoryStream.ToArray(); // Konwersja strumienia na tablicę bajtów i przypisanie do quiz.Photo
                        }
                    }
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.QuizID))
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
            return View(quiz);
        }

        // GET: Quizs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(m => m.QuizID == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                _context.Quizzes.Remove(quiz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: QuizQuestions/Delete/5
        [HttpPost, ActionName("DeleteQuizQuestions")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedQuestion(int id)
        {
            var quizQuestion = await _context.QuizQuestions.FindAsync(id);
            if (quizQuestion != null)
            {
                _context.QuizQuestions.Remove(quizQuestion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.QuizID == id);
        }
    }
}
