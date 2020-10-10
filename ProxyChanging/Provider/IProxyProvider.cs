using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ProxyChanging.Provider
{
	/// <summary>
	/// Interface for getting a list of proxies.
	/// </summary>
	public interface IProxyProvider
	{
		/// <summary>
		/// List of proxies.
		/// </summary>
		IEnumerable<IWebProxy> Proxies { get; }
	}
}
