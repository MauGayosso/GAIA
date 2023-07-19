﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Data.OleDb;

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

        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
						Debug.WriteLine("ROL : " + reader.GetValue(0).ToString());
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
                        else if(reader.GetValue(0).ToString() == "lector")
                        {
                            Debug.WriteLine("NOT ADMIN IS : " + reader.GetValue(0).ToString());
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
                System.Windows.MessageBox.Show("Usuario y/o contraseña invalidos");
            }
            
        }

        private void btnVisitante_Click(object sender, RoutedEventArgs e)
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
    }
}
