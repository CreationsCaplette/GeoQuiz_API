using GeoQuiz_API.Models.GeoQuiz;
using System.Text.Json;

namespace GeoQuiz_API.Data;

public class JsonFileData : IJsonFileData
{
    const string GeoQuizDataFileName = "GeoQuizData.json";
    private readonly string GeoQuizDataFilePath;

    public JsonFileData()
    {
        GeoQuizDataFilePath = Path.Combine(AppContext.BaseDirectory, GeoQuizDataFileName);
    }

    public async Task<GeoQuizData?> GetDataFromFile()
    {
        if (!File.Exists(GeoQuizDataFilePath))
            return null;

        var countriesJson = await File.ReadAllTextAsync(GeoQuizDataFilePath);
        var countries = JsonSerializer.Deserialize<GeoQuizData>(countriesJson);
        return countries;
    }

    public async Task SaveDataToFile(List<GeoQuizCountry> countries)
    {
        var geoQuizData = new GeoQuizData(countries, DateTimeOffset.UtcNow);
        var dataJson = JsonSerializer.Serialize(geoQuizData);
        await File.WriteAllTextAsync(GeoQuizDataFilePath, dataJson);
    }
}
