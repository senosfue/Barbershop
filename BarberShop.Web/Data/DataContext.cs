using BarberShop.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Permission> Permissions  { get; set; }
        public DbSet<BarberShopRole> BarberShopRoles  { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Haircut> Haircuts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureKeys(builder);
            ConfigureIndexes(builder);

            base.OnModelCreating(builder);
        }

        private void ConfigureKeys(ModelBuilder builder)
        {
            // Role Permissions
            builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>().HasOne(rp => rp.Role)
                                            .WithMany(r => r.RolePermissions)
                                            .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                            .WithMany(p => p.RolePermissions)
                                            .HasForeignKey(rp => rp.PermissionId);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            // Roles
            builder.Entity<BarberShopRole>().HasIndex(r => r.Name)
                                             .IsUnique();
            
            // Users
            builder.Entity<User>().HasIndex(u => u.Document)
                                             .IsUnique();
        }

    }
}
