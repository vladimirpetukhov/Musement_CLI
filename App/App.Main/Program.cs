#region usings
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
#endregion

class Program
{
    public static IConfigurationRoot configuration;

    static void Main()
    {
        // Initialize serilog logger
        Log.Logger = new LoggerConfiguration()
             .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
             .MinimumLevel.Debug()
             .Enrich.FromLogContext()
             .CreateLogger();

        // Create service collection
        Log.Information("Creating service collection");
        ServiceCollection serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        // Create service provider
        Log.Information("Building service provider");
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        // Print connection string to demonstrate configuration object is populated
        Console.WriteLine(configuration.GetSection("TUI_API:BASE_URL").Value);

        ConsoleKeyInfo cki;

        try
        {
            Log.Information("Starting service");
            Log.Information("Ending service");
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

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Add logging
        serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
        {
            builder
                .AddSerilog(dispose: true);
        }));

        serviceCollection.AddLogging();

        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

        // Add access to generic IConfigurationRoot
        serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
    }
}