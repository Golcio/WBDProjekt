using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WBDProject
{
    public partial class BeginningWindow : Form
    {
        public BeginningWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string choice = this.comboBox1.Text;

            switch (choice)
            {
                case "client":
                    ClientWindow CW = new ClientWindow();
                    CW.Show();
                    CW.Activate();
                    break;
                case "admin":
                    AdminWindow AW = new AdminWindow();
                    AW.Show();
                    AW.Activate();
                    break;
                default:
                    MessageBox.Show("Please choose either \"admin\" od \"client\".");
                    break;
            }

            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
