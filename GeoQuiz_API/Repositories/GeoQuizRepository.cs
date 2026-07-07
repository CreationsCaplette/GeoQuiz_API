using GeoQuiz_API.Data;
using GeoQuiz_API.Helper;
using GeoQuiz_API.Models;
using GeoQuiz_API.Models.GeoQuiz;
using Microsoft.Extensions.Options;

namespace GeoQuiz_API.Repositories;

public class GeoQuizRepository : IGeoQuizRepository
{
    private readonly string ApiKey;

    private readonly IJsonFileData jsonFileRepository;
    private readonly IRestCountriesData restCountriesRepository;

    public GeoQuizRepository(
        IOptions<GeoQuizGameOptions> options,
        IJsonFileData jsonFileRepository,
        IRestCountriesData restCountriesRepository
    )
    {
        var geoQuizOptions = options.Value;
        ApiKey = geoQuizOptions.RestCountriesApiKey;

        this.jsonFileRepository = jsonFileRepository;
        this.restCountriesRepository = restCountriesRepository;
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
}
