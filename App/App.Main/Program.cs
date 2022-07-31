#region usings
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using App.Services.Http;
using App.Infrastrucure;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using App.Core.Models;
using System.Diagnostics;
#endregion

public class Program
{
    public static IConfigurationRoot configuration;

    public static Dictionary<(string, int), (string, string)> entries = new Dictionary<(string, int), (string, string)>();

    public static async Task Main()
    {

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddAppServices(configuration);

        // Create service provider
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        // Create service configuration
        var config = serviceProvider.GetService<IConfigurationRoot>();

        ConsoleKeyInfo cki;

        var tuiApi = serviceProvider.GetService<TUIClient>();
        var weatherApi = serviceProvider.GetService<WeatherClient>();

        try
        {
            var cities = tuiApi.GetHome().Result;

            foreach (var city in cities)
            {
                var url = String.Format(config.GetSection("WEATHER_API:City").Value, "London");
                var weather = weatherApi.GetCityWeather(url).Result;
                JObject jsonObject = JObject.Parse(weather);

                if (!entries.ContainsKey((city.Name, city.Id)))
                {
                    Log.Information(city.Name, city.Id);
                    entries[(city.Name, city.Id)] = new();
                }

                var today = jsonObject.SelectToken("current.condition.text").ToString();
                var tomorrow = jsonObject.SelectToken("forecast.forecastday").First().SelectToken("day.condition.text").ToString();

                entries[(city.Name, city.Id)] = (today, tomorrow);

                Console.Out.WriteLine($"Processed city {city.Name} | {today} - {tomorrow}");
            }


        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Error running service");
            throw ex;
        }
        finally
        {
            do
            {
                cki = Console.ReadKey();
                // do something with each key press until escape key is pressed
            } while (cki.Key != ConsoleKey.Escape);
        }

    }


}