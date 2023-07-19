using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para OptionsGuess.xaml
	/// </summary>
	public partial class OptionsGuess : Window
	{

		Screen primaryScreen = Screen.PrimaryScreen;

		//
		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		List<string> clientes = new List<string>();

		public OptionsGuess()
		{
			InitializeComponent();
			getClientes();
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
					WindowGuess win = new WindowGuess();
					//win.parseDir = "D:/ING/" + comboBox1.Text+" /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.ParseNewDir();
					win.Show();
					Close();
				}
				else if (screenW == 1366 && screenH == 768)
				{
					WindowGuess2 win = new WindowGuess2();
					//win.parseDir = "D:/ING/" + comboBox1.Text+" /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.ParseNewDir();
					win.Show();
					Close();
				}
				else if (screenW == 1360 && screenH == 768)
				{
					WindowGuess2 win = new WindowGuess2();
					//win.parseDir = "D:/ING/" + comboBox1.Text+" /";
					win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
					win.SelectedOption = comboBox1.Text;
					win.ParseNewDir();
					win.Show();
					Close();
				}

			}
			else
			{
				MessageBox.Show("Cliente no encontrado");
			}
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			WindowLogin win = new WindowLogin();
			win.Show();
			Close();
		}

	}
}
