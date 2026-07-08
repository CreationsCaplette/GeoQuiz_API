namespace GeoQuiz_API;

public class GeoQuizGameOptions
{
    public const string SectionName = "GameConfig";

    public int NumberOfQuestions { get; set; }
    public int NumberOfChoices { get; set; }
    public string RestCountriesApiKey { get; set; } = string.Empty;
}