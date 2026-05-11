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
    public partial class Addcottage : Form
    {
        public Addcottage()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string name = CotName.Text.Trim();
            string priceText = CotPrice.Text.Trim();

            // 🔴 VALIDATION
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

            // 🔥 CALL API
            bool success = await new ApiService().AddCottage(name, price);

            if (success)
            {
                MessageBox.Show("Cottage added successfully!");

                // optional: clear inputs
                CotName.Clear();
                CotPrice.Clear();

                this.Close(); // close form
            }
            else
            {
                MessageBox.Show("Failed to add cottage");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
