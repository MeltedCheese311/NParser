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
        /// Func for creating instance of <see cref="HttpClient"/>.
        /// </summary>
        private readonly Action<HttpClient> _configureClient;

        /// <summary>
        /// Create an instance of <see cref="HttpClientFactory"/>.
        /// </summary>
        /// <param name="makeHandler">Func for creating instance of <see cref="HttpClientHandler"/>.</param>
        /// <param name="configureClient">Action for configuring <see cref="HttpClient"/>.</param>
        public HttpClientFactory(Func<HttpClientHandler> makeHandler, Action<HttpClient> configureClient)
        {
            _makeHandler = makeHandler;
            _configureClient = configureClient;
        }

        /// <summary>
        /// Create an instance of <see cref="HttpClientFactory"/>.
        /// </summary>
        /// <param name="makeHandler">Func for creating instance of <see cref="HttpClientHandler"/>.</param>
        public HttpClientFactory(Func<HttpClientHandler> makeHandler)
		{
            _makeHandler = makeHandler;
		}

        /// <summary>
        /// Create an instance of <see cref="HttpClientFactory"/> with default settings.
        /// </summary>
        public HttpClientFactory()
		{
            _makeHandler = () => new HttpClientHandler
            {
                UseCookies = true,
                UseDefaultCredentials = true,
            };

            _configureClient = (client) => client.DefaultRequestHeaders.Add("User-Agent", "C# App");
        }

        public HttpClient CreateClient()
		{
            var handler = _makeHandler?.Invoke();
            var client = handler != null
                ? new HttpClient(handler, true)
                : new HttpClient();
            _configureClient?.Invoke(client);
            return client;
        }

        public HttpClient CreateClientWithProxy(IWebProxy webProxy)
        {
            var handler = _makeHandler();
            if (handler == null)
			{
                throw new InvalidOperationException($"Func \"makeHandler\" returns null");
			}
            handler.Proxy = webProxy;
            var client = new HttpClient(handler, true);
            _configureClient?.Invoke(client);
            return client;
        }
    }
}
