using Human_Evolution.Data;
using Human_Evolution.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region 🌍 Localisation (FR/PT)

// Indique le dossier des fichiers de traduction
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Liste des cultures supportées
var supportedCultures = new[] { new CultureInfo("fr"), new CultureInfo("pt") };

// Configuration des options de localisation
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("fr"); // langue par défaut
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

#endregion

#region 📧 SMTP (envoi de mails)

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<MailService>();

#endregion

#region 🗄️ Base de données

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region 🧱 MVC + Razor + Localisation

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

#endregion

var app = builder.Build();

#region ✅ Middleware de localisation (doit être AVANT le reste)

// Récupère les options et active la localisation
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

#endregion

#region 🔐 Middlewares classiques

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

#endregion

#region 🚀 Routes

// Exemple : /contact redirige vers ContactController → Contact()
app.MapControllerRoute(
    name: "contact",
    pattern: "contact",
    defaults: new { controller = "Contact", action = "Contact" });

// Route par défaut : HomeController → Index()
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

app.Run();
