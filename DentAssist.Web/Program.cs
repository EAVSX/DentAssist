// Program.cs
using DentAssist.Web.Datos;
using DentAssist.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;

// Archivo principal de arranque y configuración de la aplicación web.
// Registra servicios (DbContext, Identity, PDF, helpers), rutas y middleware para que el sistema funcione correctamente.

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────────
// 1) Configuración de la base de datos y autenticación (Identity)
// ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<DentAssistContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Configuración clásica de usuarios y roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<DentAssistContext>()
.AddDefaultTokenProviders();

// ──────────────────────────────────────────────────────────────
// 2) Servicio para generación de PDF (DinkToPdf)
// ──────────────────────────────────────────────────────────────
builder.Services.AddSingleton<IConverter>(
    new SynchronizedConverter(new PdfTools())
);

// Carga la librería nativa para PDF según la arquitectura
string archFolder = (IntPtr.Size == 8) ? "win-x64" : "win-x86";
string libPath = Path.Combine(builder.Environment.ContentRootPath,
                              "runtimes", archFolder, "native",
                              "libwkhtmltox.dll");
new CustomAssemblyLoadContext().LoadUnmanagedLibrary(libPath);

// ──────────────────────────────────────────────────────────────
// 3) Helpers y servicios auxiliares
// ──────────────────────────────────────────────────────────────
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<RazorViewToString>();

// ──────────────────────────────────────────────────────────────
// 4) MVC clásico (Controladores y Vistas)
// ──────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

// ──────────────────────────────────────────────────────────────
// 5) Seed inicial: crea roles y usuario administrador si no existen
// ──────────────────────────────────────────────────────────────
using (IServiceScope scope = app.Services.CreateScope())
{
    RoleManager<IdentityRole> roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    UserManager<IdentityUser> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Administrador", "Recepcionista", "Odontologo" };
    foreach (string r in roles)
    {
        if (!roleMgr.RoleExistsAsync(r).Result)
        {
            roleMgr.CreateAsync(new IdentityRole(r)).Wait();
        }
    }

    string adminEmail = "admin@dentassist.com";
    IdentityUser admin = userMgr.FindByEmailAsync(adminEmail).Result;
    if (admin == null)
    {
        admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
        IdentityResult creado = userMgr.CreateAsync(admin, "Admin123!").Result;
        if (creado.Succeeded)
        {
            userMgr.AddToRoleAsync(admin, "Administrador").Wait();
        }
    }
}

// ──────────────────────────────────────────────────────────────
// 6) Middleware: archivos estáticos, autenticación y autorización
// ──────────────────────────────────────────────────────────────
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ──────────────────────────────────────────────────────────────
// 7) Rutas principales y de áreas
// ──────────────────────────────────────────────────────────────
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Landing}/{action=Index}/{id?}"
);

app.Run();
