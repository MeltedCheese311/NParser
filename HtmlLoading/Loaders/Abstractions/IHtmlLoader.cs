using HtmlLoading.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HtmlLoading.Loaders.Abstractions
{
	/// <summary>
	/// Interface for loading HTML of any Url.
	/// </summary>
	public interface IHtmlLoader
	{
		/// <summary>
		/// Get HTML code of input Url.
		/// </summary>
		/// <param name="url">Website Url.</param>
		/// <returns>HTML code as <see cref="string"/>.</returns>
		Task<string> GetHtmlStringAsync(string url);
	}
}
