namespace CharityHub.SSO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Build the configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensure base path is set correctly
            .AddJsonFile("appsettings.json") // Add appsettings.json
            .Build();

        // Retrieve the connection string from the appsettings section "AppSettings:ConnectionStrings:DefaultConnection"
        var connectionString = configuration.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value;
        
        // Configure DbContext with the connection string
        optionsBuilder.UseSqlServer(connectionString);
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}