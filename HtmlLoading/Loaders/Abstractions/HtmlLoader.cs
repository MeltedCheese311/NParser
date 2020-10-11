using HtmlLoading.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HtmlLoading.Loaders.Abstractions
{
	/// <summary>
	/// Class for loading HTML of any Url.
	/// </summary>
	public abstract class HtmlLoader : IHtmlLoader, IDisposable
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="url"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
        /// <exception cref="WebException">If response status code is not 200.</exception>
        public async Task<string> GetHtmlStringAsync(string url)
        {
            var response = await GetResponseAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Html;
            }
            else
            {
                throw new WebException($"Incorrect response. Status code: {(int)response.StatusCode} ({response.StatusCode})");
            }
        }

        /// <summary>
        /// Get response from Url.
        /// </summary>
        /// <param name="url">Website Url.</param>
        /// <returns>Response from Url.</returns>
        protected abstract Task<Response> GetResponseAsync(string url);

        public abstract void Dispose();
    }
}
