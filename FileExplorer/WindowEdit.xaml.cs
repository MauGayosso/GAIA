using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FileExplorer
{
	/// <summary>
	/// Lógica de interacción para WindowEdit.xaml
	/// </summary>
	public partial class WindowEdit : Window
	{

		//string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/attFiles.accdb;";
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/attFiles.accdb;";
		List<string> listaAtts = new List<string>();
		public string pathPass;
		public string PathsI;
		//
		public void LoadImage(string PathI)
		{
			try
			{
				displayImage.Source = new BitmapImage(new Uri("//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/image/" + PathI));

			}
			catch
			{
				displayImage.Source = new BitmapImage(new Uri("//servidorhp/Users/SGC/Documents/RED GENERAL MI/INGENIERÍA/Registros/GAIA/image/noimage.png"));
			}
		}
		public WindowEdit()
		{
			InitializeComponent();
		}

		public void Button_Click(object sender, RoutedEventArgs e)
		{
			editDat(pathPass);
		}
		public void loadName(string name)
		{
			atts(name);
			if (listaAtts.Count < 19)
			{
			}
			else
			{
				txtF.Text = listaAtts[0];
				txtParte.Text = listaAtts[1];
				txtRev.Text = listaAtts[2];
				cmbMP.Text = listaAtts[3];
				txtLote.Text = listaAtts[4];
				txtProyecto.Text = listaAtts[5];
				cmbAS.Text = listaAtts[6];
				dpPick.Text = listaAtts[7];
				cmbAcero.SelectedValue = listaAtts[8];
				chkCorte.IsChecked = bool.Parse(listaAtts[9]);
				chkDoblez.IsChecked = bool.Parse(listaAtts[10]);
				chkMaquinado.IsChecked = bool.Parse(listaAtts[11]);
				chkPintura.IsChecked = bool.Parse(listaAtts[12]);
				chkDetallado.IsChecked = bool.Parse(listaAtts[13]);
				chkSoldadura.IsChecked = bool.Parse(listaAtts[14]);
				chkArticulo.IsChecked = bool.Parse(listaAtts[15]);
				chkEstructura.IsChecked = bool.Parse(listaAtts[16]);
				chkProcesos.IsChecked = bool.Parse(listaAtts[17]);
				chkPublicacion.IsChecked = bool.Parse(listaAtts[18]);
				chkModelado.IsChecked = bool.Parse(listaAtts[19]);
			}

		}
		public void atts(string id)
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
		public void editDat(string path)
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
			string query = "UPDATE atts SET no_parte = ?, revision = ?, materia_prima = ?, lote = ?, proyecto = ?, acabado_superficial = ?, fecha_ed = ?, tipo_acero = ?, p_corte = ?, p_doblez = ?, p_maquinado = ?, p_pintura = ?, p_detallado = ?, p_soldadura = ?, creacion_articulo = ?, estructura_product = ?, desc_proceso = ?, modelado = ?, publicacion = ? WHERE Id_file = ?";

			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				connection.Open();

				using (OleDbCommand command = new OleDbCommand(query, connection))
				{
					command.Parameters.AddWithValue("@no_parte", n_parte);
					command.Parameters.AddWithValue("@revision", revi);
					command.Parameters.AddWithValue("@materia_prima", matp);
					command.Parameters.AddWithValue("@lote", lot);
					command.Parameters.AddWithValue("@proyecto", project);
					command.Parameters.AddWithValue("@acabado_superficial", acabado);
					command.Parameters.AddWithValue("@fecha_ed", fecha);
					command.Parameters.AddWithValue("@tipo_acero", tipoA);
					command.Parameters.AddWithValue("@p_corte", corte);
					command.Parameters.AddWithValue("@p_doblez", doblez);
					command.Parameters.AddWithValue("@p_maquinado", maquinado);
					command.Parameters.AddWithValue("@p_pintura", pintura);
					command.Parameters.AddWithValue("@p_detallado", detallado);
					command.Parameters.AddWithValue("@p_soldadura", soldadura);
					command.Parameters.AddWithValue("@creacion_articulo", articulo);
					command.Parameters.AddWithValue("@estructura_product", estrutura);
					command.Parameters.AddWithValue("@desc_proceso", proceso);
					command.Parameters.AddWithValue("@modelado", modelado);
					command.Parameters.AddWithValue("@publicacion", publi);
					command.Parameters.AddWithValue("@Id_fileValue", id);

					int rows = command.ExecuteNonQuery();
					if (rows > 0)
					{
						MessageBox.Show("Actualizado correctamente", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
					}
					else
					{
						MessageBox.Show("Error al actualizar valor", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
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
