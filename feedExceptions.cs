﻿
using System;

namespace dotFeedLib
{
	class CanNotOpenLocalRSSFeedException : ApplicationException
	{
	    public CanNotOpenLocalRSSFeedException() : base() {}
	}
	
	/// <summary>
	/// Is thrown whenever the opened file is a valid XML-File but no ATOM or RSS feed 
	/// </summary>
	class FileTypeNotSupportedException : ApplicationException
	{
	    public FileTypeNotSupportedException() : base() {}
	}
	
	/// <summary>
	/// Is thrown whenever doc.Load() fails and this isn't caused by  a web error
	/// </summary>
	class NotValidXmlFileException : ApplicationException
	{
	    public NotValidXmlFileException() : base() {}
	}
	
	/// <summary>
	/// Is thrown when file isn't a valid url or a valid filename
	/// </summary>
	class NotFileNeitherUrlExcepetion : ApplicationException
	{
	    public NotFileNeitherUrlExcepetion() : base() {}
	}
	
	/// <summary>
	/// Is thrown whenever an URL is valid but could not be reached
	/// </summary>
	class UnableToReachUrlExcepetion : ApplicationException
	{
	    public UnableToReachUrlExcepetion() : base() {}
	}
	
	/// <summary>
	/// Is thrown whenever file that declares additional tags is damaged
	/// </summary>
	class AdditionalTagsDeclarationDamagedException : ApplicationException
	{
	    public AdditionalTagsDeclarationDamagedException () : base() {}
	}
	
	/// <summary>
	/// Is thrown whenever the config information isn't valid
	/// </summary>
	class ConfigInformationIncorrectException : ApplicationException
	{
	    public ConfigInformationIncorrectException () : base() {}
	}
	
	/// <summary>
	/// Is thrown when you try to rename or delete a non-excisiting category
	/// </summary>
	class NoSuchCategoryException : ApplicationException
	{
	    public NoSuchCategoryException () : base() {}
	}
}
