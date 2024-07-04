using Dydaktycznie.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dydaktycznie.Controllers
{
    public class StatisticController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticController(ApplicationDbContext context)
        {
            _context = context;
        }
       public async Task<IActionResult> Index()
{
    var quizStatistics = await _context.Users
        .GroupJoin(
            _context.Quizzes,
            user => user.Id,
            quiz => quiz.AuthorID,
            (user, quizzes) => new
            {
                AuthorName = user.UserName,
                QuizCount = quizzes.Count(),
                TotalQuestions = quizzes.SelectMany(q => q.QuizQuestions).Count()
            })
        .ToListAsync();

    return View(quizStatistics);
}

    }
}
