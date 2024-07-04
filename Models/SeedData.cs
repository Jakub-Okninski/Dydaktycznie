using Dydaktycznie.Data;
using Microsoft.EntityFrameworkCore;

namespace Dydaktycznie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Categorys.Any())
                {
                    return;   // DB has been seeded
                }
                context.Categorys.AddRange(
                    new Category
                    {
                        Name = "IT"
                    },
                    new Category
                    {
                        Name = "Math"
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
 