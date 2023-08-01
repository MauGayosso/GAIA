using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowClientsMenu.xaml
	/// </summary>
	public partial class WindowClientsMenu : Window
	{
		Screen primaryScreen = Screen.PrimaryScreen;
		//
		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		List<string> clientes = new List<string>();
		public WindowClientsMenu()
		{
			InitializeComponent();
			getClientes();
		}

		private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		public void getClientes()
		{
			var query = "SELECT * FROM clientes";
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				connection.Open();
				OleDbCommand command = new OleDbCommand(query, connection);
				command.ExecuteNonQuery();

				OleDbDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					comboBox1.Items.Add(reader.GetValue(0));
					clientes.Add(reader.GetValue(0).ToString());
				}
				reader.Close();
				connection.Close();
			}
		}

		private void btnEntrar_Click(object sender, RoutedEventArgs e)
		{
			int screenW = primaryScreen.Bounds.Width;
			int screenH = primaryScreen.Bounds.Height;
			if (comboBox1.Text == null)
			{
				MessageBox.Show("Selecciona una opcion para continuar", "Advertencia", MessageBoxButton.OK);
			}
			else if (clientes.Contains(comboBox1.Text))
			{
				if (screenW == 1920 && screenH == 1080)
				{
					MainWindow win = new MainWindow();
					//win.parseDir = "D:/ING/" + comboBox1.Text+" /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.SelectedOption2 = comboBox1.Text;
					win.LoadImage(comboBox1.Text + ".png");
					win.ParseNewDir();
					win.Show();
					Close();
				}
				else if (screenW == 1366 && screenH == 768)
				{
					MainWindow2 win = new MainWindow2();
					//win.parseDir = "D:/ING/" + comboBox1.Text+" /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.ParseNewDir();
					win.Show();
					Close();
				}
				else if (screenW == 1360 && screenH == 768)
				{
					MainWindow2 win = new MainWindow2();
					//win.parseDir = "D:/ING/" + comboBox1.Text+" /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.ParseNewDir();
					win.Show();
					Close();
				}
			}
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			WindowOptions win = new WindowOptions();
			win.Show();
			Close();
		}
	}
}
