using HtmlLoading.Loaders;
using HtmlLoading.Loaders.Abstract;
using NParser.Proxy.Loader.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NParser.Proxy.Loader
{
	/// <summary>
	/// <inheritdoc/> It is possible to change the proxy.
	/// </summary>
	public sealed class DynamicWebRequestLoader : WebRequestLoader, IProxyChanger
	{
		/// <summary>
		/// Create an instance of <see cref="DynamicWebRequestLoader"/> with prepared properties of <see cref="HttpWebRequest"/>. 
		/// </summary>
		/// <param name="configureRequest"><see cref="Action"/> for set settings of <see cref="HttpWebRequest"/>.</param>
		public DynamicWebRequestLoader(Action<HttpWebRequest> configureRequest)
			: base(configureRequest)
		{
		}

		public void ChangeProxy(IWebProxy proxy)
		{
			_configureRequest -= _changeProxy;
			_changeProxy = (request) => request.Proxy = proxy;
			_configureRequest += _changeProxy;
		}
	}
}
