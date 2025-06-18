using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormLogin : Form
    {
        private readonly string connStr = "Server=localhost;Database=db_pbp;User ID=root;Password=;";

        public FormLogin()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username (email) dan password harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtUsername.Text.Trim();
            string plainPassword = txtPassword.Text;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Coba login sebagai Admin
                    using (MySqlCommand cmdAdmin = new MySqlCommand("VerifyAdminLogin", conn))
                    {
                        cmdAdmin.CommandType = CommandType.StoredProcedure;
                        cmdAdmin.Parameters.AddWithValue("p_email", email);
                        using (MySqlDataReader reader = cmdAdmin.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["password"].ToString();
                                // Perbandingan teks biasa
                                if (plainPassword == storedPassword)
                                {
                                    string userNama = reader["nama_admin"].ToString();
                                    MessageBox.Show($"Login berhasil! Selamat datang, {userNama}.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    FormMenuAdmin menu = new FormMenuAdmin();
                                    menu.Show();
                                    this.Hide();
                                    return;
                                }
                            }
                        }
                    }

                    // Coba sebagai Anggota
                    using (MySqlCommand cmdAnggota = new MySqlCommand("VerifyAnggotaLogin", conn))
                    {
                        cmdAnggota.CommandType = CommandType.StoredProcedure;
                        cmdAnggota.Parameters.AddWithValue("p_email", email);
                        using (MySqlDataReader reader = cmdAnggota.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["password"].ToString();
                                // Perbandingan teks biasa
                                if (plainPassword == storedPassword)
                                {
                                    int userId = Convert.ToInt32(reader["id_anggota"]);
                                    string userNama = reader["nama"].ToString();
                                    MessageBox.Show($"Selamat datang, {userNama}!", "Anggota", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    DashboardUser dash = new DashboardUser(userId);
                                    dash.Show();
                                    this.Hide();
                                    return;
                                }
                            }
                        }
                    }

                    MessageBox.Show("Username atau Password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}