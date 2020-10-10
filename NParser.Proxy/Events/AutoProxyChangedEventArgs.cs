using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NParser.Proxy.Events
{
	/// <summary>
	/// Delegate for automatically proxy changed event.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	public delegate void AutoProxyChangedEventHandler(object sender, AutoProxyChangedEventArgs args);

	/// <summary>
	/// Event args for automatically proxy changed event.
	/// </summary>
	public sealed class AutoProxyChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Create instance of <see cref="AutoProxyChangedEventArgs"/>.
		/// </summary>
		/// <param name="exception">An exception due to which the proxy changes.</param>
		/// <param name="newProxy">New proxy.</param>
		public AutoProxyChangedEventArgs(
			Exception exception,
			IWebProxy newProxy)
			: base()
		{
			Exception = exception;
			NewProxy = newProxy;
		}

		/// <summary>
		/// An exception due to which the proxy changes.
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		/// New proxy.
		/// </summary>
		public IWebProxy NewProxy { get; }
	}
}
