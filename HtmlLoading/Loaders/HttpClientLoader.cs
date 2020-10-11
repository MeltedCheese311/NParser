using HtmlLoading.Factory;
using HtmlLoading.Loaders.Abstractions;
using HtmlLoading.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HtmlLoading.Loaders
{
	/// <summary>
	/// Class for loading HTML of any Url using <see cref="HttpClient"/>.
	/// </summary>
	public class HttpClientLoader : HtmlLoader
	{
		/// <summary>
		/// Object for working with Url.
		/// </summary>
		protected HttpClient _client;

		/// <summary>
		/// Object for creating <see cref="HttpClient"/> with caching.
		/// </summary>
		protected readonly CachedHttpClientFactory _cachedFactory;

		/// <summary>
		/// Create an instance of <see cref="HttpClientLoader"/> with default settings.
		/// </summary>
		public HttpClientLoader()
			: this(
				  new CachedHttpClientFactory(new HttpClientFactory()),
				  new HttpClient())
		{
			_client.DefaultRequestHeaders.Add("User-Agent", "C# App");
		}

		/// <summary>
		/// Create an instance of <see cref="HttpClientLoader"/> with prepared <see cref="HttpClient"/>.     
		/// </summary>
		/// <param name="client">Prepared instance of <see cref="HttpClient"/>.</param>
		public HttpClientLoader(HttpClient client)
			: this(
				  new CachedHttpClientFactory(new HttpClientFactory()),
				  client)
		{
		}

		/// <summary>
		/// Create an instance of <see cref="HttpClientLoader"/> with prepared <see cref="CachedHttpClientFactory"/>.
		/// </summary>
		/// <param name="factory">Prepared instance of <see cref="CachedHttpClientFactory"/>.</param>
		public HttpClientLoader(CachedHttpClientFactory factory)
			: this(
				  factory,
				  factory.CreateClient())
		{
		}

		/// <summary>
		/// Create an instance of <see cref="HttpClientLoader"/>.
		/// </summary>
		/// <param name="factory">Object for creating <see cref="HttpClient"/> with caching.</param>
		/// <param name="client">Initial instance of <see cref="HttpClient"/>.</param>
		protected HttpClientLoader(
			CachedHttpClientFactory factory,
			HttpClient client)
		{
			_cachedFactory = factory;
			_client = client;
		}

		protected override async Task<Response> GetResponseAsync(string url)
		{
			var response = await _client.GetAsync(url, HttpCompletionOption.ResponseContentRead);
			var statusCode = response?.StatusCode ?? default;

			return statusCode == HttpStatusCode.OK
				? new Response(await response.Content.ReadAsStringAsync())
				: new Response(statusCode);
		}

		public override void Dispose() => _cachedFactory?.Dispose();
	}
}
