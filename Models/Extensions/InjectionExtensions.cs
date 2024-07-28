using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApi_SGI_T.Imp;

namespace WebApi_SGI_T.Models.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(SgiSacramentosContext).Assembly.FullName;

            services.AddDbContext<SgiSacramentosContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SGIConnection"), b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient
            );

            services.AddScoped<TipoSacramentoService>();

            return services;
        }
    }
}
