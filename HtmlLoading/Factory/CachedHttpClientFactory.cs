using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HtmlLoading.Factory
{
	/// <summary>
	/// Factory for creating <see cref="HttpClient"/> with caching.
	/// </summary>
	public class CachedHttpClientFactory : IHttpClientFactory, IDisposable
    {
        /// <summary>
        /// Factory for creating <see cref="HttpClient"/>.
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Cached instances of <see cref="HttpClient"/> by <see cref="HttpClientHandler.Proxy"/>.
        /// </summary>
        private readonly Dictionary<int, HttpClient> _cache = new Dictionary<int, HttpClient>();

        /// <summary>
        /// Create an instance of <see cref="CachedHttpClientFactory"/>.
        /// </summary>
        /// <param name="httpClientFactory">Factory for creating <see cref="HttpClient"/>.</param>
        public CachedHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Create instance of <see cref="HttpClient"/>.
        /// <para>If the same instance was created earlier, it will be taken from the cache.</para>
        /// </summary>
        /// <returns>Instance of <see cref="HttpClient"/>.</returns>
        public HttpClient CreateClient() => CreateClientWithProxy(new WebProxy());

        /// <summary>
        /// Create instance of <see cref="HttpClient"/>.
        /// <para>If the same instance was created earlier, it will be taken from the cache.</para>
        /// </summary>
        /// <param name="webProxy">Proxy.</param>
        /// <returns>Instance of <see cref="HttpClient"/>.</returns>
        public HttpClient CreateClientWithProxy(IWebProxy webProxy)
        {
            var key = webProxy.GetHashCode();
            lock (_cache)
            {
                if (_cache.ContainsKey(key))
                {
                    return _cache[key];
                }

                var result = _httpClientFactory.CreateClientWithProxy(webProxy);
                _cache.Add(key, result);
                return result;
            }
        }

        public void Dispose()
        {
            foreach (var client in _cache.Values)
            {
                client.Dispose();
            }
        }
    }
}
