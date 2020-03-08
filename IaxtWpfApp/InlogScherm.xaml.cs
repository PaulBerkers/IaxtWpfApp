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
using System.Windows.Shapes;

namespace IaxtWpfApp
{
    /// <summary>
    /// Interaction logic for InlogScherm.xaml
    /// </summary>
    public partial class InlogScherm : Window
    {
        private Database database;

        public InlogScherm()
        {
            InitializeComponent();
            database = new Database();
        }

        private void btnInloggen_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text != "" && pbPassword.Password != "")
            {
                if (database.CheckLogin(tbName.Text, pbPassword.Password) == true)
                {
                    MainWindow mwWindow = new MainWindow();
                    mwWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Deze gegevens zijn fout!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Zorg dat je beide velden ingevuld hebt! Probeer het opnieuw", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
