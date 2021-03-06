﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using HtmlLoading.Loaders.Abstractions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HtmlLoading.Loaders
{
	/// <summary>
	/// Class for loading <see cref="IDocument"/> of any Url.
	/// </summary>
	public sealed class DocumentLoader : IDisposable
	{
		/// <summary>
		/// Object for loading HTML of any Url.
		/// </summary>
		private readonly HtmlLoader _loader;

		/// <summary>
		/// Create an instance of <see cref="DocumentLoader"/> with default settings.
		/// </summary>
		public DocumentLoader()
			: this(new HttpClientLoader())
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DocumentLoader"/> with prepared <see cref="HtmlLoader"/>.
		/// </summary>
		/// <param name="loader">Object for loading HTML of any Url.</param>
		public DocumentLoader(HtmlLoader loader)
		{
			_loader = loader;
		}

		public void Dispose() => _loader?.Dispose();

		/// <summary>
		/// Get <see cref="IDocument"/> of input Url.
		/// </summary>
		/// <param name="url">Webpage Url.</param>
		/// <returns>Loaded <see cref="IDocument"/> of input Url.</returns>
		/// <exception cref="ArgumentNullException">If an instance of <see cref="HtmlLoader"/> was null.</exception>
		public async Task<IDocument> GetDocumentAsync(string url)
		{
			if (_loader == null)
			{
				throw new ArgumentNullException("loader", $"An instance of {nameof(HtmlLoader)} is null");
			}

			var html = await _loader.GetHtmlStringAsync(url);
			var config = Configuration.Default
				.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
				.WithCss();
			var document = await BrowsingContext
				.New(config)
				.OpenAsync(x => x.Content(html));
			return document;
		}
	}
}
