using Clinic_App_01.Models;
using Clinic_App_01.Repository;
using Microsoft.EntityFrameworkCore;

namespace Clinic_App_01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add JSON serializer & deserializer services to the container.
            builder.Services.AddControllersWithViews().AddNewtonsoftJson();
    

            builder.Services.AddHttpClient();
            //builder.Services.AddDbContext<ClinicContext>(
            //    options => options.UseSqlServer("Data Source=.;Initial Catalog=ClinicDB;Integrated Security=true;Encrypt=false"));

            builder.Services.AddDbContext<ClinicContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddDbContext<ClinicContext>(
            //    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
