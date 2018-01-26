﻿using System;
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
    public partial class ClientWindow : Form
    {
        public ClientWindow()
        {
            InitializeComponent();
            SQLRequester.getAllClientsNames(ref this.comboBox1);
            this.comboBox1.SelectedIndex = 0;
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomerInterfaceWIndow CIW = new CustomerInterfaceWIndow(comboBox1.Text);
            this.Close();
            CIW.Show();
            CIW.Activate();
            
        }

        private void formClose(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
