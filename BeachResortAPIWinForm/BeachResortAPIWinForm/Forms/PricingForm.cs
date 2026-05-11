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

namespace BeachResortAPIWinForm.Forms
{
    public partial class PricingForm : Form
    {
        public PricingForm()
        {
            InitializeComponent();
        }
        private void PricingForm_Load(object sender, EventArgs e)
        {
            LoadPricing();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private async void LoadPricing()
        {
            flowLayoutPanel1.Controls.Clear();

            var cottages = await new ApiService().GetCottages();
            var boats = await new ApiService().GetBoats();

            // 🔥 COTTAGES
            foreach (var c in cottages)
            {
                flowLayoutPanel1.Controls.Add(CreateItemPanel(c.id, c.name, c.price, "cottage"));
            }

            // 🔥 BOATS
            foreach (var b in boats)
            {
                flowLayoutPanel1.Controls.Add(CreateItemPanel(b.id, b.name, b.price, "boat"));
            }
        }
        private Panel CreateItemPanel(int id, string name, decimal price, string type)
        {
            Panel panel = new Panel();
            panel.Width = 500;
            panel.Height = 80;

            TextBox txtName = new TextBox();
            txtName.Text = name;
            txtName.Left = 10;
            txtName.Width = 150;

            TextBox txtPrice = new TextBox();
            txtPrice.Text = price.ToString();
            txtPrice.Left = 170;
            txtPrice.Width = 100;

            Button btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.Left = 280;

            Button btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Left = 350;
            btnDelete.BackColor = Color.Red;

            // 🔥 SAVE CLICK
            btnSave.Click += async (s, e) =>
            {
                decimal newPrice;

                if (!decimal.TryParse(txtPrice.Text, out newPrice))
                {
                    MessageBox.Show("Invalid price");
                    return;
                }

                bool success = false;

                if (type == "cottage")
                    success = await new ApiService().UpdateCottage(id, txtName.Text, newPrice);
                else
                    success = await new ApiService().UpdateBoat(id, txtName.Text, newPrice);

                MessageBox.Show(success ? "Updated!" : "Failed!");
            };

            panel.Controls.Add(txtName);
            panel.Controls.Add(txtPrice);
            panel.Controls.Add(btnSave);
            panel.Controls.Add(btnDelete);

            return panel;
        }

       
    }
}
