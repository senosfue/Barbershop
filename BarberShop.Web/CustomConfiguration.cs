using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using BarberShop.Web.Data;
using BarberShop.Web.Helpers;
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

            //Toast Notification
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomLeft;
            });

            return builder;


        }
        public static void AddServices(this WebApplicationBuilder builder)
        {
            //services
            builder.Services.AddScoped<IHaircutServices, HaircutServices>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            //helpers
            builder.Services.AddScoped<ICombosHelper, CombosHelper>();
            builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
        }
        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();
            return app;
        }
    }       
}
