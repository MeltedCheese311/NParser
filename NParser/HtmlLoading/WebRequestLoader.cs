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
		/// Func for set settings of <see cref="HttpWebRequest"/>.
		/// </summary>
		private readonly Func<HttpWebRequest, HttpWebRequest> _configureRequest;

		/// <summary>
		/// Create an instance of <see cref="HtmlLoader"/> with prepared properties of <see cref="HttpWebRequest"/>. 
		/// </summary>
		/// <param name="configureRequest">Func for set settings of <see cref="HttpWebRequest"/>.</param>
		internal WebRequestLoader(Func<HttpWebRequest, HttpWebRequest> configureRequest) => _configureRequest = configureRequest;

		internal override async Task<Response> GetResponseAsync(string url)
		{
			_request = (HttpWebRequest)WebRequest.Create(url);
			_request = _configureRequest(_request);

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

		internal override void ChangeProxy(string host, int port) => _request.Proxy = new WebProxy(host, port);

		public override void Dispose()
		{
		}
	}
}
