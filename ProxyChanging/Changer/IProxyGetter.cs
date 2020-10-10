using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ProxyChanging.Changer
{
	/// <summary>
	/// Interface for getting a new proxy.
	/// </summary>
	public interface IProxyGetter
	{
		/// <summary>
		/// Get new proxy.
		/// </summary>
		/// <returns>New proxy.</returns>
		IWebProxy GetNewProxy();
	}
}
