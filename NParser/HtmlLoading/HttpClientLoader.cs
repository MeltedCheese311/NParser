using NParser.Factory;
using NParser.HtmlLoading.Abstract;
using NParser.HtmlLoading.Models;
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
		private readonly CachedHttpClientFactory _cachedFactory = 
			new CachedHttpClientFactory(
				new HttpClientFactory(
					() => new HttpClientHandler
					{
						UseCookies = true,
						UseDefaultCredentials = true,
					}));

		/// <summary>
		/// Create an instance of <see cref="HtmlLoader"/> with default settings.
		/// </summary>
		internal HttpClientLoader()
		{
			_client = new HttpClient();
			_client.DefaultRequestHeaders.Add("User-Agent", "C# App");
		}

		/// <summary>
		/// Create an instance of <see cref="HtmlLoader"/> with prepated <see cref="HttpClient"/>.     
		/// </summary>
		/// <param name="client">Prepared instance of <see cref="HttpClient"/>.</param>
		internal HttpClientLoader(HttpClient client) => _client = client;

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
