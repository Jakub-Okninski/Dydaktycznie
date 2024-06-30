using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dydaktycznie.Models;
using Dydaktycznie.Data.Dydaktycznie.Models;
using Mono.TextTemplating;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dydaktycznie.Controllers
{
    public class QuizQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuizQuestions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuizQuestions.Include(q => q.Quiz);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuizQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestions
                .Include(q => q.Quiz)
                .Include(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(m => m.QuizQuestionID == id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            return View(quizQuestion);
        }
        public IActionResult Create(int quizId)
        {
            System.Diagnostics.Debug.WriteLine("Errordasdasdasd");

            var quizQuestion = new QuizQuestion
            {
                QuizID = quizId,
                QuestionAnswers = new List<QuestionAnswer>
                {
                    new QuestionAnswer(),
                    new QuestionAnswer()
                }
            };

            return View(quizQuestion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizID,Question,QuestionAnswers")] QuizQuestion quizQuestion)
        {
            System.Diagnostics.Debug.WriteLine("Debug: Entered Create POST method");

            if (ModelState.IsValid)
            {
                // Najpierw zapisuje QuizQuestion do bazy danych
                _context.Add(quizQuestion);
                await _context.SaveChangesAsync();

                // Pobiera wygenerowany QuizQuestionID
                int quizQuestionId = quizQuestion.QuizQuestionID;

                // Przypisuje QuizQuestionID do każdej QuestionAnswer i zapisuje je
                foreach (var answer in quizQuestion.QuestionAnswers)
                {
                    answer.QuizQuestionID = quizQuestionId;
                    _context.Add(answer);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Quizs", new { id = quizQuestion.QuizID });
            }
            else
            {
                // Wypisuje błędy walidacji do konsoli debugowania
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            return View(quizQuestion);
        }





        // GET: QuizQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestions
                .Include(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(m => m.QuizQuestionID == id);
            if (quizQuestion == null)
            {
                return NotFound();
            }
            return View(quizQuestion);
        }

        // POST: QuizQuestions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizQuestionID,QuizID,Question,QuestionAnswers")] QuizQuestion quizQuestion)
        {
            if (id != quizQuestion.QuizQuestionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizQuestionExists(quizQuestion.QuizQuestionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = quizQuestion.QuizID });
            }
            return View(quizQuestion);
        }

        // GET: QuizQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestions
                .Include(q => q.Quiz)
                .Include(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(m => m.QuizQuestionID == id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            return View(quizQuestion);
        }

        // POST: QuizQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizQuestion = await _context.QuizQuestions.FindAsync(id);
            if (quizQuestion != null)
            {
                _context.QuizQuestions.Remove(quizQuestion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool QuizQuestionExists(int id)
        {
            return _context.QuizQuestions.Any(e => e.QuizQuestionID == id);
        }
    }
}
