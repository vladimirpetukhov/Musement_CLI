using App.Infrastrucure;
using App.Services;
using App.Core.Models;
using App.Services.Http;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/cities", async (TUIClient client) =>
{
    IEnumerable<City> cities = default;
    try
    {
        cities = await client.GetSities();
    }
    catch (global::System.Exception)
    {
        Results.BadRequest();
    }

    return cities;
}).Produces(400).Produces(200).WithName("GetCities");

app.MapGet("/cities/{id}", async (int id, TUIClient client) =>
 {
     City city = default;
     try
     {
         city = await client.GetSities(id);

         if (city == null)
         {
             Results.NotFound(id);
         }
     }
     catch (global::System.Exception)
     {
         Results.BadRequest();
     }

     return city;
 }).Produces(400).Produces(404).Produces(200).WithName("GetCityById");

app.MapGet("/weather/{name}", async (string name, WeatherClient client) =>
{
    JObject city = new JObject();
    try
    {
        var url = String.Format(builder.Configuration.GetSection("WEATHER_API:City").Value, name);
        var result = await client.GetCityWeather(url);

        city = JObject.Parse(result);

        if (city == null || city.Count == 0)
        {
            Results.NotFound(name);
        }
    }
    catch (global::System.Exception)
    {
        Results.BadRequest();
    }

    return city["current"]["condition"]["text"].ToString();
}).Produces(400).Produces(404).Produces(200).WithName("GetCityWeatherToday");

app.MapGet("/weather/{name}/tomorrow", async (string name, WeatherClient client) =>
{
    string weather = string.Empty;
    try
    {
        JObject city = new JObject();
        var url = String.Format(builder.Configuration.GetSection("WEATHER_API:City").Value, name);
        var result = await client.GetCityWeather(url);

        city = JObject.Parse(result);

        if (city == null || city.Count == 0)
        {
            return Results.NotFound(name);
        }

        var forecastday = city.SelectToken("forecast.forecastday").ToList();

        weather = forecastday[1]["day"]["condition"]["text"].ToString();
    }
    catch (global::System.Exception)
    {
        Results.BadRequest();
    }

    return Results.Ok(weather);

}).Produces(400).Produces(404).Produces(200).WithName("GetCityWeatherTomorrow");

app.MapGet("/weather/{name}/{date}", async (string name, DateTime date ,WeatherClient client) =>
{
    string weather = string.Empty;
    try
    {
        JObject city = new JObject();
        var url = String.Format(builder.Configuration.GetSection("WEATHER_API:CityDay").Value, name, date.ToString("yyyy-MM-dd"));
        var result = await client.GetCityWeather(url);

        city = JObject.Parse(result);

        if (city == null || city.Count == 0)
        {
            return Results.NotFound(name);
        }

        var forecastday = city.SelectToken("forecast.forecastday").ToList();

        weather = forecastday[0]["day"]["condition"]["text"].ToString();
    }
    catch (global::System.Exception)
    {
        return Results.BadRequest();
    }

    return Results.Ok(weather);

}).Produces(400).Produces(404).Produces(200).WithName("GetCityWeatherByDate");

app.Run();

