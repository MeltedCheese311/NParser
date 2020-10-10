using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace NParser.Proxy.Loader.Abstractions
{
	/// <summary>
	/// Interface for changing current proxy.
	/// </summary>
	public interface IProxyChanger
	{
		/// <summary>
		/// Change current proxy.
		/// </summary>
		/// <param name="proxy">New proxy.</param>
		void ChangeProxy(IWebProxy proxy);
	}
}
