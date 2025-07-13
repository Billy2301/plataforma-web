using BL.Services;
using DA.CentralContext;
using DinkToPdf;
using DinkToPdf.Contracts;
using IOC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PortalClienteV2.Models.Model;
using PortalClienteV2.Utilities.AutoMapper;
using PortalClienteV2.Utilities.ErrorHandler;
using PortalClienteV2.Utilities.Helpers;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var builder = WebApplication.CreateBuilder(args);

#region Identity Configuration
//Agregar el servicio de identity a la aplicacion
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CPALCentralContext>().AddDefaultTokenProviders();
//esta linea es para la URL de retorno al acceder
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromDays(30); // Establecer el tiempo de expiración de las cookies persistentes 'FromDays,FromMinute,FromSeconds'
    option.LoginPath = new PathString("/Acceso/IniciarSesion");
    option.AccessDeniedPath = new PathString("/Acceso/AccesoDenegado");
    option.SlidingExpiration = true; // Renovar automáticamente la cookie si está cerca de expirar
});
//Estas son configuraciones de identity
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequiredLength = 6;
    option.Password.RequireLowercase = true;
    option.Password.RequireUppercase = true;
    option.Password.RequireDigit = true;
    option.Password.RequireNonAlphanumeric = true;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    option.Lockout.MaxFailedAccessAttempts = 3;
});

//Agregar autenticacion de facebook
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = config.GetValue<string>("Facebook_key:id");
    option.AppSecret = config.GetValue<string>("Facebook_key:secret");
});

//Agregar autenticacion de Google
builder.Services.AddAuthentication().AddGoogle(option =>
{
    option.ClientId = config.GetValue<string>("Google_key:id");
    option.ClientSecret = config.GetValue<string>("Google_key:secret");
});

//Agregar autenticacion de Microsoft
builder.Services.AddAuthentication().AddMicrosoftAccount(option =>
{
    option.ClientId = config.GetValue<string>("Microsoft_key:id");
    option.ClientSecret = config.GetValue<string>("Microsoft_key:secret");
});
#endregion 

// Configuración de servicios
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(180); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar HotjarSettings
builder.Services.Configure<HotjarSettings>(builder.Configuration.GetSection("Hotjar"));

builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services AutoMapper al contenedor de servicios
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add services to the IOC.
builder.Services.DependencyInyection(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // codigo de prueba

var rutaDllPdf = config.GetValue<string>("LibreriaPDF");
if (string.IsNullOrEmpty(rutaDllPdf)) { rutaDllPdf = Path.Combine(Directory.GetCurrentDirectory(), "Utilities/LibreriaPDF/libwkhtmltox.dll"); }
var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(rutaDllPdf);
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Configure the HTTP request pipeline.  //IsDevelopment
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting(); // Esto establece el enrutamiento

app.UseAuthentication(); // Si tienes autenticación, debe ir antes de la autorización
app.UseAuthorization(); // Esto habilita el middleware de autorización
app.UseSession(); // Agregar el middleware de sesión

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Mapear rutas de los controladores
    endpoints.MapHub<CalendarHub>("/calendarHub"); // Mapear el Hub de SignalR
});



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Index}/{id?}");

app.Run();
