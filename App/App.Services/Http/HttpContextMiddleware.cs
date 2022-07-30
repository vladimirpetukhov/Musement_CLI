namespace App.Services.Http
{
    #region usings
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    public class HttpContextMiddleware : DelegatingHandler
    {
        #region fields
        private string _ctor;
        #endregion

        #region ctor
        public HttpContextMiddleware()
        {
            _ctor = Guid.NewGuid().ToString();
        }
        #endregion

        #region methods
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var method = Guid.NewGuid().ToString();

            request.Headers.Add("Custom-Ctor", _ctor);
            request.Headers.Add("Custom-Method", method);

            return base.SendAsync(request, cancellationToken);
        }
        #endregion
    }
}
