using System;
using System.Windows;

namespace FileExplorer
{
	/// <summary>
	public partial class WindowMail : Window
	{
		public WindowMail()
		{
			InitializeComponent();
			wb.Source = new Uri("https://www.google.com");
		}
	}
}
