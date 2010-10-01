using System;
using Gtk;
namespace simpleFeedTicker
{
	public partial class settingsWindow : Gtk.Window
	{
		settings s;
		MainWindow main;
		
		
		public settingsWindow (MainWindow mw) : base(Gtk.WindowType.Toplevel)
		{
			main = mw;
			this.Build ();
			this.Present();
			s = new settings();
			this.entry1.Text = s.feedURI;
		}
		

		
		protected virtual void OnButton2Clicked (object sender, System.EventArgs e)
		{
			this.Destroy();
		}
		
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			s.feedURI = this.entry1.Text;
			main.prepareToRead();
			main.changeText("Please wait...");
			this.Destroy();
		}
		
		
		
		
		
	}
}

