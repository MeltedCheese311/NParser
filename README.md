## Description
**NParser** is a package bla-bla-bla-bla.

Пример создания парсера:
```c#
public class GeniusParser : Parser<string>
{
	protected override Task<string> ParseHtmlAsync(IDocument html)
  {
    var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
    return Task.FromResult(result);
  }
}
```

Пример использования парсера:
```c#
var parser = new GeniusParser();
var result = await parser.ParseAsync("https://genius.com/Last-dinosaurs-apollo-lyrics");
```

Поддерживаются HttpClient и HttpWebRequest для задания параметров запросов вручную:
HttpWebRequest:
```c#
var request = (HttpWebRequest)WebRequest.Create(url);
request.CookieContainer = _cookies ?? new CookieContainer();
request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
request.Proxy = new WebProxy(host, port);
request.Timeout = 10000;

var parser = new GeniusParser(request);
var result = await parser.ParseAsync(url);
```

HttpClient:
```c#
var handler = new HttpClientHandler
{
	Proxy = new WebProxy(host, port),
};
var client = new HttpClient(handler);

var parser = new GeniusParser(client);
var result = await parser.ParseAsync(url);
```

Код парсера для данных примеров:
```c#
public class GeniusParser : Parser<string>
{
	public GeniusParser(HttpWebRequest request) : base(request) { }

	public GeniusParser(HttpClient client) : base(client) { }

	protected override Task<string> ParseHtmlAsync(IDocument html)
  {
    var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
    return Task.FromResult(result);
  }
}
```

Для случаев, когда необходимо динамически изменять прокси, существует класс DynamicProxyParser, позволяющий изменять прокси в любое время. Также позволяет задавать параметры HttpClient/HttpWebRequest вручную:
```c#
var request = (HttpWebRequest)WebRequest.Create(url);
request.Proxy = new WebProxy(host, port);

var parser = new EmailParser();
var result = await parser.ParseAsync(url);

parser.ChangeProxy("167.172.247.130", 8080);
result = await parser.ParseAsync(url);
```

Пример, когда внутри парсера нужно использовать другие парсеры (парсинг сайтов с несколькими страницами):
```c#
public class TobaccoParser : Parser<IEnumerable<Store>>
{
	private readonly Parser<string> _emailParser;

	public TobaccoParser() => _emailParser = new EmailParser();

	protected override async Task<IEnumerable<Store>> ParseHtmlAsync(IDocument html)
	{
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
