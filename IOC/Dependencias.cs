using BL.IServices;
using BL.Services;
using DA.CentralContext;
using DA.ClinicaContext;
using DA.IRepository;
using DA.IUOW;
using DA.Repository;
using DA.UOW;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IOC
{
    public static class Dependencias
    {
        public static void DependencyInyection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CPALCentralContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CpalCentralConexion"))
            );
            services.AddDbContext<CPALClinicaContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CpalClinicaConexion"))
            );

            services.AddTransient(typeof(IGenericRepositoryCentral<>), typeof(GenericRepositoryCentral<>));
            services.AddTransient(typeof(IGenericRepositoryClinica<>), typeof(GenericRepositoryClinica<>));
            services.AddTransient(typeof(ICPALCentralContextProcedures), typeof(CPALCentralContextProcedures));
            services.AddTransient(typeof(ICPALClinicaContextProcedures), typeof(CPALClinicaContextProcedures));

            services.AddScoped<IUnitOfWorkClinica, UnitOfWorkClinica>();
            services.AddScoped<IUnitOfWorkCentral, UnitOfWorkCentral>();
            services.AddScoped(typeof(IUserManagerService<>), typeof(UserManagerService<>));
            services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
            services.AddScoped(typeof(IRoleManagerService<>), typeof(RoleManagerService<>));
            services.AddScoped(typeof(IRoleRepository<>), typeof(RoleRepository<>));
            services.AddScoped(typeof(ISingInManagerService<>), typeof(SingInManagerService<>));
            services.AddScoped(typeof(ISingInRepository<>), typeof(SingInRepository<>));

            services.AddScoped(typeof(IConfigSistemaService), typeof(ConfigSistemaService));
            services.AddScoped(typeof(IEmailQueueService), typeof(EmailQueueService));
            services.AddScoped(typeof(IUsuarioService), typeof(UsuarioService));
            services.AddScoped(typeof(IUbigeoService), typeof(UbigeoService));

            services.AddScoped(typeof(ITriajeOnlineService), typeof(TriajeOnlineService));
            services.AddScoped(typeof(IPagosTriajeService), typeof(PagosTriajeService));
            services.AddScoped(typeof(IEspecialistaService), typeof(EspecialistaService));
            services.AddScoped(typeof(IPacienteService), typeof(PacienteService));
            services.AddScoped(typeof(IPagoService), typeof(PagoService));
            services.AddScoped(typeof(IPagoExternoService), typeof(PagoExternoService));
            services.AddScoped(typeof(IPagoExternoVoucherService), typeof(PagoExternoVoucherService));
            services.AddScoped(typeof(ICitasService), typeof(CitasService));
            services.AddScoped(typeof(IPagoCitaService), typeof(PagoCitaService));
            services.AddScoped(typeof(IEvaluacionCitasService), typeof(EvaluacionCitasService));
            services.AddScoped(typeof(ILogService), typeof(LogService));
            services.AddScoped(typeof(IReservaTratamientoService), typeof(ReservaTratamientoService));
            //services.AddHostedService<ExpirarCitasService>();

            //Por Revisar
            services.AddSingleton<ITranslationService, TranslationService>();
        }
    }
}
