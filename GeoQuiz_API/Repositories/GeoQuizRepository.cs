using GeoQuiz_API.Data;
using GeoQuiz_API.Models;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;

namespace GeoQuiz_API.Repositories;

public class GeoQuizRepository(
    IOptions<GeoQuizGameOptions> options,
    IJsonFileData jsonFileRepository,
    IRestCountriesData restCountriesRepository
    ) : IGeoQuizRepository
{
    private readonly GeoQuizGameOptions GameOptions = options.Value;

    public async Task<List<GeoQuizCountry>> GetAllGeoQuizCountries()
    {
        var data = await jsonFileRepository.GetDataFromFile();
        if (data is not null && IsDataStillValid(data))
            return data.Countries;

        var countriesObject = await restCountriesRepository.GetAllRestCountriesObjects(GameOptions.RestCountriesApiKey);
        var countries = countriesObject.ConvertToGeoQuizCountries();

        await jsonFileRepository.SaveDataToFile(countries);

        return countries;
    }

    static private bool IsDataStillValid(GeoQuizData data) =>
        DateTime.UtcNow - data.TimeStamp <= TimeSpan.FromDays(1);
}
