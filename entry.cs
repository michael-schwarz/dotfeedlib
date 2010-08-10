using System;
using System.Xml;
using System.Web;

namespace dotFeedLib
{
	/// <summary>
	/// Description of entry.
	/// </summary>
	public class entry
	{
		public string title;
		public string description;
		public string link;
		public string author;
		public string guid;
		public string enclosure_url;
		public string enclosure_length;
		public string enclosure_type;
		public DateTime pubDate;
		public string comments;
		public categoryList category = new categoryList();
		

		
		/// <summary>
		/// Be careful: Is not updated when this entry is changed
		/// </summary>
		public XmlDocument doc;
		
		/// <summary>
		/// Be careful: Is not updated when this entry is changed
		/// </summary>
		public XmlNode node;
		
		public entry(XmlNode item,XmlDocument document,feedTypes inputType)
			{
				if(inputType == feedTypes.RSS)
				{
					node = item;
					doc = document;
					
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
						enclosure_url = node.SelectSingleNode("enclosure").Attributes["url"].Value;
					}
					catch(Exception)
					{
					enclosure_url = "";
					}
					
					try
					{
						enclosure_length = node.SelectSingleNode("enclosure").Attributes["length"].Value;
					}
					catch(Exception)
					{
					enclosure_length = "";
					}
					
					try
					{
						enclosure_type = node.SelectSingleNode("enclosure").Attributes["type"].Value;
					}
					catch(Exception)
					{
					enclosure_type = "";
					}
	
			
					foreach (XmlNode @category_object in item.ChildNodes)
					{
						if(category_object.Name == "category")
						{
							category.addCategory(category_object.InnerText);
						}
	  				}
					
				}
				
				else
				{
					XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
					nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
									
					node = item;
					doc = document;
					

					
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
						int t = node.SelectNodes("atom:link",nsmgr).Count;
						
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
							if(category_object.Attributes["label"].InnerText != "")
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
			if(type == feedTypes.RSS)
			{
				string xml = String.Concat("\r\n<item>\r\n<title>",HttpUtility.HtmlEncode(title),"</title>\r\n<description><![CDATA[",description);
			 	xml = String.Concat(xml,"]]></description>\r\n");
			 	
			 	if(link != "")
			 	{
			 		xml = String.Concat(xml,"<link>",HttpUtility.HtmlEncode(link),"</link>\r\n");
			 	}
			 	
			 	if(author != "")
			 	{
			 		xml = String.Concat(xml,"<author>",HttpUtility.HtmlEncode(author),"</author>\r\n");
			 	}
			 	
			    string comm = "";
			 	if(comments != "" && comments != null)
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
			 	
			 	if(enclosure_url != "")
			 		{
			 		enclosure= String.Concat("<enclosure url=\"",HttpUtility.HtmlEncode(enclosure_url),"\" type=\"",enclosure_type,"\" length=\"",enclosure_length,"\" />\r\n");
			 		}
			 	
			 	if(guid != "")
			 		{
			 		xml = String.Concat(xml,"<guid  isPermaLink=\"false\">",HttpUtility.HtmlEncode(guid),"</guid>\r\n");
			 		}
			 	
			 	
			 	
			 	xml = String.Concat(xml,comm,PubDate,enclosure,"</item>\r\n");
			 	
			 	return xml;
			}
			
			else			
			{
				string xml ="<entry>";
		 	    
		 		if(title != ""  && title != null)
		 			{
		 			xml = String.Concat(xml,"\r\n<title>",HttpUtility.HtmlEncode(title),"</title>");
		 			}
		 		
		 		if(description != "" && description != null)
		 	    	{
		 	    	xml = String.Concat(xml,"\r\n<content type=\"html\"><![CDATA[",description);
			 		xml = String.Concat(xml,"]]></content>\r\n<summary type=\"html\"><![CDATA[",description,"]]></summary>\r\n");
		 	   		}
			 	
			 	if(link != "")
			 		{
			 		xml = String.Concat(xml,"<link rel=\"alternate\" type=\"text/html\" href=\"",HttpUtility.HtmlEncode(link),"\"/>\r\n");
			 		}
			 	
			 	if(author != "")
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
			 	if(enclosure_url != "")
			 		{
			 		enclosure= String.Concat("<link rel=\"enclosure\" href=\"",HttpUtility.HtmlEncode(enclosure_url),"\" type=\"",enclosure_type,"\" length=\"",enclosure_length,"\" title=\"Enclosure\" />\r\n");
			 		}
			 	
			 	xml = String.Concat(xml,enclosure,PubDate,"</entry>\r\n");
				
				return xml;
			}
			
			
			
		}
	
	}
} 
