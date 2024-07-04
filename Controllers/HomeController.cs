using Dydaktycznie.Data;
using Dydaktycznie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Dydaktycznie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var quizzes = await _context.Quizzes.ToListAsync();
            return View(quizzes);
        }
        public async Task<IActionResult> MiniQuiz(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions) 
                    .ThenInclude(qq => qq.QuestionAnswers) 
                .FirstOrDefaultAsync(q => q.QuizID == id);

            if (quiz == null)
            {
                return NotFound();
            }

            return PartialView("MiniQuiz", quiz);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
