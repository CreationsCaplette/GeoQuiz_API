using GeoQuiz_API.Helper;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace GeoQuiz_API.Repositories;

public abstract class BaseGameRepository(
    IOptions<GeoQuizGameOptions> options,
    IGeoQuizRepository geoQuizRepository,
    IRandomProvider randomProvider
    )
{
    protected readonly GeoQuizGameOptions GameOptions = options.Value;
    protected readonly IGeoQuizRepository GeoQuizRepository = geoQuizRepository;
    protected readonly IRandomProvider RandomProvider = randomProvider;

    protected async Task<List<GeoQuizQuestion>> GetGameQuestions(
        Func<GeoQuizCountry, string> questionSelector,
        Func<GeoQuizCountry, string> answerSelector)
    {
        var countries = await GeoQuizRepository.GetAllGeoQuizCountries();
        var questions = new List<GeoQuizQuestion>();

        var shuffledCountries = countries.ToList();
        RandomProvider.Shuffle(CollectionsMarshal.AsSpan(shuffledCountries));

        for (int i = 0; i < GameOptions.NumberOfQuestions; i++)
        {
            var correctCountry = shuffledCountries[i % shuffledCountries.Count];
            var choices = GenerateChoices(correctCountry, countries, answerSelector);

            questions.Add(new GeoQuizQuestion(
                questionSelector(correctCountry),
                choices,
                answerSelector(correctCountry)
            ));
        }

        return questions;
    }

    private List<string> GenerateChoices(
        GeoQuizCountry correctCountry,
        List<GeoQuizCountry> allCountries,
        Func<GeoQuizCountry, string> answerSelector)
    {
        var choices = new List<string> { answerSelector(correctCountry) };

        while (choices.Count < GameOptions.NumberOfChoices)
        {
            var randomChoice = answerSelector(allCountries[RandomProvider.Next(allCountries.Count)]);
            choices.Add(randomChoice);
        }

        var uniqueChoices = choices.Distinct().ToList();

        while (uniqueChoices.Count < GameOptions.NumberOfChoices)
        {
            var randomChoice = answerSelector(allCountries[RandomProvider.Next(allCountries.Count)]);
            if (!uniqueChoices.Contains(randomChoice))
            {
                uniqueChoices.Add(randomChoice);
            }
        }

        RandomProvider.Shuffle(CollectionsMarshal.AsSpan(uniqueChoices));
        return uniqueChoices;
    }
}
