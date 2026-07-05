using GeoQuiz_API.Models.GeoQuiz;
using GeoQuiz_API.Models.RestCountries;

namespace GeoQuiz_API.Models;

public static class RestCountriesResponseConverter
{
    public static List<GeoQuizCountry> ConvertToGeoQuizCountries(this RestCountriesResponse response)
    {
        var countries = new List<GeoQuizCountry>();

        foreach (var obj in response.data.objects)
        {
            if (obj.capitals.Count == 0 || string.IsNullOrWhiteSpace(obj.flag.url_svg))
                continue;

            countries.Add(new GeoQuizCountry
            (
                obj.names.common,
                obj.capitals[0].name,
                obj.flag.url_svg,
                obj.flag.description
            ));
        }

        return countries;
    }
}
