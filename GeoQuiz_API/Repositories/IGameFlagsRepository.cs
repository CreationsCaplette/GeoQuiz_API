using GeoQuiz_API.Models.GeoQuiz;

namespace GeoQuiz_API.Repositories;

public interface IGameFlagsRepository
{
    Task<List<GeoQuizQuestion>> GetFlagsGame();
}
