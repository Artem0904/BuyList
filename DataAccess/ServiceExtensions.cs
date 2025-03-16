using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Repositories;
using DataAccess.Repostories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace DataAccess
{
    public static class ServiceExtensions
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BuyListDbContext>(opts => opts.UseNpgsql(connectionString));  
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void DataBaseMigrate(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<BuyListDbContext>();
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source);
            }
        }
        public static void AddUploadingsFolder(this WebApplication app, string CurrentDirectoryPath)
        {
            string imagesDirPath = Path.Combine(CurrentDirectoryPath, app.Configuration["DirImages"]!);

            if (!Directory.Exists(imagesDirPath))
            {
                Directory.CreateDirectory(imagesDirPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(imagesDirPath),
                RequestPath = "/" + app.Configuration["DirImages"]
            });
        }
    }
}
