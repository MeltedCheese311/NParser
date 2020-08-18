using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using NParser.HtmlLoading.Abstract;
using System;
using System.Threading.Tasks;

namespace NParser.HtmlLoading
{
	/// <summary>
	/// Class for loading <see cref="IDocument"/> of any Url.
	/// </summary>
	internal sealed class DocumentLoader : IDisposable
	{
		/// <summary>
		/// Object for loading HTML of any Url.
		/// </summary>
		private readonly HtmlLoader _loader;

		/// <summary>
		/// Create an instance of <see cref="DocumentLoader"/> with default settings.
		/// </summary>
		internal DocumentLoader()
			: this(new HttpClientLoader())
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DocumentLoader"/> with prepared <see cref="HtmlLoader"/>.
		/// </summary>
		/// <param name="loader">Object for loading HTML of any Url.</param>
		internal DocumentLoader(HtmlLoader loader)
		{
			_loader = loader;
		}

		public void Dispose() => _loader?.Dispose();

		/// <summary>
		/// Get <see cref="IDocument"/> of input Url.
		/// </summary>
		/// <param name="url">Webpage Url.</param>
		/// <returns>Loaded <see cref="IDocument"/> of input Url.</returns>
		/// <exception cref="InvalidOperationException">If <see cref="HtmlLoader"/> was null.</exception>
		internal async Task<IDocument> GetDocumentAsync(string url)
		{
			if (_loader == null)
			{
				throw new InvalidOperationException($"{nameof(HtmlLoader)} == null");
			}

			var html = await _loader.GetHtmlStringAsync(url);
			var config = Configuration.Default
				.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
				.WithCss();
			var document = await BrowsingContext.New(config).OpenAsync(x => x.Content(html));
			return document;
		}
	}
}
