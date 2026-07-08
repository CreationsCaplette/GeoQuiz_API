using GeoQuiz_API.Models.GeoQuiz;
using GeoQuiz_API.Models.RestCountries;

namespace GeoQuiz_API.Models;

public static class RestCountriesObjectConverter
{
    public static List<GeoQuizCountry> ConvertToGeoQuizCountries(this List<RestCountriesObject> objects)
    {
        var countries = new List<GeoQuizCountry>();

        foreach (var obj in objects)
        {
            if (obj.capitals.Count == 0 || string.IsNullOrWhiteSpace(obj.flag.url_svg))
                continue;

            countries.Add(obj.ConvertToGeoQuizCountry());
        }

        return countries;
    }

    public static GeoQuizCountry ConvertToGeoQuizCountry(this RestCountriesObject obj)
    {
        return new GeoQuizCountry
            (
                obj.names.common,
                obj.capitals[0].name,
                obj.flag.url_svg
            );
    }
}
