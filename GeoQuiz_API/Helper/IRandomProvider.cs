namespace GeoQuiz_API.Helper;

public interface IRandomProvider
{
    int Next(int maxValue);
    void Shuffle<T>(Span<T> values);
}