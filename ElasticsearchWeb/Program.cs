using ElasticsearchWeb.Repositories;
using ElasticsearchWeb.Repository;
using ElasticsearchWeb.Services;
using ElasticSearchWeb.Extension;

namespace ElasticsearchWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddElastic(builder.Configuration);
            builder.Services.AddScoped<BlogRepository>();
            builder.Services.AddScoped<BlogService>();
            builder.Services.AddScoped<EcommerceService>();
            builder.Services.AddScoped<EcommerceRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
