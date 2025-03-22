using CharityHub.SSO;
using Serilog;

Console.WriteLine("Starting up");

var builder = WebApplication.CreateBuilder(args);



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