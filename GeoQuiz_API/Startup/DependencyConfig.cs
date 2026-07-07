using GeoQuiz_API.Data;
using GeoQuiz_API.Repositories;

namespace GeoQuiz_API.Startup;

public static class DependencyConfig
{
    public static void AddDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IJsonFileData, JsonFileData>();
        builder.Services.AddTransient<IRestCountriesData, RestCountriesData>();
        builder.Services.AddTransient<IGeoQuizRepository, GeoQuizRepository>();
    }
}
