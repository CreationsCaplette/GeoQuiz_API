using GeoQuiz_API.Data.GeoQuiz;
using GeoQuiz_API.Data.RestCountries;

namespace GeoQuiz_API.Data;

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
                obj.flag.url_svg
            ));
        }

        return countries;
    }
}
