namespace GeoQuiz_API.Startup;

public static class BuilderConfig
{
    public static WebApplicationBuilder GetConfiguratedBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddCorsServices();

        // Add Dependencies
        DependencyConfig.AddDependencies(builder);

        return builder;
    }
}
