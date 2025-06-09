using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormPendaftaran : Form
    {
        public FormPendaftaran()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormPendaftaran_Load(object sender, EventArgs e)
        {

        }

        private void fieldNama_TextChanged(object sender, EventArgs e)
        {

        }

        private void fieldAlamat_TextChanged(object sender, EventArgs e)
        {

        }

        private void fieldEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void fieldTelepon_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 1. Ambil nilai dari form
            string nama = fieldNama.Text.Trim();
            string alamat = fieldAlamat.Text.Trim();
            string email = fieldEmail.Text.Trim();
            string telepon = fieldTelepon.Text.Trim();
            string password = fieldPassword.Text.Trim();  // jika ada

            // 2. Validasi sederhana
            if (
                string.IsNullOrEmpty(nama) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Nama, Email, dan Password wajib diisi.",
                                "Validasi Gagal",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            string connectionString = @"Data Source=MSI\ABRA;Initial Catalog=PBP;Integrated Security=True"; ;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Gagal koneksi ke database:\n{ex.Message}",
                                    "Error Koneksi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                string sql = @"
                    INSERT INTO Anggota
                        (nama, alamat, email, no_telepon, password)
                    VALUES
                        (@nama, @alamat, @email, @telp, @pass)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@nama", SqlDbType.VarChar, 100).Value = nama;
                    cmd.Parameters.Add("@alamat", SqlDbType.Text).Value = alamat;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
                    cmd.Parameters.Add("@telp", SqlDbType.VarChar, 15).Value = telepon;
                    cmd.Parameters.Add("@pass", SqlDbType.NVarChar, 100).Value = password;

                    try
                    {
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Pendaftaran berhasil!",
                                            "Sukses",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            var login = new FormLoginAdmin();
                            login.Show();
                            this.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Gagal mendaftar:\n{ex.Message}",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormLoginAdmin login = new FormLoginAdmin();
            login.Show();
            this.Close();
        }
    }
}
