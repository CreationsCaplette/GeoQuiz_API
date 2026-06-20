namespace GeoQuiz_API.Data.RestCountries;

public record RestCountriesObjects(
    RestCountriesNames names,
    List<RestCountriesCapitals> capitals,
    RestCountriesFlag flag,
    RestCountriesCountryMeta _meta
);