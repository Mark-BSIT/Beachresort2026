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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkadmin.Checked)
                checkstaff.Checked = false;
        }

        private void checkstaff_CheckedChanged(object sender, EventArgs e)
        {
            if (checkstaff.Checked)
                checkadmin.Checked = false;
        }

        private async void Login_Click(object sender, EventArgs e)
        {
            string username = logUser.Text.Trim();
            string password = logPass.Text.Trim();
            string confirm = conpass.Text.Trim();

            // validation
            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            // role
            string role = "";

            if (checkadmin.Checked)
                role = "Admin";
            else if (checkstaff.Checked)
                role = "Staff";
            else
            {
                MessageBox.Show("Please select role");
                return;
            }

            try
            {
                ApiService api = new ApiService();

                bool success = await api.Register(username, password, role);

                if (success)
                {
                    MessageBox.Show("Account created successfully!");

                    Form1 login = new Form1();
                    login.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username already exists");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
    
}
