using System;
using Gtk;

using System.IO;

namespace byzantio
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
			
		}
	}
}
