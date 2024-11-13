using BarberShop.Web.Core;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersServices _usersServices;

        public UserRolesSeeder(DataContext context, IUsersServices usersServices)
        {
            _context = context;
            _usersServices = usersServices;
        }

        public async Task SeedAsync()
        {
            await CheckRoles();
            await CheckUsers();
        }

        private async Task CheckUsers()
        {
            // Admin
            User? user = await _usersServices.GetUserAsync("Sebastian@ymail.com");

            if (user is null)
            {
                BarberShopRole adminRole = _context.BarberShopRoles.FirstOrDefault(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "Sebastian@ymail.com",
                    FirstName = "Sebastian",
                    LastName = "Ramirez",
                    PhoneNumber = "7000000",
                    UserName = "Sebastian@ymail.com",
                    Document = "11111",
                    BarberShopRole = adminRole
                };

                await _usersServices.AddUserAsync(user, "1234");

                string token = await _usersServices.GenerateEmailConfirmationTokenAsync(user);
                await _usersServices.ConfirmEmailAsync(user, token);
            }

            // Content Manager
            user = await _usersServices.GetUserAsync("anad@yopmail.com");

            if (user is null)
            {
                BarberShopRole contentManagerRole = _context.BarberShopRoles.FirstOrDefault(r => r.Name == "Gestor de contenido");

                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "31111111",
                    UserName = "anad@yopmail.com",
                    Document = "22222",
                    BarberShopRole = contentManagerRole
                };

                await _usersServices.AddUserAsync(user, "1234");

                string token = await _usersServices.GenerateEmailConfirmationTokenAsync(user);
                await _usersServices.ConfirmEmailAsync(user, token);
            }


        }


        private async Task CheckRoles()
        {
            await AdminRoleAsync();
            await ContentManagerAsync();
            await UserManagerAsync();
        }
        private async Task UserManagerAsync()
        {
            bool exists = await _context.BarberShopRoles.AnyAsync(r => r.Name == "Gestor de usuarios");

            if (!exists)
            {
                BarberShopRole role = new BarberShopRole { Name = "Gestor de usuarios" };
                await _context.BarberShopRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ContentManagerAsync()
        {
            bool exists = await _context.BarberShopRoles.AnyAsync(r => r.Name == "Gestor de contenido");

            if (!exists)
            {
                BarberShopRole role = new BarberShopRole { Name = "Gestor de contenido" };
                await _context.BarberShopRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.BarberShopRoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                BarberShopRole role = new BarberShopRole { Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.BarberShopRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
