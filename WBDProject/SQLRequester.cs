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
            "Server = .\\SQLEXPRESS;Database=LogDB;Integrated Security=true";

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

        public static void AddPreference(ref DataGridView grid, string standard, string all_inclusive, string people_in_room, 
            string vehicle_type, string country)
        {
            if (all_inclusive == "yes")
                all_inclusive = "T";
            else if (all_inclusive == "no")
                all_inclusive = "N";

            string text =
                $"SELECT * FROM WBDProject.dbo.Excursion WHERE Standard = \'{standard}\' AND All_inclusive = \'{all_inclusive}\' AND " +
                $"People_in_room = \'{people_in_room}\' AND Vehicle_Type = \'{vehicle_type}\' AND UPPER(Country) = " +
                $"\'{country.ToUpper()}\';";

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
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error!");
            }
        }

        public static List<string> getAllClientsNames(ref ComboBox comboBox)
        {
            SqlConnection conn;
            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Customer_ID, First_Name, Last_Name FROM WBDProject.dbo.Customer;", conn);
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
                    while(DR.Read())//for (int i = 0; i < DR.FieldCount; i++)
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

            return null;
        }
    }
}
