namespace GeoQuiz_API.Startup;

public static class AppConfig
{
    public static WebApplication GetConfiguratedApp(string[] args)
    {
        var builder = BuilderConfig.GetConfiguratedBuilder(args);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.ApplyCorsConfig();

        return app;
    }
}
