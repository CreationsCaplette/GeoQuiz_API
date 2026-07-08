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
.WithDescription("Get a capitals quiz game with randomized questions")
.Produces<List<GeoQuizQuestion>>(StatusCodes.Status200OK);

app.Run();
