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

        public async Task<IActionResult> Index(string ?authorSearchString, string? titleSearchString)
        {

            ViewData["TitleFilter"] = titleSearchString;
            ViewData["AuthorFilter"] = authorSearchString;

            var quizzes = from q in _context.Quizzes.Include(q => q.Author)
                          select q;

            if (!string.IsNullOrEmpty(authorSearchString))
            {
                quizzes = quizzes.Where(q => q.Author.UserName.Contains(authorSearchString));
            }
            if (!string.IsNullOrEmpty(titleSearchString))
            {
                quizzes = quizzes.Where(q => q.Title.Contains(titleSearchString));
            }

            return View(await quizzes.ToListAsync());
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
