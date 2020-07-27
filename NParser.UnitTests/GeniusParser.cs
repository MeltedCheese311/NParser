using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NParser.UnitTests
{
	internal class GeniusParser : Parser<string>
	{
		public GeniusParser() : base() { }

		public GeniusParser(Action<HttpWebRequest> configureRequest) : base(configureRequest) { }

		public GeniusParser(HttpClient client) : base(client) { }

		protected override Task<string> ParseHtmlAsync(IDocument html)
		{
			var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
			return Task.FromResult(result);
		}
	}
}
