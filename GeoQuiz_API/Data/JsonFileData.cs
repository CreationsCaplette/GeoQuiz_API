using GeoQuiz_API.Models.GeoQuiz;
using System.Text.Json;

namespace GeoQuiz_API.Data;

public class JsonFileData(ILogger<JsonFileData> logger) : IJsonFileData
{
    const string GeoQuizDataFileName = "GeoQuizData.json";
    private readonly string GeoQuizDataFilePath = Path.Combine(AppContext.BaseDirectory, GeoQuizDataFileName);

    public async Task<GeoQuizData?> GetDataFromFile()
    {
        try
        {
            if (!File.Exists(GeoQuizDataFilePath))
                return null;

            var countriesJson = await File.ReadAllTextAsync(GeoQuizDataFilePath);
            var countries = JsonSerializer.Deserialize<GeoQuizData>(countriesJson);
            return countries;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize GeoQuizData from file");
            return null;
        }
        catch (IOException ex)
        {
            logger.LogError(ex, "File I/O error while reading GeoQuizData");
            return null;
        }
    }

    public async Task SaveDataToFile(List<GeoQuizCountry> countries)
    {
        try
        {
            var geoQuizData = new GeoQuizData(countries, DateTimeOffset.UtcNow);
            var dataJson = JsonSerializer.Serialize(geoQuizData);
            await File.WriteAllTextAsync(GeoQuizDataFilePath, dataJson);
        }
        catch (IOException ex)
        {
            logger.LogError(ex, "Failed to save GeoQuizData to file");
            throw;
        }
    }
}
