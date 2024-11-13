using BarberShop.Web.Data;
using BarberShop.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Web.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetCombosBarberShopRolesAsync();
        public Task<IEnumerable<SelectListItem>> GetCombosCategory();
    }
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetCombosBarberShopRolesAsync()
        {
            List<SelectListItem> list = await _context.BarberShopRoles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString(),
            }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[seleccione un rol...]",
                Value = "0",

            });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetCombosCategory()
        {
            List<SelectListItem> list = await _context.Categories.Select(s => new SelectListItem
            {
                Text = s.CategoryName,
                Value = s.Id.ToString(),
            }).ToListAsync();
          
            list.Insert(0, new SelectListItem
            {
                Text = "[seleccione una seccion]",
                Value = "0",
            
            });
            return list;
        }
    }
}
