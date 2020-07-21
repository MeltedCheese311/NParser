using NParser.HtmlLoading.Abstract;
using System.Net;

namespace NParser
{
	/// <summary>
	/// <para>Abstract class with basic logic for parsing with dynamically changing proxy.</para>
	/// <para>Child classes should override <see cref="Parser{T}.ParseHtml(IDocument)"/> for parsing a specific site using NuGet AngleSharp.</para>
	/// </summary>
	/// <typeparam name="T">Parsing result type.</typeparam>
	public abstract class DynamicProxyParser<T> : Parser<T>
	{
		internal override HtmlLoader Loader { get; }

		/// <summary>
		/// Create an instance of <see cref="DynamicProxyParser{T}"/> with prepared <see cref="HttpWebRequest"/>.
		/// </summary>
		public DynamicProxyParser(HttpWebRequest request) : base(request) { }

		/// <summary>
		/// Change proxy for next requests.
		/// </summary>
		/// <param name="host">The name of the proxy host.</param>
		/// <param name="port">The port number on host to use.</param>
		public void ChangeProxy(string host, int port) => Loader.ChangeProxy(host, port);
	}
}
