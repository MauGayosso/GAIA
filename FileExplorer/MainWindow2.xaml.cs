using DuEDrawingControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;
using Cursors = System.Windows.Input.Cursors;
using Exception = System.Exception;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using RadioButton = System.Windows.Controls.RadioButton;
using Window = System.Windows.Window;
namespace FileExplorer

{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// The variable Node.selectedBytes is to get the path from the checkbox selected ===> It can be modified but you need to do it in the file Node.cs
	/// This project was made by https://github.com/MauGayosso - period 05/09/23 to 08/18/2023 for the enterprise Maquinados Industriales
	public partial class MainWindow2 : Window
	{
		public string SelectedOption { get; set; }
		public string SelectedOption2 { get; set; }
		private RadioButton lastCheckedRadioButton = null;
		public string nod;
		public string currentD;
		List<string> itemsp = new List<string>();
		List<string> itemsf = new List<string>();
		public ObservableCollection<ItemAtts> ItemsAtts { get; set; }
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb";
		List<string> listaAtts = new List<string>();
		private EDrawingWPFControl eDrawingView;
		private delegate Node ParseDirDelegate();
		ObservableCollection<Node> treeCtx = new ObservableCollection<Node>();
		Node firstNode;
		public string parseDir
		{
			get { return (string)GetValue(parseDirProp); }
			set { SetValue(parseDirProp, value); }
		}
		public static readonly DependencyProperty parseDirProp =
			DependencyProperty.Register("parseDir2", typeof(string), typeof(MainWindow2), new PropertyMetadata(""));
		public int folders
		{
			get { return (int)GetValue(foldersProp); }
			set { SetValue(foldersProp, value); }
		}
		public static readonly DependencyProperty foldersProp =
			DependencyProperty.Register("folders2", typeof(int), typeof(MainWindow2), new PropertyMetadata(0));
		public int files
		{
			get { return (int)GetValue(filesProp); }
			set { SetValue(filesProp, value); }
		}
		public static readonly DependencyProperty filesProp =
			DependencyProperty.Register("files2", typeof(int), typeof(MainWindow2), new PropertyMetadata(0));
		public int selectedFolders
		{
			get { return (int)GetValue(selectedFoldersProp); }
			set { SetValue(selectedFoldersProp, value); }
		}
		public static readonly DependencyProperty selectedFoldersProp =
			DependencyProperty.Register("selectedFolders2", typeof(int), typeof(MainWindow2), new PropertyMetadata(0));
		public int selectedFiles
		{
			get { return (int)GetValue(selectedFilesProp); }
			set { SetValue(selectedFilesProp, value); }
		}
		public static readonly DependencyProperty selectedFilesProp =
			DependencyProperty.Register("selectedFiles2", typeof(int), typeof(MainWindow2), new PropertyMetadata(0));
		public string sizeInBytes
		{
			get { return (string)GetValue(sizeInBytesProp); }
			set { SetValue(sizeInBytesProp, value); }
		}
		public static readonly DependencyProperty sizeInBytesProp =
			DependencyProperty.Register("sizeInBytes2", typeof(string), typeof(MainWindow2), new PropertyMetadata((string)""));
		public MainWindow2()
		{
			InitializeComponent();
			LoadPathFile();
			SelectedOption = "Consultas";
			DataContext = this;
			eDrawingView = edrawingControl;
			ItemsAtts = new ObservableCollection<ItemAtts>();
		}

		public void LoadPathFile()
		{
			try
			{
				//parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/" + SelectedOption + "/";
				parseDir = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + SelectedOption + "/";

			}
			catch (Exception ex)
			{
			}
		}
		public void LoadImage(string path)
		{
			try
			{
				displayImage.Source = new BitmapImage(new Uri("//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/image/" + path));

			}
			catch
			{
				displayImage.Source = new BitmapImage(new Uri("//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/image/noimage.png"));
			}

		}
		private void createFirstNode()
		{
			try
			{
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
					treeCtx = new ObservableCollection<Node>()
			{
				firstNode
			};
				}
			}
			catch (Exception ex)
			{

			}
		}

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

			}
		}

		public void viewTree_NodeChecked(object sender, TreeNodeMouseClickEventArgs e)
		{
			try
			{
				TreeNode newSelected = e.Node;
				DirectoryInfo _NewPath = (DirectoryInfo)newSelected.Tag;
				if (_NewPath != null && !string.IsNullOrWhiteSpace(_NewPath.FullName))
				{ }
			}
			catch (Exception ex)
			{

			}
		}

		public void viewTree_PreviewMouseRightClickDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				var p = nod;
				currentD = Directory.GetParent(p)?.FullName;
				int index = currentD.IndexOf(txtFolder.Text, StringComparison.OrdinalIgnoreCase);
				if (index >= 0)
				{
					string newP = p.Substring(0, index + txtFolder.Text.Length);
					Debug.WriteLine("NewP : " + newP);
					parseDir = newP;
					ParseNewDir();
				}
			}
			catch (Exception ex)
			{
			}

		}

		public void ParseNewDir()
		{
			try
			{
				Node.resetCounts();
				UpdateCounts();
				createFirstNode();
				fileDisplay.ItemsSource = null;
				parseMsg.Visibility = Visibility.Visible;
				ParseDirDelegate parseDelegate = new ParseDirDelegate(firstNode.DirParse);
				parseDelegate.BeginInvoke(theCallback, this);

			}
			catch (Exception ex)
			{

			}

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
			try
			{
				ParseNewDir();
			}
			catch (Exception ex)
			{

			}

		}

		private void BtnFolder_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string routeFolder = (sender as Button)?.Tag as string;
				if (!string.IsNullOrEmpty(routeFolder))
				{
					parseDir = routeFolder;
					ParseNewDir();
				}
			}
			catch (Exception ex)
			{

			}
		}

		private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
		{
			var parentObject = VisualTreeHelper.GetParent(child);

			if (parentObject == null)
				return null;
			if (parentObject is T parent)
				return parent;
			else
				return FindVisualParent<T>(parentObject);
		}

		private void chk_clicked(object sender, RoutedEventArgs e)
		{
			nod = null;
			var nodeS = Node.selectedBytes;
			Debug.WriteLine("nodeS : " + nodeS);
			nod = nodeS;
			progressB.Value = 0;
			ItemsAtts.Clear();
			listaAtts.Clear();
			listAtts.ItemsSource = null;
			try
			{
				if (Path.GetExtension(nodeS) == null)
				{
					Debug.WriteLine("NULL");
				}
				else if (Path.GetExtension(nodeS).Equals(".pdf", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".plt", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".TIF", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".tiff", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".bak", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".jpeg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".jpg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".png", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".db", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".docx", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(nodeS).Equals(".pptx", StringComparison.OrdinalIgnoreCase))
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
			}
			catch (Exception ex) { }
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Template.FindName("lbl", radioButton) is Label label)
			{
				string labelText = label.Content.ToString();
				txtFolder.Text = labelText;
				attributesFiles(labelText);
				addGridAtts();
				ContentPath(nodeS);
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
				using (var fbd = new FolderBrowserDialog())
				{
					DialogResult result = fbd.ShowDialog();
					if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
						parseDir = fbd.SelectedPath;
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

			try
			{
				File.WriteAllText(@"path.txt", parseDir);
			}
			catch (Exception ex)
			{
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
				//MessageBox.Show(ex.Message);
			}

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
				txtSearch.Text = null;
				btnBuscar.IsEnabled = true;
				Mouse.OverrideCursor = null;
			}
		}

		private void SearchBox(string searchItem)
		{
			try
			{
				Mouse.OverrideCursor = Cursors.Wait;
				string directoryPath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/ING/" + SelectedOption + "/";
				//string directoryPath = "//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Diseños/" + SelectedOption + " /";
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void UpdateProgress(int processedItems, int totalItems)
		{
			try
			{
				progressBar.Visibility = Visibility.Visible;
				double progressPercentage = (double)processedItems / totalItems * 100;
				progressBar.Value = progressPercentage;
				progressBar.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
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
				WindowClientsMenu win = new WindowClientsMenu();
				win.Show();
				Close();
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void ExitClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void LogOut(object sender, RoutedEventArgs e)
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

		private void Open_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var p = Path.GetFullPath(nod);
				toolbarOption(p);
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void toolbarOption(string path)
		{
			try
			{
				Debug.WriteLine("ToolBar: " + path);
				if (path == null)
				{
					MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					if (Path.GetExtension(path).Equals(".PDF", StringComparison.OrdinalIgnoreCase))
					{
						try
						{
							edrawingControl.Visibility = Visibility.Hidden;
							imageTIF.Visibility = Visibility.Hidden;
							wb.Visibility = Visibility.Visible;
							var pathPdf = Path.GetFullPath(path);
							Uri pdfUri = new Uri(pathPdf);
							wb.Source = pdfUri;
						}
						catch (Exception ex)
						{
							//MessageBox.Show(ex.Message);
						}
					}
					else if (Path.GetExtension(path).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
					{
						try
						{
							var pathExcel = Path.GetFullPath(path);
							Process.Start(pathExcel);
						}
						catch (Exception ex)
						{
							//MessageBox.Show(ex.Message);
						}

					}
					else if (Path.GetExtension(path).Equals(".SLDPRT", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dxf", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STEP", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STL", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".OBJ", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".SLDASM", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dwg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".stp", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".SLDDRW", StringComparison.OrdinalIgnoreCase))
					{
						try
						{
							wb.Visibility = Visibility.Hidden;
							imageTIF.Visibility = Visibility.Hidden;
							edrawingControl.Visibility = Visibility.Visible;
							eDrawingView = edrawingControl;
							var testModel = Path.GetFullPath(path);
							eDrawingView.EDrawingHost.OpenDoc(testModel, false, false, false);
						}
						catch (Exception ex)
						{
							//	MessageBox.Show(ex.Message);
						}

					}
					else if (Path.GetExtension(path).Equals(".tiff", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".jpg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".jpeg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".tif", StringComparison.OrdinalIgnoreCase))
					{
						try
						{
							wb.Visibility = Visibility.Hidden;
							edrawingControl.Visibility = Visibility.Hidden;
							imageTIF.Visibility = Visibility.Visible;
							Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
							TiffBitmapDecoder decoder = new TiffBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
							BitmapFrame frame = decoder.Frames[0];
							imageTIF.Source = frame;
						}
						catch (Exception ex)
						{
							Debug.WriteLine("Error : " + ex.Message);
							imageTIF.Source = new BitmapImage(new Uri("//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/image/noimage.png"));
						}
					}
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void SaveInfoFile_Click(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void EditInfoFile_Click(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void folder_click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (nod == null)
				{
					MessageBox.Show("Selecciona un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					Process.Start("explorer.exe", Path.GetDirectoryName(nod));
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
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
				//MessageBox.Show(ex.Message);
			}

		}

		private void backTreeView_click(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		public void attributesFiles(string id)
		{
			try
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
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void addGridAtts()
		{
			try
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
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void ContentPathTW(string folderp)
		{
			try
			{
				listFilesNode.Items.Clear();
				string[] directories = Directory.GetDirectories(folderp);
				foreach (string directory in directories)
				{
					listFilesNode.Items.Add("Carpeta: " + Path.GetFileName(directory));
				}
			}
			catch (Exception ex)
			{
				//	MessageBox.Show(ex.Message);
			}

		}

		private void ContentPath(string folderp)
		{
			try
			{
				string parentPath = Directory.GetParent(folderp)?.FullName;
				listFilesNode.Items.Clear();
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
					}
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}

		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			try
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
					else if (Path.GetExtension(path).Equals(".SLDPRT", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dxf", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STEP", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".STL", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".OBJ", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".SLDASM", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".dwg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".stp", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".SLDDRW", StringComparison.OrdinalIgnoreCase))
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
					else if (Path.GetExtension(path).Equals(".tiff", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".jpg", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(path).Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
					{
						try
						{
							wb.Visibility = Visibility.Hidden;
							edrawingControl.Visibility = Visibility.Hidden;
							imageTIF.Visibility = Visibility.Visible;
							imageTIF.Source = new BitmapImage(new Uri(nod));
						}
						catch
						{
							imageTIF.Source = new BitmapImage(new Uri("//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/image/noimage.png"));
						}
					}
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			try
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
					Process.Start("explorer.exe", Path.GetDirectoryName(path));
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

		}

		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
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
			try
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
			catch (Exception ex)
			{
				//	MessageBox.Show(ex.Message);
			}

		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				eDrawingView.Markup.ViewOperator_Set(EMVMarkupOperators.eMVOperatorMeasure);

			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}

		private void twSearched_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			try
			{
				progressB.Value = 0;
				ItemsAtts.Clear();
				listaAtts.Clear();
				listAtts.ItemsSource = null;
				txtP.Text = twSearched.SelectedItem.ToString();
				var textP = txtP.Text;
				string fullF = itemsp.Find(item => item.Contains(txtP.Text));
				string fullP = itemsf.Find(item => item.Contains(txtP.Text));
				if (fullP != null)
				{
					ContentPathTW(fullP);
				}
				else
				{
				}
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
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

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
			try
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
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}

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