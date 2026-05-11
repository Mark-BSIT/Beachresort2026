using BeachResortAPIWinForm.Models;
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
using static System.Collections.Specialized.BitVector32;

namespace BeachResortAPIWinForm.Forms
{
    public partial class Transaction : Form
    {
        public Transaction()
        {
            InitializeComponent();
        }

        private void Transaction_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0; // All
            LoadTransactions();
        }
        private List<Booking> allData = new List<Booking>();

        private async void LoadTransactions()
        {
            allData = await new ApiService().GetAllBookings();

            ApplyFilter();
        }
        private void ApplyFilter()
        {
            var filtered = allData;

            // =========================
            // 🔍 SEARCH
            // =========================
            string search = txtSearch.Text.ToLower();

            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered
                    .Where(x =>
                        x.id.ToString().Contains(search) ||
                        x.customer_name.ToLower().Contains(search)
                    ).ToList();
            }

            // =========================
            // 📅 FILTER
            // =========================
            switch (cmbFilter.Text)
            {
                case "Today":
                    filtered = filtered
                        .Where(x => x.date.Date == DateTime.Today)
                        .ToList();
                    break;

                case "This Week":
                    var startWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    filtered = filtered
                        .Where(x => x.date >= startWeek)
                        .ToList();
                    break;

                case "This Month":
                    filtered = filtered
                        .Where(x => x.date.Month == DateTime.Today.Month &&
                                    x.date.Year == DateTime.Today.Year)
                        .ToList();
                    break;
            }

            // =========================
            // 🔥 REMOVE DUPLICATES
            // =========================
            filtered = filtered
                .GroupBy(x => x.id)
                .Select(g => g.First())
                .ToList();

            // =========================
            // 🔥 CLEAN BINDING (YOUR REQUIRED FIELDS)
            // =========================
            var displayData = filtered.Select(b => new
            {
                ID = b.id,
                Customer = b.customer_name,
                Contact = b.contact,
                Address = b.address,
                Date = b.date,
                People = b.num_people,
                Status = b.status,
                Amount = b.total
            }).ToList();

            dataGridView1.DataSource = displayData;
            // 🔥 REMOVE OLD BUTTON (avoid duplicates)
            if (dataGridView1.Columns.Contains("Delete"))
                dataGridView1.Columns.Remove("Delete");

            // 🔥 ADD DELETE BUTTON COLUMN
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "Delete";
            btnDelete.HeaderText = "Action";
            btnDelete.Text = "Delete";
            btnDelete.UseColumnTextForButtonValue = true;

            if (Session.Role == "admin")
            {
                dataGridView1.Columns.Add(btnDelete);
            }
            // =========================
            // 🎨 FORMAT
            // =========================
            dataGridView1.Columns["Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dataGridView1.Columns["Amount"].DefaultCellStyle.Format = "₱#,##0.00";

            // =========================
            // 💅 UI STYLE
            // =========================
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

            // =========================
            // 🎨 STATUS COLOR
            // =========================
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string status = row.Cells["Status"].Value?.ToString();

                if (status == "Checked-out")
                    row.Cells["Status"].Style.BackColor = Color.Green;

                else if (status == "Checked-in")
                    row.Cells["Status"].Style.BackColor = Color.Orange;

                else if (status == "Cancelled")
                    row.Cells["Status"].Style.BackColor = Color.Red;

                row.Cells["Status"].Style.ForeColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete" && e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

                var confirm = MessageBox.Show(
                    "Delete this transaction?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm != DialogResult.Yes)
                    return;

                bool success = await new ApiService().DeleteBooking(id);

                if (success)
                {
                    MessageBox.Show("🗑 Deleted successfully");
                    LoadTransactions(); // 🔥 refresh
                }
                else
                {
                    MessageBox.Show("❌ Delete failed");
                }
            }
        }
    }
}
