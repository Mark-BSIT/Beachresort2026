using BeachResortAPIWinForm.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BeachResortAPIWinForm.Forms
{
    public partial class Addboat : Form
    {
        public Addboat()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string name = BoatName.Text.Trim();
            string priceText = BoatPrice.Text.Trim();

            // 🔴 validation
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Invalid price");
                return;
            }

            bool success = await new ApiService().AddBoat(name, price);

            if (success)
            {
                MessageBox.Show("Boat added successfully!");
                this.Close(); // 🔥 balik dashboard
            }
            else
            {
                MessageBox.Show("Failed to add boat");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
