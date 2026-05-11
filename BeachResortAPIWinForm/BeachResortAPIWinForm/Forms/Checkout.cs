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
    public partial class Checkout : Form
    {
        private StaffDashboard dashboard;
        public Checkout(StaffDashboard dash)
        {
            InitializeComponent();
            dashboard = dash;
            this.KeyPreview = true;
            textBox1.AcceptsReturn = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid ID");
                return;
            }

            if (!decimal.TryParse(txtCash.Text, out decimal cash))
            {
                MessageBox.Show("Enter valid cash amount");
                return;
            }

            var api = new ApiService();
            var data = await api.GetBookingById(id);

            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Booking not found");
                return;
            }

            var b = data[0];

            // 🔥 CHECK STATUS
            if (b.status != "Checked-in")
            {
                MessageBox.Show("Only checked-in guests can checkout");
                return;
            }

            decimal total = data.Sum(x => x.price * x.quantity);

            // 🔥 PAYMENT CHECK
            if (cash < total)
            {
                MessageBox.Show("Not enough cash!");
                return;
            }

            decimal change = cash - total;
            lblChange.Text = change.ToString("0.00");

            // 🔥 UPDATE STATUS
            bool success = await api.Checkout(id);

            if (!success)
            {
                MessageBox.Show("Checkout failed");
                return;
            }

            MessageBox.Show($"Checked-out successfully!\nChange: ₱{change}");

            await dashboard.RefreshData();
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtCash.Text, out decimal cash) &&
        decimal.TryParse(lblTotal.Text, out decimal total))
            {
                if (cash >= total)
                    lblChange.Text = (cash - total).ToString("0.00");
                else
                    lblChange.Text = "0.00";
            }
        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            string input = textBox1.Text;

            if (!int.TryParse(input, out int id))
            {
                lblTotal.Text = "0.00";
                return;
            }

            var api = new ApiService();
            var data = await api.GetBookingById(id);

            // 🔥 prevent race condition
            if (textBox1.Text != input)
                return;

            if (data == null || data.Count == 0)
            {
                lblTotal.Text = "0.00";
                return;
            }

            // 🔥 FIX HERE
            decimal total = data.Sum(x => x.price * x.quantity);

            lblTotal.Text = total.ToString("0.00");
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
