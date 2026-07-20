using GeoQuiz_API.Models.GeoQuiz;

namespace GeoQuiz_API.Repositories;

public interface IGameCapitalsReverseRepository
{
    Task<List<GeoQuizQuestion>> GetCapitalsReverseGame();
}
