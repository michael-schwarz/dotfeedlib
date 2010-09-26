using System;
using System.Collections.Generic;

namespace dotFeedLib
{
	/// <summary>
	/// Class that provides configuration information
	/// </summary>
	public class config
	{		
		List<configValue> configInformation = new List<configValue>();
		
		/// <summary>
		/// Creates a new config with default values
		/// </summary>
		public config()
		{
			configInformation.Add(new configValue("automatic_guid","true"));
			configInformation.Add(new configValue("new_entry_name","New entry"));
			configInformation.Add(new configValue("use_image_icon","false"));
		}
		
		
		/// <summary>
		/// Returns a config value
		/// </summary>
		/// <param name="name">Name of the setting</param>
		/// <returns>Value of the setting</returns>
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
		
		/// <summary>
		/// Changes the value of a config setting
		/// </summary>
		/// <param name="name">name of the config setting</param>
		/// <param name="newValue">new value for this setting</param>
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
	
	/// <summary>
	/// Class that stores a single configuration value
	/// </summary>
	public struct configValue
	{

		/// <summary>
		/// name of the configValue
		/// </summary>
		public string name;
		
		/// <summary>
		/// value of the configValue
		/// </summary>
		public string value;
		
		/// <summary>
		/// Creates new configValue
		/// </summary>
		/// <param name="propertyName">name of the configValue</param>
		/// <param name="propertyValue">value of the configValue</param>
		public configValue(string propertyName,string propertyValue)
		{
			name = propertyName;
			value = propertyValue;
		}
	}
}
