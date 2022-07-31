using App.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Http
{
    public class WeatherClient : BaseClient
    {

        public WeatherClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public  Task<string> GetCityWeather(string url)
        {
            return _httpClient.GetStringAsync(url);
        }
    }
}
