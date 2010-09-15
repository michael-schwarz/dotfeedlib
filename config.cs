using System;
using System.Collections.Generic;

namespace dotFeedLib
{
	public class config
	{		
		List<configValue> configInformation = new List<configValue>();
		
		public config()
		{
			configInformation.Add(new configValue("automatic_guid","true"));
			configInformation.Add(new configValue("new_entry_name","New entry"));
			configInformation.Add(new configValue("use_image_icon","false"));
		}
		
		public string getValue(string name)
		{
			foreach(configValue cfg in configInformation)
			{
				if(cfg.name == name)
				{
					return cfg.value;
				}
			}
			
			throw new ConfigInformationIncorrectException();
		}
		
		public void changeValue(string name,string newValue)
		{
			int id = -1;
			foreach(configValue c in configInformation)
			{
				if(c.name == name)
				{
					id = configInformation.IndexOf(c);
				}
			}
			
			if(id != -1)
			{
				configInformation[id] = new configValue(name,newValue);
			}
			else
			{
				throw new ArgumentException("There is no property with this name");
			}
		}


	}
	
	public struct configValue
	{
		public string name;
		public string value;
		public configValue(string propertyName,string propertyValue)
		{
			name = propertyName;
			value = propertyValue;
		}
	}
}
