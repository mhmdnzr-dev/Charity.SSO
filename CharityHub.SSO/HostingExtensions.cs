using CharityHub.SSO.Configs;
using Serilog;
using CharityHub.SSO.Extensions;
using Microsoft.Extensions.Options;


namespace CharityHub.SSO;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        
  


        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AllowAnonymousToFolder("/Account/Create");
        });

        builder.Services.AddCustomServices();
        builder.Services.AddCustomDbContext(appSettings.ConnectionStrings.DefaultConnection);
        builder.Services.AddCustomIdentityServer(appSettings.ConnectionStrings.DefaultConnection);
        builder.Services.AddCustomCors(appSettings.Cors.AllowedOrigins);
        builder.Services.AddCustomAuthentication();
        builder.Services.AddSession(); // Ensure session services are added

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSession();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication(); // Ensure authentication is applied
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}