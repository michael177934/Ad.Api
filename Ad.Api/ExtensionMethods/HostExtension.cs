namespace Ad.API.ExtensionMethods;

internal static class Startup
{
    internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        IWebHostEnvironment env = builder.Environment;
        builder.Configuration
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();
        return builder;
    }
}