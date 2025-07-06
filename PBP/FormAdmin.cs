using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace PBP
{
    public partial class FormMenuAdmin: Form
    {
        private bool isLoggingOut = false;

        public FormMenuAdmin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAnggota_Click(object sender, EventArgs e)
        {
            FormAnggota formAnggota = new FormAnggota();
            formAnggota.Show();
        }

        private void btnBuku_Click(object sender, EventArgs e)
        {
            FormBuku formBuku = new FormBuku();
            formBuku.Show();
        }

        private void btnPeminjaman_Click(object sender, EventArgs e)
        {
            FormPeminjaman formPeminjaman = new FormPeminjaman();
            formPeminjaman.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.isLoggingOut = true;

            Form loginForm = Application.OpenForms["FormLogin"];

            if (loginForm != null)
            {
                loginForm.Show();
            }
            else
            {
                btnTestConnection newLogin = new btnTestConnection();
                newLogin.Show();
            }

            this.Close();
        }

        private void FormAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !this.isLoggingOut)
            {
                DialogResult result = MessageBox.Show("Apakah Anda yakin ingin keluar dari aplikasi?", "Konfirmasi Keluar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
