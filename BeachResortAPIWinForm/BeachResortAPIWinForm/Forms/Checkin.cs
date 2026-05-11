using BeachResortAPIWinForm.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeachResortAPIWinForm.Forms
{
    public partial class Checkin : Form
    {

        StaffDashboard dashboard;
        public Checkin(StaffDashboard dash)
        {
            InitializeComponent();
            dashboard = dash;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Enter ID");
                return;
            }

            int id = Convert.ToInt32(textBox1.Text);

            var api = new ApiService();

            // STEP 1: GET booking
            var data = await api.GetBookingById(id);

            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Booking not found");
                return;
            }

            var b = data[0];

            // STEP 2: VALIDATION (FIRST)
            if (b.status != "Booked")
            {
                MessageBox.Show("Only BOOKED can check-in");
                return;
            }

            if (b.date.Date < DateTime.Today)
            {
                MessageBox.Show("Booking expired");
                return;
            }

            // STEP 3: UPDATE (ONLY ONCE)
            bool success = await api.UpdateStatus(id);

            if (!success)
            {
                MessageBox.Show("Check-in failed");
                return;
            }

            // STEP 4: SUCCESS
            MessageBox.Show("Checked-in successful");

            dashboard.RefreshData();
            this.Close();
        }

        private void Checkin_Load(object sender, EventArgs e)
        {

        }
    }
}
