using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.OleDb;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowOptionsAV.xaml
	/// </summary>
	public partial class WindowOptionsAV : Window
	{
		Screen primaryScreen = Screen.PrimaryScreen;

		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		List<string> categorias = new List<string>();
		public WindowOptionsAV()
		{
			InitializeComponent();
			getCategorias();
		}

		private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		public void getCategorias()
		{
			var query = "SELECT * FROM categorias";
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				connection.Open();
				OleDbCommand command = new OleDbCommand(query, connection);
				command.ExecuteNonQuery();

				OleDbDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					comboBox1.Items.Add(reader.GetValue(0));
					categorias.Add(reader.GetValue(0).ToString());
				}
				reader.Close();
				connection.Close();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			int screenW = primaryScreen.Bounds.Width;
			int screenH = primaryScreen.Bounds.Height;
			if (comboBox1.Text == null)
			{
				MessageBox.Show("Selecciona una opcion para continuar", "Advertencia", MessageBoxButton.OK);
			}
			else if (categorias.Contains(comboBox1.Text))
			{
				if(screenW == 1920 && screenH == 1080)
				{
					WindowAyudasVisuales winAV = new WindowAyudasVisuales();
					//winAV.parseDirCorte = "D:/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
					winAV.parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
					winAV.SelectedOption = comboBox1.Text;
					winAV.ParseNewDir();
					winAV.Show();
					Close();
				}
				else if (screenW == 1366 && screenH == 768)
				{
					WindowAyudasVisuales2 winAV = new WindowAyudasVisuales2();
					//winAV.parseDirCorte = "D:/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
					winAV.parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
					winAV.SelectedOption = comboBox1.Text;
					winAV.ParseNewDir();
					winAV.Show();
					Close();
				}
				else if (screenW == 1360 && screenH == 768)
				{
					WindowAyudasVisuales2 winAV = new WindowAyudasVisuales2();
					//winAV.parseDirCorte = "D:/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
					winAV.parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
					winAV.SelectedOption = comboBox1.Text;
					winAV.ParseNewDir();
					winAV.Show();
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
	}
}