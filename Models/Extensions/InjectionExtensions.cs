using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Imp.Constancias;
using WebApi_SGI_T.Imp.FileStorage;
using WebApi_SGI_T.Imp.Matrimonio;
using WebApi_SGI_T.Imp.Sacramentos;
using WebApi_SGI_T.Models.Extensions.WatchDog;

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
            services.AddScoped<HistoricoConstanciasService>();
            services.AddScoped<DashboardService>();
            services.AddScoped<RolService>();
            services.AddScoped<PermissionService>();
            services.AddScoped<UserRolService>();
            services.AddScoped<CategoriaSacerdoteService>();
            services.AddScoped<SacerdoteService>();
            services.AddScoped<FirmaService>();
            services.AddScoped<DataInitial>();
            services.AddScoped<matrimonioDeleteService>();
            services.AddScoped<MatrimonioById>();
            services.AddScoped<EliminarSacramentoService>();
            services.AddScoped<ConstanciaByIdService>();
            services.AddScoped<AnularConstanciaService>();
            services.AddScoped<ConsultaAnulacionService>();

            services.AddSingleton<ImageStorage>();
            services.AddTransient<ImageService>();
            //services.AddWatchDog();

            return services;
        }
    }
}
