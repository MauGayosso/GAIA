using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowAtts.xaml
	/// </summary>
	public partial class WindowAtts : Window
	{
		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";

		public string pathPass;
		public WindowAtts()
		{
			InitializeComponent();
			loadName(pathPass);
		}

		public void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(txtF.Text) && !String.IsNullOrEmpty(txtParte.Text) && !String.IsNullOrEmpty(txtRev.Text) && !String.IsNullOrEmpty(cmbMP.Text) || !String.IsNullOrEmpty(txtLote.Text)
				&& !String.IsNullOrEmpty(txtProyecto.Text) && !String.IsNullOrEmpty(cmbAS.Text) && !String.IsNullOrEmpty(dpPick.ToString()) && !String.IsNullOrEmpty(cmbAcero.Text))
			{
				save.IsEnabled = true;
				addDat();
			}
			else
			{
				save.IsEnabled = false;
				MessageBox.Show("Campos vacios","Error");
			}
		}

		public void loadName(string name)
		{
			txtF.Text = name;
		}
		public void LoadImage(string PathI)
		{
			if (!string.IsNullOrEmpty(PathI))
			{
				//displayImage.Source = new BitmapImage(new Uri(PathI));
				displayImage.Source = new BitmapImage(new Uri("//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/image/" + PathI));
			}
		}
		public void addDat()
		{
			string id = txtF.Text;
			string n_parte = txtParte.Text;
			string revi = txtRev.Text;
			string matp = cmbMP.Text;
			string lot = txtLote.Text;
			string project = txtProyecto.Text;
			string acabado = cmbAS.Text;
			string fecha = dpPick.ToString();
			string tipoA = cmbAcero.Text;
			bool corte = (bool)chkCorte.IsChecked;
			bool doblez = (bool)chkDoblez.IsChecked;
			bool maquinado = (bool)chkMaquinado.IsChecked;
			bool pintura = (bool)chkPintura.IsChecked;
			bool detallado = (bool)chkDetallado.IsChecked;
			bool soldadura = (bool)chkSoldadura.IsChecked;
			bool articulo = (bool)chkArticulo.IsChecked;
			bool estrutura = (bool)chkEstructura.IsChecked;
			bool proceso = (bool)chkProcesos.IsChecked;
			bool publi = (bool)chkPublicacion.IsChecked;
			bool modelado = (bool)chkModelado.IsChecked;
			string query =
				"INSERT INTO atts (Id_file, no_parte, revision, materia_prima, lote, proyecto, acabado_superficial, fecha_ed, tipo_acero, p_corte, p_doblez, p_maquinado, p_pintura, p_detallado, p_soldadura, creacion_articulo, estructura_product, desc_proceso, modelado, publicacion) " +
				"VALUES (@Id_files, @no_partes, @revisions, @materia_primas, @lotes, @proyectos, @acabado_superficials, @fecha_eds, " +
				"@tipo_aceros, @p_cortes, @p_doblezs, @p_maquinados, @p_pinturas, @p_detallados, @p_soldaduras, @creacion_articulos, " +
				"@estructura_products, @desc_procesos, @modelados, @publicacions)";

			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				connection.Open();

				using (OleDbCommand command = new OleDbCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Id_files", id);
					command.Parameters.AddWithValue("@no_partes", n_parte);
					command.Parameters.AddWithValue("@revisions", revi);
					command.Parameters.AddWithValue("@materia_primas", matp);
					command.Parameters.AddWithValue("@lotes", lot);
					command.Parameters.AddWithValue("@proyectos", project);
					command.Parameters.AddWithValue("@acabado_superficials", acabado);
					command.Parameters.AddWithValue("@fecha_eds", fecha);
					command.Parameters.AddWithValue("@tipo_aceros", tipoA);
					command.Parameters.AddWithValue("@p_cortes", corte);
					command.Parameters.AddWithValue("@p_doblezs", doblez);
					command.Parameters.AddWithValue("@p_maquinados", maquinado);
					command.Parameters.AddWithValue("@p_pinturas", pintura);
					command.Parameters.AddWithValue("@p_detallados", detallado);
					command.Parameters.AddWithValue("@p_soldaduras", soldadura);
					command.Parameters.AddWithValue("@creacion_articulos", articulo);
					command.Parameters.AddWithValue("@estructura_products", estrutura);
					command.Parameters.AddWithValue("@desc_procesos", proceso);
					command.Parameters.AddWithValue("@modelados", modelado);
					command.Parameters.AddWithValue("@publicacions", publi);
					// Execute the query
					command.ExecuteNonQuery();

					MessageBox.Show("Se agrego correctamente la informacion");
				}
			}
		}

		private void chkArticulo_Checked(object sender, RoutedEventArgs e)
		{
			if (chkArticulo.IsChecked == true)
			{
				chkEstructura.IsEnabled = true;
				progressB.Value += 20;
			}
		}

		private void chkArticulo_Unchecked(object sender, RoutedEventArgs e)
		{
			if (chkArticulo.IsChecked == false)
			{
				chkEstructura.IsEnabled = false;
				chkEstructura.IsChecked = false;
				chkProcesos.IsEnabled = false;
				chkProcesos.IsChecked = false;
				chkModelado.IsEnabled = false;
				chkModelado.IsChecked = false;
				chkPublicacion.IsChecked = false;
				chkPublicacion.IsEnabled = false;
				progressB.Value -= progressB.Maximum;
			}
		}

		private void chkEstructura_Checked(object sender, RoutedEventArgs e)
		{
			if (chkArticulo.IsChecked == true)
			{
				chkProcesos.IsEnabled = true;
				progressB.Value += 20;
			}
		}

		private void chkEstructura_Unchecked(object sender, RoutedEventArgs e)
		{
			if (chkEstructura.IsChecked == false)
			{
				chkProcesos.IsEnabled = false;
				chkProcesos.IsChecked = false;
				chkModelado.IsEnabled = false;
				chkModelado.IsChecked = false;
				chkPublicacion.IsChecked = false;
				chkPublicacion.IsEnabled = false;
				progressB.Value = 40;
			}
		}

		private void chkProcesos_Checked(object sender, RoutedEventArgs e)
		{
			if (chkProcesos.IsChecked == true)
			{
				chkModelado.IsEnabled = true;
				progressB.Value += 20;
			}
		}

		private void chkProcesos_Unchecked(object sender, RoutedEventArgs e)
		{
			if (chkProcesos.IsChecked == false)
			{
				chkModelado.IsEnabled = false;
				chkModelado.IsChecked = false;
				chkPublicacion.IsChecked = false;
				chkPublicacion.IsEnabled = false;
				progressB.Value = 60;
			}
		}

		private void chkModelado_Checked(object sender, RoutedEventArgs e)
		{
			if (chkModelado.IsChecked == true)
			{
				chkPublicacion.IsEnabled = true;
				progressB.Value += 20;
			}
		}

		private void chkModelado_Unchecked(object sender, RoutedEventArgs e)
		{
			if (chkModelado.IsChecked == false)
			{
				chkPublicacion.IsChecked = false;
				chkPublicacion.IsEnabled = false;
				progressB.Value = 80;
			}
		}

		private void chkPublicacion_Checked(object sender, RoutedEventArgs e)
		{
			if (chkPublicacion.IsChecked == true)
			{
				progressB.Value += 20;
			}
		}

		private void chkPublicacion_Unchecked(object sender, RoutedEventArgs e)
		{
			if (chkPublicacion.IsChecked == false)
			{
				progressB.Value -= 20;
			}
		}
	}
}
