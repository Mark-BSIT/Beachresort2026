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
    public partial class StaffDashboard : Form
    {
        public StaffDashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BookingForm form = new BookingForm(this); 
            form.Show();
        }
        public async Task RefreshData()
        {
            await LoadTodayBookings();
            await LoadSummary();
        }
        private async Task LoadChart(string mode)
        {
            var bookings = await new ApiService().GetBookings();

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            chart1.ChartAreas.Add("Main");
            chart1.ChartAreas[0].AxisY.Title = "Revenue (₱)";
            chart1.ChartAreas[0].AxisY2.Title = "Bookings";
            chart1.ChartAreas[0].AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            var seriesBookings = chart1.Series.Add("Bookings");
            seriesBookings.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            seriesBookings.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary; // 🔥 FIX

            var seriesRevenue = chart1.Series.Add("Revenue");
            seriesRevenue.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            seriesRevenue.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;

            var data = bookings;

            if (mode == "today")
            {
                data = bookings.Where(b => b.date.Date == DateTime.Today).ToList();
            }
            else if (mode == "week")
            {
                DateTime start = DateTime.Today.AddDays(-7);
                data = bookings.Where(b => b.date >= start).ToList();
            }
            else if (mode == "month")
            {
                DateTime start = DateTime.Today.AddDays(-30);
                data = bookings.Where(b => b.date >= start).ToList();
            }

            var grouped = data
                .GroupBy(b => b.date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Select(x => x.id).Distinct().Count(),
                    Revenue = g.Sum(x => x.total)
                })
                .OrderBy(x => x.Date)
                .ToList();

            foreach (var item in grouped)
            {
                chart1.Series["Bookings"].Points.AddXY(item.Date.ToShortDateString(), item.Count);
                chart1.Series["Revenue"].Points.AddXY(item.Date.ToShortDateString(), item.Revenue);
            }
        }
        private async Task LoadTodayBookings()
        {
            var bookings = await new ApiService().GetBookings();

            if (bookings == null || bookings.Count == 0)
            {
                dataGridView1.DataSource = null;
                return;
            }

            DateTime today = DateTime.Today;

            // 🔥 FILTER TODAY
            var todayBookings = bookings
                .Where(b => b.date.Date == today)
                .ToList();

            // 🔥 GROUP BY BOOKING ID (IMPORTANT FIX)
            var grouped = todayBookings
                .GroupBy(b => b.id)
                .Select(g => new
                {
                    id = g.Key,
                    customer_name = g.First().customer_name,
                    address = g.First().address,
                    date = g.First().date,
                    status = g.First().status,
                    total = g.First().total
                })
                .ToList();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = grouped;

            // 🔥 HEADERS
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["customer_name"].HeaderText = "Name";
            dataGridView1.Columns["address"].HeaderText = "Address";
            dataGridView1.Columns["date"].HeaderText = "Date";
            dataGridView1.Columns["status"].HeaderText = "Status";
            dataGridView1.Columns["total"].HeaderText = "Total (₱)";

            // 🔥 FORMAT
            dataGridView1.Columns["date"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dataGridView1.Columns["total"].DefaultCellStyle.Format = "N2";

            // 🔥 DESIGN
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
        }
        private async Task LoadSummary()
        {
            var api = new ApiService();

            var bookings = await api.GetBookings();
            var cottages = await api.GetCottage();
            var boats = await api.GetBoat();
            var items = await api.GetBookingItems(); // 🔥 IMPORTANT

            DateTime today = DateTime.Today;

            // =========================
            // ✅ TODAY BOOKINGS
            // =========================
            var todayBookings = bookings
                .Where(b => b.date.Date == today)
                .ToList();

            int todayCount = todayBookings
                .Select(b => b.id)
                .Distinct()
                .Count();

            lblTodayBookings.Text = todayCount.ToString();

            // =========================
            // 🔥 ACTIVE BOOKING IDS
            // =========================
            var activeIds = todayBookings
                .Where(b => b.status != "Checked-out" && b.status != "Cancelled")
                .Select(b => b.id)
                .ToList();

            // =========================
            // ✅ OCCUPIED COTTAGES
            // =========================
            int occupiedCottages = items
                .Where(i => i.item_type == "cottage" && activeIds.Contains(i.booking_id))
                .Select(i => i.item_id)
                .Distinct()
                .Count();

            // =========================
            // ✅ OCCUPIED BOATS
            // =========================
            int occupiedBoats = items
                .Where(i => i.item_type == "boat" && activeIds.Contains(i.booking_id))
                .Select(i => i.item_id)
                .Distinct()
                .Count();

            // =========================
            // ✅ DISPLAY
            // =========================
            lblAvailableCottage.Text = $"{cottages.Count - occupiedCottages}/{cottages.Count}";
            lblAvailableBoat.Text = $"{boats.Count - occupiedBoats}/{boats.Count}";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void StaffDashboard_Load(object sender, EventArgs e)
        {
           await RefreshData();
           await LoadChart("today");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Checkin checkin = new Checkin(this);
            checkin.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private async void button7_Click(object sender, EventArgs e)
        {
            await LoadChart("today");
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            await LoadChart("week");
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            await LoadChart("month");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Checkout checkout = new Checkout(this);
            checkout.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Transaction form = new Transaction();
            form.ShowDialog();
        }
    }
}
