using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dydaktycznie.Models;
using Dydaktycznie.Data.Dydaktycznie.Models;

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

        // GET: QuizQuestions/Create
        public IActionResult Create(int quizId)
        {
            var quizQuestion = new QuizQuestion
            {
                QuizID = quizId,
                QuestionAnswers = new List<QuestionAnswer>()
            };

            // Initial display of two answer fields
            quizQuestion.QuestionAnswers.Add(new QuestionAnswer());
            quizQuestion.QuestionAnswers.Add(new QuestionAnswer());

            return View(quizQuestion);
        }

        // POST: QuizQuestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizID,Question,QuestionAnswers")] QuizQuestion quizQuestion)
        {
            if (ModelState.IsValid)
            {
               /* foreach (var answer in quizQuestion.QuestionAnswers)
                {
                    _context.Add(answer);
                }*/
                _context.Add(quizQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Quizs", new { id = quizQuestion.QuizID });
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
