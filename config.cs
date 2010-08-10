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
