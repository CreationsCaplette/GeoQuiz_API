using GeoQuiz_API.Helper;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace GeoQuiz_API.Repositories;

public class GameCapitalsRepository(
    IOptions<GeoQuizGameOptions> options,
    IGeoQuizRepository geoQuizRepository,
    IRandomProvider randomProvider
    ) : IGameCapitalsRepository
{
    private readonly GeoQuizGameOptions GameOptions = options.Value;

    public async Task<List<GeoQuizQuestion>> GetCapitalsGame()
    {
        var countries = await geoQuizRepository.GetAllGeoQuizCountries();
        var questions = new List<GeoQuizQuestion>();

        var shuffledCountries = countries.ToList();
        randomProvider.Shuffle(CollectionsMarshal.AsSpan(shuffledCountries));

        for (int i = 0; i < GameOptions.NumberOfQuestions; i++)
        {
            var correctCountry = shuffledCountries[i % shuffledCountries.Count];
            var choices = GenerateChoices(correctCountry, countries);

            questions.Add(new GeoQuizQuestion(
                correctCountry.CountryName,
                choices,
                choices.IndexOf(correctCountry.CapitalName)
            ));
        }

        return questions;
    }

    private List<string> GenerateChoices(dynamic correctCountry, List<GeoQuizCountry> allCountries)
    {
        var choices = new List<string> { correctCountry.CapitalName };

        while (choices.Count < GameOptions.NumberOfChoices)
        {
            var randomCapital = allCountries[randomProvider.Next(allCountries.Count)].CapitalName;
            choices.Add(randomCapital);
        }

        var uniqueChoices = choices.Distinct().ToList();

        while (uniqueChoices.Count < GameOptions.NumberOfChoices)
        {
            var randomCapital = allCountries[randomProvider.Next(allCountries.Count)].CapitalName;
            if (!uniqueChoices.Contains(randomCapital))
            {
                uniqueChoices.Add(randomCapital);
            }
        }

        randomProvider.Shuffle(CollectionsMarshal.AsSpan(uniqueChoices));
        return uniqueChoices;
    }
}
