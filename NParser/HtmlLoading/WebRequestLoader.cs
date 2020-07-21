using NParser.HtmlLoading.Abstract;
using NParser.HtmlLoading.Models;
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
		private readonly HttpWebRequest _request;

		/// <summary>
		/// Create an instance of <see cref="HtmlLoader"/> with prepated <see cref="HttpWebRequest"/>.     
		/// </summary>
		/// <param name="request">Prepared instance of <see cref="HttpWebRequest"/>.</param>
		internal WebRequestLoader(HttpWebRequest request) => _request = request;

		internal override async Task<Response> GetResponseAsync(string url)
		{
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
	}
}
