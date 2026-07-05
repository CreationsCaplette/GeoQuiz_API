using GeoQuiz_API.Data;
using GeoQuiz_API.Repositories;
using GeoQuiz_API.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCorsServices();

builder.Services.AddTransient<IJsonFileData, JsonFileData>();
builder.Services.AddTransient<IRestCountriesData, RestCountriesData>();
builder.Services.AddTransient<IGeoQuizRepository, GeoQuizRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.ApplyCorsConfig();

app.MapGet("/countries/all", async (IGeoQuizRepository geoQuizRepo) =>
{
    return await geoQuizRepo.GetAllGeoQuizCountries();
})
.WithName("CountriesAll");

app.MapGet("/game/capitals", async (IGeoQuizRepository geoQuizRepo) =>
{
    return await geoQuizRepo.GetCapitalsGame();
})
.WithName("GameCapitals");

app.Run();
