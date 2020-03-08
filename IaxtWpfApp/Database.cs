using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IaxtWpfApp
{
  

    public class Database
    {
        private MySqlConnection connection;

        public Database()
        {
            connection = new MySqlConnection("Server=localhost;Database=ixat;Uid=root;");
        }

        public bool CheckLogin(string name, string password)
        {
            bool bResult = false;

            try
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM beheerwpf where Gebruikersnaam = @naam AND Wachtwoord = @password";
                command.Parameters.AddWithValue("naam", name);
                command.Parameters.AddWithValue("password", password);

                MySqlDataReader reader = command.ExecuteReader();

                DataTable result = new DataTable();
                result.Load(reader);

                if (result.Rows.Count > 0)
                {
                    bResult = true;
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return bResult;

        }

        public bool CheckAlreadyAssigned()
        {
            bool bResult = false;

            try
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Taxi_aanvraag where ChaufferuId = 0 OR Goedgekeurd = 0;";
                MySqlDataReader reader = command.ExecuteReader();

                DataTable result = new DataTable();
                result.Load(reader);

                if (result.Rows.Count > 0)
                {
                    bResult = true;
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return bResult;

        }

        public void AcepteerAanvraag(string AutoMerk, string AutoType, string Kenteken, string Aantal_passagiers, string Laadruimte, string Schadevrije_jaren, string Latitude, string Longitude )
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO chauffeur (Automerk, Autotype, Kenteken, Aantal_passagiers, Laadruimte, Schadevrije_jaren, Latitude, Longitude, Vrij) VALUES (@Automerk, @Autotype, @Kenteken, @Aantal_passagiers, @Laadruimte, @Schadevrije_jaren, @Latitude, @Longitude, 0); ";
            command.Parameters.AddWithValue("Automerk", AutoMerk);
            command.Parameters.AddWithValue("Autotype", AutoType);
            command.Parameters.AddWithValue("Kenteken", Kenteken);
            command.Parameters.AddWithValue("Aantal_passagiers", Aantal_passagiers);
            command.Parameters.AddWithValue("Laadruimte", Laadruimte);
            command.Parameters.AddWithValue("Schadevrije_jaren", Schadevrije_jaren);
            command.Parameters.AddWithValue("Latitude", Latitude);
            command.Parameters.AddWithValue("Longitude", Longitude);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateKlant(int id, string name)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "UPDATE Klant SET ChauffeurId = @id WHERE Gebruikersnaam = @Gebruikersnaam; ";
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("Gebruikersnaam", name);
            
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void DeleteAanvraag(string Gebruikersnaam)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Chauffeur_aanvraag WHERE Gebruikersnaam = @id; ";
            command.Parameters.AddWithValue("id", Gebruikersnaam);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateTaxiAanvraag(int id, string name)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "UPDATE Taxi_aanvraag SET ChaufferuId = @id WHERE Gebruikersnaam = @Gebruikersnaam; ";
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("Gebruikersnaam", name);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void OntkoppelTaxiAanvraag(string name)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "DELETE FROM `taxi_aanvraag` WHERE `taxi_aanvraag`.`Gebruikersnaam` = @Gebruikersnaam;";
            command.Parameters.AddWithValue("Gebruikersnaam", name);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateChauffeurVrijOntkoppel(int id)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "UPDATE Chauffeur SET Vrij = 0 where Id = @id";
            command.Parameters.AddWithValue("id", id);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void InsertToRijbewijs(int RijbewijsID, string ChauffeurName)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO chauffeur_rijbewijs (Gebruikersnaam, RijbewijsId) VALUES (@Chauffeur, @RijbewijsID); ";
            command.Parameters.AddWithValue("Chauffeur", ChauffeurName);
            command.Parameters.AddWithValue("RijbewijsID", RijbewijsID);

            command.ExecuteNonQuery();

            connection.Close();
        }

    }
}
