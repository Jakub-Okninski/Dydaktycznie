﻿using System;
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

            System.Diagnostics.Debug.WriteLine("Dipda");

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"Error in gfdgfdg");

        

                _context.QuizQuestions.Add(quizQuestion);
                await _context.SaveChangesAsync();

                // Przekierowanie do szczegółów quizu lub innej strony
                return RedirectToAction("Details", "Quiz", new { id = quizQuestion.QuizID });
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

            // Return the view with the same model to display validation errors
            return View(quizQuestion);
        }






        // GET: QuizQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            System.Diagnostics.Debug.WriteLine($"Edas  Edycja");

            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestions
                .Include(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(m => m.QuizQuestionID == id);
            if (quizQuestion == null)
            {
                System.Diagnostics.Debug.WriteLine("dasdasda llalala Edycja");

                return NotFound();
            }
            System.Diagnostics.Debug.WriteLine($"Edas  dsa2343Edycja");
            System.Diagnostics.Debug.WriteLine(quizQuestion.QuizID);

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
                return RedirectToAction(nameof(Details), new { id = quizQuestion.QuizQuestionID });
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