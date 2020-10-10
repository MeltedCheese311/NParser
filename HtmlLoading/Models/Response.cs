using System.Net;

namespace HtmlLoading.Models
{
	/// <summary>
	/// Response from any Url.
	/// </summary>
	public class Response
    {
        /// <summary>
        /// Create an instance of <see cref="Response"/>.
        /// </summary>
        /// <param name="html">The HTML code of webpage.</param>
        /// <param name="statusCode">The status code of response.</param>
        public Response(
            string html,
            HttpStatusCode statusCode)
        {
            Html = html;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Create an instance of <see cref="Response"/> with empty HTML code.
        /// </summary>
        /// <param name="statusCode">The status code of response.</param>
        public Response(HttpStatusCode statusCode)
            : this(
                  string.Empty,
                  statusCode)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="Response"/> with 200 status code.
        /// </summary>
        /// <param name="html">The HTML code of webpage.</param>
        public Response(string html)
            : this(
                  html,
                  HttpStatusCode.OK)
        {
        }

        /// <summary>
        /// HTML code of webpage.
        /// </summary>
        public string Html { get; }

        /// <summary>
        /// Status code of response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
    }
}
