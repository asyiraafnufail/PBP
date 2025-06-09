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
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connectionString = @"Data Source=DESKTOP-A3U4VR2\SQLEXPRESS;Initial Catalog=PBP;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Cek di tabel Admin berdasarkan email dan password
                string sqlAdmin = @"
                    SELECT id_admin, nama_admin
                    FROM Admin
                    WHERE email = @user AND password = @pass";

                using (SqlCommand cmd = new SqlCommand(sqlAdmin, conn))
                {
                    cmd.Parameters.Add("@user", SqlDbType.VarChar, 100).Value = username;
                    cmd.Parameters.Add("@pass", SqlDbType.NVarChar, 100).Value = password;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Jika ketemu di tabel Admin
                            /*string idAdmin = reader.GetString(reader.GetOrdinal("id_admin"));*/
                            string idAdmin = reader["id_admin"].ToString();
                            string namaAdmin = reader.GetString(reader.GetOrdinal("nama_admin"));
                            MessageBox.Show($"Login berhasil! Selamat datang, {namaAdmin}.", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Buka FormMenuAdmin
                            FormMenuAdmin menu = new FormMenuAdmin();
                            menu.Show();
                            this.Hide();
                            return;
                        }
                    }
                }

                // Jika tidak ada di Admin, cek di tabel Anggota
                string sqlAnggota = @"
                    SELECT id_anggota, nama
                    FROM Anggota
                    WHERE email = @user AND password = @pass";

                using (SqlCommand cmd = new SqlCommand(sqlAnggota, conn))
                {
                    cmd.Parameters.Add("@user", SqlDbType.VarChar, 100).Value = username;
                    cmd.Parameters.Add("@pass", SqlDbType.NVarChar, 100).Value = password;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //string idAnggota = reader.GetString(reader.GetOrdinal("id_anggota"));
                            //string idAnggota = reader["id_anggota"].ToString();
                            string namaAnggota = reader.GetString(reader.GetOrdinal("nama"));
                            MessageBox.Show($"Selamat datang, {namaAnggota}!", "Anggota", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // setelah reader.Read() dan kamu dapatkan idAnggota
                            int idAnggota = Convert.ToInt32(reader["id_anggota"]);

                            var dash = new DashboardUser(idAnggota);
                            // Buka DashboardUser
                            //DashboardUser dashboard = new DashboardUser();
                            dash.Show();
                            this.Hide();
                            return;
                        }
                    }
                }

                // Jika tidak ada di kedua tabel
                MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormPendaftaran daftar = new FormPendaftaran();
            daftar.Show();
            this.Hide();
        }
    }
}
