using NParser.Factory;
using NParser.HtmlLoading.Abstract;
using NParser.HtmlLoading.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NParser.HtmlLoading
{
	/// <summary>
	/// Class for loading HTML of any Url using <see cref="HttpClient"/>.
	/// </summary>
	internal sealed class HttpClientLoader : HtmlLoader
	{
		/// <summary>
		/// Object for working with Url.
		/// </summary>
		private HttpClient _client;

		/// <summary>
		/// Object for creating <see cref="HttpClient"/> with caching.
		/// </summary>
		private readonly CachedHttpClientFactory _cachedFactory;

		/// <summary>
		/// Create an instance of <see cref="HttpClientLoader"/> with default settings.
		/// </summary>
		internal HttpClientLoader()
			: this(new CachedHttpClientFactory(new HttpClientFactory()))
		{
			_client = new HttpClient();
			_client.DefaultRequestHeaders.Add("User-Agent", "C# App");
		}

		/// <summary>
		/// Create an instance of <see cref="HttpClientLoader"/> with prepared <see cref="HttpClient"/>.     
		/// </summary>
		/// <param name="client">Prepared instance of <see cref="HttpClient"/>.</param>
		internal HttpClientLoader(HttpClient client)
		: this(new CachedHttpClientFactory(new HttpClientFactory()))
		{
			_client = client;
		}

		/// <summary>
		/// Create an instance of <see cref="HttpClientLoader"/> with prepared <see cref="CachedHttpClientFactory"/>.
		/// </summary>
		/// <param name="factory">Prepared instance of <see cref="CachedHttpClientFactory"/>.</param>
		internal HttpClientLoader(CachedHttpClientFactory factory)
		{
			_cachedFactory = factory;
			_client = _cachedFactory.CreateClient();
		}

		internal override async Task<Response> GetResponseAsync(string url)
		{
			var response = await _client.GetAsync(url, HttpCompletionOption.ResponseContentRead);
			var statusCode = response?.StatusCode ?? default;

			return statusCode == HttpStatusCode.OK
			? new Response(await response.Content.ReadAsStringAsync())
			: new Response(statusCode);
		}

		internal override void ChangeProxy(IWebProxy proxy) => _client = _cachedFactory.CreateClientWithProxy(proxy);

		public override void Dispose() => _cachedFactory.Dispose();
	}
}
