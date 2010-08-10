using System;
using System.Collections.Generic;

namespace dotFeedLib
{
	
	public class authorList
	{
		/// <summary>
		/// Coantains alist of authors
		/// </summary>
		List<String> authors;
		
		public authorList()
		{
			authors = new List<String>();
		}
		
		/// <summary>
		/// Adds an author (if not already added) to this list
		/// </summary>
		/// <param name="author">author that is to be added</param>
		public void author_add(string author)
		{
			if(authors.IndexOf(author) == -1 && author != "")
			{
				authors.Add(author);
			}
		}
		/// <summary>
		/// gets a list that coantains all authores in this list
		/// </summary>
		/// <returns>string[] that conatins alla uthors</returns>
		public string[] get_authors()
		{
			return authors.ToArray();
		}
		
	}
}
