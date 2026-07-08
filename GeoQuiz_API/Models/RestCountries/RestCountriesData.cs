namespace GeoQuiz_API.Models.RestCountries;

public record RestCountriesData(
    List<RestCountriesObject> objects,
    RestCountriesMeta meta
);
