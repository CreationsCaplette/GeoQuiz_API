using GeoQuiz_API.Models.GeoQuiz;

namespace GeoQuiz_API.Data;

public interface IJsonFileData
{
    Task<GeoQuizData?> GetDataFromFile();
    Task SaveDataToFile(List<GeoQuizCountry> countries);
}
