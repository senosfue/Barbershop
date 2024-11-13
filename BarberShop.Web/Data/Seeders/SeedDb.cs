using BarberShop.Web.Services;

namespace BarberShop.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersServices _usersServices;

        public SeedDb(DataContext context, IUsersServices usersServices)
        {
            _context = context;
            _usersServices = usersServices;
        }

        public async Task SeedAsync()
        {
            
            await new CategoriesSeeder(_context).SeedAsync();
            await new PermissionsSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _usersServices).SeedAsync();
        }
    }
}
