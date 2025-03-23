namespace CharityHub.SSO.Configs;

public class AppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
    public required CorsSettings Cors { get; set; }
}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }
}

public class CorsSettings
{
    public required string[] AllowedOrigins { get; set; }
}


