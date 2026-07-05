using GeoQuiz_API.Models;
using GeoQuiz_API.Models.GeoQuiz;
using GeoQuiz_API.Repositories;
using GeoQuiz_API.Startup;
using System.Text.Json;

const string ConfigApiKey = "RestCountriesApiKey";
const string GeoQuizDataFileName = "GeoQuizData.json";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCorsServices();

builder.Services.AddTransient<IRestCountriesRepository, RestCountriesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.ApplyCorsConfig();

app.MapGet("/countries/all", async (IConfiguration config, IRestCountriesRepository restCountriesRepository) =>
{
    var data = GetDataFromFile();
    if (data is not null && IsDataStillValid(data))
        return data.Countries;

    var apiKey = config.GetValue<string>(ConfigApiKey);
    var countriesObject = await restCountriesRepository.GetAllRestCountriesObjects(apiKey);
    var countries = countriesObject.ConvertToGeoQuizCountries();

    SaveDataToFile(countries);

    return countries;
})
.WithName("CountriesData");

app.Run();

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