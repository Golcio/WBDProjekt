using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WBDProject
{
    public partial class Form1 : Form
    {
        string table = null;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;
            SQLRequest("SELECT name FROM Sys.Tables", dataGridView2);
            dataGridView2.Columns[0].Width = 190;
            dataGridView2.Columns[0].HeaderText = "Nazwa Tabeli";
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToResizeRows = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SQLRequest("SELECT name FROM Sys.Tables", dataGridView2);
            dataGridView2.Columns[0].Width = 190;
            dataGridView2.Columns[0].HeaderText = "Nazwa Tabeli";
            SQLRequest("select * from " + table, dataGridView1);

        }


        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                table = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                SQLRequest("select * from " + table, dataGridView1);
            }

        }

        delegate void StringArgReturningVoidDelegate(string text);
        private void SetText(string text)
        {

            if (this.listBox1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listBox1.Items.Add(text);
            }
        }

        private void SQLRequestWithoutDisplay(string text)
        {
            SqlConnection conn;
            string connectionString = "Data Source=GOLTER;Initial Catalog=LogDB;Integrated Security=True";
            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(text, conn);
            SqlDataReader DR = null;

            try
            {
                conn.Open();
                DR = cmd.ExecuteReader();
                BindingSource source = new BindingSource();
                source.DataSource = DR;
                conn.Close();
            }
            catch (Exception e)
            {
                SetText("An Error occured");
                MessageBox.Show(e.ToString(), "Error!");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            string filtrecolumn = textBox1.Text;
            string filtrevalue = textBox2.Text;

            SQLRequest("select * from " + table + " where " + filtrecolumn + "= '" + filtrevalue + "'", dataGridView1);
        }

        private void DataGridViewError(DataGridView grid)
        {

            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.DataSource = null;
            grid.ColumnCount = 1;
            grid.Columns[0].Name = "ERROR";
            grid.Rows.Add("ERROR");
            grid.Refresh();
        }


        private void SQLRequest(string text, DataGridView grid)
        {
            SqlConnection conn;
            string connectionString = "Data Source=GOLTER;Initial Catalog=LogDB;Integrated Security=True";
            conn = new SqlConnection(connectionString);
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
                    SetText("An Error occured, the table is empty.");
                    DataGridViewError(grid);
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                SetText("An Error occured");
                MessageBox.Show(ee.ToString(), "Error!");
                DataGridViewError(grid);
            }
        }

        //nagłówek kolumny w tabeli
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string header = dataGridView1.Columns[e.ColumnIndex].HeaderText;

            if (radioButton1.Checked)
            {
                SQLRequest("select * from " + table + " order by " + header + " asc", dataGridView1);
            }
            else if (radioButton2.Checked)
            {
                SQLRequest("select * from " + table + " order by " + header + " desc", dataGridView1);
            }
        }

        //nagłówek kolumny z tabelami
        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string header = dataGridView2.Columns[e.ColumnIndex].HeaderText;
            if (header == "Nazwa Tabeli")
            {
                header = "name";
            }
            if (radioButton1.Checked)
            {
                SQLRequest("SELECT name FROM Sys.Tables order by " + header + " asc", dataGridView2);
                dataGridView2.Columns[0].Width = 190;
                dataGridView2.Columns[0].HeaderText = "Nazwa Tabeli";
            }
            else if (radioButton2.Checked)
            {
                SQLRequest("SELECT name FROM Sys.Tables order by " + header + " desc", dataGridView2);
                dataGridView2.Columns[0].Width = 190;
                dataGridView2.Columns[0].HeaderText = "Nazwa Tabeli";
            }
        }

        //dodaj wpis
        private void button1_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView2.CurrentCell.RowIndex;
            int columnindex = dataGridView2.CurrentCell.ColumnIndex;
            table = dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString();
            SQLRequest("select * from " + table, dataGridView1);
            int columns = dataGridView1.ColumnCount;
            string query = "INSERT INTO " + table + " VALUES (";
            for (int i = 0; i <= columns - 1; i++)
            {
                query += "'" + Microsoft.VisualBasic.Interaction.InputBox("Podaj " + dataGridView1.Columns[i].HeaderText) + "', ";
            }
            query = query.Remove(query.Length - 2);
            query += ");";

            SQLRequestWithoutDisplay(query);
            SQLRequest("select * from " + table, dataGridView1);
        }

        //usuń wpis
        private void button2_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            int columns = dataGridView1.ColumnCount;
            string query = "DELETE FROM " + table + " WHERE ";
            DateTime date;
            for (int i = 0; i <= columns - 1; i++)
            {
                if (dataGridView1.Rows[rowindex].Cells[i].Value.ToString() == "" || DateTime.TryParse(dataGridView1.Rows[rowindex].Cells[i].Value.ToString(), out date))
                {
                    continue;
                }
                query += dataGridView1.Columns[i].HeaderText + " = '" + dataGridView1.Rows[rowindex].Cells[i].Value.ToString() + "' AND ";
            }
            query = query.Remove(query.Length - 5);
            query += ";";
            SQLRequestWithoutDisplay(query);
            SQLRequest("select * from " + table, dataGridView1);
        }

        private void formClose(object sender, FormClosedEventArgs e)
        {
            BeginningWindow BW = new BeginningWindow();
            BW.Show();
            BW.Activate();
        }
        
    }
}

