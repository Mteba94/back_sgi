using System.Reflection;

namespace WebApi_SGI_T.Imp.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionImp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            return services;
        }
    }
}
