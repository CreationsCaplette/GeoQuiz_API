using GeoQuiz_API.Data;
using GeoQuiz_API.Helper;
using GeoQuiz_API.Models;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace GeoQuiz_API.Repositories;

public class GeoQuizRepository : IGeoQuizRepository
{
    private readonly int NumberOfQuestions;
    private readonly int NumberOfChoices;
    private readonly string ApiKey;

    private readonly IJsonFileData jsonFileRepository;
    private readonly IRestCountriesData restCountriesRepository;
    private readonly IRandomProvider randomProvider;

    public GeoQuizRepository(
        IOptions<GeoQuizGameOptions> options,
        IJsonFileData jsonFileRepository,
        IRestCountriesData restCountriesRepository,
        IRandomProvider randomProvider
    )
    {
        var geoQuizOptions = options.Value;
        ApiKey = geoQuizOptions.RestCountriesApiKey;
        NumberOfQuestions = geoQuizOptions.NumberOfQuestions;
        NumberOfChoices = geoQuizOptions.NumberOfChoices;

        this.jsonFileRepository = jsonFileRepository;
        this.restCountriesRepository = restCountriesRepository;
        this.randomProvider = randomProvider;
    }

    public async Task<List<GeoQuizCountry>> GetAllGeoQuizCountries()
    {
        var data = await jsonFileRepository.GetDataFromFile();
        if (data is not null && IsDataStillValid(data))
            return data.Countries;

        var countriesObject = await restCountriesRepository.GetAllRestCountriesObjects(ApiKey);
        var countries = countriesObject.ConvertToGeoQuizCountries();

        await jsonFileRepository.SaveDataToFile(countries);

        return countries;
    }

    static private bool IsDataStillValid(GeoQuizData data) =>
        DateTime.UtcNow - data.TimeStamp <= TimeSpan.FromDays(1);

    public async Task<List<GeoQuizQuestion>> GetCapitalsGame()
    {
        var countries = await GetAllGeoQuizCountries();

        var questions = new HashSet<GeoQuizQuestion>(new GeoQuizQuestionComparer());

        while (questions.Count < NumberOfQuestions)
        {
            questions.Add(GetGeoQuizQuestion(countries));
        }

        return [.. questions];
    }

    private GeoQuizCountry GetRandomCountry(List<GeoQuizCountry> countries)
    {
        var index = randomProvider.Next(countries.Count);
        return countries[index];
    }

    private GeoQuizQuestion GetGeoQuizQuestion(List<GeoQuizCountry> countries)
    {
        var question = GetRandomCountry(countries);
        var choices = GetQuestionChoices(question.CapitalName, countries);
        randomProvider.Shuffle(CollectionsMarshal.AsSpan(choices));

        return new GeoQuizQuestion(question.CountryName, choices, choices.IndexOf(question.CapitalName));
    }

    private List<string> GetQuestionChoices(string answer, List<GeoQuizCountry> countries)
    {
        var choices = new HashSet<string>
        {
            answer
        };

        while (choices.Count < NumberOfChoices)
        {
            choices.Add(GetRandomCountry(countries).CapitalName);
        }

        return [.. choices];
    }
}
