namespace GeoQuiz_API.Models.RestCountries;

public record RestCountriesObjects(
    RestCountriesNames names,
    List<RestCountriesCapitals> capitals,
    RestCountriesFlag flag,
    RestCountriesCountryMeta _meta
);