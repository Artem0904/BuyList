using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Services.BotServices;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic.Exstensions
{
    public static class ServiceExtentions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        public static void AddCustomServices(this IServiceCollection services)
        {       
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBotUserService, BotUserService>();

        }
        public static void AddValidationServices(this IServiceCollection services)
        { 

        }
    }
}
