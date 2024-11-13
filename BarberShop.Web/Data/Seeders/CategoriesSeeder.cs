using Microsoft.EntityFrameworkCore;
using BarberShop.Web.Data.Entities;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Data.Seeders
{
    public class CategoriesSeeder
    {
        private readonly DataContext _context;

        public CategoriesSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Category> categories = new List<Category>
            {
                new Category { CategoryName = "EJEMPLO1" },
                new Category { CategoryName = "EJEMPLO2" },
                new Category { CategoryName = "EJEMPLO3" },
                new Category { CategoryName = "EJEMPLO4"},
            };

            foreach (Category category in categories)
            {
                bool exists = await _context.Categories.AnyAsync(c => c.CategoryName == category.CategoryName);

                if (!exists)
                {
                    await _context.Categories.AddAsync(category);
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
