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

namespace LogAgent
{
    public partial class Form1 : Form
    {
        bool loop = true;
        string table = "Sys.Tables";
        public Form1()
        {
            InitializeComponent();
            SQLRequest("SELECT name FROM Sys.Tables", dataGridView2);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SQLRequest("SELECT name FROM Sys.Tables", dataGridView2);
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
            string connectionString = "Data Source=DESKTOP-VULS1Q1;Initial Catalog=DatabaseZST;Integrated Security=True";
            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(text, conn);

            try
            {
                conn.Open();
                SqlDataReader DR = cmd.ExecuteReader();
                BindingSource source = new BindingSource();
                source.DataSource = DR;

                SetText("Connected with DatabaseZST");
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
            string connectionString = "Data Source=DESKTOP-VULS1Q1;Initial Catalog=DatabaseZST;Integrated Security=True";
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
                SetText("Connected with DatabaseZST");
                conn.Close();
            }
            catch (Exception ee)
            {
                SetText("An Error occured");
                MessageBox.Show(ee.ToString(), "Error!");
                DataGridViewError(grid);
            }
        }

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

        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string header = dataGridView2.Columns[e.ColumnIndex].HeaderText;
            if (radioButton1.Checked)
            {
                SQLRequest("SELECT name FROM Sys.Tables order by " + header + " asc", dataGridView2);
            }
            else if (radioButton2.Checked)
            {
                SQLRequest("SELECT name FROM Sys.Tables order by " + header + " desc", dataGridView2);
            }
        }
    }
}
