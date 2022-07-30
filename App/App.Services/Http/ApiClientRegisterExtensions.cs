namespace App.Services.Http
{
    #region usings
    using Microsoft.Extensions.DependencyInjection;
    #endregion

    public class ApiClientRegisterExtensions
    {
        public static IServiceCollection AddApiClient(
            this IServiceCollection services,
            Action<HttpClient> clientConfiguration)
        {
            services.AddTransient<HttpContextMiddleware>();

            services.AddHttpClient<TUIClient>(clientConfiguration)
                .AddHttpMessageHandler<HttpContextMiddleware>();

            return services;
        }
    }
}
