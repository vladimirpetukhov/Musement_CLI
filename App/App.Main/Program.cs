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

    public static async Task Main(string[] args)
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
            var cities = tuiApi.GetSities().Result;

            foreach (var city in cities)
            {
                var url = String.Format(config.GetSection("WEATHER_API:City").Value, city.Name);
                var weather = weatherApi.GetCityWeather(url).Result;
                JObject jsonObject = JObject.Parse(weather);

                if (!entries.ContainsKey((city.Name, city.Id)))
                {
                    Log.Information(city.Name, city.Id);
                    entries[(city.Name, city.Id)] = new();
                }
                var forecastday = jsonObject.SelectToken("forecast.forecastday").ToList();
                var first = forecastday[0]["day"]["condition"]["text"];
                var second = forecastday[1]["day"]["condition"]["text"];

                entries[(city.Name, city.Id)] = (first.ToString(), second.ToString());

                Console.Out.WriteLine($"Processed city {city.Name} | {first} - {second}");
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
            Start:
                var citi = args.AsQueryable().FirstOrDefault();
                if (citi == null)
                {
                    Console.WriteLine("City name: ");
                    citi = Console.ReadLine()!.Trim();
                }

                if (entries.Keys.Any(k => k.Item1 == citi))
                {
                    var c = entries.Keys.FirstOrDefault(k => k.Item1 == citi);
                    var val = entries[c];
                    Console.Out.WriteLine($"Processed city {c.Item1} | {val.Item1} - {val.Item2}");

                    goto Start;
                }
                else
                {
                    Console.Out.WriteLine("========== Not Found! ==========");
                    goto Start;
                }
            }
            // do something with each key press until escape key is pressed
            while (cki.Key != ConsoleKey.Escape);
        }






    }


}