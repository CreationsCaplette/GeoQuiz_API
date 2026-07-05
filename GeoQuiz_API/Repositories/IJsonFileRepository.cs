using GeoQuiz_API.Models.GeoQuiz;

namespace GeoQuiz_API.Repositories;

public interface IJsonFileRepository
{
    Task<GeoQuizData?> GetDataFromFile();
    Task SaveDataToFile(List<GeoQuizCountry> countries);
}
