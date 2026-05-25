using Microsoft.AspNetCore.Authentication.Cookies;
using CapaDatos;
using CapaPresentacion.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar cadena de conexión global
ConexionDAL.CadenaConexion = builder.Configuration.GetConnectionString("ElFogonBD");

// 2. Configurar Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(Opciones =>
    {
        Opciones.LoginPath = "/Acceso/Login";         // Si no está logueado
        Opciones.AccessDeniedPath = "/Acceso/AccesoDenegado"; // Si no tiene permiso
        Opciones.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        Opciones.SlidingExpiration = true; // Renueva cookie si hay actividad
    });

builder.Services.AddAuthorization();

// 3. Registrar Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

//primero Authentication, luego Authorization, luego el middleware de permisos
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<VerificadorPermisosMiddleware>();

app.MapRazorPages();
app.Run();