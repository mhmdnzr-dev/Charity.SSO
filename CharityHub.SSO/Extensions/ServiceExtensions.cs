using CharityHub.SSO.Pages.Admin.ApiScopes;
using CharityHub.SSO.Pages.Admin.Clients;
using CharityHub.SSO.Pages.Admin.IdentityScopes;
using PortalClientRepository = CharityHub.SSO.Pages.Portal.ClientRepository;


namespace CharityHub.SSO.Extensions;
public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<PortalClientRepository>();
        services.AddTransient<ClientRepository>();
        services.AddTransient<IdentityScopeRepository>();
        services.AddTransient<ApiScopeRepository>();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = false;
            options.Cookie.IsEssential = true;
        });
    }
}
