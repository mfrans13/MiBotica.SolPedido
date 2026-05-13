using Microsoft.AspNetCore.Authentication.Cookies;
using log4net;
using log4net.Config;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllersWithViews();

// Agregar autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";      // Ruta del login
        options.LogoutPath = "/Login/Logout";    // Ruta del logout
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.AccessDeniedPath = "/Home/AccessDenied";  // ✅ única línea
    });

// Agregar servicio de sesión (opcional, para guardar datos del usuario)
builder.Services.AddSession();

// Configurar log4net desde el archivo de configuración
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

var app = builder.Build();

// Configurar pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();          // Asegura que sirva archivos estáticos (CSS, JS)
app.UseRouting();

// Middlewares importantes en el orden correcto
app.UseSession();              // Habilita sesión
app.UseAuthentication();       // Habilita autenticación
app.UseAuthorization();        // Habilita autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");   // Redirige al Login por defecto

app.Run();