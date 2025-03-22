using CharityHub.SSO;
using CharityHub.SSO.Configs;
using CharityHub.SSO.Data;
using CharityHub.SSO.Models;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Hosting;

Console.WriteLine("Starting up");

var builder = WebApplication.CreateBuilder(args);

// ✅ Proper Serilog setup
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);





// ✅ Ensure authentication is not being added multiple times
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddSingleton<DiagnosticContext>();

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// ✅ Apply all migrations before starting the app
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContexts = new DbContext[]
    {
        services.GetRequiredService<ApplicationDbContext>(),
        services.GetRequiredService<ConfigurationDbContext>(),
        services.GetRequiredService<PersistedGrantDbContext>()
    };

    foreach (var dbContext in dbContexts)
    {
        Console.WriteLine($"Applying migrations for {dbContext.GetType().Name}...");
        dbContext.Database.Migrate();
    }

    Console.WriteLine("Database is up to date.");
}

if (args.Contains("/seed"))
{
    Console.WriteLine("Seeding database...");
    await SeedData.EnsureSeedDataAsync(app);
    Console.WriteLine("Done seeding database. Exiting.");
    return;
}

app.Run();

Console.WriteLine("Shut down complete");
