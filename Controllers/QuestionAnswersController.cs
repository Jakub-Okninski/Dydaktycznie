using System;
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
    public class QuestionAnswersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionAnswersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuestionAnswers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuestionAnswers.Include(q => q.QuizQuestion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuestionAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswers
                .Include(q => q.QuizQuestion)
                .FirstOrDefaultAsync(m => m.QuestionAnswerID == id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Create
        public IActionResult Create()
        {
            ViewData["QuizQuestionID"] = new SelectList(_context.QuizQuestions, "QuizQuestionID", "QuizQuestionID");
            return View();
        }

        // POST: QuestionAnswers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionAnswerID,QuizQuestionID,Answer,Correct")] QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionAnswer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuizQuestionID"] = new SelectList(_context.QuizQuestions, "QuizQuestionID", "QuizQuestionID", questionAnswer.QuizQuestionID);
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return NotFound();
            }
            ViewData["QuizQuestionID"] = new SelectList(_context.QuizQuestions, "QuizQuestionID", "QuizQuestionID", questionAnswer.QuizQuestionID);
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionAnswerID,QuizQuestionID,Answer,Correct")] QuestionAnswer questionAnswer)
        {
            if (id != questionAnswer.QuestionAnswerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionAnswer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionAnswerExists(questionAnswer.QuestionAnswerID))
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
            ViewData["QuizQuestionID"] = new SelectList(_context.QuizQuestions, "QuizQuestionID", "QuizQuestionID", questionAnswer.QuizQuestionID);
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswers
                .Include(q => q.QuizQuestion)
                .FirstOrDefaultAsync(m => m.QuestionAnswerID == id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionAnswer = await _context.QuestionAnswers.FindAsync(id);


           



            if (questionAnswer != null)
            {
                _context.QuestionAnswers.Remove(questionAnswer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "QuizQuestions", new { id = questionAnswer.QuizQuestionID });
        }

        private bool QuestionAnswerExists(int id)
        {
            return _context.QuestionAnswers.Any(e => e.QuestionAnswerID == id);
        }
    }
}
