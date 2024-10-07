using BarberShop.Web.Data;
using BarberShop.Web.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Web
{
    public static class CustomConfiguration 
    {

        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {

            // Data Context

            builder.Services.AddDbContext<DataContext>(configuration =>
            {
                configuration.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));

            });
            // Services
            AddServices(builder);

            return builder;


        }
        public static void AddServices(this WebApplicationBuilder builder)
        {
            //services
            builder.Services.AddScoped<IHaircutServices, HaircutServices>();
        }
    }       
}
