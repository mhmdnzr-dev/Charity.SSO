using CharityHub.SSO.Data;
using Microsoft.EntityFrameworkCore;

namespace CharityHub.SSO.Extensions;
public static class DbContextExtensions
{
    public static void AddCustomDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}
