using System.Net;
using System.Net.Http;

namespace HtmlLoading.Factory
{
	/// <summary>
	/// Factory for creating <see cref="HttpClient"/>.
	/// </summary>
	public interface IHttpClientFactory
    {
        /// <summary>
        /// Create an instance of <see cref="HttpClient"/>.
        /// </summary>
        /// <returns>Instance of <see cref="HttpClient"/>.</returns>
        HttpClient CreateClient();

        /// <summary>
        /// Create an instance of <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="webProxy">Proxy.</param>
        /// <returns>Instance of <see cref="HttpClient"/>.</returns>
        HttpClient CreateClientWithProxy(IWebProxy webProxy);
    }
}
