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
    public partial class Forgotpass : Form
    {
        public Forgotpass()
        {
            InitializeComponent();
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

            try
            {
                ApiService api = new ApiService();

                bool success = await api.ResetPassword(username, password);

                if (success)
                {
                    MessageBox.Show("Password reset successful!");

                    Form1 login = new Form1(); // open login balik
                    login.Show();

                    this.Close(); // close reset form
                }
                else
                {
                    MessageBox.Show("User not found or error occurred");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    
    }
}
