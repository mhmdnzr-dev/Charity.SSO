using CharityHub.SSO;
using CharityHub.SSO.Configs;
using CharityHub.SSO.Data;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

Console.WriteLine("Starting up");

var builder = WebApplication.CreateBuilder(args);
// Bind AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

// Configure Serilog using AppSettings
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration.GetSection("AppSettings:Serilog"))
    .Enrich.FromLogContext()
    .CreateLogger();



try
{
    var app = builder.ConfigureServices().ConfigurePipeline();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var dbContexts = new DbContext[]
    {
        services.GetRequiredService<ApplicationDbContext>(), 
        services.GetRequiredService<ConfigurationDbContext>(),
        services.GetRequiredService<PersistedGrantDbContext>()
    };

    foreach (var dbContext in dbContexts)
    {
        try
        {
            Console.WriteLine($"Applying migrations for {dbContext.GetType().Name}...");
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration failed for {dbContext.GetType().Name}: {ex.Message}");
            Log.Error(ex, "Migration error for {DbContext}", dbContext.GetType().Name);
            throw;
        }
    }

    Console.WriteLine("Database is up to date.");

    if (args.Contains("/seed"))
    {
        Console.WriteLine("Seeding database...");
        await SeedData.EnsureSeedDataAsync(app);
        Console.WriteLine("Done seeding database. Exiting.");
        return;
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed.");
}
finally
{
    Log.CloseAndFlush();
}

Console.WriteLine("Shut down complete");