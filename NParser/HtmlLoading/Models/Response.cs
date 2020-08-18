using System.Net;

namespace NParser.HtmlLoading.Models
{
	/// <summary>
	/// Response from any Url.
	/// </summary>
	internal class Response
    {
        /// <summary>
        /// Create an instance of <see cref="Response"/>.
        /// </summary>
        /// <param name="html">The HTML code of webpage.</param>
        /// <param name="statusCode">The status code of response.</param>
        internal Response(string html, HttpStatusCode statusCode)
        {
            Html = html;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Create an instance of <see cref="Response"/> with empty HTML code.
        /// </summary>
        /// <param name="statusCode">The status code of response.</param>
        internal Response(HttpStatusCode statusCode)
            : this(string.Empty, statusCode)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="Response"/> with 200 status code.
        /// </summary>
        /// <param name="html">The HTML code of webpage.</param>
        internal Response(string html)
            : this(html, HttpStatusCode.OK)
        {
        }

        /// <summary>
        /// HTML code of webpage.
        /// </summary>
        internal string Html { get; }

        /// <summary>
        /// Status code of response.
        /// </summary>
        internal HttpStatusCode StatusCode { get; }
    }
}
