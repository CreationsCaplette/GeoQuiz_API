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
        var questions = new HashSet<GeoQuizQuestion>(new GeoQuizQuestionComparer());

        while (questions.Count < GameOptions.NumberOfQuestions)
        {
            var correctCountry = countries[randomProvider.Next(countries.Count)];
            var choices = new HashSet<string> { correctCountry.CapitalName };

            while (choices.Count < GameOptions.NumberOfChoices)
            {
                choices.Add(countries[randomProvider.Next(countries.Count)].CapitalName);
            }

            var choicesList = choices.ToList();
            randomProvider.Shuffle(CollectionsMarshal.AsSpan(choicesList));

            questions.Add(new GeoQuizQuestion(
                correctCountry.CountryName,
                choicesList,
                choicesList.IndexOf(correctCountry.CapitalName)
            ));
        }

        return [.. questions];
    }
}
