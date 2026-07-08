using GeoQuiz_API.Data;
using GeoQuiz_API.Helper;
using GeoQuiz_API.Middleware;
using GeoQuiz_API.Repositories;
using Polly;

namespace GeoQuiz_API.Startup;

public static class AppConfig
{
    public static WebApplication BuildAndConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register Configuration
        builder.Configuration.AddUserSecrets<Program>();

        // Register services
        builder.Services.AddOpenApi();
        builder.Services.AddCorsServices();
        builder.Services.AddLogging(config =>
        {
            config.AddConsole();
            config.AddDebug();
        });

        // Register HttpClientFactory
        builder.Services.AddHttpClient("RestCountriesClient")
            .ConfigureHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: attempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, attempt))));

        // Register dependencies
        builder.Services.AddTransient<IJsonFileData, JsonFileData>();
        builder.Services.AddTransient<IRestCountriesData, RestCountriesData>();
        builder.Services.AddSingleton<IRandomProvider, RandomProvider>();
        builder.Services.Configure<GeoQuizGameOptions>(
            builder.Configuration.GetSection(GeoQuizGameOptions.SectionName)
        );
        builder.Services.AddScoped<IGeoQuizRepository, GeoQuizRepository>();
        builder.Services.AddScoped<IGameCapitalsRepository, GameCapitalsRepository>();

        var app = builder.Build();

        // Configure middleware
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.ApplyCorsConfig();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
