using GeoQuiz_API.Helper;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;

namespace GeoQuiz_API.Repositories;

public class GameCapitalsRepository(
    IOptions<GeoQuizGameOptions> options,
    IGeoQuizRepository geoQuizRepository,
    IRandomProvider randomProvider
) : BaseGameRepository(options, geoQuizRepository, randomProvider), IGameCapitalsRepository
{
    public Task<List<GeoQuizQuestion>> GetCapitalsGame() =>
        GetGameQuestions(
            country => country.CountryName,
            country => country.CapitalName
        );
}
