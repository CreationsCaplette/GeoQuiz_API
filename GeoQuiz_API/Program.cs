using GeoQuiz_API.Data;
using GeoQuiz_API.Data.GeoQuiz;
using GeoQuiz_API.Data.RestCountries;

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
    var apiKey = config.GetValue<string>("RestCountriesApiKey");

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
})
.WithName("CountriesData");

app.Run();
