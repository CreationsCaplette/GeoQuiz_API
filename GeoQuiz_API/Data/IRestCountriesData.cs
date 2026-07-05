using GeoQuiz_API.Models.RestCountries;

namespace GeoQuiz_API.Data;

public interface IRestCountriesData
{
    Task<List<RestCountriesObject>> GetAllRestCountriesObjects(string apiKey);
}
