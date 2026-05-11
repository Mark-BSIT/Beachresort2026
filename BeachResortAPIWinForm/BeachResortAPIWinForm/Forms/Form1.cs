using BeachResortAPIWinForm.Forms;
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
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
namespace BeachResortAPIWinForm
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, EventArgs e)
        {
            string username = logUser.Text.Trim();
            string password = logPass.Text.Trim();

            // 🔴 Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password");
                return;
            }

            try
            {
                ApiService api = new ApiService();

                var result = await api.Login(username, password);

                if (result != null)
                {
                    MessageBox.Show("Welcome " + result.username);

                    // 👉 CHECK ROLE
                    if (result.role == "Admin")
                    {
                        Session.Role = "admin";
                        Dashboard admindashboard = new Dashboard();
                        admindashboard.Show();
                    }
                    else if (result.role == "Staff")
                    {
                        Session.Role = "staff";
                        StaffDashboard staff = new StaffDashboard();
                        staff.Show();
                    }
                    else
                    {
                        MessageBox.Show("Unknown role: " + result.role);
                        return;
                    }

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login Failed",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
            (sender2, cert, chain, sslPolicyErrors) => true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                logPass.UseSystemPasswordChar = false; // 👁 show password
            }
            else
            {
                logPass.UseSystemPasswordChar = true; // 🔒 hide password
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Forgotpass forgotpass = new Forgotpass();
            forgotpass.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
