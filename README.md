# dotfeedlib
dotFeedLib is an OpenSource .NET-library that can be used to get information from feeds and to create feeds in your own
application. dotFeedLib doesn't use any Windows-specific libraries and therefore runs on GNU/Linux (with Mono) as well as 
on any Windows computer.
dotFeedLib is extremely lightweight (~36 kB) but implements nearly everything from the ATOM- and the RSS-standard.

## Status (2016)
**The last changes were made in 2011. All commits after that stem from moving from CVS to GIT and from SF.net to here.**

**If there are bugs, I'll supply bugfixes but there will be no new features for the reason
given below.** 

**Since .NET v3.5 working with feeds is supported via the System.ServiceModel.Syndication
namespace. I would always recommend using this if you are targeting version 3.5 and above.
If you need to support version 2.0 or are using Mono, dotFeedLib might be for you.**

## Features

You can use dotFeedLib whenever you want to create or edit a feed and whenever you would like to parse a feed 
(e.g. for displaying its content to the user). dotFeedLib used to be part of Easy Feed Editor, a free and OpenSource 
.NET-Feededitor but now Easy Feed Editor is based on dotFeedLib. You could for example use dotFeedLib to create 
another feededitor or to develop a feedreader. But there are also many other possibilities to use dotFeedLib. 

## Example usage

 The first step is to download dotFeedLib and to add the downloaded file to your project. This page will demonstrate how easy it is to use dotFeedLib in your application:

```C#
   1:  ...
   2:   
   3:  public List<string> getAllTitles(string feedUri)
   4:  {
   5:      //The list that will contain all titles later
   6:      List<string> titles = new List<string>();
   7:        
   8:      //There could be a problem during connection, etc.
   9:      try
  10:      {
  11:        //Load the feed
  12:        feed myFeed = new feed(feedUri);
  13:        
  14:        //Go through all entries in that feed
  15:        foreach(entry e in myFeed.entries)
  16:          {
  17:            //Add the title of this entry to the list
  18:            titles.Add(e.title);
  19:          }
  20:      }
  21:      catch(Exception)
  22:      {
  23:      //Do something in here
  24:      }
  25:
  26:      //Return the list
  27:      return titles;
  28:  }
  30:  ....
```

So,let's take a look at the code step by step:

Opening the feed
```C#
  12:  feed myFeed = new feed(feedUri)
```
What happens in this line is that a feed is opened. This can be either an RSS- or an ATOM-feed either on the net or on a local device. For example you could use
```C#
  --:  feed myFeed = new feed("http://example.org/feed.xml")
  --:  feed myFeed2 = new feed("C:\someDirectory\feed.rss");
```
to open a feed via the web and to open another feed from the computer's harddrive.
If you would like to create a new feed, you have to use this syntax:
```C#
  --:  feed myFeed3 = newfeed(true,"en-us");
```
The first parameter can either be true or false because it doesn't have any function. The second parameter is the language code of the feed (e.g. `de-de` for German (Germany) or `en-us` for American English)

Getting the information
```C#
  15:  foreach(entry e in myFeed.entries)
  16:    {
  17:      //Add the title of this entry to the list
  18:      titles.Add(e.title);
  19:    }
```
You can access all entries of this feed by using the feed.entries property. It is an array that contains dotFeedLib.entry objects. These have properties such as title, author or link. In this case the title of every entry is added to titles. You could, for example, use
```C#
  --:  entry firstEntry = myFeed.entries[0];
  --:  string link = firstEntry.link;
  --:   
  --:  string title = myFeed2.entries[1].title;
```
to get the link of the first entry of myFeed and to get the title of the second entry of myFeed2.
To get properties of the whole feed such as title, copyright or description you can use code auch as this:
```C#
  --:  string title = myFeed.title;
  --:  string copyright = myFeed.copyright;
  --:  string description = myFeed.description;
```
Saving the feed

If you want to save the feed to a local volume you can use the `feed.save()` method. The first parameter is the path to which the feed should be saved. The second parameter determines whether the feed should be saved as an ATOM- or as a RSS-Feed.
```C#
  --:  myFeed.save("C:\someDirectory\myFeed.atom",feedTypes.ATOM);
  --:  string xml = myFeed2.getXML(feedTypes.RSS);
```
If you e.g. want to upload the via FTP, you will have to implemt that functionality yourself. To get the XML-code use `feed.getXML(feedTypes.RSS)` if you want to get RSS-code or `feed.getXML(feedTypes.ATOM)` for RSS-code.

##Documentation
A complete documentation (auto-generated from the comments) can be found at http://dotfeedlib.sourceforge.net/doc/index.html .
