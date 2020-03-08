using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for ContactPagina.xaml
    /// </summary>
    public partial class ContactPagina : UserControl
    {
        int ContactID;

        public ContactPagina()
        {
            InitializeComponent();
            FillListbox();
        }

        private void FillListbox()
        {
            string constring = "Server=localhost;Database=ixat;Uid=root;";
            string Query = "SELECT * FROM contact;";

            MySqlConnection conDataBase = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand(Query, conDataBase);
            MySqlDataReader myReader;

            try
            {
                conDataBase.Open();
                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    string sName = myReader.GetString("Voornaam");
                    contactlist.Items.Add(sName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Contactlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string nameselected = contactlist.SelectedItem.ToString();
            ContactID = 0;

            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=ixat;Uid=root;Pwd=;");
            
            conn.Open();

            MySqlCommand command2 = conn.CreateCommand();
            command2.CommandText = $"SELECT * FROM `contact` WHERE Voornaam = '{nameselected}';";

            MySqlDataReader reader2 = command2.ExecuteReader();

            DataTable dtData2 = new DataTable();
            dtData2.Load(reader2);

            foreach (DataRow row in dtData2.Rows)
            {
               ContactID += int.Parse(row["ID"].ToString());
            }

            MySqlCommand command = conn.CreateCommand();
            command.CommandText = $"SELECT * FROM `contact` WHERE ID = '{ContactID}';";
            
            MySqlDataReader reader = command.ExecuteReader();
            
            DataTable dtData = new DataTable();
            dtData.Load(reader);
            
            foreach (DataRow row in dtData.Rows)
            {
                boxreset();

                cname.Text += row["Voornaam"].ToString();
                cachternaam.Text += row["Achternaam"].ToString();
                ctel.Text += row["Mobiel"].ToString();
                cmail.Text += row["Email"].ToString();
                cbericht.Text += row["Bericht"].ToString();
            }
            
            conn.Close();
            
            btnDel.IsEnabled = true;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            string nameselected = contactlist.SelectedItem.ToString();
            
            MessageBoxResult result = MessageBox.Show($"Weet u zeker dat je de contactnemende {nameselected} wilt verwijderen?", "Zeker?", MessageBoxButton.YesNo, MessageBoxImage.Hand);

            if (result == MessageBoxResult.Yes)
            {
                MySqlConnection conn = new MySqlConnection("Server=localhost;Database=ixat;Uid=root;Pwd=;");
                
                conn.Open();
                
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = $"DELETE FROM contact WHERE ID = '{ContactID}';";

                MySqlDataReader reader = command.ExecuteReader();
                
                DataTable dtData = new DataTable();
                dtData.Load(reader);
                
                MessageBox.Show($"De contactnemende {nameselected} is verwijderd", "Verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);
                
                FillListbox();
                
                btnDel.IsEnabled = false;
            }
            else
            {
                MessageBox.Show($"De partij {nameselected} is niet verwijderd", "Niet verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void boxreset()
        {
            cname.Text = "";
            cachternaam.Text = "";
            ctel.Text = "";
            cmail.Text = "";
            cbericht.Text = "";
        }
    }
}
