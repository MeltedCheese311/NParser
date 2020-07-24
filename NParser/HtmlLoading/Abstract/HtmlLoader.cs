using NParser.HtmlLoading.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NParser.HtmlLoading.Abstract
{
	/// <summary>
	/// Class for loading HTML of any Url.
	/// </summary>
	internal abstract class HtmlLoader : IDisposable
    {
        /// <summary>
        /// Get HTML code of input Url.
        /// </summary>
        /// <param name="url">Website Url.</param>
        /// <returns>HTML code as <see cref="string"/>.</returns>
        /// <exception cref="WebException">If response status code is not 200.</exception>
        internal async Task<string> GetHtmlStringAsync(string url)
        {
            Response response;
            try
            {
                response = await GetResponseAsync(url);
            }
            catch
            {
                throw;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Html;
            }
            else
            {
                throw new WebException($"Incorrect response. Status code: {response.StatusCode}");
            }
        }

        /// <summary>
        /// Get response from Url.
        /// </summary>
        /// <param name="url">Website Url.</param>
        /// <returns>Response from Url.</returns>
        internal abstract Task<Response> GetResponseAsync(string url);

        /// <summary>
        /// Change proxy for next requests.
        /// </summary>
        /// <param name="proxy">Prepared proxy.</param>
        internal abstract void ChangeProxy(WebProxy proxy);

        public abstract void Dispose();
	}
}
