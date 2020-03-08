using Microsoft.Maps.MapControl.WPF;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Maps;

namespace IaxtWpfApp
{
    /// <summary>
    /// Interaction logic for Chauffeursontkoppelen.xaml
    /// </summary>
    public partial class Chauffeursontkoppelen : UserControl
    {
        private Database database;
        private decimal latitude2;
        private decimal longitude2;
        private decimal LatitudeChauffeur;
        private decimal LongitudeChauffeur;
        private string textvalue2 = "";
        string username;
        string ChauffeurUsername;
        int GetChauffeursId;
        Pushpin PushPin = new Pushpin();
        Pushpin pin2 = new Pushpin();



        public Chauffeursontkoppelen()
        {
            database = new Database();
            InitializeComponent();
            Fillcombo();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox temp = sender as TextBox;
            if (temp.Text == temp.Tag.ToString())
            {
                temp.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox temp = sender as TextBox;
            if (temp.Text == "")
            {
                temp.Text = temp.Tag.ToString();
            }
            else if (temp.Text != "")
            {
                temp.Foreground = Brushes.Black;
            }
        }
        
        void Fillcombo()
        {

            try
            {

                MySqlConnection conDataBase = new MySqlConnection("Server=localhost;Database=ixat;Uid=root;");
                conDataBase.Open();

                MySqlCommand cmdDataBase = conDataBase.CreateCommand();
                cmdDataBase.CommandText = "SELECT * FROM Taxi_aanvraag where ChaufferuId > 0 OR Goedgekeurd > 0;";

                MySqlDataReader myReader  = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    string sName = myReader.GetString("gebruikersnaam");
                    OntkoppelAanvraag.Items.Add(sName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reset()
        {
            decimal.TryParse(textvalue2, out latitude2);
            decimal.TryParse(textvalue2, out longitude2);

            decimal.TryParse(textvalue2, out LatitudeChauffeur);
            decimal.TryParse(textvalue2, out LongitudeChauffeur);

            OntkoppelAanvraagGegevensPassagiers.Text = "";
            OntkoppelAanvraagGegevensLaadruimte.Text = "";
            OntkoppelAanvraagGegevensEmail.Text = "";
            OntkoppelAanvraagGegevensDatum.Text = "";
            OntkoppelAanvraagGegevensTijd.Text = "";
            OntkoppelAanvraagGegevensMobiel.Text = "";
            tbOntkoppelChauffeurBox.Text = "";
        }

        private void OntkoppelAanvraag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            username = OntkoppelAanvraag.SelectedItem.ToString();

            reset();

            MyMap2.Children.Remove(PushPin);
            MyMap2.Children.Remove(pin2);

            string source = "Server=localhost;Database=ixat;Uid=root;";
            MySqlConnection con = new MySqlConnection(source);

            con.Open();

            MySqlCommand command = con.CreateCommand();
            command.CommandText = $"SELECT * FROM taxi_aanvraag inner join klant where klant.Gebruikersnaam = '{username}' && taxi_aanvraag.Gebruikersnaam = '{username}';";

            MySqlDataReader reader = command.ExecuteReader();

            DataTable dtData = new DataTable();
            dtData.Load(reader);

            foreach (DataRow row in dtData.Rows)
            {
                OntkoppelAanvraagGegevensPassagiers.Text = (row["Passagiers"].ToString());
                OntkoppelAanvraagGegevensLaadruimte.Text = (row["Minimale_laadruimte"].ToString());
                OntkoppelAanvraagGegevensEmail.Text = (row["Email"].ToString());
                OntkoppelAanvraagGegevensDatum.Text = (row["Datum"].ToString());
                OntkoppelAanvraagGegevensTijd.Text = (row["Tijd"].ToString());
                OntkoppelAanvraagGegevensMobiel.Text = (row["Mobiel"].ToString());
                // The pushpin to add to the map.

                PushPin.Location = new Location(double.Parse(row["Latitude"].ToString()), double.Parse(row["Longitude"].ToString()));
                PushPin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 0));

                // Adds the pushpin to the map.
                MyMap2.Children.Add(PushPin);
            }


            MySqlCommand command2 = con.CreateCommand();
            command2.CommandText = $"SELECT * FROM `klant` inner join taxi_aanvraag WHERE taxi_aanvraag.ChaufferuId = klant.ChauffeurId and taxi_aanvraag.Gebruikersnaam = '{username}';";

            MySqlDataReader reader2 = command2.ExecuteReader();

            DataTable dtData2 = new DataTable();
            dtData2.Load(reader2);

            ChauffeurUsername = "";

            foreach (DataRow row in dtData2.Rows)
            {
                ChauffeurUsername += (row["Gebruikersnaam"].ToString());
                GetChauffeursId += (int.Parse(row["ChauffeurId"].ToString()));
                tbOntkoppelChauffeurBox.Text = ChauffeurUsername;
            }
            
            MySqlCommand command3 = con.CreateCommand();
            command3.CommandText = $"SELECT * FROM CHAUFFEUR where ID = '{GetChauffeursId}';";

            MySqlDataReader reader3 = command3.ExecuteReader();

            DataTable dtData3 = new DataTable();
            dtData3.Load(reader3);

            foreach (DataRow row in dtData3.Rows)
            {
                pin2.Location = new Location(double.Parse(row["Latitude"].ToString()), double.Parse(row["Longitude"].ToString()));
                pin2.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                MyMap2.Children.Add(pin2);
            }

            con.Close();

            SetForegroundBlack();
        }

        void SetForegroundBlack()
        {
            OntkoppelAanvraagGegevensPassagiers.Foreground = Brushes.Black;
            OntkoppelAanvraagGegevensLaadruimte.Foreground = Brushes.Black;
            OntkoppelAanvraagGegevensEmail.Foreground = Brushes.Black;
            OntkoppelAanvraagGegevensDatum.Foreground = Brushes.Black;
            OntkoppelAanvraagGegevensTijd.Foreground = Brushes.Black;
            OntkoppelAanvraagGegevensMobiel.Foreground = Brushes.Black;
            OntkoppelAanvraag.Foreground = Brushes.Black;
            tbOntkoppelChauffeurBox.Foreground = Brushes.Black;
        }

        private void btnOntkoppelChauffeur_Click(object sender, RoutedEventArgs e)
        {
            database.OntkoppelTaxiAanvraag(username);
            database.UpdateChauffeurVrijOntkoppel(GetChauffeursId);
            MessageBox.Show("De aanvraag van " + username + " is succesvol ontkoppeld met " + ChauffeurUsername, "Ontkoppeld!", MessageBoxButton.OK, MessageBoxImage.Information);
            AlertImage.Visibility = Visibility.Visible;
        }


        private void AlertImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            AlertImage.Visibility = Visibility.Hidden;

            Window.GetWindow(this).Close();
        }
    }
}
