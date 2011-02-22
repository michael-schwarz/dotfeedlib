using System;
using System.Xml;
using System.Web;

namespace dotFeedLib
{
	/// <summary>
	/// Description of entry.
	/// </summary>
	public class entry :ICloneable
	{
		/// <summary>
		/// Title of the entry
		/// </summary>
		public string title;
		/// <summary>
		/// Description of the entry
		/// </summary>
		public string description;
		/// <summary>
		/// Link to this entry on the website
		/// </summary>
		public string link;
		/// <summary>
		/// Author of this entry
		/// </summary>
		public string author;
		/// <summary>
		/// GUID of th entry
		/// </summary>
		public string guid;
		/// <summary>
		/// URL of enclosure
		/// </summary>
		public string enclosure_url;
		/// <summary>
		/// Filesize (in bytes) of enclosure
		/// </summary>
		public string enclosure_length;
		/// <summary>
		/// MIME-Type of enclosure
		/// </summary>
		public string enclosure_type;
		/// <summary>
		/// Publishment of this entry
		/// </summary>
		public DateTime pubDate;
		/// <summary>
		/// Link to the comments for this entry
		/// </summary>
		public string comments;
		/// <summary>
		/// categoryList that conatins all categories this entry belongs to
		/// </summary>
		public categoryList category = new categoryList();
		
		
		private string additionalXmlInternal = "";
		
		/// <summary>
		/// Additional XML-Tags that should be added to the entries XML
		/// Note: write-only you have to handle this completly by yourself; dotFeedLib does only add this;
		/// apart from this,nothing happens with this information
		/// </summary>
		public string additionalXml
		{
			set
			{
				additionalXmlInternal = value;
			}
		}
		
		/// <summary>
		/// Be careful: not updated
		/// </summary>
		public XmlDocument doc;
		

		/// <summary>
		/// Be careful: not updated
		/// </summary>
		public XmlNode node;
		
		/// <summary>
		/// Creates a new entry out of an XML-Document
		/// </summary>
		/// <param name="item">Node that contains this entry</param>
		/// <param name="document">Document this entry belongs to</param>
		/// <param name="inputType">Indicates whether this is ATOM or RSS</param>
		public entry(XmlNode item,XmlDocument document,feedTypes inputType)
			{
				node = item;
				doc = document;
				if(inputType == feedTypes.RSS || inputType == feedTypes.MRSS)
				{
					XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
					
					if(inputType == feedTypes.MRSS)
					{					
						nsmgr.AddNamespace("media", "http://search.yahoo.com/mrss/");
					}
					
					

					
					try
					{
					title = node.SelectSingleNode("title").InnerText;
					}
					catch(Exception)
					{
					title = "";
					}
					
					try
					{
					description = node.SelectSingleNode("description").InnerText;
					}
					catch(Exception)
					{
					description = "";
					}
	
					try
					{
					link = node.SelectSingleNode("link").InnerText;
					}
					catch(Exception)
					{
					link = "";
					}
					
					try
					{
					author = node.SelectSingleNode("author").InnerText;
					}
					catch(Exception)
					{
					author = "";
					}
					
					try
					{
					guid = node.SelectSingleNode("guid").InnerText;
					}
					catch(Exception)
					{
					guid = "";
					}
					
					try
					{
					pubDate = misc.DTfromRSS(node.SelectSingleNode("pubDate").InnerText);
	
					}
					catch(Exception)
					{
	
					pubDate = DateTime.Now;
					}
					
					try
					{
					comments = node.SelectSingleNode("comments").InnerText;
					}
					catch(Exception)
					{
					comments = "";
					}
					
					try
					{
						if(inputType == feedTypes.RSS)
						{
							enclosure_url = node.SelectSingleNode("enclosure").Attributes["url"].Value;
						}
						else
						{
							enclosure_url = node.SelectSingleNode("media:content",nsmgr).Attributes["url"].Value;
						}
					}
					catch(Exception)
					{
					enclosure_url = "";
					}
					
					try
					{
						if(inputType == feedTypes.RSS)
						{
							enclosure_length = node.SelectSingleNode("enclosure").Attributes["length"].Value;
						}
						else
						{
							enclosure_length = node.SelectSingleNode("media:content",nsmgr).Attributes["fileSize"].Value;
						}
					}
					catch(Exception)
					{
					enclosure_length = "";
					}
					
					try
					{
						if(inputType == feedTypes.RSS)
						{
							enclosure_type = node.SelectSingleNode("enclosure").Attributes["type"].Value;
						}
						else
						{
							enclosure_type = node.SelectSingleNode("media:content",nsmgr).Attributes["type"].Value;
						}
					}
					catch(Exception)
					{
					enclosure_type = "";
					}
	
			
					foreach (XmlNode @category_object in node.ChildNodes)
					{
						if(category_object.Name == "category")
						{
							category.addCategory(category_object.InnerText);
						}
	  				}
					
				}
				
				else
				{
					XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
					nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
									

					

					
					try
					{
					title = node.SelectSingleNode("atom:title",nsmgr).InnerText;
					}
					catch(Exception)
					{
					title = "";
					}
					
					try
					{
						try
						{

							description = node.SelectSingleNode("atom:content",nsmgr).InnerText;
								
						}
						
						catch(Exception)
						{
							
							description = node.SelectSingleNode("atom:summary",nsmgr).InnerText;
						}

					}
					catch(Exception)
					{
						
					}
					
					try
					{
					author = node.SelectSingleNode("atom:author",nsmgr).InnerText;
					}
					catch(Exception)
					{
					author = "";
					}
					
					try
					{
					guid = node.SelectSingleNode("atom:id",nsmgr).InnerText;
					}
					catch(Exception)
					{
					guid = "";
					}
					
					try
					{
					pubDate = misc.DTfromAtom(node.SelectSingleNode("atom:updated",nsmgr).InnerText);
					}
					catch(Exception)
					{
					pubDate = DateTime.Now;
					}
					
					

					comments = "";
					
					enclosure_url = "";
					enclosure_length = "";
					enclosure_type = "";
					link = "";
					
					try
					{ 
						
						foreach (XmlNode n in node.SelectNodes("atom:link",nsmgr))
					         {
					         	try
								{
						         	if(n.Attributes["rel"].Value.ToLower() == "enclosure")
						         		{
						         			enclosure_url = n.Attributes["href"].Value;
						         			enclosure_length = n.Attributes["length"].Value;
						         			enclosure_type = n.Attributes["type"].Value;
						         		}
						         	
						         	else if(n.Attributes["rel"].Value.ToLower() == "alternate")
						         		{
						         			link = n.Attributes["href"].InnerText;
						         		}
					         	}
					         	
					         	catch(Exception)
					         	{
									try
									{
										link = n.Attributes["href"].InnerText;
									}
									catch(Exception)
									{

									}
								}
					         	
					         }
					}
					
					catch(Exception)
					{
						
					}


				
					foreach (XmlNode @category_object in node.SelectNodes("atom:category",nsmgr))
					{
						if(!String.IsNullOrEmpty(category_object.Attributes["label"].InnerText))
								{
									category.addCategory(category_object.Attributes["label"].InnerText);
								}
							
							else
								{
								category.addCategory(category_object.Attributes["term"].InnerText);
								}						
	  				}
				}
		}
		
		/// <summary>
		/// Creates a new, empty entry
		/// </summary>
		public entry()
			{
				title="";
				description="";
				link="";
				author="";
				guid="";
				pubDate= DateTime.Now;
				comments="";
				
				config conf = new config();
				
				if(conf.getValue("automatic_guid") == "true")
				{
					add_new_guid();
				}
			}
		
		/// <summary>
		/// Creates a new GUID and sets it as the current GUiD
		/// </summary>
		public void add_new_guid()
			{
				guid = System.Guid.NewGuid().ToString();
			}
							
				
		 /// <summary>
		 /// Returns XML-Code for this feed
		 /// </summary>
		 /// <param name="type">desired type</param>
		 /// <returns>XML-Code for this feed</returns>
		public string getXML(feedTypes type)
		{
			string additionalTags = additionalXmlInternal;
			if(type == feedTypes.RSS || type == feedTypes.MRSS)
			{
				string xml = String.Concat("\r\n<item>\r\n<title>",HttpUtility.HtmlEncode(title),"</title>\r\n<description><![CDATA[",description);
			 	xml = String.Concat(xml,"]]></description>\r\n");
			 	
			 	if(!String.IsNullOrEmpty(link))
			 	{
			 		xml = String.Concat(xml,"<link>",HttpUtility.HtmlEncode(link),"</link>\r\n");
			 	}
			 	
			 	if(!String.IsNullOrEmpty(author))
			 	{
			 		xml = String.Concat(xml,"<author>",HttpUtility.HtmlEncode(author),"</author>\r\n");
			 	}
			 	
			    string comm = "";
			    if(!String.IsNullOrEmpty(comments))
			 		{
			 		comm = "<comments>";
			 		comm = String.Concat(comm,HttpUtility.HtmlEncode(comments),"</comments>\r\n");
			 		}
	
				string PubDate = "<pubDate>";
				PubDate = String.Concat(PubDate,misc.DTtoRSS(pubDate),"</pubDate>\r\n");
			 		
				foreach(string cat_hand in category.get_categories())
			 		{

			 		xml = String.Concat(xml,"<category>",HttpUtility.HtmlEncode(cat_hand),"</category>\r\n");
			 		}
			 	
			 	string enclosure = "";
			 	
			 	if(!String.IsNullOrEmpty(enclosure_url))
			 		{
			 			if(type == feedTypes.RSS)
			 			{
			 				enclosure= String.Concat("<enclosure url=\"",HttpUtility.HtmlEncode(enclosure_url),"\" type=\"",enclosure_type,"\" length=\"",enclosure_length,"\" />\r\n");
			 			}
			 			else
			 			{
			 				enclosure= String.Concat("<media:content url=\"",HttpUtility.HtmlEncode(enclosure_url),"\" type=\"",enclosure_type,"\" fileSize=\"",enclosure_length,"\" />\r\n");
			 			}
			 		}
			 	
			 	if(!String.IsNullOrEmpty(guid))
			 		{
			 		xml = String.Concat(xml,"<guid  isPermaLink=\"false\">",HttpUtility.HtmlEncode(guid),"</guid>\r\n");
			 		}
			 	
			 	
			 	
			 	xml = String.Concat(xml,comm,PubDate,enclosure,additionalTags,"</item>\r\n");
			 	
			 	return xml;
			}
			
			else			
			{
				string xml ="<entry>";
		 	    
				if(!String.IsNullOrEmpty(title))
		 			{
		 				xml = String.Concat(xml,"\r\n<title>",HttpUtility.HtmlEncode(title),"</title>\r\n");
		 			}
		 		
				if(!String.IsNullOrEmpty(description))
		 	    	{
			 	    	xml = String.Concat(xml,"\r\n<content type=\"html\"><![CDATA[",description);
				 		xml = String.Concat(xml,"]]></content>\r\n<summary type=\"html\"><![CDATA[",description,"]]></summary>\r\n");
		 	   		}
			 	
				if(!String.IsNullOrEmpty(link))
			 		{
			 			xml = String.Concat(xml,"<link rel=\"alternate\" type=\"text/html\" href=\"",HttpUtility.HtmlEncode(link),"\"/>\r\n");
			 		}
			 	
				if(!String.IsNullOrEmpty(author))
			 		{
			 			xml = String.Concat(xml,"<author><name>",HttpUtility.HtmlEncode(author),"</name></author>\r\n");
			 		}
			 	
			 	foreach (string cat_hand in category.get_categories())
			 		{
			 			xml = String.Concat(xml,"<category label=\"",HttpUtility.HtmlEncode(cat_hand),"\" term=\"",HttpUtility.HtmlEncode(cat_hand),"\"/>\r\n");
			 		}
			 	
				string PubDate = "<updated>";
				PubDate = String.Concat(PubDate,misc.DTtoAtom(pubDate),"</updated>\r\n");
			 	
			 	string enclosure = "";
			 	if(!String.IsNullOrEmpty(enclosure_url))
			 		{
			 		enclosure= String.Concat("<link rel=\"enclosure\" href=\"",HttpUtility.HtmlEncode(enclosure_url),"\" type=\"",enclosure_type,"\" length=\"",enclosure_length,"\" title=\"Enclosure\" />\r\n");
			 		}
			 	
			 	xml = String.Concat(xml,enclosure,PubDate,additionalTags,"</entry>\r\n");
				
				return xml;
			}	
			
		}
		
		/// <summary>
		/// Clones the object (Except pubDate and guid)
		/// </summary>
		/// <returns>Clone of the object</returns>
		public object Clone()
		{
			entry n = new entry();
			n.title = title;
			n.description = description;
			n.link = link;
			n.author = author;
			n.enclosure_url = enclosure_url;
			n.enclosure_length = enclosure_length;
			n.enclosure_type = enclosure_type;
			n.comments = comments;
			n.category.addCategory(category);
			
			return n;
		}
	
	}
} 
