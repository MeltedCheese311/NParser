using NParser.Factory;
using NParser.HtmlLoading.Abstract;
using System;
using System.Net;
using System.Net.Http;

namespace NParser
{
	/// <summary>
	/// <para>Abstract class with basic logic for parsing with dynamically changing proxy.</para>
	/// <para>Child classes should override <see cref="Parser{T}.ParseHtml(IDocument)"/> for parsing a specific site using NuGet AngleSharp.</para>
	/// </summary>
	/// <typeparam name="T">Parsing result type.</typeparam>
	public abstract class DynamicProxyParser<T> : Parser<T>
	{
		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="IWebProxy"/>.
		/// </summary>
		/// <param name="proxy">Prepared proxy.</param>
		public DynamicProxyParser(IWebProxy proxy)
		: base(proxy)
		{

		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with proxy.
		/// </summary>
		/// <param name="host">The name of the proxy host.</param>
		/// <param name="port">The port number of host to use.</param>
		public DynamicProxyParser(string host, int port)
		: base(new WebProxy(host, port))
		{

		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="HttpWebRequest"/>.
		/// </summary>
		/// <param name="configureRequest"><see cref="Action"/> for setting properties of <see cref="HttpWebRequest"/>.</param>
		public DynamicProxyParser(Action<HttpWebRequest> configureRequest)
		: base(configureRequest)
		{

		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="makeHandler"><see cref="Func{TResult}"/> for creating an instance of <see cref="HttpClientHandler"/>.</param>
		/// <param name="configureClient"><see cref="Action"/> for setting properties of <see cref="HttpClient"/>.</param>
		public DynamicProxyParser(Func<HttpClientHandler> makeHandler, Action<HttpClient> configureClient)
		: base(new CachedHttpClientFactory(new HttpClientFactory(makeHandler, configureClient)))
		{

		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="makeHandler"><see cref="Func{TResult}"/> for creating an instance of <see cref="HttpClientHandler"/>.</param>
		public DynamicProxyParser(Func<HttpClientHandler> makeHandler)
		: base(new CachedHttpClientFactory(new HttpClientFactory(makeHandler)))
		{

		}

		/// <summary>
		/// Change proxy for next requests.
		/// </summary>
		/// <param name="proxy">Prepared proxy.</param>
		public void ChangeProxy(IWebProxy proxy) => Loader.ChangeProxy(proxy);

		/// <summary>
		/// Change proxy for next requests.
		/// </summary>
		/// <param name="host">The name of the proxy host.</param>
		/// <param name="port">The port number of host to use.</param>
		public void ChangeProxy(string host, int port) => ChangeProxy(new WebProxy(host, port));
	}
}
