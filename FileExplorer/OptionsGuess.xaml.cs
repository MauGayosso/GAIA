using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para OptionsGuess.xaml
	/// </summary>
	public partial class OptionsGuess : Window
	{
		Screen primaryScreen = Screen.PrimaryScreen;

		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		List<string> clientes = new List<string>();

		public OptionsGuess()
		{
			InitializeComponent();
			getClientes();
			LoadClientes();
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
			else if (comboBox1.Text != null)
			{
				if (screenW == 1920 && screenH == 1080)
				{
					WindowGuess win = new WindowGuess();
					//win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/" + comboBox1.Text + " /";
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
					WindowGuess2 win = new WindowGuess2();
					//win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/" + comboBox1.Text + " /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.ParseNewDir();
					win.LoadImage(comboBox1.Text + ".png");
					win.Show();
					Close();
				}
				else if (screenW == 1360 && screenH == 768)
				{
					WindowGuess2 win = new WindowGuess2();
					//win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/" + comboBox1.Text + " /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.ParseNewDir();
					win.LoadImage(comboBox1.Text + ".png");
					win.Show();
					Close();
				}
			}
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			WindowLogin win = new WindowLogin();
			win.Show();
			Close();
		}

		private void btnReload_Click(object sender, RoutedEventArgs e)
		{
			LoadClientes();
		}
		private void LoadClientes()
		{
			string pathClientes = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/";
			string[] directory = Directory.GetDirectories(pathClientes);
			foreach (string directoryEntry in directory)
			{
				string directoryName = new DirectoryInfo(directoryEntry).Name;
				comboBox1.Items.Add(directoryName);
			}
		}
	}
}
