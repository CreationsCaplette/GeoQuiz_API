using GeoQuiz_API.Models.GeoQuiz;

namespace GeoQuiz_API.Repositories;

public interface IGeoQuizRepository
{
    Task<List<GeoQuizCountry>> GetAllGeoQuizCountries();
    Task<List<GeoQuizQuestion>> GetCapitalsGame();
}
