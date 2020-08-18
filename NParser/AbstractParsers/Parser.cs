using AngleSharp.Dom;
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
		private protected readonly HtmlLoader _loader;

		/// <summary>
		/// Object for loading <see cref="IDocument"/> of any Url.
		/// </summary>
		private readonly DocumentLoader _documentLoader;

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with default settings.
		/// </summary>
		public Parser()
			: this(new HttpClientLoader())
		{
		}

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with prepared <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="client">Prepared instance of <see cref="HttpClient"/>.</param>
		public Parser(HttpClient client)
			: this(new HttpClientLoader(client))
		{
		}

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/> with prepared <see cref="HttpWebRequest"/>.
		/// </summary>
		/// <param name="configureRequest"><see cref="Action"/> for setting properties of <see cref="HttpWebRequest"/>.</param>
		public Parser(Action<HttpWebRequest> configureRequest)
			: this(new WebRequestLoader(configureRequest))
		{
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
			: this(new HttpClientLoader(factory))
		{
		}

		/// <summary>
		/// Create an instance of <see cref="Parser{T}"/>.
		/// </summary>
		/// <param name="loader">Object for loading HTML of any Url.</param>
		private Parser(HtmlLoader loader)
		{
			_loader = loader;
			_documentLoader = new DocumentLoader(_loader);
		}

		/// <summary>
		/// Parse the site. This method use logic of overridden method <see cref="Parser{T}.ParseHtmlAsync(IDocument)"/>.
		/// </summary>
		/// <param name="url">Website Url.</param>
		/// <returns>Parsing result as type <see cref="T"/>.</returns>
		public async Task<T> ParseAsync(string url)
		{
			var document = await _documentLoader.GetDocumentAsync(url);
			return await ParseHtmlAsync(document);
		}

		public void Dispose() => _documentLoader.Dispose();

		/// <summary>
		/// Get the necessary data from HTML using AngleSharp and convert it to type <see cref="T"/>.
		/// </summary>
		/// <param name="html">Webpage HTML code.</param>
		/// <returns>Parsing result as type <see cref="T"/>.</returns>
		protected abstract Task<T> ParseHtmlAsync(IDocument html);
	}
}
