using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
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
	public partial class WindowAdmin : Window
	{
		public ObservableCollection<ItemData> Items { get; set; }
		public ObservableCollection<ItemDataUsers> ItemsUsers { get; set; }
		public ObservableCollection<ItemDataTip> ItemTips { get; set; }

		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		Dictionary<string, string> usuarios = new Dictionary<string, string>();
		Dictionary<string, string> tips = new Dictionary<string, string>();
		List<Tuple<string, string, string>> usuariosL = new List<Tuple<string, string, string>>();

		public Tuple<string, string, string> tupleUsers { get; set; }
		public WindowAdmin()
		{
			InitializeComponent();
			Items = new ObservableCollection<ItemData>();
			ItemsUsers = new ObservableCollection<ItemDataUsers>();
			ItemTips = new ObservableCollection<ItemDataTip>();
			addItemUsers();
			addItemsTips();
			// Load Grid with info
			addToGridUsers();
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
			try
			{
				foreach (var tip in tips)
				{
					ItemTips.Add(new ItemDataTip { Message = tip.Key, Category = tip.Value, EditCommand = new RelayCommand(EditItemTip), DeleteCommand = new RelayCommand(DeleteItemTip) });
				}
				dynamicTableTips.ItemsSource = ItemTips;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
		private void addToGridUsers()
		{
			try
			{
				foreach (var user in usuariosL)
				{
					ItemsUsers.Add(new ItemDataUsers { Name = user.Item1, Password = user.Item2, Rol = user.Item3, EditCommand = new RelayCommand(EditItemUser), DeleteCommand = new RelayCommand(DeleteItemUsers) }); ;
				}

				dynamicTableUsers.ItemsSource = ItemsUsers;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void addItemUsers()
		{
			try
			{
				ItemsUsers.Clear();
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void addItemsTips()
		{
			try
			{
				ItemTips.Clear();
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		// EDIT BUTTON TIP 
		private void EditItemTip(object ItemTips)
		{
			try
			{
				ItemDataTip selectedItem = (ItemDataTip)ItemTips;
				oldMsg.Text = selectedItem.Message;
				txtMEDit.Text = selectedItem.Message;
				cmbMEditCategoria.Text = selectedItem.Category;
				if (canvaMessageseEdit.Visibility == Visibility.Hidden)
				{
					canvaMessageseEdit.Visibility = Visibility.Visible;
				}
				else if (canvaMessageseEdit.Visibility == Visibility.Visible)
				{
					canvaMessageseEdit.Visibility = Visibility.Hidden;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		//EDIT BUTTON USER

		private void EditItemUser(object item)
		{
			try
			{
				ItemDataUsers selectedItem = (ItemDataUsers)item;
				txtEditUser.Text = selectedItem.Name;
				txtEditNewUser.Text = selectedItem.Name;
				txtPassword.Text = selectedItem.Password;
				cmbEditRol.Text = selectedItem.Rol;
				if (editUsersCanva.Visibility == Visibility.Hidden)
				{
					editUsersCanva.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		// DELETE BUTTON TO USERS
		private void DeleteItemUsers(object itemUsers)
		{
			try
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
						connection.Close();
					}
					usuarios.Clear();
					ItemsUsers.Clear();
					usuariosL.Clear();
					dynamicTableUsers.ItemsSource = null;
					addItemUsers();
					addToGridUsers();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		// DELETE BUTTON TO TIPS

		private void DeleteItemTip(object ItemTips)
		{
			try
			{
				// Handle delete command here
				ItemDataTip selectedItem = (ItemDataTip)ItemTips;
				MessageBoxResult result = MessageBox.Show("¿Estas seguro de eliminar el elemento : " + selectedItem.Message + "?", "Eliminar", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
				if (result == MessageBoxResult.OK)
				{
					string query = "DELETE FROM messages WHERE msg = ?";

					using (OleDbConnection connection = new OleDbConnection(connectionString))
					{
						connection.Open();

						using (OleDbCommand command = new OleDbCommand(query, connection))
						{
							command.Parameters.AddWithValue("@msg", selectedItem.Message);
							int rows = command.ExecuteNonQuery();
						}
						connection.Close();
					}
					dynamicTableTips.ItemsSource = null;
					tips.Clear();
					addItemsTips();
					addToGridTips();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
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

		private void ExitClick(object sender, RoutedEventArgs e)

		{
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}


		//BUTTON RELOAD USERS PAGE
		private void btnReloadUsers_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				usuarios.Clear();
				ItemsUsers.Clear();
				usuariosL.Clear();
				dynamicTableUsers.ItemsSource = null;
				addItemUsers();
				addToGridUsers();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
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
					usuariosL.Clear();
					ItemsUsers.Clear();
					dynamicTableUsers.ItemsSource = null;
					addItemUsers();
					addToGridUsers();
					canvaUsers.Visibility = Visibility.Hidden;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
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
			catch (Exception)
			{
			}
		}


		private void btnAgg_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (txtMAdd.Text == null)
				{
					MessageBox.Show("Usuario faltante", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else if (cmbMCategoria.Text == null)
				{
					MessageBox.Show("Contraseña faltante", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else
				{
					string query = "INSERT INTO [messages] ([msg], [category]) VALUES (@idm, @categorym)";

					using (OleDbConnection connection = new OleDbConnection(connectionString))
					{
						connection.Open();

						using (OleDbCommand command = new OleDbCommand(query, connection))
						{
							command.Parameters.AddWithValue("@idm", txtMAdd.Text);
							command.Parameters.AddWithValue("@categorym", cmbMCategoria.Text);
							command.ExecuteNonQuery();
							MessageBox.Show("Agregado correctamente", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
							canvaMessages.Visibility = Visibility.Hidden;
							dynamicTableTips.ItemsSource = null;
							tips.Clear();
							ItemTips.Clear();
							addItemsTips();
							addToGridTips();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Tip duplicado, intentelo de nuevo");
			}

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (canvaMessages.Visibility == Visibility.Visible)
			{
				canvaMessages.Visibility = Visibility.Hidden;
			}
			else if (canvaMessages.Visibility == Visibility.Hidden)
			{
				canvaMessages.Visibility = Visibility.Visible;
			}
		}

		private void btnReloadTips_Click(object sender, RoutedEventArgs e)
		{
			dynamicTableTips.ItemsSource = null;
			tips.Clear();
			ItemTips.Clear();
			addItemsTips();
			addToGridTips();
		}

		private void btnUpdateM_Click(object sender, RoutedEventArgs e)
		{
			string oldMs = oldMsg.Text;
			string newTip = txtMEDit.Text;
			string cate = cmbMEditCategoria.Text;

			if (string.IsNullOrEmpty(newTip) ||
				string.IsNullOrEmpty(cate))
			{
				MessageBox.Show("Datos faltantes");
				return;
			}

			string query = "UPDATE messages SET msg = @msg_n, category = @categ WHERE msg = @value1";
			try
			{
				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					using (OleDbCommand command = new OleDbCommand(query, connection))
					{
						command.Parameters.AddWithValue("@msg_n", newTip);
						command.Parameters.AddWithValue("@categ", cate);
						command.Parameters.AddWithValue("value1", oldMs);

						int rows = command.ExecuteNonQuery();
						if (rows > 0)
						{
							MessageBox.Show("Actualizado correctamente", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
							canvaMessageseEdit.Visibility = Visibility.Hidden;
							dynamicTableTips.ItemsSource = null;
							tips.Clear();
							ItemTips.Clear();
							addItemsTips();
							addToGridTips();
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
				MessageBox.Show(err.Message);
			}
		}
	}
}