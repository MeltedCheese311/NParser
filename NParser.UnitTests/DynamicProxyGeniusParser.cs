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
		public DynamicProxyGeniusParser(IWebProxy proxy) : base(proxy) { }

		public DynamicProxyGeniusParser(string host, int port) : base(host, port) { }

		public DynamicProxyGeniusParser(Action<HttpWebRequest> configureRequest) : base(configureRequest) { }

		public DynamicProxyGeniusParser(Func<HttpClientHandler> makeHandler, Action<HttpClient> configureClient) : base(makeHandler, configureClient) { }

		public DynamicProxyGeniusParser(Func<HttpClientHandler> makeHandler) : base(makeHandler) { }

		protected override Task<string> ParseHtmlAsync(IDocument html)
		{
			var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
			return Task.FromResult(result);
		}
	}
}
