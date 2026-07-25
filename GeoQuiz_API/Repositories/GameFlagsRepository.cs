using GeoQuiz_API.Helper;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;

namespace GeoQuiz_API.Repositories;

public class GameFlagsRepository(
    IOptions<GeoQuizGameOptions> options,
    IGeoQuizRepository geoQuizRepository,
    IRandomProvider randomProvider
) : BaseGameRepository(options, geoQuizRepository, randomProvider), IGameFlagsRepository
{
    public Task<List<GeoQuizQuestion>> GetFlagsGame() =>
        GetGameQuestions(
            country => country.FlagUrl,
            country => country.CountryName
        );
}
