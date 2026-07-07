using GeoQuiz_API.Models.GeoQuiz;

namespace GeoQuiz_API.Repositories;

public interface IGameCapitalsRepository
{
    Task<List<GeoQuizQuestion>> GetCapitalsGame();
}
