using Dydaktycznie.Data;
using Dydaktycznie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dydaktycznie.Controllers
{
    public class QuizQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public QuizQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuizQuestions.Include(q => q.Quiz);
            return View(await applicationDbContext.ToListAsync());
        }
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
        public async Task<IActionResult> Create(int quizId)
        {
            var quizQuestion = new QuizQuestion
            {
                Quiz = await _context.Quizzes.FirstOrDefaultAsync(m => m.QuizID == quizId),
                QuestionAnswers = new List<QuestionAnswer>()
            };
            quizQuestion.QuestionAnswers.Add(new QuestionAnswer());
            quizQuestion.QuestionAnswers.Add(new QuestionAnswer());

            return View(quizQuestion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizID,Question,QuestionAnswers")] QuizQuestion quizQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.QuizQuestions.Add(quizQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Quiz", new { id = quizQuestion.QuizID });
            }
      
            return View(quizQuestion);
        }

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
            System.Diagnostics.Debug.WriteLine(quizQuestion.QuizID);

            return View(quizQuestion);
        }

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
                return RedirectToAction(nameof(Details), new { id = quizQuestion.QuizQuestionID });
            }
            return View(quizQuestion);
        }

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
