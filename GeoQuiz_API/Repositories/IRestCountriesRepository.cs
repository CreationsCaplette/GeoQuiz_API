using GeoQuiz_API.Models.RestCountries;

namespace GeoQuiz_API.Repositories;

public interface IRestCountriesRepository
{
    Task<List<RestCountriesObject>> GetAllRestCountriesObjects(string apiKey);
}
