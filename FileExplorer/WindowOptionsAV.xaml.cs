﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.OleDb;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowOptionsAV.xaml
	/// </summary>
	public partial class WindowOptionsAV : Window
	{
		Screen primaryScreen = Screen.PrimaryScreen;
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";
		List<string> categorias = new List<string>();
		public WindowOptionsAV()
		{
			InitializeComponent();
			getCategorias();
		}
		public void getCategorias()
		{
			try
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int screenW = primaryScreen.Bounds.Width;
				int screenH = primaryScreen.Bounds.Height;
				if (comboBox1.Text == null)
				{
					MessageBox.Show("Selecciona una opcion para continuar", "Advertencia", MessageBoxButton.OK);
				}
				else if (categorias.Contains(comboBox1.Text))
				{
					if (screenW == 1920 && screenH == 1080)
					{
						var uwu = comboBox1.Text;
						var owo = comboBox1.Text;
						WindowAyudasVisuales winAV = new WindowAyudasVisuales();
						//winAV.parseDirCorte = "D:/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
						winAV.parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
						winAV.SelectedOption = uwu;
						winAV.getMessages(owo);
						winAV.ParseNewDir();
						winAV.Show();
						Close();
					}
					else if (screenW == 1366 && screenH == 768)
					{
						WindowAyudasVisuales2 winAV = new WindowAyudasVisuales2();
						winAV.parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
						winAV.SelectedOption = comboBox1.Text;
						winAV.ParseNewDir();
						winAV.Show();
						Close();
					}
					else if (screenW == 1360 && screenH == 768)
					{
						WindowAyudasVisuales2 winAV = new WindowAyudasVisuales2();
						winAV.parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
						winAV.SelectedOption = comboBox1.Text;
						winAV.ParseNewDir();
						winAV.Show();
						Close();
					}
					else if (screenW == 1440 && screenH == 900)
					{
						WindowAyudasVisuales3 winAV = new WindowAyudasVisuales3();
						winAV.parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/" + comboBox1.Text + "/";
						winAV.SelectedOption = comboBox1.Text;
						winAV.ParseNewDir();
						winAV.Show();
						Close();
					}
				}
			}
			catch (Exception ex)
			{
			}

		}
		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				WindowLogin win = new WindowLogin();
				win.Show();
				Close();
			}
			catch (Exception ex)
			{
			}

		}
	}
}