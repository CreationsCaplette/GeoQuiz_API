using GeoQuiz_API.Data;
using GeoQuiz_API.Helper;
using GeoQuiz_API.Repositories;

namespace GeoQuiz_API.Startup;

public static class AppConfig
{
    public static WebApplication BuildAndConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddOpenApi();
        builder.Services.AddCorsServices();

        builder.Configuration.AddUserSecrets<Program>();

        // Register dependencies
        builder.Services.AddTransient<IJsonFileData, JsonFileData>();
        builder.Services.AddTransient<IRestCountriesData, RestCountriesData>();
        builder.Services.AddSingleton<IRandomProvider, RandomProvider>();
        builder.Services.Configure<GeoQuizGameOptions>(
            builder.Configuration.GetSection(GeoQuizGameOptions.SectionName)
        );
        builder.Services.AddScoped<IGeoQuizRepository, GeoQuizRepository>();

        var app = builder.Build();

        // Configure middleware
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.ApplyCorsConfig();

        return app;
    }
}
