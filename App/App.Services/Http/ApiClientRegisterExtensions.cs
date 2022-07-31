namespace App.Services.Http
{
    #region usings
    using Microsoft.Extensions.DependencyInjection;
    using System.Net.Http;
    #endregion

    public static class ApiClientRegisterExtensions
    {
        public static IServiceCollection  AddApiClient<T>(
            this IServiceCollection services,
            Action<HttpClient> clientConfiguration) where T : class
        {
            services.AddTransient<HttpContextMiddleware>();
            
            services.AddHttpClient<T>(clientConfiguration)
                .AddHttpMessageHandler<HttpContextMiddleware>();

            return services;
        }
    }
}
