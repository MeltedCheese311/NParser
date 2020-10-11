using AngleSharp.Dom;
using HtmlLoading.Factory;
using HtmlLoading.Proxy.Loaders;
using HtmlLoading.Proxy.Loaders.Abstractions;
using ProxyChanging.Changer;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NParser.Proxy
{
	/// <summary>
	/// <para>Abstract class with basic logic for parsing with dynamically changing proxy.</para>
	/// <para>Child classes should override <see cref="Parser{T}.ParseHtmlAsync(IDocument)"/> for parsing a specific site using NuGet AngleSharp.</para>
	/// </summary>
	/// <typeparam name="T">Parsing result type.</typeparam>
	public abstract class DynamicProxyParser<T> : Parser<T>, IProxyChanger
	{
		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="IWebProxy"/>.
		/// </summary>
		/// <param name="proxy">Prepared proxy.</param>
		public DynamicProxyParser(IWebProxy proxy)
			: base(
				  new DynamicHttpClientLoader(
					  new HttpClient(
						  new HttpClientHandler { Proxy = proxy })))
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with proxy.
		/// </summary>
		/// <param name="host">The name of the proxy host.</param>
		/// <param name="port">The port number of host to use.</param>
		public DynamicProxyParser(
			string host,
			int port)
			: this(new WebProxy(host, port))
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="HttpWebRequest"/>.
		/// </summary>
		/// <param name="configureRequest"><see cref="Action"/> for setting properties of <see cref="HttpWebRequest"/>.</param>
		public DynamicProxyParser(Action<HttpWebRequest> configureRequest)
			: base(new DynamicWebRequestLoader(configureRequest))
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="makeHandler"><see cref="Func{TResult}"/> for creating an instance of <see cref="HttpClientHandler"/>.</param>
		/// <param name="configureClient"><see cref="Action"/> for setting properties of <see cref="HttpClient"/>.</param>
		public DynamicProxyParser(
			Func<HttpClientHandler> makeHandler,
			Action<HttpClient> configureClient)
			: base(
				  new DynamicHttpClientLoader(
					  new CachedHttpClientFactory(
						  new HttpClientFactory(
							  makeHandler,
							  configureClient))))
		{
		}

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="makeHandler"><see cref="Func{TResult}"/> for creating an instance of <see cref="HttpClientHandler"/>.</param>
		public DynamicProxyParser(Func<HttpClientHandler> makeHandler)
			: base(
				  new DynamicHttpClientLoader(
					  new CachedHttpClientFactory(
						  new HttpClientFactory(makeHandler))))
		{
		}

		/// <summary>
		/// Change proxy for next requests.
		/// </summary>
		/// <param name="proxy">Prepared proxy.</param>
		/// <exception cref="InvalidOperationException">If <see cref="HtmlLoader"/> not implement <see cref="IProxyChanger"/>.</exception>
		public void ChangeProxy(IWebProxy proxy)
		{
			if (!(_loader is IProxyChanger proxyChanger))
			{
				throw new InvalidOperationException($"Unable to change proxy because {nameof(_loader)} not implement {nameof(IProxyChanger)}.");
			}

			proxyChanger.ChangeProxy(proxy);
		}

		/// <summary>
		/// Change proxy for next requests.
		/// </summary>
		/// <param name="host">The name of the proxy host.</param>
		/// <param name="port">The port number of host to use.</param>
		public void ChangeProxy(string host, int port) => ChangeProxy(new WebProxy(host, port));
	}
}
