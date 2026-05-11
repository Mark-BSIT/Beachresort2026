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
using System.Windows.Forms.DataVisualization.Charting;
namespace BeachResortAPIWinForm.Forms
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadDashboardSummary();
            LoadBookingsChart();
            LoadRevenueChart();
            LoadTodayBookings();
        }
        private async void LoadBookingsChart()
        {
            chart1.Series.Clear();

            var bookings = await new ApiService().GetAllBookings();

            var grouped = bookings
                .GroupBy(b => b.date.ToString("MM/dd"))
                .Select(g => new
                {
                    date = g.Key,
                    total = g.Select(x => x.id).Distinct().Count()
                });

            Series series = new Series("Bookings");
            series.ChartType = SeriesChartType.Column;

            foreach (var item in grouped)
            {
                series.Points.AddXY(item.date, item.total);
            }

            chart1.Series.Add(series);
        }
        private async void LoadTodayBookings()
        {
            var data = await new ApiService().GetAllBookings();

            var today = DateTime.Today;

            // 🔥 FILTER TODAY
            var filtered = data
                .Where(x => x.date.Date == today)
                .ToList();

            // 🔥 REMOVE DUPLICATES (1 booking = 1 row)
            var uniqueBookings = filtered
                .GroupBy(x => x.id)
                .Select(g => g.First())
                .ToList();

            // 🔥 CLEAN BINDING (NO NEED TO HIDE COLUMNS)
            dataGridView1.DataSource = uniqueBookings.Select(b => new
            {
                ID = b.id,
                Customer = b.customer_name,
                Address = b.address,
                Date = b.date,
                Status = b.status,
                Amount = b.total
            }).ToList();

            // =========================
            // 🔥 FORMAT
            // =========================
            dataGridView1.Columns["Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dataGridView1.Columns["Amount"].DefaultCellStyle.Format = "₱#,##0.00";

            // =========================
            // 🔥 DESIGN
            // =========================
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
        }
        private async void LoadDashboardSummary()
        {
            var api = new ApiService();

            var data = await api.GetAllBookings();
            if (data == null) return;

            // ✔ REMOVE DUPLICATES (1 row = 1 booking)
            var uniqueBookings = data
                .GroupBy(x => x.id)
                .Select(g => g.First())
                .ToList();

            // ✔ TOTAL BOOKINGS
            int totalBookings = uniqueBookings.Count;

            // ✔ TOTAL REVENUE (ONLY CHECKED-OUT)
            decimal totalRevenue = uniqueBookings
                .Where(x => x.status == "Checked-out")
                .Sum(x => x.total);

            // ✔ LOAD MASTER DATA
            var cottages = await api.GetCottage();
            var boats = await api.GetBoat();

            // 🔥 NEW: GET BOOKING ITEMS
            var items = await api.GetBookingItems();

            var today = DateTime.Today;

            // 🔥 ACTIVE BOOKINGS TODAY
            var activeBookingIds = uniqueBookings
                .Where(b => b.date.Date == today &&
                            b.status != "Checked-out" &&
                            b.status != "Cancelled")
                .Select(b => b.id)
                .ToList();

            // 🔥 OCCUPIED COTTAGES
            var occupiedCottages = items
                .Where(i => i.item_type == "cottage" &&
                            activeBookingIds.Contains(i.booking_id))
                .Select(i => i.item_id)
                .Distinct()
                .ToList();

            // 🔥 OCCUPIED BOATS
            var occupiedBoats = items
                .Where(i => i.item_type == "boat" &&
                            activeBookingIds.Contains(i.booking_id))
                .Select(i => i.item_id)
                .Distinct()
                .ToList();

            // ✔ COMPUTE AVAILABILITY
            int totalCottages = cottages.Count;
            int availableCottages = cottages.Count(c => !occupiedCottages.Contains(c.id));

            int totalBoats = boats.Count;
            int availableBoats = boats.Count(b => !occupiedBoats.Contains(b.id));

            // ✔ DISPLAY
            totalbook.Text = totalBookings.ToString();
            totalrev.Text = "₱" + totalRevenue.ToString("N0");
            availcottage.Text = $"{availableCottages}/{totalCottages}";
            availboat.Text = $"{availableBoats}/{totalBoats}";
        }
        private async void LoadRevenueChart()
        {
            chart2.Series.Clear();

            var bookings = await new ApiService().GetAllBookings();

            var revenueByDate = new Dictionary<string, decimal>();

            foreach (var b in bookings)
            {
                // ✔ only checked-out
                if (b.status == null || !b.status.ToLower().Contains("out"))
                    continue;

                string date = b.date.ToString("MM/dd");

                if (!revenueByDate.ContainsKey(date))
                    revenueByDate[date] = 0;

                revenueByDate[date] += b.total;
            }

            Series series = new Series("Revenue");
            series.ChartType = SeriesChartType.Column;

            foreach (var item in revenueByDate)
            {
                series.Points.AddXY(item.Key, item.Value);
            }

            chart2.Series.Add(series);
        }

        private async void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Addcottage form = new Addcottage();

            form.FormClosed += (s, args) =>
            {
                LoadDashboardSummary(); // refresh totals
                LoadTodayBookings();   // refresh table
            };

            form.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Addcottage form = new Addcottage();

            form.FormClosed += (s, args) =>
            {
                LoadDashboardSummary(); // refresh totals
                LoadTodayBookings();   // refresh table
            };

            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Addboat addboat = new Addboat();

            addboat.FormClosed += (s, args) =>
            {
                LoadDashboardSummary(); // refresh totals
                LoadTodayBookings();   // refresh table
            };

            addboat.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Addboat addboat = new Addboat();

            addboat.FormClosed += (s, args) =>
            {
                LoadDashboardSummary(); // refresh totals
                LoadTodayBookings();   // refresh table
            };

            addboat.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Transaction form = new Transaction();
            form.ShowDialog();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            PricingForm form = new PricingForm();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PricingForm form = new PricingForm();
            form.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
