using AngleSharp.Dom;
using NParser.HtmlLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NParser
{
	/// <summary>
	/// Simple class for parsing webpages using CSS-selectors.
	/// </summary>
	public sealed class DefaultParser
	{
		/// <summary>
		/// Object for loading <see cref="IDocument"/> of any Url.
		/// </summary>
		private readonly DocumentLoader _documentLoader = new DocumentLoader();

		/// <summary>
		/// Parse the site.
		/// </summary>
		/// <param name="url">Website Url.</param>
		/// <param name="query">Query with CSS-selectors.</param>
		/// <returns>Text content of all HTML document nodes that finded using CSS-selectors.</returns>
		public async Task<IEnumerable<string>> ParseAsync(string url, string query)
		{
			var document = await _documentLoader.GetDocumentAsync(url);
			return document.QuerySelectorAll(query).Select(x => x?.TextContent).Where(x => !string.IsNullOrWhiteSpace(x));
		}
	}
}
