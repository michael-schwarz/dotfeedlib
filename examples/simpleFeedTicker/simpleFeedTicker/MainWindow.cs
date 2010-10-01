using System;
using Gtk;
using dotFeedLib;
using System.Collections.Generic;
using simpleFeedTicker;

public partial class MainWindow : Gtk.Window
{
	private feed currentFeed;
	private List<String> titles;
	private List<String> links;
	private int pointer;
	
	private bool gettingFeed = true;
	private bool startedGetting = false;
	private bool canClick = false;
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();

		
		this.label1.SetAlignment((float)0.0,(float)0.5);
		progress_timeout();
		GLib.Timeout.Add(10000, new GLib.TimeoutHandler(progress_timeout));
		GLib.Timeout.Add(300000, new GLib.TimeoutHandler(prepareToRead));
	}
	

	 bool progress_timeout()
	 {
		if(!startedGetting)
			{
				
				if(gettingFeed)
					{
						startedGetting = true;
						canClick = false;
						changeText("Feed is beeing downloaded...");
						try
						{
							read();
					    	gettingFeed = false;
						}
						catch(Exception)
						{
							changeText("An error occurred...");
						}
						startedGetting = false;
					}
			
				 if(!gettingFeed)
					{
						nextEntry();
					}
			
			}
		return true;
	 }
	
	
	 public bool prepareToRead()
	 {
		gettingFeed = true;
		return true;
	 }
	
	private void nextEntry()
	{
		changeText(titles[pointer]);
		canClick = true;
		pointer++;
		if(pointer >= currentFeed.entries.Length)
		{
			pointer = 0;	
		}		
	}
	
	public void changeText(string t)
	{
		this.label1.LabelProp = "<span size='x-large'>"+ t +"</span>";

		
		
	}
	
	private void read()
	{
		pointer = 0;
		
		settings s = new settings();
		currentFeed = new feed(s.feedURI);
		
		titles = new List<string>();
		links = new List<string>();
		
		foreach(entry e in currentFeed.entries)
		{
			titles.Add(e.title);
			links.Add(e.link);
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	protected virtual void OnEventbox1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
	{
		if(canClick)
			{
				System.Diagnostics.Process.Start(links[pointer]);
		}
	}
	
	protected virtual void OnImage1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
	{
		
		
		settingsWindow w = new settingsWindow(this);
		w.Show();
		
		prepareToRead();
	}

	
	
	
	
}
