using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace PBP
{
    public partial class btnTestConnection : Form
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["PBP.Properties.Settings.PBPConnectionString"].ConnectionString;
        private bool isConnectionTested = false;

        public btnTestConnection()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open(); // Coba buka koneksi
                }
                // Jika berhasil, tampilkan pesan sukses
                MessageBox.Show("Koneksi ke database berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isConnectionTested = true; // Set flag menjadi true
            }
            catch (Exception ex)
            {
                // Jika gagal, tampilkan pesan error
                MessageBox.Show($"Koneksi ke database gagal.\n\nError: {ex.Message}",
                                "Koneksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnectionTested = false; // Set flag menjadi false
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!isConnectionTested)
            {
                MessageBox.Show("Silakan uji koneksi ke database terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username (email) dan password harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtUsername.Text.Trim();
            string plainPassword = txtPassword.Text;
            bool loginSuccess = false;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string adminQuery = "SELECT password, nama_admin FROM Admin WHERE email = @email";
                    using (SqlCommand cmdAdmin = new SqlCommand(adminQuery, conn))
                    {
                        cmdAdmin.CommandType = CommandType.Text;
                        cmdAdmin.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader reader = cmdAdmin.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["password"].ToString();
                                if (plainPassword == storedPassword)
                                {
                                    string userNama = reader["nama_admin"].ToString();
                                    MessageBox.Show($"Login berhasil! Selamat datang, {userNama}.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    FormMenuAdmin menu = new FormMenuAdmin();
                                    menu.Show();
                                    this.Hide();
                                    loginSuccess = true;
                                }
                            }
                        }
                    }

                    if (loginSuccess) return;

                    string anggotaQuery = "SELECT id_anggota, nama, password FROM Anggota WHERE email = @email";
                    using (SqlCommand cmdAnggota = new SqlCommand(anggotaQuery, conn))
                    {
                        cmdAnggota.CommandType = CommandType.Text;
                        cmdAnggota.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader reader = cmdAnggota.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["password"].ToString();
                                if (plainPassword == storedPassword)
                                {
                                    int userId = Convert.ToInt32(reader["id_anggota"]);
                                    string userNama = reader["nama"].ToString();
                                    MessageBox.Show($"Selamat datang, {userNama}!", "Anggota", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    DashboardUser dash = new DashboardUser(userId);
                                    dash.Show();
                                    this.Hide();
                                    loginSuccess = true;
                                }
                            }
                        }
                    }

                    if (!loginSuccess)
                    {
                        MessageBox.Show("Username atau Password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
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