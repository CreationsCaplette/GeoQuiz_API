using GeoQuiz_API.Data;
using GeoQuiz_API.Models;
using GeoQuiz_API.Models.GeoQuiz;
using System.Runtime.InteropServices;

namespace GeoQuiz_API.Repositories;

public class GeoQuizRepository(
    IConfiguration config,
    IJsonFileData jsonFileRepository,
    IRestCountriesData restCountriesRepository
    ) : IGeoQuizRepository
{
    const string ConfigApiKey = "RestCountriesApiKey";
    const int NumberOfQuestions = 10;
    const int NumberOfChoices = 4;

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

    static private bool IsDataStillValid(GeoQuizData data) =>
        DateTime.UtcNow - data.TimeStamp <= TimeSpan.FromDays(1);

    public async Task<List<GeoQuizQuestion>> GetCapitalsGame()
    {
        var countries = await GetAllGeoQuizCountries();
        var random = new Random();

        var questions = new HashSet<GeoQuizQuestion>(new GeoQuizQuestionComparer());

        while (questions.Count < NumberOfQuestions)
        {
            questions.Add(GetGeoQuizQuestion(countries, random));
        }

        return [.. questions];
    }

    static private GeoQuizCountry GetRandomCountry(List<GeoQuizCountry> countries, Random random)
    {
        var index = random.Next(countries.Count);
        return countries[index];
    }

    static private GeoQuizQuestion GetGeoQuizQuestion(List<GeoQuizCountry> countries, Random random)
    {
        var question = GetRandomCountry(countries, random);
        var choices = GetQuestionChoices(question.CapitalName, countries, random);
        random.Shuffle(CollectionsMarshal.AsSpan(choices));

        return new GeoQuizQuestion(question.CountryName, choices, choices.IndexOf(question.CapitalName));
    }

    static private List<string> GetQuestionChoices(string answer, List<GeoQuizCountry> countries, Random random)
    {
        var choices = new HashSet<string>
        {
            answer
        };

        while (choices.Count < NumberOfChoices)
        {
            choices.Add(GetRandomCountry(countries, random).CapitalName);
        }

        return [.. choices];
    }
}
