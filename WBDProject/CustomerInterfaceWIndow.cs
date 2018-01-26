using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WBDProject
{
    public partial class CustomerInterfaceWIndow : Form
    {
        public CustomerInterfaceWIndow(string CustomerName)
        {
            InitializeComponent();
            this.Text = CustomerName;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLRequester.AddPreference(ref this.dataGridView1, this.comboBox1.Text, this.comboBox3.Text,
                this.comboBox2.Text, this.comboBox5.Text, this.Text.ToCharArray()[0].ToString());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CustomerInterfaceWIndow_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SQLRequester.FindExcursion(ref this.dataGridView2, this.comboBox1.Text, this.comboBox3.Text,
                this.comboBox2.Text, this.comboBox5.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SQLRequester.FindPreferences(ref this.dataGridView1, this.Text.ToCharArray()[0].ToString());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                //Standard
                if (!comboBox1.Items.Contains(dataGridView1.SelectedRows[0].Cells[1].FormattedValue))
                    this.comboBox1.Items.Add(dataGridView1.SelectedRows[0].Cells[1].FormattedValue);
                this.comboBox1.SelectedIndex =
                    comboBox1.Items.IndexOf(dataGridView1.SelectedRows[0].Cells[1].FormattedValue);
                //People_in_room 
                if (!comboBox2.Items.Contains(dataGridView1.SelectedRows[0].Cells[3].FormattedValue))
                    this.comboBox2.Items.Add(dataGridView1.SelectedRows[0].Cells[3].FormattedValue);
                this.comboBox2.SelectedIndex = comboBox2.Items.IndexOf(dataGridView1.SelectedRows[0].Cells[3].FormattedValue);
                //All inclusive
                string all_inclusive_english = "";
                if (dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "no" ||
                    dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "Nie" ||
                    dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "N" ||
                    dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "nie")
                    all_inclusive_english = "no";


                else if (dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "yes" ||
                         dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "Tak" ||
                         dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "T" ||
                         dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString() == "tak")
                    all_inclusive_english = "yes";

                if (!comboBox3.Items.Contains(dataGridView1.SelectedRows[0].Cells[3].FormattedValue))
                    this.comboBox3.SelectedIndex = comboBox3.Items.IndexOf(all_inclusive_english);

                //Vehicle type cbox5 cell 4
                if (!comboBox5.Items.Contains(dataGridView1.SelectedRows[0].Cells[4].FormattedValue))
                    this.comboBox5.Items.Add(dataGridView1.SelectedRows[0].Cells[4].FormattedValue);
                this.comboBox5.SelectedIndex = comboBox5.Items.IndexOf(dataGridView1.SelectedRows[0].Cells[4].FormattedValue);
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please choose only one row of the preferences!");
            }
            else if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("No rows of preferences are chosen!");
            }
        }
    }
}
