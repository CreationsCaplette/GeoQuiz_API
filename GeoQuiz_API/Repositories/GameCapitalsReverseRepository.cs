using GeoQuiz_API.Helper;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;

namespace GeoQuiz_API.Repositories;

public class GameCapitalsReverseRepository(
    IOptions<GeoQuizGameOptions> options,
    IGeoQuizRepository geoQuizRepository,
    IRandomProvider randomProvider
) : BaseGameCapitalsRepository(options, geoQuizRepository, randomProvider), IGameCapitalsReverseRepository
{
    public Task<List<GeoQuizQuestion>> GetCapitalsReverseGame() =>
        GetGameQuestions(
            country => country.CapitalName,
            country => country.CountryName
        );
}