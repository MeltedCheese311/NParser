using NUnit.Framework;
using System;
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
		private readonly string _lyrics = "[Verse 1]\nI don't want to know\nYou're driving me crazy\nSay it again my memory's hazy\nI'm falling down the rabbit hole again, yeah\nAll things in time will fade away\nBut I by design will never stray from knowing this life is not the one for me\n\n[Chorus]\nOh I'm ready to be somebody else\nI'll forget how to feel the things I've felt\n\n[Verse 2]\nIdeas in the air\nThe miracle methods\nI'll never get\nI'm easily tempted\nI'll follow you if you're offering the truth, yeah\nMy mind is made up\nI'm willing to come down and wake up\nThe longest I would know of this life\nIt's not the one for me\n\n[Chorus]\nOh I'm ready to be somebody else\nI'll forget how to feel the things I've felt\n\n[Bridge]\nOne more time\nI need to see you one more time\nI'm leaving 'cause I need to know if there's more than this, yeah\n\n[Chorus]\nOh I'm ready to be somebody else\nI'll forget how to feel the things I've felt";

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void ParseAsync_NullArg()
		{
			using var parser = new GeniusParser();

			Assert.CatchAsync<InvalidOperationException>(async () =>
			{
				await parser.ParseAsync(null);
			});
		}

		[Test]
		public void ParseAsync_EmptyStringArg()
		{
			using var parser = new GeniusParser();

			Assert.CatchAsync<InvalidOperationException>(async () => 
			{
				await parser.ParseAsync("");
			});
		}

		[Test]
		public void ParseAsync_RandomStringArg()
		{
			using var parser = new GeniusParser();

			Assert.CatchAsync<InvalidOperationException>(async () =>
			{
				await parser.ParseAsync("qwe");
			});
		}

		[Test]
		public void ParseAsync_UrlWithoutHttpsArg()
		{
			using var parser = new GeniusParser();

			Assert.CatchAsync<InvalidOperationException>(async () =>
			{
				await parser.ParseAsync(_urlWithoutHttps);
			});
		}

		[Test]
		public async Task ParseAsync_CorrectUrlArg()
		{
			using var parser = new GeniusParser();

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task ParseAsync_EmptyHttpClient()
		{
			var client = new HttpClient();
			using var parser = new GeniusParser(client);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public void ParseAsync_WrongProxyHttpClient()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _wrongProxy
			};
			var client = new HttpClient(handler);
			using var parser = new GeniusParser(client);

			Assert.CatchAsync<WebException>(async() => 
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public async Task ParseAsync_NullRequest()
		{
			using var parser = new GeniusParser((request) => request = null);

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task ParseAsync_NewRequest()
		{
			using var parser = new GeniusParser((request) => request = (HttpWebRequest)WebRequest.Create(_correctUrl));

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task ParseAsync_UnchangedRequest()
		{
			using var parser = new GeniusParser((request) => { });

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task ParseAsync_CustomizedRequest()
		{
			using var parser = new GeniusParser((request) =>
			{
				request.CookieContainer = new CookieContainer();
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
				request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
				request.Timeout = 5000;
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public void ParseAsync_WrongProxyRequest()
		{
			using var parser = new GeniusParser((request) =>
			{
				request.Proxy = _wrongProxy;
			});

			Assert.CatchAsync<WebException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public void ParseAsync_DynamicWebProxyConstructor()
		{
			using var parser = new DynamicProxyGeniusParser(_correctProxy);

			Assert.CatchAsync<HttpRequestException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
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

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task ParseAsync_RequestEmptyWebProxy()
		{
			using var parser = new GeniusParser((request) =>
			{
				request.Proxy = new WebProxy();
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task ParseAsync_DynamicEmptyWebProxy()
		{
			using var parser = new DynamicProxyGeniusParser(new WebProxy());

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
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

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task ParseAsync_DynamicRequestEmptyWebProxy()
		{
			using var parser = new DynamicProxyGeniusParser((request) =>
			{
				request.Proxy = new WebProxy();
			});

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public void ParseAsync_CorrectProxyRequest()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy
			};
			var client = new HttpClient(handler)
			{
				Timeout = TimeSpan.FromSeconds(2)
			};
			using var parser = new GeniusParser(client);

			Assert.CatchAsync<TaskCanceledException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public void ParseAsync_DynamicProxyHttpClient()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy1
			};
			var client = new HttpClient(handler)
			{
				Timeout = TimeSpan.FromSeconds(2)
			};
			using var parser = new DynamicProxyGeniusParser(client);

			Assert.CatchAsync<TaskCanceledException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});

			parser.ChangeProxy(_correctProxy2);

			Assert.CatchAsync<TaskCanceledException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public void ParseAsync_DynamicProxyRequest()
		{
			using var parser = new DynamicProxyGeniusParser((request) =>
			{
				request.Proxy = _correctProxy1;
				request.Timeout = 2000;
			});

			Assert.CatchAsync<WebException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});

			parser.ChangeProxy(_correctProxy2);

			Assert.CatchAsync<WebException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public void ParseAsync_CorrectProxyHttpClient()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy
			};
			var client = new HttpClient(handler)
			{
				Timeout = TimeSpan.FromSeconds(2)
			};
			using var parser = new GeniusParser(client);

			Assert.CatchAsync<TaskCanceledException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public void ParseAsync_HttpClientChangeEmptyWebProxy()
		{
			var handler = new HttpClientHandler
			{
				Proxy = _correctProxy
			};
			var client = new HttpClient(handler)
			{
				Timeout = TimeSpan.FromSeconds(2)
			};
			using var parser = new DynamicProxyGeniusParser(client);

			Assert.CatchAsync<TaskCanceledException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});

			parser.ChangeProxy(new WebProxy());

			Assert.CatchAsync<TaskCanceledException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public async Task ParseAsync_RequestChangeEmptyWebProxy()
		{
			using var parser = new DynamicProxyGeniusParser((request) =>
			{
				request.Proxy = _correctProxy;
			});

			Assert.CatchAsync<WebException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});

			parser.ChangeProxy(new WebProxy());

			var result = await parser.ParseAsync(_correctUrl);

			Assert.IsNotNull(result);
		}

		[Test]
		public void ParseAsync_DynamicProxyConstructor()
		{
			using var parser = new DynamicProxyGeniusParser(_correctProxyHost, _correctProxyPort);

			Assert.CatchAsync<HttpRequestException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}

		[Test]
		public void ParseAsync_DynamicProxyChange()
		{
			using var parser = new DynamicProxyGeniusParser(_wrongProxy);

			Assert.CatchAsync<WebException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});

			parser.ChangeProxy(_correctProxyHost, _correctProxyPort);

			Assert.CatchAsync<HttpRequestException>(async () =>
			{
				await parser.ParseAsync(_correctUrl);
			});
		}
	}
}