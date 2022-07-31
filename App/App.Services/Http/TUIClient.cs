namespace App.Services.Http
{
    #region usings
    using App.Core.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using static Core.Constants;
    #endregion

    public class TUIClient : BaseClient
    {
        public TUIClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public Task<IEnumerable<City>> GetHome()
            => _httpClient.GetFromJsonAsync<IEnumerable<City>>(CITIES);
        
    }
}
