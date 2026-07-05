namespace GeoQuiz_API.Models.GeoQuiz;

public record GeoQuizQuestion(
    string Question,
    List<string>  Choices,
    int AnswerIndex
);
