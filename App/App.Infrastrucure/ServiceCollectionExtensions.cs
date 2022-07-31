using App.Core.Models;
using App.Services.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog;

namespace App.Infrastrucure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfigurationRoot configuration)
        {


            services.AddApiClient<TUIClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("TUI_API:BaseUrl").Value);
            });

            services.AddApiClient<WeatherClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("WEATHER_API:BaseUrl").Value);
            });

            var dir = Directory.GetParent(AppContext.BaseDirectory).FullName;

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            

            // Add access to generic IConfigurationRoot
            services.AddSingleton<IConfigurationRoot>(configuration);

            return services;
        }

        public static IServiceCollection AddAppLogging(this IServiceCollection services)
        {

            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            services.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder
                    .AddSerilog(dispose: true);
            }));

            services.AddLogging();

            return services;
        }
    }
}