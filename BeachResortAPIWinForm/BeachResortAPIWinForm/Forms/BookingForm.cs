using BeachResortAPIWinForm.Models;
using BeachResortAPIWinForm.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeachResortAPIWinForm.Forms
{
    public partial class BookingForm : Form
    {
        private StaffDashboard dashboard;

        ApiService api = new ApiService();

        List<Cottage> cottages = new List<Cottage>();
        List<Boat> boats = new List<Boat>();

        List<int> selectedCottages = new List<int>();
        List<int> selectedBoats = new List<int>();
        decimal total = 0;

        public BookingForm(StaffDashboard dash)
        {
            InitializeComponent();
            this.Load += Booking_Load;
            dashboard = dash;
        }

        private async void Booking_Load(object sender, EventArgs e)
        {
            await LoadUnits();
        }

        private async Task LoadUnits()
        {
            // ✅ FIX: use GLOBAL lists
            this.cottages = await api.GetCottage();
            this.boats = await api.GetBoat();
            var bookings = await api.GetBookings();

            DateTime selectedDate = dateTimePicker1.Value.Date;

            // ✅ NEW (same sa web logic)
            var bookedCottageIds = bookings
                .Where(b => b.date.Date == selectedDate &&
                b.status != "Checked-out" &&
                b.status != "Cancelled" &&
                b.item_type == "cottage" &&
                b.item_id != null)
                .Select(b => b.item_id.Value) // 🔥 convert to int
                .ToList();

            var bookedBoatIds = bookings
                .Where(b => b.date.Date == selectedDate &&
                b.status != "Checked-out" &&
                b.status != "Cancelled" &&
                b.item_type == "boat" &&
                b.item_id != null)
                .Select(b => b.item_id.Value) // 🔥 convert to int
                .ToList();

            LoadCottageUI(this.cottages, bookedCottageIds);
            LoadBoatUI(this.boats, bookedBoatIds);
        }

        private void LoadCottageUI(List<Cottage> cottages, List<int> bookedIds)
        {
            flowCottage.Controls.Clear();

            foreach (var c in cottages)
            {
                Button btn = new Button();
                btn.Width = 140;
                btn.Height = 60;
                btn.Text = $"{c.name}\n₱{c.price}";

                if (bookedIds.Contains(c.id))
                {
                    btn.BackColor = Color.Gray;
                    btn.Enabled = false;
                    btn.Text += "\n(Occupied)";
                }
                else
                {
                    btn.BackColor = Color.LightGreen;

                    btn.Click += (s, e) =>
                    {
                        if (selectedCottages.Contains(c.id))
                        {
                            selectedCottages.Remove(c.id);
                            btn.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            selectedCottages.Add(c.id);
                            btn.BackColor = Color.Blue;
                        }

                        ComputeTotal();
                    };
                }

                flowCottage.Controls.Add(btn);
            }
        }

        private void LoadBoatUI(List<Boat> boats, List<int> bookedIds)
        {
            flowBoat.Controls.Clear();

            foreach (var b in boats)
            {
                Button btn = new Button();
                btn.Width = 140;
                btn.Height = 60;
                btn.Text = $"{b.name}\n₱{b.price}";

                if (bookedIds.Contains(b.id))
                {
                    btn.BackColor = Color.Gray;
                    btn.Enabled = false;
                    btn.Text += "\n(Occupied)";
                }
                else
                {
                    btn.BackColor = Color.LightGreen;

                    btn.Click += (s, e) =>
                    {
                        if (selectedBoats.Contains(b.id))
                        {
                            selectedBoats.Remove(b.id);
                            btn.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            selectedBoats.Add(b.id);
                            btn.BackColor = Color.Blue;
                        }

                        ComputeTotal();
                    };
                }

                flowBoat.Controls.Add(btn);
            }
        }

        private void ComputeTotal()
        {
            total = 0;

            // 🏠 COTTAGES
            foreach (var id in selectedCottages)
            {
                var c = cottages.First(x => x.id == id);
                total += c.price;
            }

            // 🚤 BOATS
            foreach (var id in selectedBoats)
            {
                var b = boats.First(x => x.id == id);
                total += b.price;
            }

            // 👤 ENTRY FEE (₱100 per guest)
            int guests = (int)numericGuest.Value;
            total += guests * 100;

            lbltotal.Text = "Total: ₱" + total.ToString("N2");
        }

        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            await LoadUnits();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (selectedCottages.Count == 0)
            {
                MessageBox.Show("Select at least one cottage!");
                return;
            }

            var success = await api.CreateBooking(new
            {
                customer_name = textName.Text,
                contact = textContact.Text,
                address = textAddress.Text,
                date = dateTimePicker1.Value,
                num_people = (int)numericGuest.Value,
                total = total,
                status = "Booked",

                cottages = selectedCottages.Select(id => new
                {
                    id = id,
                    quantity = 1
                }),

                boats = selectedBoats.Select(id => new
                {
                    id = id,
                    quantity = 1
                })
            });

            if (!success) // ❗ IMPORTANT
            {
                MessageBox.Show("❌ Booking failed (API error)");
                return;
            }

            MessageBox.Show("✅ Booking success!");

            await dashboard.RefreshData();
            await LoadUnits();

            selectedCottages.Clear();
            selectedBoats.Clear();
            total = 0;
            lbltotal.Text = "Total: ₱0";
        }

        private void numericGuest_ValueChanged(object sender, EventArgs e)
        {
            ComputeTotal();
        }

        private void lbltotal_Click(object sender, EventArgs e)
        {

        }
    }
}