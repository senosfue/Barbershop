using BarberShop.Web.Data.Entities;
using BarberShop.Web.Data;
using Microsoft.EntityFrameworkCore;
using BarberShop.Web.Data.Entities;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Data.Seeders
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = [.. Haircuts(), .. Categories()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Name == permission.Name
                                                                        && p.Module == permission.Module);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Haircuts()
        {
            return new List<Permission>
            {
                new Permission { Name = "showHaricuts", Description = "Ver Haricuts", Module = "Haricuts" },
                new Permission { Name = "createHaricuts", Description = "Crear Haricuts", Module = "Haricuts" },
                new Permission { Name = "editHaricuts", Description = "Editar Haricuts", Module = "Haricuts" },
                new Permission { Name = "deleteHaricuts", Description = "Eliminar Haricuts", Module = "Haricuts" },
            };
        }

        private List<Permission> Categories()
        {
            return new List<Permission>
            {
                new Permission { Name = "showCategories", Description = "Ver Categories", Module = "Categories" },
                new Permission { Name = "createCategories", Description = "Crear Categories", Module = "Categories" },
                new Permission { Name = "editCategories", Description = "Editar Categories", Module = "Categories" },
                new Permission { Name = "deleteCategories", Description = "Eliminar Categories", Module = "Categories" },
            };
        }
    }
}