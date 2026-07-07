namespace GeoQuiz_API.Helper;

public class RandomProvider : IRandomProvider
{
    private static readonly Random _random = new();

    public int Next(int maxValue) => _random.Next(maxValue);

    public void Shuffle<T>(Span<T> values) => _random.Shuffle(values);
}
