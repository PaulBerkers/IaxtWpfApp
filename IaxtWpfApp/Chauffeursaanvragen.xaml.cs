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
    /// Interaction logic for Chauffeursaanvragen.xaml
    /// </summary>
    /// 

    public partial class Chauffeursaanvragen : UserControl
    {
        private Database database;
        string Latitude;
        string Longitude;
        string username;
        string Rijbewijsnaam;
        int ChauffeurID;
        int RijbewijsID;

        public Chauffeursaanvragen()
        {
            database = new Database();

            InitializeComponent();
            Fillcombo();
            FillcomboRijbewijs();
            
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

        private void reset()
        {
            ChauffeurAanvraagAantalPassagiers.Text = "";
            ChauffeurAanvraagLaadRuimte.Text = "";
            ChauffeurAanvraagEmail.Text = "";
            ChauffeurAanvraagKenteken.Text = "";
            ChauffeurAanvraagMerkAuto.Text = "";
            ChauffeurAanvraagNaam.Text = "";
            ChauffeurAanvraagTypeAuto.Text = "";
            ChauffeurAanvraagVrijeJaren.Text = "";
            ChauffeurAanvraagMobielNummer.Text = "";
        }

        void SetForegroundBlack()
        {
            KiesGebruikersNaam.Foreground = Brushes.Black;
            ChauffeurAanvraagNaam.Foreground = Brushes.Black;
            ChauffeurAanvraagMobielNummer.Foreground = Brushes.Black;
            ChauffeurAanvraagEmail.Foreground = Brushes.Black;
            ChauffeurAanvraagMerkAuto.Foreground = Brushes.Black;
            ChauffeurAanvraagTypeAuto.Foreground = Brushes.Black;
            ChauffeurAanvraagKenteken.Foreground = Brushes.Black;
            ChauffeurAanvraagAantalPassagiers.Foreground = Brushes.Black;
            ChauffeurAanvraagLaadRuimte.Foreground = Brushes.Black;
            ChauffeurAanvraagVrijeJaren.Foreground = Brushes.Black;
        }

        void Fillcombo()
        {
            string constring = "Server=localhost;Database=ixat;Uid=root;";
            string Query = "SELECT * FROM chauffeur_aanvraag;";

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
                    KiesGebruikersNaam.Items.Add(sName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void FillcomboRijbewijs()
        {
            try
            {
                MySqlConnection conDataBase = new MySqlConnection("Server=localhost;Database=ixat;Uid=root;");
                conDataBase.Open();

                MySqlCommand cmdDataBase = conDataBase.CreateCommand();
                cmdDataBase.CommandText = "SELECT * FROM rijbewijs;";

                MySqlDataReader myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    string sRijbewijsnaam = myReader.GetString("Naam");
                    ChauffeurRijbewijs.Items.Add(sRijbewijsnaam);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void KiesGebruikersNaam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            username = KiesGebruikersNaam.SelectedItem.ToString();

            string source = "Server=localhost;Database=ixat;Uid=root;";
            MySqlConnection con = new MySqlConnection(source);

            con.Open();

            MySqlCommand command = con.CreateCommand();
            command.CommandText = $"SELECT * FROM chauffeur_aanvraag inner join klant where klant.Gebruikersnaam = '{username}' && chauffeur_aanvraag.Gebruikersnaam = '{username}';";

            MySqlDataReader reader = command.ExecuteReader();

            DataTable dtData = new DataTable();
            dtData.Load(reader);

            foreach (DataRow row in dtData.Rows)
            {
                ChauffeurAanvraagAantalPassagiers.Text = (row["Aantal_passagiers"].ToString());
                ChauffeurAanvraagLaadRuimte.Text = (row["Laadruimte"].ToString());
                ChauffeurAanvraagEmail.Text = (row["Email"].ToString());
                ChauffeurAanvraagKenteken.Text = (row["Kenteken"].ToString());
                ChauffeurAanvraagMerkAuto.Text = (row["Automerk"].ToString());
                ChauffeurAanvraagNaam.Text = (row["Gebruikersnaam"].ToString());
                ChauffeurAanvraagTypeAuto.Text = (row["Autotype"].ToString());
                ChauffeurAanvraagVrijeJaren.Text = (row["Schadevrije_jaren"].ToString());
                ChauffeurAanvraagMobielNummer.Text = (row["Mobiel"].ToString());
                Latitude = (row["Latitude"].ToString());
                Longitude = (row["Longitude"].ToString());
            }


            MySqlCommand command2 = con.CreateCommand();
            command2.CommandText = $"SELECT * FROM chauffeur ;";

            MySqlDataReader reader2 = command2.ExecuteReader();

            DataTable dtData2 = new DataTable();
            dtData2.Load(reader2);

            foreach (DataRow row in dtData2.Rows)
            {
                ChauffeurID = (int.Parse(row["Id"].ToString()));
            }

            btnAccepteren.IsEnabled = true;
            btnWeigeren.IsEnabled = true;

            con.Close();

            SetForegroundBlack();
        }

        private void btnAccepteren_Click(object sender, RoutedEventArgs e)
        {
            database.AcepteerAanvraag(ChauffeurAanvraagMerkAuto.Text, ChauffeurAanvraagTypeAuto.Text, ChauffeurAanvraagKenteken.Text, ChauffeurAanvraagAantalPassagiers.Text, ChauffeurAanvraagLaadRuimte.Text, ChauffeurAanvraagVrijeJaren.Text, Latitude, Longitude );
            database.DeleteAanvraag(username.ToString());
            database.InsertToRijbewijs(RijbewijsID, username);
            ChauffeurID += 1;
            database.UpdateKlant(ChauffeurID, username.ToString());

            MessageBox.Show("Je hebt de klant " + username + " officeel chauffeur gemaakt", "Gelukt", MessageBoxButton.OK, MessageBoxImage.Information);
            AlertImage.Visibility = Visibility.Visible;
        }

        private void btnWeigeren_Click(object sender, RoutedEventArgs e)
        {
            database.DeleteAanvraag(username.ToString());
            MessageBox.Show("De aanvraag van " + username + " is succesvol verwijderd", "Verwijderd", MessageBoxButton.OK, MessageBoxImage.Error);
            AlertImage.Visibility = Visibility.Visible;
        }

        private void AlertImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            AlertImage.Visibility = Visibility.Hidden;

            Window.GetWindow(this).Close();
        }

        private void ChauffeurRijbewijs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rijbewijsnaam = ChauffeurRijbewijs.SelectedItem.ToString();

            string source = "Server=localhost;Database=ixat;Uid=root;";
            MySqlConnection con = new MySqlConnection(source);

            con.Open();

            MySqlCommand command = con.CreateCommand();
            command.CommandText = $"SELECT * FROM rijbewijs where Naam = '{Rijbewijsnaam}';";

            MySqlDataReader reader = command.ExecuteReader();

            DataTable dtData = new DataTable();
            dtData.Load(reader);

            foreach (DataRow row in dtData.Rows)
            {
                RijbewijsID = (int.Parse(row["Id"].ToString()));
            }

            ChauffeurRijbewijs.Foreground = Brushes.Black;
        }
    }
}