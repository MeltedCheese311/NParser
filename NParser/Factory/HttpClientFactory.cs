using System;
using System.Net;
using System.Net.Http;

namespace NParser.Factory
{
	/// <summary>
	/// Factory for creating <see cref="HttpClient"/>.
	/// </summary>
	internal class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Func for creating instance of <see cref="HttpClientHandler"/>.
        /// </summary>
        private readonly Func<HttpClientHandler> _makeHandler;

        /// <summary>
        /// Create an instance of <see cref="HttpClientFactory"/>.
        /// </summary>
        /// <param name="makeHandler">Func for creating instance of <see cref="HttpClientHandler"/>.</param>
        public HttpClientFactory(Func<HttpClientHandler> makeHandler) => _makeHandler = makeHandler;

        public HttpClient CreateClientWithProxy(IWebProxy webProxy)
        {
            var handler = _makeHandler();
            handler.Proxy = webProxy;
            return new HttpClient(handler, true);
        }
    }
}
