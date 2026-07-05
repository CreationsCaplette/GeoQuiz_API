namespace GeoQuiz_API.Models.GeoQuiz;

public record GeoQuizData(
    List<GeoQuizCountry> Countries,
    DateTimeOffset TimeStamp
);
