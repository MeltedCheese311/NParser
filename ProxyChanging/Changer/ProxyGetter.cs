using ProxyChanging.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ProxyChanging.Changer
{
	// TODO
	// запоминать прокси, с которыми был получен результат. идти по кругу с ними.
	// вывод куда-нибудь прокси, которые запомнили.

	/// <summary>
	/// Class for getting a new proxy from list of proxies.
	/// </summary>
	public sealed class ProxyGetter : IProxyGetter
	{
		/// <summary>
		/// Array of proxies.
		/// </summary>
		private readonly IWebProxy[] _proxies;

		/// <summary>
		/// Index of current proxy in <see cref="_proxies"/>.
		/// </summary>
		private int _currentIndex;

		/// <summary>
		/// Create an instance of <see cref="ProxyGetter"/>.
		/// </summary>
		/// <param name="provider">Object for getting a list of proxies.</param>
		public ProxyGetter(IProxyProvider provider)
		{
			_proxies = provider.Proxies.ToArray();
			_currentIndex = 0;
		}

		public IWebProxy GetNewProxy()
		{
			var proxy = _proxies[_currentIndex];
			_currentIndex = (_currentIndex + 1) % _proxies.Length;
			return proxy;
		}
	}
}
