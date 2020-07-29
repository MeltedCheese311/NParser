using NParser.HtmlLoading.Abstract;
using NParser.HtmlLoading.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NParser.HtmlLoading
{
	/// <summary>
	/// Class for loading HTML of any Url using <see cref="HttpWebRequest"/>.
	/// </summary>
	internal sealed class WebRequestLoader : HtmlLoader
	{
		/// <summary>
		/// Object for working with Url.
		/// </summary>
		private HttpWebRequest _request;

		/// <summary>
		/// <see cref="Action"/> for set settings of <see cref="HttpWebRequest"/>.
		/// </summary>
		private Action<HttpWebRequest> _configureRequest;

		/// <summary>
		/// <see cref="Action"/> for set new proxy for next requests.
		/// </summary>
		private Action<HttpWebRequest> _changeProxy;

		/// <summary>
		/// Create an instance of <see cref="WebRequestLoader"/> with prepared properties of <see cref="HttpWebRequest"/>. 
		/// </summary>
		/// <param name="configureRequest"><see cref="Action"/> for set settings of <see cref="HttpWebRequest"/>.</param>
		internal WebRequestLoader(Action<HttpWebRequest> configureRequest)
		{
			_configureRequest = configureRequest;
		}

		internal override async Task<Response> GetResponseAsync(string url)
		{
			_request = (HttpWebRequest)WebRequest.Create(url);
			_configureRequest(_request);

			if (_request == null)
			{
				throw new InvalidOperationException($"{nameof(HttpWebRequest)} was configured incorrectly.");
			}

			var response = _request.GetResponse() as HttpWebResponse;
			var statusCode = response?.StatusCode ?? default;

			if (statusCode == HttpStatusCode.OK)
			{
				using var reader = new StreamReader(response.GetResponseStream());
				return new Response(await reader.ReadToEndAsync());
			}
			else
			{
				return new Response(statusCode);
			}
		}

		internal override void ChangeProxy(IWebProxy proxy)
		{
			_configureRequest -= _changeProxy;
			_changeProxy = (request) => request.Proxy = proxy;
			_configureRequest += _changeProxy;
		}

		public override void Dispose()
		{
		}
	}
}
