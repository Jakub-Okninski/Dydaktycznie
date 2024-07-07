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
                if (context.Categorys.Any())
                {
                    return;   
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
