using GeoQuiz_API.Data;
using GeoQuiz_API.Data.GeoQuiz;
using GeoQuiz_API.Data.RestCountries;
using System.Text.Json;

const string GeoQuizDataFileName = "GeoQuizData.json";

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
    if (data is not null)
    {
        return data.Countries;
    }

    var apiKey = config.GetValue<string>("RestCountriesApiKey");
    var countries = await GetCountriesFromAPI(apiKey);

    SaveDataToFile(countries);

    return countries;
})
.WithName("CountriesData");

app.Run();

static async Task<List<GeoQuizCountry>> GetCountriesFromAPI(string apiKey)
{
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("Authorization", apiKey);

    var limit = 100;
    var offset = 0;
    var uri = $"https://api.restcountries.com/countries/v5?limit={limit}&offset={offset}&response_fields=names.common,capitals.name,flag.url_svg";
    var data = await client.GetFromJsonAsync<RestCountriesResponse>(uri);

    if (data is null)
    {
        return [];
    }

    var countries = data.ConvertToGeoQuizCountries();

    return countries;
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