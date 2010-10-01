using System;
using Gtk;
using dotFeedLib;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.IO;

namespace simpleFeedTicker
{
	public class settings
	{
		XmlDocument doc = new XmlDocument();
		private string feed;
		private string path;
		
		public string feedURI
		{
			get
			{
				return feed;
			}
			set
			{
				feed = value;
				doc.SelectSingleNode("settings/uri").InnerText = feed;
				doc.Save(path);
			}
		}
		
		public settings ()
		{
			
			Uri u = new Uri(Assembly.GetEntryAssembly().CodeBase);
			path = String.Concat(Path.GetDirectoryName(u.LocalPath),"/settings.xml");
			doc.Load(path);
			feed = doc.SelectSingleNode("settings/uri").InnerText;			
		}
	}
}

