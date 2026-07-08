namespace GeoQuiz_API.Models.RestCountries;

public record RestCountriesObject(
    RestCountriesNames names,
    List<RestCountriesCapital> capitals,
    RestCountriesFlag flag,
    RestCountriesCountryMeta _meta
);