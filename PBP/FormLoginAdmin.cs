using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormLoginAdmin : Form
    {
        public FormLoginAdmin()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void FormLoginAdmin_Load(object sender, EventArgs e)
        {

        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Ganti ini dengan validasi database jika ingin lebih aman
            if (username == "admin" && password == "admin123")
            {
                MessageBox.Show("Login berhasil!");
                FormMenuAdmin menu = new FormMenuAdmin();
                menu.Show();
                this.Hide();
            }

            if (username == "rafa" && password == "12345")
            {
                MessageBox.Show("Selamat Datang");
                DashboardUser dashboard = new DashboardUser();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
