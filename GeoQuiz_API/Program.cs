using GeoQuiz_API.Repositories;
using GeoQuiz_API.Startup;

var app = AppConfig.GetConfiguratedApp(args);

app.MapGet("/countries/all", async (IGeoQuizRepository geoQuizRepo) =>
{
    return await geoQuizRepo.GetAllGeoQuizCountries();
})
.WithName("CountriesAll");

app.MapGet("/game/capitals", async (IGeoQuizRepository geoQuizRepo) =>
{
    return await geoQuizRepo.GetCapitalsGame();
})
.WithName("GameCapitals");

app.Run();
