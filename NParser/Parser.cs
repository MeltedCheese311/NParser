using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using NParser.Factory;
using NParser.HtmlLoading;
using NParser.HtmlLoading.Abstract;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NParser
{
	/// <summary>
	/// <para>Abstract class with basic logic for parsing.</para>
	/// <para>Child classes should override <see cref="Parser{T}.ParseHtmlAsync(IDocument)"/> for parsing a specific site using NuGet AngleSharp.</para>
	/// </summary>
	/// <typeparam name="T">Parsing result type.</typeparam>
	public abstract class Parser<T> : IDisposable
	{
		/// <summary>
		/// Object for loading HTML of any Url.
		/// </summary>
		internal HtmlLoader Loader { get; }

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with default settings.
		/// </summary>
		public Parser()
		{
			Loader = new HttpClientLoader();
		}

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with prepared <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="client">Prepared instance of <see cref="HttpClient"/>.</param>
		public Parser(HttpClient client)
		{
			Loader = new HttpClientLoader(client);
		}

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with prepared <see cref="HttpWebRequest"/>.
		/// </summary>
		/// <param name="configureRequest"><see cref="Action"/> for setting properties of <see cref="HttpWebRequest"/>.</param>
		public Parser(Action<HttpWebRequest> configureRequest)
		{
			Loader = new WebRequestLoader(configureRequest);
		}

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with prepared <see cref="IWebProxy"/>.
		/// </summary>
		/// <param name="proxy">Proxy.</param>
		internal Parser(IWebProxy proxy)
		: this(new HttpClient(new HttpClientHandler { Proxy = proxy }))
		{

		}

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with prepared <see cref="CachedHttpClientFactory"/>.
		/// </summary>
		/// <param name="factory">Factory for creating <see cref="HttpClient"/>.</param>
		internal Parser(CachedHttpClientFactory factory)
		{
			Loader = new HttpClientLoader(factory);
		}

		/// <summary>
		/// Parse the site. This method use logic of overridden method <see cref="Parser{T}.ParseHtmlAsync(IDocument)"/>.
		/// </summary>
		/// <param name="url">Website Url.</param>
		/// <returns>Parsing result as type <see cref="T"/>.</returns>
		public async Task<T> ParseAsync(string url)
		{
			var html = await Loader.GetHtmlStringAsync(url);
			var config = Configuration.Default
			.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
			.WithCss();
			var document = await BrowsingContext.New(config).OpenAsync(x => x.Content(html));
			return await ParseHtmlAsync(document);
		}

		public void Dispose() => Loader.Dispose();

		/// <summary>
		/// Get the necessary data from HTML using AngleSharp and convert it to type <see cref="T"/>.
		/// </summary>
		/// <param name="html">Webpage HTML code.</param>
		/// <returns>Parsing result as type <see cref="T"/>.</returns>
		protected abstract Task<T> ParseHtmlAsync(IDocument html);
	}
}
