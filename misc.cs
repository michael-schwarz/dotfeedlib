using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;

namespace dotFeedLib
{
	/// <summary>
	/// Provides some functions that do not belong anywhere
	/// </summary>
	public static class misc
	{
		
		/// <summary>
		/// Checks whether given string is valid URL
		/// </summary>
		/// <param name="url">string to check</param>
		/// <returns>given string is URL</returns>
		public static bool IsUrl(String url)
    	{
     		 return Regex.IsMatch(url, @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
    	}
		
		/// <summary>
		/// DateTime out of AtomDateConstruct
		/// </summary>
		/// <param name="date">AtomDateConstruct</param>
		/// <returns>DateTime</returns>
		public static DateTime DTfromAtom(string date)
		{
			try
			{
				return XmlConvert.ToDateTime(date);
			}
			catch(Exception)
			{
				return DateTime.Now;
			}
		}
			
		/// <summary>
		/// DateTime out of RSSDateConstruct
		/// </summary>
		/// <param name="date">RssDateConstruct</param>
		/// <returns>DateTime</returns>
		public static DateTime DTfromRSS(string date)
		{
			try
			{
				return DateTime.ParseExact(date,"R",DateTimeFormatInfo.InvariantInfo);
			}
			catch(Exception)
			{
				return DateTime.Parse(date);
			}
		}
		
		/// <summary>
		/// AtomDateConstruct out of DateTime
		/// </summary>
		/// <param name="dt">DateTime</param>
		/// <returns>AtomDateConstruct</returns>
		public static string DTtoAtom(DateTime dt)
		{
			return  dt.ToString("yyyy-MM-dd'T'HH:mm:sszzz");		
		}
		
		/// <summary>
		/// RSSDateConstruct out of DateTime
		/// </summary>
		/// <param name="dt">DateTime</param>
		/// <returns>RSSDateConstruct</returns>
		public static string DTtoRSS(DateTime dt)
		{
			return  dt.ToString("R");	
		}
	}
	
	public enum feedTypes {RSS,ATOM};
}
