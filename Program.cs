using DynamicSun.Data;
using DynamicSun.Repository;
using DynamicSun.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DynamicSun
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DynamicSunDb"));
            });

            builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

            var app = builder.Build();           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Weather}/{action=List}");

            app.Run();
        }
    }
}
