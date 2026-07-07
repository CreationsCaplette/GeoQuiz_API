using GeoQuiz_API.Helper;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace GeoQuiz_API.Repositories;

public class GameCapitalsRepository : IGameCapitalsRepository
{
    private readonly int NumberOfQuestions;
    private readonly int NumberOfChoices;

    private readonly IGeoQuizRepository geoQuizRepository;
    private readonly IRandomProvider randomProvider;

    public GameCapitalsRepository(
        IOptions<GeoQuizGameOptions> options,
        IGeoQuizRepository geoQuizRepository,
        IRandomProvider randomProvider
    )
    {
        var geoQuizOptions = options.Value;
        NumberOfQuestions = geoQuizOptions.NumberOfQuestions;
        NumberOfChoices = geoQuizOptions.NumberOfChoices;

        this.geoQuizRepository = geoQuizRepository;

        this.randomProvider = randomProvider;
    }

    public async Task<List<GeoQuizQuestion>> GetCapitalsGame()
    {
        var countries = await geoQuizRepository.GetAllGeoQuizCountries();

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
