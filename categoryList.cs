using System;
using System.Collections.Generic;

namespace dotFeedLib
{

	public class categoryList
	{
	
		// Autorenarray
		List<String> categories;
		
		public categoryList()
		{
			 categories = new List<string>();
		}
		
		public void addCategory(string category)
		{
			if(categories.IndexOf(category) == -1 && category != "")
			{
				categories.Add(category);
			}
		}
		public void addCategory(categoryList list)
		{
			foreach(string str in list.categories)
			{
				addCategory(str);
			}
			
		}
		
		public string[] get_categories()
		{
			return categories.ToArray();
		}
		
		public int Length
		{
			get
        	{
           		 return categories.Count;
        	}

		}
		
		/// <summary>
		/// Renames a category; Please be careful this only changes the name of the catgeory for this entry
		/// After doing this, feeds that belonged to the same category will be in an new one with the old name
		/// If this is not what you would like to happen call dotFeedLib.feed.rename_category(string old,string new_name) instead
		/// </summary>
		/// <param name="old">Current name of the category</param>
		/// <param name="new_text">New name of the category</param>		
		public void rename_category(string old,string newName)
		{
			if(categories.IndexOf(old) != -1)
			{
				categories[categories.IndexOf(old)] = newName;
			}
			else
			{
				throw(new NoSuchCategoryException ());
			}
		}
		
		/// <summary>
		/// Remove this entry from a category
		/// </summary>
		/// <param name="name">Name of the category that this feed should be removed from</param>
		public void removeCategory(string name)
		{
			if(categories.IndexOf(name) != -1)
			{
				categories.RemoveAt(categories.IndexOf(name));
			}
		}
		
		/// <summary>
		/// Checks wheter this category is part of this categoryList
		/// </summary>
		/// <param name="category">category you would like to look for</param>
		/// <returns>Is category part of this list ?</returns>
		public bool contains(string category)
		{
			return categories.Contains(category);			
		}
		
		
		/// <summary>
		/// Removes all categories at once
		/// </summary>
		public void removeAllCategories()
		{
			categories = new List<string>();
		}
		
		/// <summary>
		/// Changes all categories at once
		/// </summary>
		public void changeAllCategories(string[] newCategories)
		{
			categories = new List<string>();
			categories.AddRange(newCategories);
			
		}
		
		
	}
}
