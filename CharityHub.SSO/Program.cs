using CharityHub.SSO;
using Microsoft.AspNetCore.DataProtection;
using Serilog;

Console.WriteLine("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/home/app/.aspnet/DataProtection-Keys"))
    .SetApplicationName("CharityHubSSO");


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(); 


try
{
    var app = builder.ConfigureServices().ConfigurePipeline();
    
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