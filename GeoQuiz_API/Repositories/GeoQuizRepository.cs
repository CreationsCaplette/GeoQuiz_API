using GeoQuiz_API.Data;
using GeoQuiz_API.Helper;
using GeoQuiz_API.Models;
using GeoQuiz_API.Models.GeoQuiz;
using System.Runtime.InteropServices;

namespace GeoQuiz_API.Repositories;

public class GeoQuizRepository : IGeoQuizRepository
{
    private const string RestCountriesApiKeyConfig = "RestCountriesApiKey";
    private const string GameConfigSection = "GameConfig";
    private const string NumberOfQuestionsConfig = "NumberOfQuestions";
    private const string NumberOfChoicesConfig = "NumberOfChoices";

    private readonly string ApiKey;
    private readonly int NumberOfQuestions;
    private readonly int NumberOfChoices;

    private readonly IJsonFileData jsonFileRepository;
    private readonly IRestCountriesData restCountriesRepository;
    private readonly IRandomProvider randomProvider;

    public GeoQuizRepository(
        IConfiguration config,
        IJsonFileData jsonFileRepository,
        IRestCountriesData restCountriesRepository,
        IRandomProvider randomProvider
    )
    {
        this.jsonFileRepository = jsonFileRepository;
        this.restCountriesRepository = restCountriesRepository;

        this.randomProvider = randomProvider;

        ApiKey = config.GetValue<string>(RestCountriesApiKeyConfig) ?? "";

        if (string.IsNullOrEmpty(ApiKey))
            throw new InvalidOperationException("Rest Countries API key not found");

        NumberOfQuestions = config.GetSection(GameConfigSection).GetValue<int>(NumberOfQuestionsConfig);
        NumberOfChoices = config.GetSection(GameConfigSection).GetValue<int>(NumberOfChoicesConfig);

        if (NumberOfQuestions <= 0 || NumberOfChoices <= 0)
            throw new InvalidOperationException("Game configuration values must be greater than 0");
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
