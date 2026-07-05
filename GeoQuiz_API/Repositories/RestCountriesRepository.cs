using GeoQuiz_API.Models.RestCountries;

namespace GeoQuiz_API.Repositories;

public class RestCountriesRepository : IRestCountriesRepository
{
    const string HeadersAuthorization = "Authorization";
    const int APILimit = 100;
    const int APIOffset = 0;

    public async Task<List<RestCountriesObject>> GetAllRestCountriesObjects(string apiKey)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add(HeadersAuthorization, apiKey);

        var limit = APILimit;
        var offset = APIOffset;

        var response = await FetchResponseFromAPI(client, limit, offset);
        if (response is null)
        {
            return [];
        }

        var objects = response.data.objects;

        while (response.data.meta.more)
        {
            offset += response.data.meta.count;
            response = await FetchResponseFromAPI(client, limit, offset);
            if (response is null)
            {
                return objects;
            }

            objects.AddRange(response.data.objects);
        }

        return objects;
    }

    private static async Task<RestCountriesResponse?> FetchResponseFromAPI(HttpClient client, int limit, int offset)
    {
        var uri = $"https://api.restcountries.com/countries/v5?limit={limit}&offset={offset}&response_fields=names.common,capitals.name,flag.url_svg,flag.description";
        var data = await client.GetFromJsonAsync<RestCountriesResponse>(uri);

        return data;
    }
}
