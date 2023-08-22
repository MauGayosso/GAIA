﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
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
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		List<string> clientes = new List<string>();
		public WindowClientsMenu()
		{
			InitializeComponent();
			getClientes();
			LoadClientes();
		}

		private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		public void getClientes()
		{
			try
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void btnEntrar_Click(object sender, RoutedEventArgs e)
		{
			try
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
						MainWindow win = new MainWindow();
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
						MainWindow2 win = new MainWindow2();
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
						MainWindow2 win = new MainWindow2();
						//win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/" + comboBox1.Text + " /";
						win.parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + comboBox1.Text + " /";
						win.SelectedOption = comboBox1.Text;
						win.ParseNewDir();
						win.LoadImage(comboBox1.Text + ".png");
						win.Show();
						Close();
					}
					else if (screenW == 1440 && screenH == 900)
					{
						MainWindow3 win = new MainWindow3();
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				WindowOptions win = new WindowOptions();
				win.Show();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void btnReload_Click(object sender, RoutedEventArgs e)
		{
			LoadClientes();
		}
		private void LoadClientes()
		{
			try
			{
				comboBox1.Items.Clear();
				string pathClientes = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/";
				string[] directory = Directory.GetDirectories(pathClientes);
				foreach (string directoryEntry in directory)
				{
					string directoryName = new DirectoryInfo(directoryEntry).Name;
					comboBox1.Items.Add(directoryName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
	}
}
