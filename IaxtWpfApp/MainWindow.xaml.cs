using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IaxtWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UserControl usc = new HomeWindow();
            GridMain.Children.Add(usc);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "HomeItem":
                    usc = new HomeWindow();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemAanvragen":
                    usc = new Chauffeursaanvragen();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemKoppelen":
                    usc = new Chauffeurskoppelen();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemOntkoppelen":
                    usc = new Chauffeursontkoppelen();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemContact":
                    usc = new ContactPagina();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
