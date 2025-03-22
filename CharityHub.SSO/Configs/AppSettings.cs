namespace CharityHub.SSO.Configs;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public CorsSettings Cors { get; set; }
    public SerilogSettings Serilog { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; }
}

public class SerilogSettings
{
    public string MinimumLevel { get; set; }
    public List<SerilogSink> WriteTo { get; set; }
}

public class SerilogSink
{
    public string Name { get; set; }
    public Dictionary<string, object> Args { get; set; }
}
