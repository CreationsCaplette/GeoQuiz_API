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
    var limit = 100;
    var offset = 0;

    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("Authorization", apiKey);
    var data = await client.GetFromJsonAsync<RestCountriesResponse>($"https://api.restcountries.com/countries/v5?limit={limit}&offset={offset}&response_fields=names.common,capitals.name,flag.url_svg");
    return data;
})
.WithName("CountriesData");

app.Run();
