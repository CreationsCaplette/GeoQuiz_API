using GeoQuiz_API.Data;
using GeoQuiz_API.Models;
using GeoQuiz_API.Models.GeoQuiz;

namespace GeoQuiz_API.Repositories;

public class GeoQuizRepository(
    IConfiguration config,
    IJsonFileData jsonFileRepository,
    IRestCountriesData restCountriesRepository
    ) : IGeoQuizRepository
{
    const string ConfigApiKey = "RestCountriesApiKey";

    public async Task<List<GeoQuizCountry>> GetAllGeoQuizCountries()
    {
        var data = await jsonFileRepository.GetDataFromFile();
        if (data is not null && IsDataStillValid(data))
            return data.Countries;

        var apiKey = config.GetValue<string>(ConfigApiKey);
        var countriesObject = await restCountriesRepository.GetAllRestCountriesObjects(apiKey);
        var countries = countriesObject.ConvertToGeoQuizCountries();

        await jsonFileRepository.SaveDataToFile(countries);

        return countries;
    }

    public async Task<List<GeoQuizQuestion>> GetCapitalsGame()
    {
        var countries = await GetAllGeoQuizCountries();

        var random = new Random();

        var questionIndex = random.Next(countries.Count);
        var question = countries[questionIndex];

        var choice1Index = random.Next(countries.Count);
        var choice1 = countries[choice1Index];

        var choice2Index = random.Next(countries.Count);
        var choice2 = countries[choice2Index];

        var choice3Index = random.Next(countries.Count);
        var choice3 = countries[choice3Index];

        var geoQuizQuestion = new GeoQuizQuestion(question.CountryName, [question.CapitalName, choice1.CapitalName, choice2.CapitalName, choice3.CapitalName], 0);

        return [geoQuizQuestion];
    }

    static private bool IsDataStillValid(GeoQuizData data)
    {
        var dateDiff = DateTime.UtcNow - data.TimeStamp;
        if (dateDiff > TimeSpan.FromDays(1))
            return false;
        return true;
    }
}
