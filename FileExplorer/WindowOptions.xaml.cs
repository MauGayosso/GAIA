using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowOptions.xaml
	/// </summary>
	public partial class WindowOptions : Window
	{
		Screen primaryScreen = Screen.PrimaryScreen;

		public WindowOptions()
		{
			InitializeComponent();
		}

		private void btnClientes_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				WindowClientsMenu win = new WindowClientsMenu();
				win.Show();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnAgregar_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int screenW = primaryScreen.Bounds.Width;
				int screenH = primaryScreen.Bounds.Height;
				if (screenW == 1920 & screenH == 1080)
				{
					WindowAdmin win = new WindowAdmin();
					win.Show();
					Close();
				}
				else if (screenW == 1366 & screenH == 768)
				{
					WindowAdmin2 win = new WindowAdmin2();
					win.Show();
					Close();
				}
				else if (screenW == 1360 & screenH == 768)
				{
					WindowAdmin2 win = new WindowAdmin2();
					win.Show();
					Close();
				}
				else if (screenW == 1440 & screenH == 900)
				{
					WindowAdmin2 win = new WindowAdmin2();
					win.Show();
					Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
