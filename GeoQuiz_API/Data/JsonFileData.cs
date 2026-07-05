using GeoQuiz_API.Models.GeoQuiz;
using System.Text.Json;

namespace GeoQuiz_API.Data;

public class JsonFileData : IJsonFileData
{
    const string GeoQuizDataFileName = "GeoQuizData.json";

    public async Task<GeoQuizData?> GetDataFromFile()
    {
        if (!File.Exists(GeoQuizDataFileName))
            return null;

        var countriesJson = await File.ReadAllTextAsync(GeoQuizDataFileName);
        var countries = JsonSerializer.Deserialize<GeoQuizData>(countriesJson);
        return countries;
    }

    public async Task SaveDataToFile(List<GeoQuizCountry> countries)
    {
        var geoQuizData = new GeoQuizData(countries, DateTimeOffset.UtcNow);
        var dataJson = JsonSerializer.Serialize(geoQuizData);
        await File.WriteAllTextAsync(GeoQuizDataFileName, dataJson);
    }
}
