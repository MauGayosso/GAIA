﻿
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.Forms.MessageBox;
using Path = System.IO.Path;
using RadioButton = System.Windows.Controls.RadioButton;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowAyudasVisuales.xaml
	/// </summary>
	public partial class WindowAyudasVisuales3 : Window
	{
		private RadioButton lastCheckedRadioButton = null;
		public string nod;
		public string SelectedOption { get; set; }
		public static System.Windows.Media.Color WindowGlassColor { get; }
		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		List<string> titulos = new List<string>();
		List<string> mensajes = new List<string>();

		private delegate Node ParseDirDelegate();
		// Notification

		//tree display source
		ObservableCollection<Node> treeCtx = new ObservableCollection<Node>();
		Node firstNode;

		//Dependancy properties for all labels
		public string parseDirCorte
		{
			get { return (string)GetValue(parseDirProp); }
			set { SetValue(parseDirProp, value); }
		}

		public static readonly DependencyProperty parseDirProp =
			DependencyProperty.Register("parseDirCorte3", typeof(string), typeof(WindowAyudasVisuales2), new PropertyMetadata(""));

		public int folders
		{
			get { return (int)GetValue(foldersProp); }
			set { SetValue(foldersProp, value); }
		}

		public static readonly DependencyProperty foldersProp =
			DependencyProperty.Register("foldersCorte3", typeof(int), typeof(WindowAyudasVisuales2), new PropertyMetadata(0));

		public int files
		{
			get { return (int)GetValue(filesProp); }
			set { SetValue(filesProp, value); }
		}

		public static readonly DependencyProperty filesProp =
			DependencyProperty.Register("filesCorte3", typeof(int), typeof(WindowAyudasVisuales2), new PropertyMetadata(0));

		public int selectedFolders
		{
			get { return (int)GetValue(selectedFoldersProp); }
			set { SetValue(selectedFoldersProp, value); }
		}

		public static readonly DependencyProperty selectedFoldersProp =
			DependencyProperty.Register("selectedFoldersCorte3", typeof(int), typeof(WindowAyudasVisuales2), new PropertyMetadata(0));

		public int selectedFiles
		{
			get { return (int)GetValue(selectedFilesProp); }
			set { SetValue(selectedFilesProp, value); }
		}

		public static readonly DependencyProperty selectedFilesProp =
			DependencyProperty.Register("selectedFilesCorte3", typeof(int), typeof(WindowAyudasVisuales2), new PropertyMetadata(0));

		public string sizeInBytes
		{
			get { return (string)GetValue(sizeInBytesProp); }
			set { SetValue(sizeInBytesProp, value); }
		}

		public static readonly DependencyProperty sizeInBytesProp =
			DependencyProperty.Register("sizeInBytesCorte3", typeof(string), typeof(WindowAyudasVisuales2), new PropertyMetadata((string)""));
		private string MODELPATH = Node.selectedBytes;
		//Dependancy properties for all labels
		public WindowAyudasVisuales3()
		{
			InitializeComponent();
			LoadPathFile();
			DataContext = this;
			Brush titleBarBrush = new SolidColorBrush(WindowGlassColor);
			SelectedOption = "Consultas";

			getMessages(SelectedOption);
			foreach (var m in mensajes)
			{
				// notifications.Add(new NotificationContent { Title = "Tip", Message = m, Type = NotificationType.Information });
			}

			// Configure and start the timer
			//timer.Interval = TimeSpan.FromSeconds(5);
			// timer.Tick += Timer_Tick;
			// timer.Start();
		}

		private void getMessages(string cat)
		{
			try
			{
				var query = "SELECT * FROM messages WHERE category = @idC";
				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();
					OleDbCommand command = new OleDbCommand(query, connection);
					command.Parameters.AddWithValue("@idC", cat);
					command.ExecuteNonQuery();

					OleDbDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						mensajes.Add(reader.GetValue(0).ToString());
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

		/* private void Timer_Tick(object sender, EventArgs e)
		 {
			 // Get a random notification from the list
			 Random random = new Random();
			 int index = random.Next(notifications.Count);
			 NotificationContent notification = notifications[index];

			 // Show the notification
			 notificationManager.Show(notification, areaName: "WindowArea");
		 }*/
		public void LoadPathFile()
		{
			try
			{
				//parseDirCorte = "D:/ARCHIVOS DE AYUDAS VISUALES";
				parseDirCorte = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ARCHIVOS DE AYUDAS VISUALES/";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void createFirstNode()
		{
			try
			{
				//initialize first node to hold all other nodes
				DirectoryInfo dirInfo = new DirectoryInfo(parseDirCorte);
				firstNode = new Node()
				{
					name = dirInfo.Name,
					fullPath = parseDirCorte,
					byteSize = 0,
					parent = null,
					iconLoc = Node.folderIcon,
					isFile = false,
				};
				++Node.folders;
				//add first node to display         
				treeCtx = new ObservableCollection<Node>()
			{
				firstNode
			};
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		//update all dependacy properties to current static values in Node class
		private void UpdateCounts()
		{
			try
			{
				files = Node.files;
				folders = Node.folders;
				selectedFolders = Node.selectedFolders;
				selectedFiles = Node.selectedFiles;
				sizeInBytes = Node.selectedBytes;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void viewTree_NodeChecked(object sender, TreeNodeMouseClickEventArgs e)
		{
			try
			{
				TreeNode newSelected = e.Node;
				DirectoryInfo _NewPath = (DirectoryInfo)newSelected.Tag;
				if (_NewPath != null && !string.IsNullOrWhiteSpace(_NewPath.FullName))
				{
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
		public void viewTree_PreviewMouseRightClickDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				var carpetaOnly = Path.GetDirectoryName(Node.selectedBytes);
				parseDirCorte = carpetaOnly;
				//LoadPathFile();
				ParseNewDir();
				// createFirstNode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		public void viewTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
		}

		public void ParseNewDir()
		{
			try
			{
				//resets all counts and displays to 0
				Node.resetCounts();
				UpdateCounts();

				//create first node, empty tree display, display parsing msg
				createFirstNode();
				fileDisplay.ItemsSource = null;
				parseMsg.Visibility = Visibility.Visible;

				//recursively parse directory asynchronously 
				ParseDirDelegate parseDelegate = new ParseDirDelegate(firstNode.DirParse);
				parseDelegate.BeginInvoke(theCallback, this);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		public void theCallback(IAsyncResult theResults)
		{
			AsyncResult result = (AsyncResult)theResults;
			ParseDirDelegate parseDelegate = (ParseDirDelegate)result.AsyncDelegate;
			Node node = parseDelegate.EndInvoke(theResults);
			//Back to the GUI thread to update tree display and counts with newly parsed directory
			//hide parsing msg and display complete msg
			this.Dispatcher.Invoke(DispatcherPriority.Background, ((Action)(() =>
			{
				parseMsg.Visibility = Visibility.Hidden;
				firstNode = node;
				//firstNode.isChecked = false;
				UpdateCounts();
				fileDisplay.ItemsSource = treeCtx;
			})));
		}
		private void dirDisplay_TextChanged(object sender, TextChangedEventArgs e)
		{
			ParseNewDir();
		}

		private void chk_clicked(object sender, RoutedEventArgs e)
		{
			nod = null;
			var nodeS = Node.selectedBytes;
			Debug.WriteLine("nodeS : " + nodeS);
			nod = nodeS;
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Template.FindName("lbl", radioButton) is Label label)
			{

				string labelText = label.Content.ToString();
				//txtFolder.Text = labelText;
			}
			if (radioButton.Template.FindName("lbl", radioButton) is Label label2)
			{
				if (radioButton.IsChecked == true && radioButton != lastCheckedRadioButton)
				{
					if (lastCheckedRadioButton != null)
					{
						lastCheckedRadioButton.IsChecked = false;
					}
					lastCheckedRadioButton = radioButton;
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//display folder dialog so user can select directory to parse
				using (var fbd = new FolderBrowserDialog())
				{
					DialogResult result = fbd.ShowDialog();
					if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
						parseDirCorte = fbd.SelectedPath;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				System.IO.File.WriteAllText(@"pathAVCorte.txt", parseDirCorte);
			}
			catch (Exception)
			{
				//System.Windows.MessageBox.Show("There has been an error saving the current directory.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string pathF = Node.selectedBytes;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}


		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			//twSearched.Items.Clear();
			//string searchItem = txtSearch.Text;
			// SearchItemTreeView(searchItem);
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
				//MainWindow winMain = new MainWindow();
				//winMain.Close();
				WindowOptionsAV win = new WindowOptionsAV();
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
			try
			{
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void Open_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (nod == null)
				{
					System.Windows.MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

				}
				else
				{

					if (Path.GetExtension(nod).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
					{
						var pathPdf = Path.GetFullPath(nod);
						//extract_Rev();
						//Process.Start(pathPdf);
						Uri pdfUri = new Uri(pathPdf);
						wb.Source = pdfUri;
					}
					else if (Path.GetExtension(nod).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
					{
						var pathExcel = Path.GetFullPath(nod);
						Process.Start(pathExcel);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{

		}
		private void folder_click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Node.selectedBytes == null)
				{
					System.Windows.MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					Process.Start("explorer.exe", Path.GetDirectoryName(nod));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void reloadTreeView_click(object sender, RoutedEventArgs e)
		{
			try
			{
				LoadPathFile();
				ParseNewDir();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void InicioClick(object sender, RoutedEventArgs e)
		{
			try
			{
				WindowLogin win = new WindowLogin();
				win.Show();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

	}
}


