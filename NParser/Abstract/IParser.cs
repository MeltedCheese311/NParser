using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NParser.Abstract
{
	/// <summary>
	/// Interface for parsing web pages.
	/// </summary>
	/// <typeparam name="T">Parsing result type.</typeparam>
	public interface IParser<T>
	{
		/// <summary>
		/// Parse the site.
		/// </summary>
		/// <param name="url">Website Url.</param>
		/// <returns>Parsing result as type <see cref="T"/>.</returns>
		Task<T> ParseAsync(string url);
	}
}
