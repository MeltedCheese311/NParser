using HtmlLoading.Loaders.Abstractions;
using HtmlLoading.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HtmlLoading.Loaders
{
	/// <summary>
	/// Class for loading HTML of any Url using <see cref="HttpWebRequest"/>.
	/// </summary>
	public class WebRequestLoader : HtmlLoader
	{
		/// <summary>
		/// <see cref="Action"/> for set settings of <see cref="HttpWebRequest"/>.
		/// </summary>
		protected Action<HttpWebRequest> _configureRequest;

		/// <summary>
		/// Object for working with Url.
		/// </summary>
		private HttpWebRequest _request;

		/// <summary>
		/// <see cref="Action"/> for set new proxy for next requests.
		/// </summary>
		protected Action<HttpWebRequest> _changeProxy;

		/// <summary>
		/// Create an instance of <see cref="WebRequestLoader"/> with prepared properties of <see cref="HttpWebRequest"/>. 
		/// </summary>
		/// <param name="configureRequest"><see cref="Action"/> for set settings of <see cref="HttpWebRequest"/>.</param>
		public WebRequestLoader(Action<HttpWebRequest> configureRequest)
		{
			_configureRequest = configureRequest;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="url"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException">If an instance of <see cref="HttpWebRequest"/> was null.</exception>
		protected override async Task<Response> GetResponseAsync(string url)
		{
			_request = (HttpWebRequest)WebRequest.Create(url);
			_configureRequest(_request);

			if (_request == null)
			{
				throw new InvalidOperationException($"{nameof(HttpWebRequest)} was configured incorrectly. An instance of {nameof(HttpWebRequest)} is null.");
			}

			var response = _request.GetResponse() as HttpWebResponse;
			var statusCode = response?.StatusCode ?? default;

			if (statusCode == HttpStatusCode.OK)
			{
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					return new Response(await reader.ReadToEndAsync());
				}
			}
			else
			{
				return new Response(statusCode);
			}
		}

		public override void Dispose()
		{
		}
	}
}
