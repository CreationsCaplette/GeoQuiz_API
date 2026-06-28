namespace GeoQuiz_API.Data.GeoQuiz;

public record GeoQuizData(
    List<GeoQuizCountry> Countries,
    DateTimeOffset TimeStamp
);
