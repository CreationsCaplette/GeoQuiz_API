namespace GeoQuiz_API.Data.RestCountries;

public record RestCountriesData(
    List<RestCountriesObjects> objects,
    RestCountriesMeta meta
);
