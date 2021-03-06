## Description
There are several NuGet packages for parsing web pages using CSS selectors (AngleSharp):  
* **NParser** is a package for simple parsing.  
* **NParser.Proxy** is an improved `NParser` package with dynamically changing proxy.

Secondary packages:
* **HtmlLoading** is a package for loading HTML content of web pages.
* **HtmlLoading.Proxy** is an improved `HtmlLoading` package with dynamically changing proxy.
* **ProxyChanging** is a package for changing proxy.

## Table of contents
1. [How to use this package](#package_using)
2. [How to use CSS selectors](#selectors_description)
3. [Examples](#examples)

<a name=package_using></a>
## How to use this package
You need to inherit `Parser<T>`, `DynamicProxyParser<T>` or `AutoProxyParser<T>` class and override `ParseHtmlAsync` method.  
In this method you need to find the necessary nodes in HTML document using AngleSharp with CSS selectors and convert result to the data type you need.  
Also you can use `NParser.DefaultParser` class.  

**Class description**:
* `NParser.Parser<T>` is an abstract class for parsing web pages using AngleSharp.  
* `NParser.DefaultParser` is a simple concrete class for parsing using CSS-selectors.  
* `NParser.Proxy.DynamicProxyParser<T>` is an abstract class for parsing web pages using AngleSharp with dynamically changing proxy.  
* `NParser.Proxy.AutoProxyParser<T>` is an abstract class for parsing web pages using AngleSharp with automatically changing proxy when an incorrect request is made.  

<a name=selectors_description></a>
## How to use CSS selectors
**Basic types of selectors:**
* `*` - any elements.
* `div` - elements with this tag.
* `#id` - the element with the given id.
* `.class` - elements with this class.
* `[name = "value"]` - selectors for an attribute.
* `:visited` - "pseudo-classes", other different conditions for the element.

**Selectors can be combined by writing sequentially, without a space:**
* `.c1.c2` - elements with two classes `c1` and `c2`.
* `a#id.c1.c2:visited` - An element `a` with a given `id`, classes `c1` and `c2`, and the `visited` pseudo-class.

**Relations:**  
There are four kinds of relationships between elements in CSS3.
* `div p` - `p` elements that are children of `div`.
* `div > p` - only immediate descendants.
* `div ~ p` - right neighbors: all `p`'s at the same nesting level that come after the `div`.
* `div + p` - first right neighbor: `p` at the same nesting level immediately after the `div`.

**Attribute selectors:**  
*For the whole attribute:*
* `[attr]` - the attribute is set,
* `[attr = "val"]` - the attribute is equal to `val`.  

*For the start of the attribute:*
* `[attr ^ = "val"]` - the attribute starts with `val`, for example `value`.
* `[attr | = "val"]` - attribute is equal to `val` or starts with `val-`, for example equal to `val-1`.  

*For the content of the attribute:*
* `[attr * = "val"]` - the attribute contains the substring `val`, for example, it is equal to `myvalue`.
* `[attr ~ = "val"]` - the attribute contains `val` as one of the values separated by a space. For example: `[attr ~ = "delete"]` is true for `edit delete` and not true for `undelete` and also wrong for `no-delete`.  

*For the end of the attribute:*
* `[attr $ = "val"]` - the attribute ends with `val`, for example, it is equal to `myval`.

<a name=examples></a>
## Examples
An example of using simple parser class `DefaultParser`:
```c#
// create an instance of parser.
using var parser = new DefaultParser();
// parse webpage.
var result = await parser.ParseAsync("https://genius.com/Last-dinosaurs-apollo-lyrics", "div.lyrics");
```

Also you can create your own parsers using abstract class `Parser<T>`.  
An example of creating a parser:
```c#
public class ExampleParser : Parser<string>
{
	protected override Task<string> ParseHtmlAsync(IDocument html)
	{
		// find node using CSS selectors and read the text content
		var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
		return Task.FromResult(result);
	}
}
```

An example of using a parser:
```c#
// create an instance of concrete parser
using var parser = new ExampleParser();
// parse webpage
var result = await parser.ParseAsync("https://genius.com/Last-dinosaurs-apollo-lyrics");
```

HttpClient and HttpWebRequest are supported for setting request parameters.  
An example using HttpWebRequest:
```c#
// configure HttpWebRequest and create an instance of parser
using var parser = new ExampleParser((request) =>
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
using var parser = new ExampleParser(client);
var result = await parser.ParseAsync(url);
```

For the examples above, the parser code looks like this:
```c#
public class ExampleParser : Parser<string>
{
	// add the necessary constructors and just pass the parameters to the base constructor
	public ExampleParser(Action<HttpWebRequest> configureRequest) : base(configureRequest) { }

	public ExampleParser(HttpClient client) : base(client) { }

	protected override Task<string> ParseHtmlAsync(IDocument html)
	{
		var result = html.QuerySelectorAll("div.lyrics").FirstOrDefault()?.TextContent ?? "";
		return Task.FromResult(result);
	}
}
```

For cases when it is necessary to dynamically change the proxy, there is a `DynamicProxyParser` class that allows you to change the proxy at any time. This class also allows you to set parameters of `HttpWebRequest` and `HttpClient`:
```c#
// ExampleParser - your inheritor of DynamicProxyParser class
using var parser = new ExampleParser((request) =>
{
	request.Proxy = new WebProxy(host, port);
});
var result1 = await parser.ParseAsync(url);

parser.ChangeProxy("167.172.247.130", 8080);
var result2 = await parser.ParseAsync(url);
```

There is also a `AutoProxyParser<T>` class with the ability to automatically change proxies when an incorrect request is made.  
To create an instance of this class you need create instance of your own `DynamicProxyParser<T>` and instance of `IProxyGetter`/`IProxyProvider`.  
Event `AutoProxyChanged` triggered when the proxy changes automatically. To stop changing proxies, call `StopChangingProxy` method:
```c#
// AutoProxyExampleParser - your inheritor of AutoProxyParser class.
// dynamicProxyExampleParser - your inheritor of DynamicProxyParser class.
// proxyProvider - your inheritor of IProxyProvider interface for getting list of proxies.
using var parser = new AutoProxyExampleParser(dynamicProxyExampleParser, proxyProvider);

parser.AutoProxyChanged += (sender, args)
{
	// your event handling logic.
}

var result = await parser.ParseAsync(url);
```

A real example when you need to use other parsers inside the parser:
```c#
public class StoresParser : Parser<IEnumerable<Store>>
{
	// nested parser
	private readonly Parser<string> _itemParser;

	public StoresParser() => _itemParser = new ItemParser();

	protected override async Task<IEnumerable<Store>> ParseHtmlAsync(IDocument html)
	{
		// parse using nested parser
		var items = html.QuerySelectorAll("a.name");
		var result = (await Task.WhenAll(items
			.Select(async x =>
			{
				var name = x?.TextContent ?? "";
				var link = $"https://www.qwerty.com{x.GetAttribute("href")}";
				var email = await _itemParser.ParseAsync(link);
				return new Store(name, email);
			})))
			.Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Content))
			.ToArray();

		return result;
	}
}

public class ItemParser : Parser<string>
{
	protected override Task<string> ParseHtmlAsync(IDocument html)
	{
		var result = html
			.QuerySelectorAll("a")
			.FirstOrDefault(x => x.GetAttribute("href")?.Contains("item:") == true)
			?.TextContent ?? "";
		return Task.FromResult(result);
	}
}
```
