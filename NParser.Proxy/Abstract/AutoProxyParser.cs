using NParser.Abstract;
using NParser.Proxy.Events;
using ProxyChanging.Changer;
using ProxyChanging.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NParser.Proxy
{
	/// <summary>
	/// Decorator of <see cref="DynamicProxyParser{T}"/> for automatically changing proxy when an incorrect request is made.
	/// </summary>
	/// <typeparam name="T">Parsing result type.</typeparam>
	public abstract class AutoProxyParser<T> : IParser<T>
	{
		/// <summary>
		/// Object for parsing webpages with dynamic proxy.
		/// </summary>
		private readonly DynamicProxyParser<T> _innerParser;

		/// <summary>
		/// Object for getting a new proxy.
		/// </summary>
		private readonly IProxyGetter _proxyGetter;

		/// <summary>
		/// Object for cancel all cancellation tokens.
		/// </summary>
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		/// <summary>
		/// Create an instance of <see cref="AutoProxyParser{T}"/> with default instance of <see cref="ProxyGetter"/>.
		/// </summary>
		/// <param name="dynamicProxyParser">Object for parsing webpages with dynamic proxy.</param>
		/// <param name="proxyProvider">Object for getting list of proxies.</param>
		public AutoProxyParser(
			DynamicProxyParser<T> dynamicProxyParser,
			IProxyProvider proxyProvider)
			: this(
				  dynamicProxyParser,
				  new ProxyGetter(proxyProvider))
		{
		}

		/// <summary>
		/// Create an instance of <see cref="AutoProxyParser{T}"/>.
		/// </summary>
		/// <param name="dynamicProxyParser">Object for parsing webpages with dynamic proxy.</param>
		/// <param name="proxyGetter">Object for getting a new proxy.</param>
		public AutoProxyParser(
			DynamicProxyParser<T> dynamicProxyParser,
			IProxyGetter proxyGetter)
		{
			_innerParser = dynamicProxyParser;
			_proxyGetter = proxyGetter;
		}

		/// <summary>
		/// Automatically proxy changed event.
		/// </summary>
		public event AutoProxyChangedEventHandler AutoProxyChanged;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="url"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="ArgumentNullException">One of the arguments passed to the constructor is null.</exception>
		public async Task<T> ParseAsync(string url)
		{
			if (_innerParser is null)
			{
				throw new ArgumentNullException("dynamicProxyParser", $"Instance of {nameof(DynamicProxyParser<T>)} is null.");
			}

			try
			{
				return await _innerParser.ParseAsync(url);
			}
			catch (Exception ex)
			{
				var handlingExceptions = new[]
				{
					typeof(HttpRequestException),
					typeof(WebException),
				};

				if (handlingExceptions.Contains(ex.GetType()))
				{
					if (_proxyGetter is null)
					{
						throw new ArgumentNullException("proxyGetter", $"Instance of {nameof(IProxyGetter)} is null.");
					}

					var cancellationToken = _cancellationTokenSource.Token;
					var isHandledException = true;

					while (!cancellationToken.IsCancellationRequested && isHandledException)
					{
						var newProxy = _proxyGetter.GetNewProxy();
						var eventArgs = new AutoProxyChangedEventArgs(ex, newProxy);

						_innerParser.ChangeProxy(newProxy);
						AutoProxyChanged?.Invoke(this, eventArgs);
						try
						{
							return await _innerParser.ParseAsync(url);
						}
						catch (Exception exception)
						{
							if (!handlingExceptions.Contains(exception.GetType()))
							{
								throw;
							}
						}
					}

					cancellationToken.ThrowIfCancellationRequested();
					throw;
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Stop changing proxy.
		/// </summary>
		public void StopChangingProxy() => _cancellationTokenSource.Cancel();
	}
}
