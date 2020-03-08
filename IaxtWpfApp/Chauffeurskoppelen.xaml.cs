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

namespace IaxtWpfApp
{
    /// <summary>
    /// Interaction logic for Chauffeurskoppelen.xaml
    /// </summary>
    public partial class Chauffeurskoppelen : UserControl
    {
        string ChauffeurUsername;
        private Database database;
        private decimal latitude;
        private decimal longitude;
        private decimal LatitudeChauffeur;
        private decimal LongitudeChauffeur;
        int GetChauffeursId;
        bool setButtonTrue = false;
        bool setButtonTrue2 = false;
        int ChauffeurId;
        string username;
        private string textvalue = "";
        Pushpin pin = new Pushpin();
        Pushpin pin2 = new Pushpin();

        public Chauffeurskoppelen()
        {
            database = new Database();
            InitializeComponent();
            Fillcombo();
            Fillcombo2();
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
            string constring = "Server=localhost;Database=ixat;Uid=root;";
            string Query = "SELECT * FROM Taxi_aanvraag where ChaufferuId = 0 OR Goedgekeurd = 0;";

            MySqlConnection conDataBase = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand(Query, conDataBase);
            MySqlDataReader myReader;

            try
            {
                conDataBase.Open();
                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    string sName = myReader.GetString("gebruikersnaam");
                    SelecteerAanvraag.Items.Add(sName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void reset()
        { 
            decimal.TryParse(textvalue, out latitude);
            decimal.TryParse(textvalue, out longitude);

            MyMap.Children.Remove(pin);
            AanvraagGegevensPassagiers.Text = "";
            AanvraagGegevensLaadruimte.Text = "";
            AanvraagGegevensEmail.Text = "";
            AanvraagGegevensDatum.Text = "";
            AanvraagGegevensTijd.Text = "";
            AanvraagGegevensTelefoon.Text = "";
        }

        private void SelecteerAanvraag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            username = SelecteerAanvraag.SelectedItem.ToString();

            reset();

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
                AanvraagGegevensPassagiers.Text = (row["Passagiers"].ToString());
                AanvraagGegevensLaadruimte.Text = (row["Minimale_laadruimte"].ToString());
                AanvraagGegevensEmail.Text = (row["Email"].ToString());
                AanvraagGegevensDatum.Text = (row["Datum"].ToString());
                AanvraagGegevensTijd.Text = (row["Tijd"].ToString());
                AanvraagGegevensTelefoon.Text = (row["Mobiel"].ToString());
                latitude += (decimal.Parse(row["Latitude"].ToString()));
                longitude += (decimal.Parse(row["Longitude"].ToString()));
            }


            // The pushpin to add to the map

            pin.Location = new Location(double.Parse(latitude.ToString()), double.Parse(longitude.ToString()));
            pin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 0));
            

            // Adds the pushpin to the map.
            MyMap.Children.Add(pin);
            
            con.Close();

            SetForegroundBlack();

            setButtonTrue = true;
        }


        void Fillcombo2()
        { 
            string source = "Server=localhost;Database=ixat;Uid=root;";
            MySqlConnection con = new MySqlConnection(source);

            con.Open();

            MySqlCommand command2 = con.CreateCommand();
            command2.CommandText = $"SELECT * FROM chauffeur where Vrij <> 1;";

            MySqlDataReader reader2 = command2.ExecuteReader();

            DataTable dtData2 = new DataTable();
            dtData2.Load(reader2);

            foreach (DataRow row in dtData2.Rows)
            {
                ChauffeurId = (int.Parse(row["Id"].ToString()));

                string constring = "Server=localhost;Database=ixat;Uid=root;";
                string Query = $"SELECT * FROM klant inner join chauffeur where klant.ChauffeurId = '{ChauffeurId}' && Chauffeur.Id = '{ChauffeurId}';";

                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDataBase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    myReader = cmdDataBase.ExecuteReader();

                    while (myReader.Read())
                    {
                        string sName = myReader.GetString("gebruikersnaam");
                        SelecteerEenChauffeur.Items.Add(sName);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            con.Close();
        }
        
        private void SelecteerEenChauffeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelecteerEenChauffeur.Foreground = Brushes.Black;
            ChauffeurUsername = SelecteerEenChauffeur.SelectedItem.ToString();
            GetChauffeursId = 0;
            
            decimal.TryParse(textvalue, out LatitudeChauffeur);
            decimal.TryParse(textvalue, out LongitudeChauffeur);

            MyMap.Children.Remove(pin2);

            string source = "Server=localhost;Database=ixat;Uid=root;";
            MySqlConnection con = new MySqlConnection(source);

            con.Open();

            MySqlCommand command2 = con.CreateCommand();
            command2.CommandText = $"SELECT * from klant where Gebruikersnaam = '{ChauffeurUsername}' ;";

            MySqlDataReader reader2 = command2.ExecuteReader();

            DataTable dtData2 = new DataTable();
            dtData2.Load(reader2);

            foreach (DataRow row in dtData2.Rows)
            {
                GetChauffeursId += (int.Parse(row["ChauffeurId"].ToString()));
            }

            MySqlCommand command = con.CreateCommand();
            command.CommandText = $"SELECT * FROM Chauffeur where id = '{GetChauffeursId}' ;";

            MySqlDataReader reader = command.ExecuteReader();

            DataTable dtData = new DataTable();
            dtData.Load(reader);

            foreach (DataRow row in dtData.Rows)
            {
                LatitudeChauffeur += (decimal.Parse(row["Latitude"].ToString()));
                LongitudeChauffeur += (decimal.Parse(row["Longitude"].ToString()));
            }

            // The pushpin to add to the map.
            pin2.Location = new Location(double.Parse(LatitudeChauffeur.ToString()), double.Parse(LongitudeChauffeur.ToString()));
            pin2.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            // Adds the pushpin to the map.
            MyMap.Children.Add(pin2);

            con.Close();

            setButtonTrue2 = true;
        }

        void SetForegroundBlack()
        {
            SelecteerAanvraag.Foreground = Brushes.Black;
            AanvraagGegevensPassagiers.Foreground = Brushes.Black;
            AanvraagGegevensLaadruimte.Foreground = Brushes.Black;
            AanvraagGegevensEmail.Foreground = Brushes.Black;
            AanvraagGegevensDatum.Foreground = Brushes.Black;
            AanvraagGegevensTijd.Foreground = Brushes.Black;
            AanvraagGegevensTelefoon.Foreground = Brushes.Black;
        }

        private void KoppelChauffeur_Click(object sender, RoutedEventArgs e)
        {
            if (setButtonTrue == true && setButtonTrue2 == true)
            {
                database.UpdateTaxiAanvraag(GetChauffeursId, username);
                MessageBox.Show("De aanvraag van " + username + " is gekoppeld met " + ChauffeurUsername, "Gekoppeld!", MessageBoxButton.OK, MessageBoxImage.Information);
                AlertImage.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Zorg dat je een klant en chauffeur hebt gekozen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
