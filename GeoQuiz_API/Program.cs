using GeoQuiz_API.Models.GeoQuiz;
using GeoQuiz_API.Repositories;
using GeoQuiz_API.Startup;

var app = AppConfig.BuildAndConfigureApp(args);

app.MapGet("/countries/all", async (IGeoQuizRepository geoQuizRepo) =>
{
    return await geoQuizRepo.GetAllGeoQuizCountries();
})
.WithName("CountriesAll")
.WithDescription("Get all the countries information")
.Produces<List<GeoQuizCountry>>(StatusCodes.Status200OK);

app.MapGet("/game/capitals", async (IGameCapitalsRepository gameCapitalsRepo) =>
{
    return await gameCapitalsRepo.GetCapitalsGame();
})
.WithName("GameCapitals")
.WithDescription("Generates a randomized capitals quiz game")
.Produces<List<GeoQuizQuestion>>(StatusCodes.Status200OK);

app.MapGet("/game/capitals_reverse", async (IGameCapitalsReverseRepository gameCapitalsReverseRepo) =>
{
    return await gameCapitalsReverseRepo.GetCapitalsReverseGame();
})
.WithName("GameCapitalsReverse")
.WithDescription("Generates a randomized capitals reverse quiz game")
.Produces<List<GeoQuizQuestion>>(StatusCodes.Status200OK);

app.Run();
