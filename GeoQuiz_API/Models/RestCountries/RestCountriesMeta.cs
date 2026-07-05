namespace GeoQuiz_API.Models.RestCountries;

public record RestCountriesMeta(
    int total,
    int count,
    int limit,
    int offset,
    bool more,
    Guid request_id,
    int duration
);