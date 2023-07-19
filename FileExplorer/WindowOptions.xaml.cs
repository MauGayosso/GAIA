using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileExplorer
{
    /// <summary>
    /// Lógica de interacción para WindowOptions.xaml
    /// </summary>
    public partial class WindowOptions : Window
    {
		Screen primaryScreen = Screen.PrimaryScreen;

		public WindowOptions()
        {
            InitializeComponent();
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            WindowClientsMenu win = new WindowClientsMenu();
            win.Show();
            Close();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
			int screenW = primaryScreen.Bounds.Width;
			int screenH = primaryScreen.Bounds.Height;
            if(screenW == 1920 & screenH == 1080)
            {
				WindowAdmin win = new WindowAdmin();
				win.Show();
				Close();
			}
			else if (screenW == 1366 & screenH == 768) {
				WindowAdmin2 win = new WindowAdmin2();
				win.Show();
				Close();
			}
			else if (screenW == 1360 & screenH == 768)
			{
				WindowAdmin2 win = new WindowAdmin2();
				win.Show();
				Close();
			}
		}
    }
}
