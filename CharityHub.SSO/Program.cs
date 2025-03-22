using CharityHub.SSO;
using CharityHub.SSO.Configs;
using Serilog;

Console.WriteLine("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(ctx.Configuration));

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddSingleton(appSettings);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

if (args.Contains("/seed"))
{
    Console.WriteLine("Seeding database...");
    await SeedData.EnsureSeedDataAsync(app);
    Console.WriteLine("Done seeding database. Exiting.");
    return;
}

app.Run();

Console.WriteLine("Shut down complete");