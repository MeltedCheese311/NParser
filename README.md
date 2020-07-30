## Description
**NParser** is a NuGet package for parsing web pages using CSS selectors (AngleSharp).

An example of creating a parser:
```c#
public class GeniusParser : Parser<string>
{
	protected override Task<string> ParseHtmlAsync(IDocument html)
	{
		// find a new node using selectors and read the text content
		var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
		return Task.FromResult(result);
	}
}
```

An example of using a parser:
```c#
// create an instance of concrete parser
using var parser = new GeniusParser();
// parse webpage
var result = await parser.ParseAsync("https://genius.com/Last-dinosaurs-apollo-lyrics");
```

HttpClient and HttpWebRequest are supported for setting request parameters.
An example using HttpWebRequest:
```c#
// configure HttpWebRequest and create an instance of parser
using var parser = new GeniusParser((request) =>
{
	request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
	request.Proxy = new WebProxy(host, port);
	request.Timeout = 10000;
});
var result = await parser.ParseAsync(url);
```

An example using HttpClient:
```c#
// create an instance of HttpClient with arbitrary settings
var handler = new HttpClientHandler
{
	Proxy = new WebProxy(host, port),
};
var client = new HttpClient(handler);

// call constructor with configured HttpClient
using var parser = new GeniusParser(client);
var result = await parser.ParseAsync(url);
```

For the examples above, the parser code looks like this:
```c#
public class GeniusParser : Parser<string>
{
	// add the necessary constructors and just pass the parameters to the base constructor
	public GeniusParser(Action<HttpWebRequest> configureRequest) : base(configureRequest) { }

	public GeniusParser(HttpClient client) : base(client) { }

	protected override Task<string> ParseHtmlAsync(IDocument html)
	{
		var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
		return Task.FromResult(result);
	}
}
```

For cases when it is necessary to dynamically change the proxy, there is a `DynamicProxyParser` class that allows you to change the proxy at any time. This class also allows you to set parameters of `HttpWebRequest` and `HttpClient`:
```c#
using var parser = new EmailParser((request) =>
{
	request.Proxy = new WebProxy(host, port);
});
var result1 = await parser.ParseAsync(url);

parser.ChangeProxy("167.172.247.130", 8080);
var result2 = await parser.ParseAsync(url);
```

An example when you need to use other parsers inside the parser:
```c#
public class TobaccoParser : Parser<IEnumerable<Store>>
{
	// nested parser
	private readonly Parser<string> _emailParser;

	public TobaccoParser() => _emailParser = new EmailParser();

	protected override async Task<IEnumerable<Store>> ParseHtmlAsync(IDocument html)
	{
		// parse using nested parser
		var items = html.QuerySelectorAll("a.name");
		var result = (await Task.WhenAll(items
			.Select(async x =>
			{
				var name = x?.TextContent ?? "";
				var link = $"https://www.moscowmap.ru{x.GetAttribute("href")}";
				var email = await _emailParser.ParseAsync(link);
				return new Store(name, email);
			})))
			.Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Email))
			.ToArray();

		return result;
	}
}

public class EmailParser : Parser<string>
{
	protected override Task<string> ParseHtmlAsync(IDocument html)
	{
		var result = html
			.QuerySelectorAll("a")
			.FirstOrDefault(x => x.GetAttribute("href")?.Contains("mailto:") == true)
			?.TextContent ?? "";
		return Task.FromResult(result);
	}
}
```
