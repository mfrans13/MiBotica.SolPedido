using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllersWithViews();

// Agregar autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";    // Ruta del login
        options.LogoutPath = "/Login/Logout";  // Ruta del logout
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.AccessDeniedPath = "/Home/AccesoDenegado";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

// Agregar servicio de sesión (opcional, para guardar datos del usuario)
builder.Services.AddSession();

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