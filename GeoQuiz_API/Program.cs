using GeoQuiz_API.Data;
using GeoQuiz_API.Data.GeoQuiz;
using GeoQuiz_API.Data.RestCountries;
using System.Text.Json;

const string ConfigApiKey = "RestCountriesApiKey";
const string HeadersAuthorization = "Authorization";
const string GeoQuizDataFileName = "GeoQuizData.json";
const int APILimit = 100;
const int APIOffset = 0;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/countries", async (IConfiguration config) =>
{
    var data = GetDataFromFile();
    if (data is not null && IsDataStillValid(data))
        return data.Countries;

    var apiKey = config.GetValue<string>(ConfigApiKey);
    var countries = await GetDataFromAPI(apiKey);

    SaveDataToFile(countries);

    return countries;
})
.WithName("CountriesData");

app.Run();

static async Task<List<GeoQuizCountry>> GetDataFromAPI(string apiKey)
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

    var countries = response.ConvertToGeoQuizCountries();

    while (response.data.meta.more)
    {
        offset += response.data.meta.count;
        response = await FetchResponseFromAPI(client, limit, offset);
        if (response is null)
        {
            return countries;
        }

        countries.AddRange(response.ConvertToGeoQuizCountries());
    }

    return countries;
}

static async Task<RestCountriesResponse?> FetchResponseFromAPI(HttpClient client, int limit, int offset)
{
    var uri = $"https://api.restcountries.com/countries/v5?limit={limit}&offset={offset}&response_fields=names.common,capitals.name,flag.url_svg";
    var data = await client.GetFromJsonAsync<RestCountriesResponse>(uri);

    return data;
}

static GeoQuizData? GetDataFromFile()
{
    if (!File.Exists(GeoQuizDataFileName))
        return null;

    var countriesJson = File.ReadAllText(GeoQuizDataFileName);
    var countries = JsonSerializer.Deserialize<GeoQuizData>(countriesJson);
    return countries;
}

static void SaveDataToFile(List<GeoQuizCountry> countries)
{
    var geoQuizData = new GeoQuizData(countries, DateTimeOffset.UtcNow);
    var dataJson = JsonSerializer.Serialize(geoQuizData);
    File.WriteAllText(GeoQuizDataFileName, dataJson);
}

static bool IsDataStillValid(GeoQuizData data)
{
    var dateDiff = DateTime.UtcNow - data.TimeStamp;
    if (dateDiff > TimeSpan.FromDays(1))
        return false;
    return true;
}