using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Web;


namespace dotFeedLib
{
	/// <summary>
	/// Class that contains all information on a feed
	/// </summary>
	public class feed
	{		
		XmlDocument doc = new XmlDocument();

		/// <summary>
		/// Is used to save configuration
		/// </summary>
		public config cfg = new config();
				
		//Variables for content		
		
		/// <summary>
		/// only assigend when an RSS-feed is opened, "" if ATOM-Feed
		/// </summary>
		string version;
		
		
		
		/// <summary>
		/// RSS-Version
		/// This is only set if the feed is a RSS-Feed
		/// </summary>
		public string rssVersion
		{
			get
			{
				return version;
			}
		}
		
		/// <summary>
		/// Time when this feed was last changed
		/// </summary>
		public DateTime pubDate = DateTime.Now;
		/// <summary>
		/// Title of the feed
		/// </summary>
		public string title;
		/// <summary>
		/// Link to the website of this feed
		/// </summary>
		public string link;
		/// <summary>
		/// Description of this feed
		/// </summary>
		public string description;
		/// <summary>
		/// Language of this Feed
		/// e.g:
		/// de-de : German (Germany)
		/// en-us : American English
		/// en-en : British English
		/// </summary>
		public string language;
		/// <summary>
		/// The feeds copyright
		/// e. G:
		/// (c) 2008-2010 Michael Schwarz
		/// </summary>
		public string copyright;
		/// <summary>
		/// Name of the tool that was used to create this feed
		/// </summary>
		public string generator;
		
		/// <summary>
		/// Contains a link to the image
		/// This is (by default) used as feed/logo when saving ATOM-Feeds
		/// If you would prefer it to be used as feed/icon you have to set a specific config value in config.cs when compiling
		/// Therefore, you should use an image with the ratio of 2:1
		/// </summary>
		public string imageUrl;
		
		/// <summary>
		/// Not part of ATOM-specification. Please set this to the same value as feed.title
		/// </summary>
		public string imageTitle;
		
		/// <summary>
		/// Not part of ATOM-specification. Please set this to the same value as feed.link
		/// </summary>
		public string imageLink;
		
		/// <summary>
		/// The author of the whole feed (e.g. somebody@example.com (John Somebody)
		/// </summary>
		public string authorOfWholeFeed;
		
		/// <summary>
		/// the type of the feed (RSS or ATOM)
		/// </summary>
		public feedTypes feedType = feedTypes.RSS;
		
		/// <summary>
		/// Array that contains all entries of this feed
		/// </summary>
		public entry[] entries;
		
		/// <summary>
		/// If true, the atom namespace will be added to RSS-Feeds
		/// If you set this to true, you can use atom:link="rel" in entries
		/// </summary>
		public bool useAtomAttributesInRss = false;
		
		/// <summary>
		/// The path that can be used to rech this file
		/// Is only used if feed is ATOM or if useAtomAttributesInRss is true
		/// </summary>
		public string linkToFeedItself = "";
		

		private string additionalXmlnsInternal = "";
		
		/// <summary>
		/// Additional XMLNS-Declerations that should be added to the rss(RSS) or feed(ATOM) tag
		/// Note: write-only you have to handle this completly by yourself; dotFeedLib does only add this;
		/// apart from this,nothing happens with this information
		/// </summary>
		public string additionalXmlns
		{
			set
			{
				additionalXmlnsInternal = value;
			}
		}
		
		/// <summary>
		/// If file has been saved as ATOM-Feed / RSS-Feed execute this
		/// </summary>
		/// <param name="f">new feedType</param>
		public void changeFormat(feedTypes f)
		{
			feedType = f;
		}
		
				
		/// <summary>
		/// Search and replace in authors
		/// </summary>
		/// <param name="old">search for</param>
		/// <param name="new_text">replace with</param>
		public void search_replace_author(string old,string new_text)
			{
			foreach(entry element in entries)
				{
					element.author = element.author.Replace(old,new_text);
				}
			}
		
		/// <summary>
		/// Search and replace in titles
		/// </summary>
		/// <param name="old">search for</param>
		/// <param name="new_text">replace with</param>
		public void search_replace_title(string old,string new_text)
			{
			foreach(entry element in entries)
				{
					element.title = element.title.Replace(old,new_text);
				}
			}
		
		/// <summary>
		/// Rename a category
		/// </summary>
		/// <param name="old">old name</param>
		/// <param name="new_name">new name</param>
		public void rename_category(string old,string new_name)
			{
				foreach(entry element in entries)
				{
					element.category.rename_category(old,new_name);
				}
			
			}
		
		/// <summary>
		/// Remove a category
		/// </summary>
		/// <param name="name">Name of the category</param>
		public void remove_category(string name)
			{	
				foreach(entry element in entries)
				{
					element.category.removeCategory(name);
				}
			
			}
		
		/// <summary>
		/// Get an array that contains all authors that are used in this feed
		/// </summary>
		/// <returns>authorList that conatains all authors</returns>
		public authorList get_authors()
		{			
			authorList authors = new authorList();
			foreach(entry item in entries)
			{
				if(!String.IsNullOrEmpty(item.author))
				{
					authors.author_add(item.author);
				}
			}
			return authors;
		}
		

		
		/// <summary>
		/// Get an array that contains all categories that are udes in this feed
		/// </summary>
		/// <returns>categoryList that conatains all categories</returns>
		public categoryList get_categories()
		{
			categoryList categories = new categoryList();
			
			foreach(entry item in entries)
			{
				categories.addCategory(item.category);
			}
			
			return categories;
		}
		

		
		/// <summary>
		/// Add a new entry
		/// </summary>
		/// <returns>id of the new entry</returns>
		public int new_entry()
			{
			entry[] entries_new;
			int i =0;
			
			int new_length = entries.Length +1;
			
			entries_new = new entry[new_length];
			foreach (entry objekt in entries)
				{
				entries_new[i+1] = objekt;
				i= i+1;
				}
			
			entries_new[0] = new entry();
			
			config conf = new config();

			
			
			entries_new[0].title = conf.getValue("new_entry_name");
			entries = entries_new;
			return 0;
			
			}
		
		/// <summary>
		/// Adds a new entry (part of the merge feeds functiionality / the open functionality)
		/// </summary>
		/// <param name="entry_item">XML-Node of the current-Item</param>
		/// <param name="entry_doc">XML-Document that contains current-Item</param>
		/// <param name="inputFormat">the input format</param>
		public void entry_add(XmlNode entry_item,XmlDocument entry_doc,feedTypes inputFormat)
			{
				entry[] entries_new;
				int i =0;
				
				int new_length = entries.Length +1;
				
				entries_new = new entry[new_length];
				
				foreach (entry single_entry in entries)
				{
				entries_new[i] = single_entry;
				i= i+1;
				}
				

				entries_new[i] = new entry(@entry_item,entry_doc,inputFormat);
				
				entries = entries_new;
		
			}
		                    
		/// <summary>
		/// Add an entry
		/// </summary>
		/// <param name="n">The entry that will be added (make sure you it's cloned if you want to copy an entry)</param>
		public void entry_add_beginning(entry n)
			{
				entry[] entries_new;
				int i =0;
				
				int new_length = entries.Length +1;
				
				entries_new = new entry[new_length];
				
				foreach (entry single_entry in entries)
				{
					entries_new[i+1] = single_entry;
					i= i+1;
				}
				
				entries_new[0] = n;
				
				entries = entries_new;
			}
	
		
	
		
		/// <summary>
		/// Returns the feeds XML
		/// </summary>
		/// <param name="type">feedType you would like to use</param>
		/// <returns>XML-Code</returns>
		public string getXML(feedTypes type)
		{			
			if(type == feedTypes.RSS || type == feedTypes.MRSS)
			   {
			   		
				
					 string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<rss version=\"2.0\"";
			 
					 //Start: declaration of additional namespaces
					 if(type == feedTypes.MRSS)
					 {
						xml = String.Concat(xml," xmlns:media=\"http://search.yahoo.com/mrss/\"");
					 }
					 
					 if(useAtomAttributesInRss)
					 {
					 	xml = String.Concat(xml," xmlns:atom=\"http://www.w3.org/2005/Atom\"");					 	
					 }
					 if(!String.IsNullOrEmpty(additionalXmlnsInternal))
					 {
					 	xml = String.Concat(xml," ",additionalXmlnsInternal);
					 }
			 
					 //End: decleration of additional namespaces
					 
					 xml = String.Concat(xml,">\r\n<channel>\r\n");
					 
					 if(!String.IsNullOrEmpty(this.generator))
					 {
					 	xml = String.Concat(xml,"<generator>",this.generator,"</generator>\r\n");
					 }
					
					 xml = String.Concat(xml,"<title>",HttpUtility.HtmlEncode(this.title),"</title>\r\n<description>",HttpUtility.HtmlEncode((this.description)),"</description>\r\n");
				
					 
					 if(!String.IsNullOrEmpty(this.link))
					 {
					 	xml = String.Concat(xml,"<link>",HttpUtility.HtmlEncode(this.link),"</link>\r\n");
					 }
					 	
					 
					 if(!String.IsNullOrEmpty(this.copyright))
					 {
					 	xml = String.Concat(xml,"<copyright>",HttpUtility.HtmlEncode(this.copyright),"</copyright>\r\n");
					 }
					 
					 if(!String.IsNullOrEmpty(this.imageUrl))
					 {
					 	xml = String.Concat(xml,"<image><url>",this.imageUrl,"</url>");
					 	
					 	if(!String.IsNullOrEmpty(this.imageTitle))
					 	{
					 		xml = String.Concat(xml,"<title>",this.imageTitle,"</title>");
					 	}
					 	else
					 	{
					 		xml = String.Concat(xml,"<title></title>");
					 	}
					 	
					 	if(!String.IsNullOrEmpty(this.imageLink))
					 	{
					 		xml = String.Concat(xml,"<link>",this.imageLink,"</link></image>\r\n");
					 	}
					 	else
					 	{
					 		xml = String.Concat(xml,"<link></link></image>\r\n");
					 	}
					 }
					 
					 xml = String.Concat(xml,"<language>",this.language,"</language>\r\n<pubDate>");
					 
								 
					
					 xml = String.Concat(xml,misc.DTtoRSS(this.pubDate),"</pubDate>\r\n");
					 
					 if(useAtomAttributesInRss && !String.IsNullOrEmpty(linkToFeedItself))
					 {
					 	xml = String.Concat(xml,"<atom:link href=\"",linkToFeedItself,"\" rel=\"self\" type=\"application/rss+xml\" />\r\n");
					 }
					 
					 if(!String.IsNullOrEmpty(authorOfWholeFeed))
					 {
					 	xml = String.Concat(xml,"<managingEditor>",HttpUtility.HtmlEncode(this.authorOfWholeFeed),"</managingEditor>\r\n");
					 }
					 
					 foreach(entry element in entries)
					 	{
					 	xml = String.Concat(xml,element.getXML(type));
					 	}
					 
					 xml = String.Concat(xml,"</channel>\r\n</rss>");
					
					 
					 return xml;
			   	
			   }
			
			else
				{
					 string xml = String.Concat("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<feed xmlns=\"http://www.w3.org/2005/Atom\"");
				     
					 if(!String.IsNullOrEmpty(additionalXmlnsInternal))
					 {
					 	xml = String.Concat(xml," ",additionalXmlnsInternal);
					 }
					 
					 if(!String.IsNullOrEmpty(language))
					 {
					 	xml = String.Concat(xml," xml:lang=\"",language,"\"");
					 }
					 	
					 xml = String.Concat(xml,">\r\n");
		
		
					 xml = String.Concat(xml,"<title>");
					 xml = String.Concat(xml,HttpUtility.HtmlEncode(this.title),"</title>\r\n<subtitle>",HttpUtility.HtmlEncode(this.description),"</subtitle>\r\n");
					 
					 if(!String.IsNullOrEmpty(this.link))
					 	{
					 	xml = String.Concat(xml,"<link href=\"",HttpUtility.HtmlEncode(this.link),"\"/>\r\n");
					 	}
					 
				
					 
					 if(!String.IsNullOrEmpty(this.copyright))
					 	{
					  	xml = String.Concat(xml,"<rights>",HttpUtility.HtmlEncode(this.copyright),"</rights>\r\n");
					  	}
					
					 
					 xml = String.Concat(xml,"<generator uri=\"http://easy-feed-editor.tk\" version=\"1.0\">dotFeedLib</generator>\r\n");
					 
					 if(!String.IsNullOrEmpty(this.imageUrl))
					 {

						
						if(cfg.getValue("use_image_icon") != "true")
							{
								xml = String.Concat(xml,"<logo>",this.imageUrl,"</logo>");
							}
						else
							{
								xml = String.Concat(xml,"<icon>",this.imageUrl,"</icon>");
							}
					 }
					 
					   if(!String.IsNullOrEmpty(authorOfWholeFeed))
					 {
					 	xml = String.Concat(xml,"<author><name>",HttpUtility.HtmlEncode(this.authorOfWholeFeed),"</name></author>\r\n");
					 }
					 //TODO: schaun ob pubdate überhaupt bei ATOM eingelesen wird
					 xml = String.Concat(xml,"<updated>",misc.DTtoAtom(this.pubDate),"</updated>\r\n");
					 
					 if(!String.IsNullOrEmpty(linkToFeedItself))
					 {
					 	xml = String.Concat(xml,"<link href=\"",linkToFeedItself,"\" rel=\"self\" type=\"application/atom+xml\" />\r\n");
					 }
					 
					 foreach(entry element in entries)
					 	{						 	
					 		xml = String.Concat(xml,element.getXML(type));	 	
					 	}
					 
					 xml = String.Concat(xml,"</feed>");
		
					 return xml;
				}
			
		}
		/// <summary>
		/// Deletes an entry
		/// </summary>
		/// <param name="number">the position of the entry that is to be deleted</param>
		public void deleteEntry(int number)
		{
			int new_length = entries.Length-1;
			
			
			entry[] entries_old = entries;
			entry[] entries_new;
			entries_new = new entry[new_length];
			for(int i=0;i <number;i = i+1)
				{
				entries_new[i] = entries_old[i];
				}
			
			for(int akt = number;akt<new_length;akt = akt+1)
				{
				entries_new[akt] = entries_old[akt+1];
				}
			entries = entries_new;
	
		}
		
		/// <summary>
		/// Gets all entries that belong to a certain category
		/// </summary>
		/// <param name="category">category that items should belong to</param>
		/// <returns>all entries that are part of this category</returns>
		public List<entry> getEntriesInCategory(string category)
		{
			List<entry> selectedEntries = new List<entry>();
			foreach(entry item in entries)
			{
				if(item.category.contains(category))
				{
					selectedEntries.Add(item);
				}
			}
			
			return selectedEntries;
		}

		/// <summary>
		/// Adds GUIDS to all elements that don't have any at the moment
		/// </summary>
		public void add_guids()
		{
		
			foreach(entry element in entries)
			{
				if(String.IsNullOrEmpty(element.guid))
				{
					element.add_new_guid();

				}				
			}
			
		}
		/// <summary>
		/// If you have a GUID and want to get its position in entries[]
		/// </summary>
		/// <param name="guid">The GUID you're looking for</param>
		/// <returns>position in entries[] or -1 if there's no entry with this GUID</returns>
		public int getPositionOfEntryWithCertainGUID(string guid)
		{
			if(String.IsNullOrEmpty(guid))
			{
				return -1;
			}
			
			else
			{
				List<string> guids = new List<string>();
	
				foreach(entry ein in entries)
					{
					if(!String.IsNullOrEmpty(ein.guid))
							{
							guids.Add(ein.guid);
							}
					}
				
				 return guids.IndexOf(guid);
			}
				
		}
		
		/// <summary>
		/// Creates a new, empty feed 
		/// </summary>
		/// <param name="newFeed">ignored</param>
		/// <param name="languageCode">languageCode for the new feed e.g. en-us or de-de</param>
		public feed(bool newFeed,string languageCode)
		{
			
			

			
			// Creates new,empty feed
			version = "";
			title = "";
			link = "";
			description = "";
	
			
			language = languageCode;
			copyright = "";
	
			generator = "";
			authorOfWholeFeed = "";
			useAtomAttributesInRss = false;
			linkToFeedItself = "";
			
			entries= new entry[0];
			

		}
		
		private static bool namespaceIsDefinedRSS(XmlDocument doc,string ns)
		{
			return (!String.IsNullOrEmpty(doc.SelectSingleNode("rss").GetNamespaceOfPrefix(ns)));
		}

		/// <summary>
		/// Please do not use. Only for internal use
		/// </summary>
		/// <param name="doc">XMlDocument</param>
		protected void readRestRSS(XmlDocument doc)
			{

				if(namespaceIsDefinedRSS(doc,"media"))
				{
					if(doc.SelectNodes("rss/channel/item/enclosure").Count < 1)
					{
						this.feedType = feedTypes.MRSS;
					}				
				}
		
				if(namespaceIsDefinedRSS(doc,"atom"))
				{
					useAtomAttributesInRss = true;
					
					XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
					nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
					
					foreach(XmlNode n in doc.SelectNodes("rss/channel/atom:link",nsmgr))
					{
						try
						{
							if(n.Attributes["rel"].Value == "self")
							{
								linkToFeedItself = n.Attributes["href"].Value;
							}
						}
						catch(Exception)
						{
							
						}
					}
					
				}
			
			
				try
				{
					version = doc.SelectSingleNode("rss").Attributes["version"].InnerText;
				}
		
				catch(Exception)
				{
					version = "";
				}
			
			
				try
				{
					title = doc.SelectSingleNode("rss/channel/title").InnerText;
				}
				catch(Exception)
				{
					title = "";
				}
				try
				{
					link = doc.SelectSingleNode("rss/channel/link").InnerText;
				}
				catch(Exception)
				{
					link = "";
				}
				try
				{
					description = doc.SelectSingleNode("rss/channel/description").InnerText;
				}
				catch(Exception)
				{
					description = "";
				}					
				
				try
				{
					language = doc.SelectSingleNode("rss/channel/language").InnerText;
				}
				catch(Exception)
				{
					language = "";
				}
				
				try
				{
					copyright = doc.SelectSingleNode("rss/channel/copyright").InnerText;
				}
				catch(Exception)
				{
					copyright = "";
				}
				
				try
				{
					authorOfWholeFeed = doc.SelectSingleNode("rss/channel/managingEditor").InnerText;
				}
				catch(Exception)
				{
					authorOfWholeFeed = "";
				}
				
				try
				{
					pubDate = misc.DTfromRSS(doc.SelectSingleNode("rss/channel/pubDate").InnerText);
				}
				catch(Exception)
				{
					
				}
				
				try
				{
					generator = doc.SelectSingleNode("rss/channel/generator").InnerText;
				}
				catch(Exception)
				{
					generator = "";
				}
				
				try
				{
					imageUrl = doc.SelectSingleNode("rss/channel/image/url").InnerText;
					
					try
					{
						imageLink = doc.SelectSingleNode("rss/channel/image/link").InnerText;
					}
					catch(Exception)
					{
						imageLink = "";
					}
					
					try
					{
						imageTitle = doc.SelectSingleNode("rss/channel/image/title").InnerText;
					}
					catch(Exception)
					{
						imageTitle = "";
					}
				}
				catch(Exception)
				{
					imageUrl = "";
					imageLink = "";
					imageTitle = "";
				}
				
				
				
				XmlNodeList list = doc.SelectNodes("rss/channel/item");	
				entries = new entry[list.Count];
				
				int count=0;
				foreach (XmlNode @item in list)
				{
					if(item.Name == "item")
					{
					entries[count] = new entry(@item,doc,feedType);
					count++;
					}
				}
			}
		
		
		private static bool is_rss(XmlDocument doc)
		{
			try
			{
				string r = doc.SelectSingleNode("rss/channel/title").InnerXml;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
					
		}
		
		private static bool is_atom(XmlDocument doc)
		{
			try
				{
					XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
					nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
					doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:title", nsmgr);
					
					
					return true;
				}
			catch(Exception)
				{
					
					return false;
				}
			
		}
		
		/// <summary>
		/// Opens an excisting feed either via the Web or from an excisitng file
		/// </summary>
		/// <param name="path">absolute path or valid URI</param>
		public feed(string path)
		{
	
			// Wenn Internetfile, dann spezielle Aktionen
			if(misc.IsUrl(path))
				{				
					try
					{
						doc.Load(path);
						
						
						if(is_rss(doc) == true)
							{			
								
								readRestRSS(doc);
							}
						
						 else if(is_atom(doc) == true)
							{
						 		feedType = feedTypes.ATOM;
						 		feedReadAtom();
							}
						else
							{							
							throw new FileTypeNotSupportedException();
							}					

					}
					
					catch(FileTypeNotSupportedException)
					{
						throw new FileTypeNotSupportedException();
					}
					
					catch(System.Net.WebException)
					{

						throw new UnableToReachUrlExcepetion();
					}
				
					catch(Exception)
					{						
						throw new NotValidXmlFileException();
					}
			
				}
			
			else if(File.Exists(path))
			{
					
					try
					{
						
						doc.Load(path);
						
						
						if(is_rss(doc) == true)
							{
								
								readRestRSS(doc);
							}
						
						 else if(is_atom(doc) == true)
							{
						 		feedType = feedTypes.ATOM;
								feedReadAtom();
							}
						else
							{
							
							throw new FileTypeNotSupportedException();	
							
							}					

					}
					
					
					catch(FileTypeNotSupportedException)
					{
						throw new FileTypeNotSupportedException();
					}
					
					catch(AdditionalTagsDeclarationDamagedException)
					{
						throw new AdditionalTagsDeclarationDamagedException();
					}
					
					catch(Exception)
					{		
						throw new NotValidXmlFileException();
					}
			}
			
			else
			{
					throw new NotFileNeitherUrlExcepetion();
				

			}
			
			
			
		}
		
		

		

	
		/// <summary>
		///  Open ATOM file
		/// </summary>
		private void feedReadAtom()
		{
			
	


			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");


			
			try
			{
				title =  doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:title", nsmgr).InnerText;
			}
			catch(Exception)
			{
				title = "";
			}
			
			try
			{
				pubDate = misc.DTfromAtom(doc.SelectSingleNode("atom:updated").InnerText);
			}
			catch(Exception)
			{
				
			}
			
			foreach(XmlNode n in doc.SelectNodes("atom:feed/atom:link",nsmgr))
			{
				try
				{
					if(n.Attributes["rel"].Value == "self")
					{
						linkToFeedItself = n.Attributes["href"].Value;
					}
				}
				catch(Exception)
				{
					
				}
			}
			
			try
			{
			
				link = doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:link", nsmgr).Attributes["href"].InnerText;
			}
			catch(Exception)
			{
				link = "";
			}
			try
			{
				description = doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:subtitle", nsmgr).InnerText;
			}
			catch(Exception)
			{
				description = "";
			}					
			
			try
			{
				language = doc.SelectSingleNode("atom:feed", nsmgr).Attributes["xml:lang"].Value;
			}
			catch(Exception)
			{
				language = "";
			}	

			
			try
			{
				copyright = doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:rights", nsmgr).InnerText;
			}
			catch(Exception)
			{
				copyright = "";
			}


			
			
			try
			{
				generator = doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:generator", nsmgr).InnerText;
			}
			catch(Exception)
			{
				generator = "";
			}
			
			try
			{
				authorOfWholeFeed = doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:author",nsmgr).InnerText;
			}
			catch(Exception)
			{
				authorOfWholeFeed = "";
			}
			
			entries= new entry[doc.SelectSingleNode("atom:feed", nsmgr).SelectNodes("atom:entry", nsmgr).Count];
			
			
			int count = 0;
		
			foreach (XmlNode @item in doc.SelectSingleNode("atom:feed", nsmgr).SelectNodes("atom:entry", nsmgr))
				{
					entries[count] = new entry(@item,doc,feedTypes.ATOM);
					count++;
				}
			
			
			try
			{

				
				if(cfg.getValue("use_image_icon") != "true")
					{
						imageUrl = doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:logo", nsmgr).InnerText;
					}
				else
					{
						imageUrl = doc.SelectSingleNode("atom:feed", nsmgr).SelectSingleNode("atom:icon", nsmgr).InnerText;
					}
				
				imageLink = "";
				imageTitle = "";

			}
			catch(Exception)
			{
				imageUrl = "";
				imageLink = "";
				imageTitle = "";
			}
				
			
		}
		
		/// <summary>
		/// Saves the feed
		/// </summary>
		/// <param name="path">Full path of File</param>
		/// <param name="saveFormat">Fomrat in which Feed should be saved</param>
		public void save(string path,feedTypes saveFormat)
		{
			File.WriteAllText(path,getXML(saveFormat));
			changeFormat(saveFormat);
			
		}

	}
}