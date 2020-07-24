using AngleSharp.Dom;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NParser.UnitTests
{
	public class ParserTests
	{
		private readonly WebProxy _wrongProxy = new WebProxy("1.0.0.64", 80);
		private readonly string _correctProxyHost = "54.37.131.91";
		private readonly int _correctProxyPort = 3128;
		private readonly WebProxy _correctProxy = new WebProxy("54.37.131.91", 3128);
		private readonly WebProxy _correctProxy1 = new WebProxy("80.187.140.26", 8080);
		private readonly WebProxy _correctProxy2 = new WebProxy("199.195.251.143", 3128);
		private readonly string _correctUrl = "https://genius.com/Last-dinosaurs-apollo-lyrics";
		private readonly string _urlWithoutHttps = "genius.com/Last-dinosaurs-apollo-lyrics";

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public async Task ParseAsync_NullArg()
		{
			using var parser = new GeniusParser();

			var result = await parser.ParseAsync(null);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_EmptyStringArg()
		{
			using var parser = new GeniusParser();

			var result = await parser.ParseAsync("");

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_RandomStringArg()
		{
			using var parser = new GeniusParser();

			var result = await parser.ParseAsync("qwe");

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_UrlWithoutHttpsArg()
		{
			using var parser = new GeniusParser();

			var result = await parser.ParseAsync(_urlWithoutHttps);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_CorrectUrlArg()
		{
			using var parser = new GeniusParser();

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_EmptyHttpClient()
		{
			var client = new HttpClient();
			using var parser = new GeniusParser(client);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_WrongProxyHttpClient()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _wrongProxy
			};
			var client = new HttpClient(handler);
			using var parser = new GeniusParser(client);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_CorrectProxyHttpClient()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy
			};
			var client = new HttpClient(handler);
			using var parser = new GeniusParser(client);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_NullRequest()
		{
			using var parser = new GeniusParser((request) => null);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_NewRequest()
		{
			using var parser = new GeniusParser((request) => (HttpWebRequest)WebRequest.Create(_correctUrl));

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_UnchangedRequest()
		{
			using var parser = new GeniusParser((request) =>
			{
				return request;
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_CustomizedRequest()
		{
			using var parser = new GeniusParser((request) =>
			{
				request.CookieContainer = new CookieContainer();
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
				request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
				request.Timeout = 10000;
				return request;
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicProxyRequest()
		{
			using var parser = new DynamicProxyGeniusParser((request) =>
			{
				request.Proxy = _correctProxy1;
				return request;
			});

			var result1 = await parser.ParseAsync(_correctUrl);
			parser.ChangeProxy(_correctProxy2);
			var result2 = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicProxyHttpClient()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy1
			};
			var client = new HttpClient(handler);
			using var parser = new DynamicProxyGeniusParser(client);

			var result1 = await parser.ParseAsync(_correctUrl);
			parser.ChangeProxy(_correctProxy2);
			var result2 = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_WrongProxyRequest()
		{
			using var parser = new GeniusParser((request) =>
			{
				request.Proxy = _wrongProxy;
				return request;
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_CorrectProxyRequest()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy
			};
			var client = new HttpClient(handler);
			using var parser = new GeniusParser(client);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicWebProxyConstructor()
		{
			using var parser = new DynamicProxyGeniusParser(_correctProxy);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_HttpClientEmptyWebProxy()
		{
			var handler = new HttpClientHandler
			{
				Proxy = new WebProxy()
			};
			var client = new HttpClient(handler);
			using var parser = new GeniusParser(client);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_RequestEmptyWebProxy()
		{
			using var parser = new GeniusParser((request) =>
			{
				request.Proxy = new WebProxy();
				return request;
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicEmptyWebProxy()
		{
			using var parser = new DynamicProxyGeniusParser(new WebProxy());

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicHttpClientEmptyWebProxy()
		{
			var handler = new HttpClientHandler
			{
				Proxy = new WebProxy()
			};
			var client = new HttpClient(handler);
			using var parser = new DynamicProxyGeniusParser(client);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicRequestEmptyWebProxy()
		{
			using var parser = new DynamicProxyGeniusParser((request) =>
			{
				request.Proxy = new WebProxy();
				return request;
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_HttpClientChangeEmptyWebProxy()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy
			};
			var client = new HttpClient(handler);
			using var parser = new DynamicProxyGeniusParser(client);

			var result1 = await parser.ParseAsync(_correctUrl);
			parser.ChangeProxy(new WebProxy());
			var result2 = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_RequestChangeEmptyWebProxy()
		{
			using var parser = new DynamicProxyGeniusParser((request) =>
			{
				request.Proxy = _correctProxy;
				return request;
			});

			var result1 = await parser.ParseAsync(_correctUrl);
			parser.ChangeProxy(new WebProxy());
			var result2 = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicProxyConstructor()
		{
			using var parser = new DynamicProxyGeniusParser(_correctProxyHost, _correctProxyPort);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}

		[Test]
		public async Task ParseAsync_DynamicProxyChange()
		{
			using var parser = new DynamicProxyGeniusParser(_wrongProxy);

			var result1 = await parser.ParseAsync(_correctUrl);
			parser.ChangeProxy(_correctProxyHost, _correctProxyPort);
			var result2 = await parser.ParseAsync(_correctUrl);

			Assert.Pass();
		}
	}
}