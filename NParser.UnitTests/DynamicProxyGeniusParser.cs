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
	internal class DynamicProxyGeniusParser : DynamicProxyParser<string>
	{
		public DynamicProxyGeniusParser(WebProxy proxy) : base(proxy) { }

		public DynamicProxyGeniusParser(Func<HttpWebRequest, HttpWebRequest> configureRequest) : base(configureRequest) { }

		public DynamicProxyGeniusParser(HttpClient client) : base(client) { }

		protected override Task<string> ParseHtmlAsync(IDocument html)
		{
			var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
			return Task.FromResult(result);
		}
	}
}
