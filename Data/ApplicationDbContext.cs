namespace Dydaktycznie.Data
{
    using global::Dydaktycznie.Models;
    using Microsoft.EntityFrameworkCore;

    namespace Dydaktycznie.Models
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Quiz> Quizzes { get; set; }
            public DbSet<QuizQuestion> QuizQuestions { get; set; }
            public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        }
    }

}
