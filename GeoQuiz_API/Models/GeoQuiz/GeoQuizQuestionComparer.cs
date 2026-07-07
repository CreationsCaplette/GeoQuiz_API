using System.Diagnostics.CodeAnalysis;

namespace GeoQuiz_API.Models.GeoQuiz;

public class GeoQuizQuestionComparer : IEqualityComparer<GeoQuizQuestion>
{
    public bool Equals(GeoQuizQuestion? x, GeoQuizQuestion? y)
    {
        if (Object.ReferenceEquals(x, y)) return true;

        if (x is null || y is null)
            return false;

        return x.Question == y.Question;
    }

    public int GetHashCode([DisallowNull] GeoQuizQuestion question)
    {
        if (question is null) return 0;

        int hashQuestionQuestion = question.Question == null ? 0 : question.Question.GetHashCode();

        return hashQuestionQuestion;
    }
}
