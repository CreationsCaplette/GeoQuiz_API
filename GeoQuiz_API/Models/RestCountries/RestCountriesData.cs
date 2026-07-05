namespace GeoQuiz_API.Models.RestCountries;

public record RestCountriesData(
    List<RestCountriesObjects> objects,
    RestCountriesMeta meta
);
