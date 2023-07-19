using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Wpf.Ui.Common;
using MessageBox = System.Windows.MessageBox;


namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowAdmin.xaml
	/// </summary>
	public partial class WindowAdmin2 : Window
	{
		public ObservableCollection<ItemData> Items { get; set; }
		public ObservableCollection<ItemDataUsers> ItemsUsers { get; set; }
		public ObservableCollection<ItemDataTip> ItemTips { get; set; }

		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		Dictionary<string, string> clients = new Dictionary<string, string>();
		Dictionary<string, string> usuarios = new Dictionary<string, string>();
		Dictionary<string, string> tips = new Dictionary<string, string>();
		List<Tuple<string, string, string>> usuariosL = new List<Tuple<string, string, string>>();

		public Tuple<string, string, string> tupleUsers { get; set; }
		public WindowAdmin2()
		{
			InitializeComponent();
			Items = new ObservableCollection<ItemData>();
			ItemsUsers = new ObservableCollection<ItemDataUsers>();
			ItemTips = new ObservableCollection<ItemDataTip>();
			addItemsClientes();
			addItemUsers();
			addItemsTips();
			// Load Grid with info
			addToGridUsers();
			addToGridClient();
			addToGridTips();
		}

		private void LogOut(object sender, RoutedEventArgs e)
		{
			WindowLogin win = new WindowLogin();
			win.Show();
			Close();
		}
		private void addToGridTips()
		{
			foreach (var tip in tips)
			{
				ItemTips.Add(new ItemDataTip { Message = tip.Key, Category = tip.Value, EditCommand = new RelayCommand(EditItemTip), DeleteCommand = new RelayCommand(DeleteItemTip) });
			}
			dynamicTableTips.ItemsSource = ItemTips;
		}
		private void addToGridUsers()
		{
			foreach (var user in usuariosL)
			{
				ItemsUsers.Add(new ItemDataUsers { Name = user.Item1, Password = user.Item2, Rol = user.Item3, EditCommand = new RelayCommand(EditItemUser), DeleteCommand = new RelayCommand(DeleteItemUsers) }); ;
			}

			dynamicTableUsers.ItemsSource = ItemsUsers;
		}
		private void addToGridClient()
		{
			foreach (var clien in clients)
			{
				Items.Add(new ItemData { Name = clien.Key, PathClient = clien.Value, EditCommand = new RelayCommand(EditItem), DeleteCommand = new RelayCommand(DeleteItem) });
			}
			dynamicTable.ItemsSource = Items;
		}
		private void addItemsClientes()
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
					clients.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
				}
				reader.Close();
				connection.Close();
			}
		}

		private void addItemUsers()
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
					//usuariosL.Add(reader.GetValue(0).ToString());
					//usuariosL.Add(reader.GetValue(1).ToString());
					//usuariosL.Add(reader.GetValue(2).ToString());
					//tupleUsers = Tuple.Create(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
					tupleUsers = Tuple.Create(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
					usuariosL.Add(tupleUsers);
					//usuarios.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
				}
				reader.Close();
				connection.Close();
			}
		}

		private void addItemsTips()
		{
			var query = "SELECT * FROM messages";
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				connection.Open();
				OleDbCommand command = new OleDbCommand(query, connection);
				command.ExecuteNonQuery();

				OleDbDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					tips.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
				}
				reader.Close();
				connection.Close();
			}
		}

		private void UpdateCiente(string id)
		{
		}
		private void EditItem(object item)
		{
			// Handle edit command here
			ItemData selectedItem = (ItemData)item;
			//MessageBox.Show("Edit " + selectedItem.Name + "+ " + selectedItem.PathClient);
			txtEditCliente.Text = selectedItem.Name;
			txtEditPathClient.Text = selectedItem.PathClient;
			if (editClientCanva.Visibility == Visibility.Hidden)
			{
				editClientCanva.Visibility = Visibility.Visible;
			}
		}

		// EDIT BUTTON TIP 
		private void EditItemTip()
		{

		}

		//EDIT BUTTON USER

		private void EditItemUser(object item)
		{
			ItemDataUsers selectedItem = (ItemDataUsers)item;
			txtEditUser.Text = selectedItem.Name;
			txtPassword.Text = selectedItem.Password;
			cmbEditRol.Text = selectedItem.Rol;
			if (editUsersCanva.Visibility == Visibility.Hidden)
			{
				editUsersCanva.Visibility = Visibility.Visible;
			}
		}

		// DELETE BUTTON TO CLIENTS
		private void DeleteItem(object item)
		{
			// Handle delete command here
			ItemData selectedItem = (ItemData)item;
			MessageBoxResult result = MessageBox.Show("¿Estas seguro de eliminar el cliente : " + selectedItem.Name + "?", "Eliminar", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
			if (result == MessageBoxResult.OK)
			{
				string query = "DELETE FROM clientes WHERE id_usuario = ?";

				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					using (OleDbCommand command = new OleDbCommand(query, connection))
					{
						command.Parameters.AddWithValue("@id_usuario", selectedItem.Name);
						int rows = command.ExecuteNonQuery();
					}
				}
				clients.Clear();
				Items.Clear();
				dynamicTable.ItemsSource = null;
				addItemsClientes();
				addToGridClient();
			}
		}


		// DELETE BUTTON TO USERS
		private void DeleteItemUsers(object itemUsers)
		{
			// Handle delete command here
			ItemDataUsers selectedItem = (ItemDataUsers)itemUsers;
			MessageBoxResult result = MessageBox.Show("¿Estas seguro de eliminar el cliente : " + selectedItem.Name + "?", "Eliminar", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
			if (result == MessageBoxResult.OK)
			{
				string query = "DELETE FROM usuarios WHERE id_usuario = ?";

				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					using (OleDbCommand command = new OleDbCommand(query, connection))
					{
						command.Parameters.AddWithValue("@id_usuario", selectedItem.Name);
						int rows = command.ExecuteNonQuery();
					}
				}
				usuarios.Clear();
				ItemsUsers.Clear();
				dynamicTableUsers.ItemsSource = null;
				addItemUsers();
				addToGridUsers();
			}
		}

		// DELETE BUTTON TO TIPS

		private void DeleteItemTip()
		{

		}

		private void HamburgerButton_Click(object sender, RoutedEventArgs e)
		{
			if (MenuItemsPanel.Visibility == Visibility.Visible)
			{
				MenuItemsPanel.Visibility = Visibility.Hidden;
			}
			else if (MenuItemsPanel.Visibility == Visibility.Hidden)
			{
				MenuItemsPanel.Visibility = Visibility.Visible;
			}

		}
		private void MenuClick(object sender, RoutedEventArgs e)
		{
			WindowOptions win = new WindowOptions();
			win.Show();
			Close();
		}

		private void ExitClick(object sender, RoutedEventArgs e)

		{
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		//BUTTON RELOAD CLIENTS PAGE
		private void btnReload_Click(object sender, RoutedEventArgs e)
		{
			clients.Clear();
			Items.Clear();
			dynamicTable.ItemsSource = null;
			addItemsClientes();
			addToGridClient();
		}

		//BUTTON RELOAD USERS PAGE
		private void btnReloadUsers_Click_1(object sender, RoutedEventArgs e)
		{
			usuarios.Clear();
			ItemsUsers.Clear();
			usuariosL.Clear();
			dynamicTableUsers.ItemsSource = null;
			addItemUsers();
			addToGridUsers();
		}

		private void btnSelectFolderCliente_Click(object sender, RoutedEventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				DialogResult result = folderBrowserDialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					// Get the selected folder path from the dialog
					string folderPath = folderBrowserDialog.SelectedPath;
					txtPath.Text = folderPath;
				}

			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (txtPath.Text == null)
			{
				MessageBox.Show("Selecciona una carpeta", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else if (txtNameInsertClient.Text == null)
			{
				MessageBox.Show("Nombre faltante", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else
			{
				var query = "insert into clientes (id_usuario,path_cliente) values (@id_usuario,@path_cliente)";

				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					using (OleDbCommand command = new OleDbCommand(query, connection))
					{
						command.Parameters.AddWithValue("@id_usuario", txtNameInsertClient.Text);
						command.Parameters.AddWithValue("@path_cliente", txtPath.Text);
						command.ExecuteNonQuery();
					}
				}
				clients.Clear();
				Items.Clear();
				dynamicTable.ItemsSource = null;
				addItemsClientes();
				addToGridClient();
			}

		}

		private void btnAddClient_Click(object sender, RoutedEventArgs e)
		{
			if (canvaCliente.Visibility == Visibility.Hidden)
			{
				btnAddClient.BorderBrush = Brushes.Red;
				btnAddClient.BorderThickness = new Thickness(2);
				canvaCliente.Visibility = Visibility.Visible;
			}
			else
			{
				btnAddClient.BorderBrush = Brushes.Green;
				btnAddClient.BorderThickness = new Thickness(2);
				canvaCliente.Visibility = Visibility.Hidden;
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (txtUsuario.Text == null)
			{
				MessageBox.Show("Usuario faltante", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else if (txtContraUsusario.Text == null)
			{
				MessageBox.Show("Contraseña faltante", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else
			{
				string query = "INSERT INTO [usuarios] ([id_usuario], [password], [rol_user]) VALUES (@id,@pass,@rol_u)";

				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					using (OleDbCommand command = new OleDbCommand(query, connection))
					{
						command.Parameters.AddWithValue("@id", txtUsuario.Text);
						command.Parameters.AddWithValue("@pass", txtContraUsusario.Text);
						command.Parameters.AddWithValue("@rol_u", cmbRol.Text);
						command.ExecuteNonQuery();
					}
				}
				usuarios.Clear();
				ItemsUsers.Clear();
				dynamicTableUsers.ItemsSource = null;
				addItemUsers();
				addToGridUsers();
			}
		}

		private void btnAddUsers_Click(object sender, RoutedEventArgs e)
		{
			if (canvaUsers.Visibility == Visibility.Hidden)
			{
				btnAddUsers.BorderBrush = Brushes.Red;
				btnAddUsers.BorderThickness = new Thickness(2);
				canvaUsers.Visibility = Visibility.Visible;
			}
			else
			{
				btnAddUsers.BorderBrush = Brushes.Red;
				btnAddUsers.BorderThickness = new Thickness(2);
				canvaUsers.Visibility = Visibility.Hidden;
			}
		}

		private void btnEditPathCliente_Click(object sender, RoutedEventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				DialogResult result = folderBrowserDialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					// Get the selected folder path from the dialog
					string folderPath = folderBrowserDialog.SelectedPath;
					txtEditPathClient.Text = folderPath;
				}

			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (txtEditPathClient is null || txtEditCliente is null || txtNewNameCliente is null)
			{
				MessageBox.Show("Datos faltantes");
			}
			else
			{
				string query = "UPDATE clientes SET id_usuario = ?, path_cliente = ? WHERE id_usuario = @value1";

				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					using (OleDbCommand command = new OleDbCommand(query, connection))
					{
						command.Parameters.AddWithValue("@id_usuario", txtNewNameCliente.Text);
						command.Parameters.AddWithValue("@path_cliente", txtEditPathClient.Text);
						command.Parameters.AddWithValue("value1", txtEditCliente.Text);
						int rows = command.ExecuteNonQuery();
						if (rows > 0)
						{
							MessageBox.Show("Actualizado correctamente", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
							clients.Clear();
							Items.Clear();
							dynamicTable.ItemsSource = null;
							addItemsClientes();
							addToGridClient();
							editClientCanva.Visibility = Visibility.Hidden;
						}
						else
						{
							MessageBox.Show("Error al actualizar valor", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
						}
					}
				}
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			string olduser = txtEditUser.Text;
			string newuser = txtEditNewUser.Text;
			string passu = txtPassword.Text;
			string rolu = cmbEditRol.Text;
			if (string.IsNullOrEmpty(newuser) ||
				string.IsNullOrEmpty(passu) || string.IsNullOrEmpty(rolu))
			{
				MessageBox.Show("Datos faltantes");
				return;
			}

			string query = "UPDATE usuarios SET id_usuario = @id_usuario, [password] = @password, rol_user = @rol_user WHERE id_usuario = @value1";
			try
			{
				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					using (OleDbCommand command = new OleDbCommand(query, connection))
					{
						command.Parameters.AddWithValue("@id_usuario", newuser);
						command.Parameters.AddWithValue("@password", passu);
						command.Parameters.AddWithValue("@rol_user", rolu);
						command.Parameters.AddWithValue("value1", olduser);

						int rows = command.ExecuteNonQuery();
						if (rows > 0)
						{
							MessageBox.Show("Actualizado correctamente", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
							editUsersCanva.Visibility = Visibility.Hidden;
							dynamicTableUsers.ItemsSource = null;
							usuariosL.Clear();
							tupleUsers = null;
							usuarios.Clear();
							ItemsUsers.Clear();
							addItemUsers();
							addToGridUsers();
						}
						else if (rows < 1)
						{
							MessageBox.Show("Error al actualizar valor", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
						}
					}
				}

			}
			catch (Exception err)
			{
				Debug.WriteLine("Error : " + err.Message);
			}
		}
	}
}

