using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormAnggota : Form
    {
        // Pindahkan connection string menjadi satu-satunya sumber kebenaran
        private readonly string connStr = "Data Source=DESKTOP-A3U4VR2\\SQLEXPRESS;Initial Catalog=PBP;Integrated Security=True";

        public FormAnggota()
        {
            InitializeComponent();
        }

        private void FormAnggota_Load(object sender, EventArgs e)
        {
            // Panggil LoadData() saat form pertama kali dibuka
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Gunakan 'using' untuk memastikan koneksi ditutup otomatis
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Anggota ORDER BY nama ASC", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtID.Clear();
            txtNama.Clear();
            txtAlamat.Clear();
            txtEmail.Clear();
            txtTelepon.Clear();
            txtID.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show("ID dan Nama Anggota wajib diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "INSERT INTO Anggota (id_anggota, nama, alamat, email, no_telepon) VALUES (@id, @nama, @alamat, @email, @telp)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", txtID.Text);
                        cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@telp", txtTelepon.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Data anggota berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData(); // Muat ulang data untuk menampilkan data baru
                ClearFields(); // Bersihkan textbox
            }
            catch (SqlException ex)
            {
                // Menangani error jika ID sudah ada (Primary Key violation)
                if (ex.Number == 2627)
                {
                    MessageBox.Show($"Gagal menyimpan: ID Anggota '{txtID.Text}' sudah ada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Gagal menyimpan data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Pilih data yang akan dihapus dengan mengklik baris pada tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tampilkan dialog konfirmasi sebelum menghapus
            var confirmResult = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string query = "DELETE FROM Anggota WHERE id_anggota=@id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtID.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Data anggota berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Gagal menghapus data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Pilih data yang akan diupdate dengan mengklik baris pada tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "UPDATE Anggota SET nama=@nama, alamat=@alamat, email=@email, no_telepon=@telp WHERE id_anggota=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", txtID.Text);
                        cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@telp", txtTelepon.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Data anggota berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mengupdate data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handler saat sel di grid diklik
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan yang diklik adalah baris yang valid (bukan header)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                // Ambil data dari sel dan tampilkan di textbox
                txtID.Text = row.Cells["id_anggota"].Value.ToString();
                txtNama.Text = row.Cells["nama"].Value.ToString();
                txtAlamat.Text = row.Cells["alamat"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtTelepon.Text = row.Cells["no_telepon"].Value.ToString();
            }
        }
    }
}