using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowLogin.xaml
	/// </summary>
	public partial class WindowLogin : Window
	{
		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		Dictionary<string, string> usuarios = new Dictionary<string, string>();

		public WindowLogin()
		{
			InitializeComponent();
			getUsers();
		}

		private void getUsers()
		{
			try
			{
				var query = "SELECT * FROM usuarios";
				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();
					OleDbCommand command = new OleDbCommand(query, connection);
					command.ExecuteNonQuery();
					OleDbDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						usuarios.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
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

		private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string username = txtUsuario.Text;
				string password = txtPassword.Password;
				if (usuarios.ContainsKey(username) && usuarios.ContainsValue(password))
				{

					var query = "SELECT rol_user FROM usuarios WHERE id_usuario = @v1 AND password = @v2";
					using (OleDbConnection connection = new OleDbConnection(connectionString))
					{
						connection.Open();
						OleDbCommand command = new OleDbCommand(query, connection);
						command.Parameters.AddWithValue("@v1", username);
						command.Parameters.AddWithValue("@V2", password);
						command.ExecuteNonQuery();
						OleDbDataReader reader = command.ExecuteReader();
						while (reader.Read())
						{
							if (reader.GetValue(0).ToString() == "admin")
							{
								string nameMachine = System.Environment.MachineName;
								//string logFilePath = "D:/log.txt";
								string logFilePath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/log.txt";
								using (StreamWriter write = File.AppendText(logFilePath))
								{
									write.WriteLine($"[{DateTime.Now}, {nameMachine},{username}]");
								}
								WindowOptions win = new WindowOptions();
								win.Show();
								Close();
							}
							else if (reader.GetValue(0).ToString() == "lector")
							{
								string nameMachine = System.Environment.MachineName;
								//string logFilePath = "D:/log.txt";
								string logFilePath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/log.txt";
								using (StreamWriter write = File.AppendText(logFilePath))
								{
									write.WriteLine($"[{DateTime.Now}, {nameMachine},{username}]");
								}
								OptionsGuess win = new OptionsGuess();
								win.Show();
								Close();
							}
						}
						reader.Close();
						connection.Close();
					}
				}
				else
				{
					MessageBox.Show("Usuario y/o contraseña invalidos");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void btnVisitante_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string nameMachine = System.Environment.MachineName;
				//string logFilePath = "D:/log.txt";
				string logFilePath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/log.txt";
				using (StreamWriter write = File.AppendText(logFilePath))
				{
					write.WriteLine($"[{DateTime.Now}, {nameMachine}]");
				}
				WindowOptionsAV win = new WindowOptionsAV();
				win.Show();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Enter)
				{
					string username = txtUsuario.Text;
					string password = txtPassword.Password;
					if (usuarios.ContainsKey(username) && usuarios.ContainsValue(password))
					{

						var query = "SELECT rol_user FROM usuarios WHERE id_usuario = @v1 AND password = @v2";
						using (OleDbConnection connection = new OleDbConnection(connectionString))
						{
							connection.Open();
							OleDbCommand command = new OleDbCommand(query, connection);
							command.Parameters.AddWithValue("@v1", username);
							command.Parameters.AddWithValue("@V2", password);
							command.ExecuteNonQuery();
							OleDbDataReader reader = command.ExecuteReader();
							while (reader.Read())
							{
								if (reader.GetValue(0).ToString() == "admin")
								{
									string nameMachine = System.Environment.MachineName;
									//string logFilePath = "D:/log.txt";
									string logFilePath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/log.txt";
									using (StreamWriter write = File.AppendText(logFilePath))
									{
										write.WriteLine($"[{DateTime.Now}, {nameMachine},{username}]");
									}
									WindowOptions win = new WindowOptions();
									win.Show();
									Close();
								}
								else if (reader.GetValue(0).ToString() == "lector")
								{
									string nameMachine = System.Environment.MachineName;
									//string logFilePath = "D:/log.txt";
									string logFilePath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/log.txt";
									using (StreamWriter write = File.AppendText(logFilePath))
									{
										write.WriteLine($"[{DateTime.Now}, {nameMachine},{username}]");
									}
									OptionsGuess win = new OptionsGuess();
									win.Show();
									Close();
								}
							}
							reader.Close();
							connection.Close();
						}
					}
					else
					{
						MessageBox.Show("Usuario y/o contraseña invalidos");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
	}
}
