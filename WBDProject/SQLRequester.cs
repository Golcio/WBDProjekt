using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WBDProject
{
    /// <summary>
    /// A class to send and receive messages from database
    /// </summary>
    public class SQLRequester
    {
        private static string connectionString =
            "Data Source=GOLTER;Initial Catalog=LogDB;Integrated Security=True";

        private void SQLRequest(string text, ref ComboBox comboBox)
        {
            SqlConnection conn;
            //string connectionString = "Data Source=DESKTOP-N1BL4OR;Initial Catalog=WBDProject;Integrated Security=True";
            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(text, conn);

            try
            {
                conn.Open();
                SqlDataReader DR = cmd.ExecuteReader();
                BindingSource source = new BindingSource();
                source.DataSource = DR;
                comboBox.Items.Clear();
                if (DR.HasRows)
                {
                    comboBox.DataSource = source;
                }
                else
                {
                    MessageBox.Show("No clients in the database!");
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error!");
            }
        }

        public static void ShowAllExcursions(ref DataGridView grid)
        {
            var conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Excursion;", conn);

            try
            {
                conn.Open();
                SqlDataReader DR = cmd.ExecuteReader();
                BindingSource source = new BindingSource();
                source.DataSource = DR;
                grid.Rows.Clear();
                grid.Columns.Clear();
                if (DR.HasRows)
                {
                    grid.DataSource = source;
                }
                else
                {
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error!");
            }
        }

        public static void FindExcursion(ref DataGridView grid, string standard, string all_inclusive, string people_in_room,
            string vehicle_type)
        {
            if (all_inclusive == "yes")
                all_inclusive = "Tak";
            else if (all_inclusive == "no")
                all_inclusive = "Nie";

            StringBuilder textBuilder = new StringBuilder(
                $"SELECT Excursion_Name, Description, Beggining_time, End_time, Standard, All_inclusive, Price, " +
                $"Excursion_type FROM Excursion ");

            if (string.IsNullOrEmpty(people_in_room) &&
                string.IsNullOrEmpty(standard) &&
                string.IsNullOrEmpty(all_inclusive) &&
                string.IsNullOrEmpty(vehicle_type)) 
            {
            }
            else
            {
                textBuilder.Append("WHERE 1=1 ");

                if (!string.IsNullOrEmpty(people_in_room))
                {
                    textBuilder.Append("AND Excursion_ID IN " +
                                       $"(SELECT Excursion_ID FROM Room_Rent " +
                                       "WHERE Room_ID IN (SELECT Room_ID FROM Room " +
                                       $"WHERE People_in_room = {people_in_room})) ");
                }
                if (!string.IsNullOrEmpty(standard))
                {
                    textBuilder.Append($"AND Standard = '{standard}' ");
                }
                if (!string.IsNullOrEmpty(all_inclusive))
                {
                    textBuilder.Append($"AND All_inclusive = '{all_inclusive}' ");
                }
                if (!string.IsNullOrEmpty(vehicle_type))
                {
                    textBuilder.Append("AND Excursion_ID IN (SELECT Excursion_ID FROM Vehicle_Rent " +
                                       "WHERE Vehicle_ID IN (SELECT Vehicle_ID FROM Vehicle " +
                                       $"WHERE Vehicle_Type = '{vehicle_type}')) ");
                }
            }

            textBuilder.Append(";");
            string text = textBuilder.ToString();

            var conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(text, conn);

            try
            {
                conn.Open();
                SqlDataReader DR = cmd.ExecuteReader();
                BindingSource source = new BindingSource();
                source.DataSource = DR;
                grid.Rows.Clear();
                grid.Columns.Clear();
                if (DR.HasRows)
                {
                    grid.DataSource = source;
                }
                else
                {
                    MessageBox.Show("Nothing found!");
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Query: " + text + " " + ee.ToString(), "Error!");
            }
        }

        public static void AddPreference(ref DataGridView grid, string standard, string all_inclusive,
            string people_in_room,
            string vehicle_type, string ClientID)
        {
            all_inclusive = all_inclusive == "yes" ? "Tak" : "Nie";
            StringBuilder insertBuilder = new StringBuilder($"INSERT INTO Preference (Pref_ID, " +
                            $"Standard, All_inclusive, People_in_room, Vehicle_Type, " +
                            $"Customer_ID) VALUES ( (SELECT COUNT(Pref_ID) FROM " +
                            $"Preference) + 1, ");
            insertBuilder.Append(string.IsNullOrEmpty(standard) ? "NULL, " : $"\'{standard}\', ");
            insertBuilder.Append(string.IsNullOrEmpty(all_inclusive) ? "NULL, " : $"\'{all_inclusive}\', ");
            insertBuilder.Append(string.IsNullOrEmpty(people_in_room) ? "NULL, " : $"\'{people_in_room}\', ");
            insertBuilder.Append(string.IsNullOrEmpty(vehicle_type) ? "NULL, " : $"\'{vehicle_type}\', ");
            insertBuilder.Append(string.IsNullOrEmpty(ClientID) ? "1, " : $"\'{ClientID}\'");
            insertBuilder.Append(");");

            string insert = insertBuilder.ToString();

            var conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(insert, conn);

            try
            {
                conn.Open();
                cmd.ExecuteReader();
                //Aktualizacja preferencji w DataGridView
                FindPreferences(ref grid, ClientID);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Query: \n\n" + insert + "\n\n" + ee.ToString(), "Error!");
            }
        }

        public static void FindPreferences(ref DataGridView grid, string ClientID)
        {
            var conn = new SqlConnection(connectionString);
            string query = $"SELECT Pref_ID, Standard, All_inclusive, People_in_room, Vehicle_Type, Customer_ID FROM Preference WHERE Customer_ID = {ClientID};";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader DR = cmd.ExecuteReader();
                BindingSource source = new BindingSource();
                source.DataSource = DR;
                grid.Rows.Clear();
                grid.Columns.Clear();
                if (DR.HasRows)
                {
                    grid.DataSource = source;
                }
                else
                {
                    MessageBox.Show("Found nothing!");
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Query: \n\n" + query + "\n\n" + ee.ToString(), "Error!");
            }
        }

        public static void getAllClientsNames(ref ComboBox comboBox)
        {
            SqlConnection conn;
            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Customer_ID, First_Name, Last_Name FROM Customer;", conn);
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();
                SqlDataReader DR = cmd.ExecuteReader();
                //BindingSource source = new BindingSource();
                //source.DataSource = DR;
                comboBox.Items.Clear();
                if (DR.HasRows)
                {
                    comboBox.Items.Clear();
                    while (DR.Read())//for (int i = 0; i < DR.FieldCount; i++)
                    {
                        comboBox.Items.Add(DR.GetInt32(0) + ". " + DR.GetString(1) + " " + DR.GetString(2));
                    }
                }
                else
                {
                    MessageBox.Show("No clients in the database!");
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error!");
            }
        }
    }
}
