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


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────────
// 1)  DbContext  +  Identity
// ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<DentAssistContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

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
// 2)  DinkToPdf  (PDF)
// ──────────────────────────────────────────────────────────────
builder.Services.AddSingleton<IConverter>(
    new SynchronizedConverter(new PdfTools())
);

string archFolder = (IntPtr.Size == 8) ? "win-x64" : "win-x86";
string libPath = Path.Combine(builder.Environment.ContentRootPath,
                              "runtimes", archFolder, "native",
                              "libwkhtmltox.dll");
new CustomAssemblyLoadContext().LoadUnmanagedLibrary(libPath);

// ──────────────────────────────────────────────────────────────
// 3)  Helper Razor → renderizar vistas a string
// ──────────────────────────────────────────────────────────────
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<RazorViewToString>();

// ──────────────────────────────────────────────────────────────
// 4)  MVC
// ──────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

// ──────────────────────────────────────────────────────────────
// 5)  Seed de roles + usuario Administrador (igual que antes)
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
// 6)  Middleware
// ──────────────────────────────────────────────────────────────
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ──────────────────────────────────────────────────────────────
// 7)  Rutas
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
