using HtmlLoading.Factory;
using HtmlLoading.Loaders;
using HtmlLoading.Loaders.Abstract;
using NParser.Proxy.Loader.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace NParser.Proxy.Loader
{
	/// <summary>
	/// <inheritdoc/> It is possible to change the proxy.
	/// </summary>
	public sealed class DynamicHttpClientLoader : HttpClientLoader, IProxyChanger
	{
		/// <summary>
		/// Create an instance of <see cref="DynamicHttpClientLoader"/> with default settings.
		/// </summary>
		public DynamicHttpClientLoader()
			: base()
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DynamicHttpClientLoader"/> with prepared <see cref="HttpClient"/>.     
		/// </summary>
		/// <param name="client">Prepared instance of <see cref="HttpClient"/>.</param>
		public DynamicHttpClientLoader(HttpClient client)
			: base(client)
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DynamicHttpClientLoader"/> with prepared <see cref="CachedHttpClientFactory"/>.
		/// </summary>
		/// <param name="factory">Prepared instance of <see cref="CachedHttpClientFactory"/>.</param>
		public DynamicHttpClientLoader(CachedHttpClientFactory factory)
			: base(factory)
		{
		}

		public void ChangeProxy(IWebProxy proxy) => _client = _cachedFactory.CreateClientWithProxy(proxy);
	}
}
