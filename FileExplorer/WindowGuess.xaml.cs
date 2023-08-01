using DuEDrawingControl;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;
using CheckBox = System.Windows.Controls.CheckBox;
using Cursors = System.Windows.Input.Cursors;
using Exception = System.Exception;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using Window = System.Windows.Window;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowGuess.xaml
	/// </summary>
	public partial class WindowGuess : Window
	{
		//
		public string SelectedOption { get; set; }
		public string currentD;
		//
		List<string> itemsp = new List<string>();
		List<string> itemsf = new List<string>();
		//
		public ObservableCollection<ItemAtts> ItemsAtts { get; set; }
		//conection to database in access
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb";
		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		List<string> listaAtts = new List<string>();

		//Reference to a EDrawing to WPF
		private EDrawingWPFControl eDrawingView;

		// Parse Dir
		private delegate Node ParseDirDelegate();

		//tree display source
		ObservableCollection<Node> treeCtx = new ObservableCollection<Node>();
		Node firstNode;

		//Dependancy properties for all labels
		public string parseDir
		{
			get { return (string)GetValue(parseDirProp); }
			set { SetValue(parseDirProp, value); }
		}

		// Register parseDirProp -> Needs to be modified if you use this code in another window
		public static readonly DependencyProperty parseDirProp =
			DependencyProperty.Register("parseDirGuess", typeof(string), typeof(WindowGuess), new PropertyMetadata(""));

		// Number of folders
		public int folders
		{
			get { return (int)GetValue(foldersProp); }
			set { SetValue(foldersProp, value); }
		}

		// Register FoldersProp -> Needs to be modified if you use this code in another window
		public static readonly DependencyProperty foldersProp =
			DependencyProperty.Register("foldersGuess", typeof(int), typeof(WindowGuess), new PropertyMetadata(0));

		public int files
		{
			get { return (int)GetValue(filesProp); }
			set { SetValue(filesProp, value); }
		}

		// Register FilesProp -> Needs to be modified if you use this code in another window
		public static readonly DependencyProperty filesProp =
			DependencyProperty.Register("filesGuess", typeof(int), typeof(WindowGuess), new PropertyMetadata(0));

		public int selectedFolders
		{
			get { return (int)GetValue(selectedFoldersProp); }
			set { SetValue(selectedFoldersProp, value); }
		}

		// Register selectedFoldersProp -> Needs to be modified if you use this code in another window
		public static readonly DependencyProperty selectedFoldersProp =
			DependencyProperty.Register("selectedFoldersGuess", typeof(int), typeof(WindowGuess), new PropertyMetadata(0));

		public int selectedFiles
		{
			get { return (int)GetValue(selectedFilesProp); }
			set { SetValue(selectedFilesProp, value); }
		}

		// Register selectedFilesProp -> Needs to be modified if you use this code in another window
		public static readonly DependencyProperty selectedFilesProp =
			DependencyProperty.Register("selectedFilesGuess", typeof(int), typeof(WindowGuess), new PropertyMetadata(0));

		public string sizeInBytes
		{
			get { return (string)GetValue(sizeInBytesProp); }
			set { SetValue(sizeInBytesProp, value); }
		}

		// Register sizeInBytesProp -> Needs to be modified if you use this code in another window
		public static readonly DependencyProperty sizeInBytesProp =
			DependencyProperty.Register("sizeInBytesGuess", typeof(string), typeof(WindowGuess), new PropertyMetadata((string)""));
		public WindowGuess()
		{
			InitializeComponent();
			InitializeComponent();
			LoadPathFile();
			SelectedOption = "Consultas";
			DataContext = this;
			eDrawingView = edrawingControl;
			ItemsAtts = new ObservableCollection<ItemAtts>();

		}

		public void LoadPathFile()
		{
			//parseDir = "D:/ING/"+SelectedOption+"/";
			parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + SelectedOption + "/";

		}
		private void createFirstNode()
		{
			//initialize first node to hold all other nodes
			if (parseDir is null)
			{

			}
			else
			{
				DirectoryInfo dirInfo = new DirectoryInfo(parseDir);

				firstNode = new Node()
				{
					name = dirInfo.Name,
					fullPath = parseDir,
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

		}

		//update all dependacy properties to current static values in Node class
		private void UpdateCounts()
		{
			files = Node.files;
			folders = Node.folders;
			selectedFolders = Node.selectedFolders;
			selectedFiles = Node.selectedFiles;
			sizeInBytes = Node.selectedBytes;
		}

		public void viewTree_NodeChecked(object sender, TreeNodeMouseClickEventArgs e)
		{
			TreeNode newSelected = e.Node;
			DirectoryInfo _NewPath = (DirectoryInfo)newSelected.Tag;
			if (_NewPath != null && !string.IsNullOrWhiteSpace(_NewPath.FullName))
			{

			}
		}
		public void viewTree_PreviewMouseRightClickDown(object sender, MouseButtonEventArgs e)
		{
			var carpetaOnly = Path.GetDirectoryName(Node.selectedBytes);
			parseDir = carpetaOnly;

			ParseNewDir();

		}

		//It can be deleted --- Make sure to delete from .xaml
		public void viewTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{

		}

		public void ParseNewDir()
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

		public void theCallback(IAsyncResult theResults)
		{
			AsyncResult result = (AsyncResult)theResults;
			ParseDirDelegate parseDelegate = (ParseDirDelegate)result.AsyncDelegate;
			Node node = parseDelegate.EndInvoke(theResults);

			this.Dispatcher.Invoke(DispatcherPriority.Background, ((System.Action)(() =>
			{
				parseMsg.Visibility = Visibility.Hidden;
				firstNode = node;
				UpdateCounts();
				fileDisplay.ItemsSource = treeCtx;
			})));
		}


		private void dirDisplay_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			ParseNewDir();
		}
		private void BtnFolder_Click(object sender, RoutedEventArgs e)
		{
			string routeFolder = (sender as Button)?.Tag as string;
			if (!string.IsNullOrEmpty(routeFolder))
			{
				parseDir = routeFolder;
				ParseNewDir();
			}
		}
		private void chk_clicked(object sender, RoutedEventArgs e)
		{
			progressB.Value = 0;
			ItemsAtts.Clear();
			listaAtts.Clear();
			listAtts.ItemsSource = null;
			if (Path.GetExtension(Node.selectedBytes).Equals(".pdf") || Path.GetExtension(Node.selectedBytes).Equals(".plt") || Path.GetExtension(Node.selectedBytes).Equals(".xlsx"))
			{
				measureB.IsEnabled = false;
				sB.Visibility = Visibility.Collapsed;
				measureB.Visibility = Visibility.Collapsed;
			}
			else
			{
				measureB.IsEnabled = true;
				sB.Visibility = Visibility.Visible;
				measureB.Visibility = Visibility.Visible;
			}
			CheckBox checkBox = (CheckBox)sender;
			if (checkBox.Template.FindName("lbl", checkBox) is Label label)
			{
				//stck.Background = new SolidColorBrush(Colors.Blue);
				string labelText = label.Content.ToString();

				txtFolder.Text = labelText;
				attributesFiles(labelText);
				addGridAtts();
			}
			ContentPath(Node.selectedBytes);
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				DialogResult result = fbd.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
					parseDir = fbd.SelectedPath;
			}
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				File.WriteAllText(@"path.txt", parseDir);
			}
			catch (Exception)
			{
			}
		}
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			string pathF = Node.selectedBytes;
		}
		private void SendConfirmation()
		{
			WindowMail win = new WindowMail();
			win.Show();
		}
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			SendConfirmation();
		}
		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Mouse.OverrideCursor = Cursors.Wait;
				btnBuscar.IsEnabled = false;
				twSearched.Items.Clear();
				string searchItem = txtSearch.Text;
				SearchBox(searchItem);
			}
			catch
			{
				txtSearch.Text = "Error";
			}
			finally
			{
				btnBuscar.IsEnabled = true;
				Mouse.OverrideCursor = null;
			}
		}
		private void SearchBox(string searchItem)
		{
			Mouse.OverrideCursor = Cursors.Wait;
			string directoryPath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + SelectedOption + "/";
			string[] files = Directory.GetFiles(directoryPath, "*" + searchItem + "*" + ".*", SearchOption.AllDirectories);
			string[] directories = Directory.GetDirectories(directoryPath, "*" + searchItem + "*", SearchOption.AllDirectories);
			int totalItems = files.Length + directories.Length;
			int processedItems = 0;
			if (totalItems == 0)
			{
				twSearched.Items.Add("No se encontro ningun resultado");
				return;
			}
			foreach (string file in files)
			{
				string fileName = Path.GetFileName(file);
				twSearched.Items.Add(fileName);
				itemsp.Add(file);

				processedItems++;
				UpdateProgress(processedItems, totalItems);
			}
			foreach (string directory in directories)
			{
				string directoryName = new DirectoryInfo(directory).Name;
				twSearched.Items.Add(directoryName);
				itemsf.Add(directory);
				processedItems++;
				UpdateProgress(processedItems, totalItems);
			}
		}

		private void UpdateProgress(int processedItems, int totalItems)
		{
			progressBar.Visibility = Visibility.Visible;
			double progressPercentage = (double)processedItems / totalItems * 100;
			progressBar.Value = progressPercentage;
			progressBar.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
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
			WindowClientsMenu win = new WindowClientsMenu();
			win.Show();
			Close();
		}
		private void ExitClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void LogOut(object sender, RoutedEventArgs e)
		{
			WindowLogin win = new WindowLogin();
			win.Show();
			Close();
		}

		private void Open_Click(object sender, RoutedEventArgs e)
		{
			toolbarOption(Node.selectedBytes);
		}

		private void toolbarOption(string path)
		{
			if (path == null)
			{
				MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

			}
			else
			{
				if (Path.GetExtension(path).Equals(".PDF", StringComparison.OrdinalIgnoreCase))
				{
					edrawingControl.Visibility = Visibility.Hidden;
					wb.Visibility = Visibility.Visible;
					var pathPdf = Path.GetFullPath(path);
					Uri pdfUri = new Uri(pathPdf);
					wb.Source = pdfUri;
				}
				else if (Path.GetExtension(path).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
				{
					var pathExcel = Path.GetFullPath(path);
					Process.Start(pathExcel);
				}
				else if (Path.GetExtension(path).Equals(".SLDPRT", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dxf", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STEP", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STL", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".OBJ", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".SLDASM", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dwg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".stp", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".SLDDRW", StringComparison.OrdinalIgnoreCase))
				{
					wb.Visibility = Visibility.Hidden;
					edrawingControl.Visibility = Visibility.Visible;
					eDrawingView = edrawingControl;
					var testModel = Path.GetFullPath(Node.selectedBytes);
					eDrawingView.EDrawingHost.OpenDoc(testModel, false, false, false);
				}
			}
		}
		private void SaveInfoFile_Click(object sender, RoutedEventArgs e)
		{
			var file = txtFolder.Text;
			if (file != null)
			{
				var pathI = SelectedOption + ".png";
				WindowAtts win = new WindowAtts();
				win.pathPass = file;
				win.LoadImage(pathI);
				win.loadName(file);
				win.Show();
			}
			else if (Path.GetExtension(file).Length > 1)
			{
				MessageBox.Show("Selecciona una carpeta");
			}
			else if (string.IsNullOrEmpty(file))
			{
				MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Exclamation);
			}
		}
		private void EditInfoFile_Click(object sender, RoutedEventArgs e)
		{
			if (Node.selectedBytes != null)
			{
				var pathI = SelectedOption + ".png";
				WindowEdit win = new WindowEdit();
				win.loadName(txtFolder.Text);
				win.atts(txtFolder.Text);
				win.LoadImage(pathI);
				win.pathPass = Node.selectedBytes;
				win.Show();
			}
			else
			{
				MessageBox.Show("Selecciona una archivo para editar", "Error");
			}
		}
		private void folder_click(object sender, RoutedEventArgs e)
		{
			if (Node.selectedBytes == null)
			{
				MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				Process.Start("explorer.exe", Path.GetDirectoryName(Node.selectedBytes));
			}
		}
		private void reloadTreeView_click(object sender, RoutedEventArgs e)
		{
			LoadPathFile();
			ParseNewDir();
		}
		private void backTreeView_click(object sender, RoutedEventArgs e)
		{
			var p = Node.selectedBytes;
			if (p is null)
			{
				MessageBox.Show("No hay ninguna carpeta seleccionada");
			}
			else
			{
				currentD = Directory.GetParent(p)?.FullName;
				int index = currentD.IndexOf(txtFolder.Text);
				if (index != -1)
				{
					string newP = currentD.Substring(0, index);
					parseDir = newP;
					ParseNewDir();
				}
			}
		}
		public void attributesFiles(string id)
		{
			var query = "SELECT * FROM atts WHERE Id_file=@Value1";
			if (id is null)
			{
			}
			else
			{
				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();
					OleDbCommand command = new OleDbCommand(query, connection);
					command.Parameters.AddWithValue("@Value1", id);
					command.ExecuteNonQuery();
					OleDbDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						listaAtts.Add(reader.GetValue(0).ToString());
						listaAtts.Add(reader.GetValue(1).ToString());
						listaAtts.Add(reader.GetValue(2).ToString());
						listaAtts.Add(reader.GetValue(3).ToString());
						listaAtts.Add(reader.GetValue(4).ToString());
						listaAtts.Add(reader.GetValue(5).ToString());
						listaAtts.Add(reader.GetValue(6).ToString());
						listaAtts.Add(reader.GetValue(7).ToString());
						listaAtts.Add(reader.GetValue(8).ToString());
						listaAtts.Add(reader.GetValue(9).ToString());
						listaAtts.Add(reader.GetValue(10).ToString());
						listaAtts.Add(reader.GetValue(11).ToString());
						listaAtts.Add(reader.GetValue(12).ToString());
						listaAtts.Add(reader.GetValue(13).ToString());
						listaAtts.Add(reader.GetValue(14).ToString());
						listaAtts.Add(reader.GetValue(15).ToString());
						listaAtts.Add(reader.GetValue(16).ToString());
						listaAtts.Add(reader.GetValue(17).ToString());
						listaAtts.Add(reader.GetValue(18).ToString());
						listaAtts.Add(reader.GetValue(19).ToString());
					}
					reader.Close();
					connection.Close();
				}
			}
		}
		private void addGridAtts()
		{
			if (listaAtts.Count < 14)
			{
			}
			else
			{
				string id = listaAtts[0];
				string no_p = listaAtts[1];
				string rev = listaAtts[2];
				string mat_p = listaAtts[3];
				string lot = listaAtts[4];
				string proj = listaAtts[5];
				string aS = listaAtts[6];
				string fechaed = listaAtts[7];
				string acero = listaAtts[8];
				bool cort = bool.Parse(listaAtts[9]);
				bool dblez = bool.Parse(listaAtts[10]);
				bool maquina = bool.Parse(listaAtts[11]);
				bool pint = bool.Parse(listaAtts[12]);
				bool detallad = bool.Parse(listaAtts[13]);
				bool soldadure = bool.Parse(listaAtts[14]);
				bool s_articulos = bool.Parse(listaAtts[15]);
				bool s_estructura = bool.Parse(listaAtts[16]);
				bool s_procesos = bool.Parse(listaAtts[17]);
				bool s_modelado = bool.Parse(listaAtts[18]);
				bool s_publicacion = bool.Parse(listaAtts[19]);
				if (s_articulos == true)
				{
					chkArticulo.IsChecked = true;
					progressB.Value = 20;
					if (s_estructura == true)
					{
						chkEstructura.IsChecked = true;
						progressB.Value = 40;
						if (s_procesos == true)
						{
							chkProcesos.IsChecked = true;
							progressB.Value = 60;
							if (s_modelado == true)
							{
								chkModelado.IsChecked = true;
								progressB.Value = 80;
								if (s_publicacion == true)
								{
									chkPublicacion.IsChecked = true;
									progressB.Value = 100;
								}
								else if (s_publicacion == false)
								{
									chkPublicacion.IsChecked = false;
									progressB.Value = 80;
								}
							}
							else if (s_modelado == false)
							{
								chkModelado.IsChecked = false;
								chkPublicacion.IsChecked = false;
								progressB.Value = 60;
							}
						}
						else if (s_procesos == false)
						{
							chkProcesos.IsChecked = false;
							chkModelado.IsChecked = false;
							chkPublicacion.IsChecked = false;
							progressB.Value = 40;
						}
					}
					else if (s_estructura == false)
					{
						chkEstructura.IsChecked = false;
						chkProcesos.IsChecked = false;
						chkModelado.IsChecked = false;
						chkPublicacion.IsChecked = false;
						progressB.Value = 20;
					}
				}
				else if (s_articulos == false)
				{
					chkArticulo.IsChecked = false;
					chkEstructura.IsChecked = false;
					chkProcesos.IsChecked = false;
					chkModelado.IsChecked = false;
					chkPublicacion.IsChecked = false;
					progressB.Value = 0;
				}
				ItemsAtts.Add(new ItemAtts
				{
					Name = id,
					no_parte = no_p,
					Revision = rev,
					Materia_Prima = mat_p,
					lote = lot,
					proyecto = proj,
					acabado_s = aS,
					fecha_e = fechaed,
					tipo_acero = acero,
					p_corte = cort,
					p_doblez = dblez,
					p_maquinado = maquina,
					p_pintura = pint,
					p_detallado = detallad,
					p_soldadura = soldadure
				});

				listAtts.ItemsSource = ItemsAtts;
			}
		}

		private void ContentPathTW(string folderp)
		{
			listFilesNode.Items.Clear();
			Debug.WriteLine("folderP : " + folderp);
			string[] directories = Directory.GetDirectories(folderp);
			foreach (string directory in directories)
			{
				Debug.WriteLine("dName : " + Path.GetDirectoryName(directory));
				listFilesNode.Items.Add("Carpeta: " + Path.GetFileName(directory));
			}
		}
		private void ContentPath(string folderp)
		{
			string parentPath = Directory.GetParent(folderp)?.FullName;
			listFilesNode.Items.Clear();
			//var pathF = Path.GetDirectoryName(folderp);
			var p = Node.selectedBytes;
			if (p is null)
			{
				MessageBox.Show("No hay ninguna carpeta seleccionada");
			}
			else
			{
				currentD = Directory.GetParent(p)?.FullName;
				int index = currentD.IndexOf(txtFolder.Text, StringComparison.OrdinalIgnoreCase);
				if (index != -1)
				{
					string newP = currentD.Substring(0, index + txtFolder.Text.Length);
					string[] directories = Directory.GetDirectories(newP);
					foreach (string directory in directories)
					{
						listFilesNode.Items.Add("Carpeta: " + directory.Replace(Path.GetDirectoryName(directory) + Path.DirectorySeparatorChar, ""));
					}
					Debug.WriteLine(newP);
				}
			}

		}
		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			var textP = txtP.Text;
			string fullF = itemsp.Find(item => item.Contains(txtP.Text));
			var path = fullF;
			if (path == null)
			{
				MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				if (Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
				{
					if (measureB.Visibility == Visibility.Visible)
					{
						sB.Visibility = Visibility.Collapsed;
						measureB.Visibility = Visibility.Collapsed;
					}
					edrawingControl.Visibility = Visibility.Hidden;
					wb.Visibility = Visibility.Visible;
					var pathPdf = Path.GetFullPath(path);
					Uri pdfUri = new Uri(pathPdf);
					wb.Source = pdfUri;
				}
				else if (Path.GetExtension(path).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
				{
					var pathExcel = Path.GetFullPath(path);
					Process.Start(pathExcel);
				}
				else if (Path.GetExtension(path).Equals(".SLDPRT", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dxf", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STEP", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STL", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".OBJ", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".SDLASM", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dwg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".stp", StringComparison.OrdinalIgnoreCase))
				{
					if (measureB.Visibility == Visibility.Collapsed)
					{
						sB.Visibility = Visibility.Visible;

						measureB.Visibility = Visibility.Visible;

					}
					wb.Visibility = Visibility.Hidden;
					edrawingControl.Visibility = Visibility.Visible;
					eDrawingView = edrawingControl;
					var testModel = Path.GetFullPath(path);
					eDrawingView.EDrawingHost.OpenDoc(testModel, false, false, false);
				}
			}
		}
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			var path = twSearched.SelectedItem.ToString();
			var pathF = txtP.Text;
			/*if (path == null)
			{
				MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				Process.Start("explorer.exe", Path.GetDirectoryName(path));
			}*/
		}

		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			if (twSearched.SelectedItem != null)
			{
				WindowAtts win = new WindowAtts();
				win.loadName(twSearched.SelectedItem.ToString());
				win.pathPass = twSearched.SelectedItem.ToString();
				win.Show();
			}
			else
			{
				MessageBox.Show("Selecciona un archivo", "Error");
			}
		}
		private void MenuItem_Click_4(object sender, RoutedEventArgs e)
		{
			if (twSearched.SelectedItem.ToString() != null)
			{
				WindowEdit win = new WindowEdit();
				win.loadName(twSearched.SelectedItem.ToString());
				win.atts(twSearched.SelectedItem.ToString());
				win.pathPass = twSearched.SelectedItem.ToString();
				win.Show();
			}
			else
			{
				MessageBox.Show("Selecciona una archivo para editar", "Error");
			}
		}
		private void MenuItem_Click_5(object sender, RoutedEventArgs e)
		{
			var p = txtP.Text;
			if (Path.GetExtension(p) is string)
			{
				var textP = txtP.Text;
				string fullF = itemsp.Find(item => item.Contains(txtP.Text));
				var path = Path.GetDirectoryName(fullF);
				parseDir = path;
				ParseNewDir();
			}
			else
			{
				var textP = txtP.Text;
				string fullF = itemsf.Find(item => item.Contains(txtP.Text));
				var path = Path.GetDirectoryName(fullF);
				parseDir = path;
				ParseNewDir();
			}
		}
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			eDrawingView.Markup.ViewOperator_Set(EMVMarkupOperators.eMVOperatorMeasure);
		}
		private void twSearched_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			progressB.Value = 0;
			ItemsAtts.Clear();
			listaAtts.Clear();
			listAtts.ItemsSource = null;
			txtP.Text = twSearched.SelectedItem.ToString();
			var textP = txtP.Text;
			string fullF = itemsp.Find(item => item.Contains(txtP.Text));
			string fullP = itemsf.Find(item => item.Contains(txtP.Text));
			ContentPathTW(fullP);
			if (Path.GetExtension(textP).Length > 1)
			{
				ver.IsEnabled = true;
			}
			else
			{
				ver.IsEnabled = false;
			}
			attributesFiles(textP);
			addGridAtts();
		}
		private void exportButton_Click(object sender, RoutedEventArgs e)
		{
			string excelFilePath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/exportAtts.xlsx";
			string query = "SELECT * FROM atts";
			try
			{
				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();
					OleDbCommand command = new OleDbCommand(query, connection);
					OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
					DataTable dataTable = new DataTable();
					dataAdapter.Fill(dataTable);
					ExportDataTableToExcel(dataTable, excelFilePath);
				}
				MessageBox.Show("Datos exportados correctamente", "Exportacion satisfactoria", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ocurrio un error al exportar los datos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private void ExportDataTableToExcel(DataTable dataTable, string filePath)
		{
			Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
			Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add();
			Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				worksheet.Cells[1, i + 1] = dataTable.Columns[i].ColumnName;
			}
			for (int row = 0; row < dataTable.Rows.Count; row++)
			{
				for (int col = 0; col < dataTable.Columns.Count; col++)
				{
					worksheet.Cells[row + 2, col + 1] = dataTable.Rows[row][col];
				}
			}
			workbook.SaveAs(filePath);
			workbook.Close();
			excelApp.Quit();
			System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
		}
		private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				try
				{
					Mouse.OverrideCursor = Cursors.Wait;
					btnBuscar.IsEnabled = false;
					twSearched.Items.Clear();
					string searchItem = txtSearch.Text;
					SearchBox(searchItem);
				}
				catch
				{
					txtSearch.Text = "Error";
				}
				finally
				{
					btnBuscar.IsEnabled = true;
					Mouse.OverrideCursor = null;
				}
			}
		}

		private void btnParte_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				progressB.Value = 0;
				ItemsAtts.Clear();
				listaAtts.Clear();
				listAtts.ItemsSource = null;
				Mouse.OverrideCursor = Cursors.Wait;
				attributesFiles(txtParte.Text);
				addGridAtts();
			}
			catch
			{
				txtParte.Text = "Numero de parte no encontrado";
			}
			finally
			{
				Mouse.OverrideCursor = null;
			}
		}
		private void txtParte_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				try
				{
					progressB.Value = 0;
					ItemsAtts.Clear();
					listaAtts.Clear();
					listAtts.ItemsSource = null;
					Mouse.OverrideCursor = Cursors.Wait;
					attributesFiles(txtParte.Text);
					addGridAtts();
				}
				catch
				{
					txtParte.Text = "Numero de parte no encontrado";
				}
				finally
				{
					Mouse.OverrideCursor = null;
				}
			}
		}
	}
}