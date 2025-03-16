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
        }
        public static void AddValidationServices(this IServiceCollection services)
        { 

        }
    }
}
