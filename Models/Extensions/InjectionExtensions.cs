using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.FileStorage;

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
            services.AddScoped<UsuarioService>();
            services.AddScoped<SacramentoService>();
            services.AddScoped<TipoDocumentoService>();
            services.AddScoped<TipoSexoService>();
            services.AddScoped<CertificationService>();

            services.AddSingleton<ImageStorage>();
            services.AddTransient<ImageService>();

            return services;
        }
    }
}
