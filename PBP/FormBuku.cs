using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormBuku : Form
    {
        private readonly string connStr = "Data Source =DESKTOP-A3U4VR2\\SQLEXPRESS; Initial Catalog = PBP;Integrated Security=True";

        public FormBuku()
        {
            InitializeComponent();
        }

        private void FormBuku_Load(object sender, EventArgs e)
        {
            LoadDataBuku();
        }

        private void LoadDataBuku()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Buku ORDER BY judul ASC", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewBuku.DataSource = dt;

                    // [BARU] Sembunyikan kolom 'status' dari tampilan DataGridView
                    // Data status tetap ada di belakang layar, tapi tidak terlihat oleh admin.
                    if (dataGridViewBuku.Columns["status"] != null)
                    {
                        dataGridViewBuku.Columns["status"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data buku: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtIdBuku.Clear();
            txtJudul.Clear();
            txtPengarang.Clear();
            txtPenerbit.Clear();
            txtTahun.Clear();
            txtKategori.Clear();
            // [DIHAPUS] Referensi ke cmbStatus dihapus
            txtIdBuku.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdBuku.Text) || string.IsNullOrWhiteSpace(txtJudul.Text))
            {
                MessageBox.Show("ID Buku dan Judul wajib diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    // [DIUBAH] Status di-hardcode menjadi 'Tersedia' saat buku baru ditambahkan
                    string query = "INSERT INTO Buku (id_buku, judul, pengarang, penerbit, tahun_terbit, kategori, status) VALUES (@id, @judul, @pengarang, @penerbit, @tahun, @kategori, 'Tersedia')";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", txtIdBuku.Text);
                        cmd.Parameters.AddWithValue("@judul", txtJudul.Text);
                        cmd.Parameters.AddWithValue("@pengarang", txtPengarang.Text);
                        cmd.Parameters.AddWithValue("@penerbit", txtPenerbit.Text);
                        cmd.Parameters.AddWithValue("@tahun", txtTahun.Text);
                        cmd.Parameters.AddWithValue("@kategori", txtKategori.Text);
                        // [DIHAPUS] Parameter untuk status tidak lagi diperlukan
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Data buku baru berhasil disimpan dengan status 'Tersedia'.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataBuku();
                ClearFields();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show($"Gagal menyimpan: ID Buku '{txtIdBuku.Text}' sudah ada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Gagal menyimpan data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdBuku.Text))
            {
                MessageBox.Show("Pilih buku yang akan diupdate dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    // [DIUBAH] Kolom status dihapus dari perintah UPDATE. Admin tidak bisa mengubah status.
                    string query = "UPDATE Buku SET judul=@judul, pengarang=@pengarang, penerbit=@penerbit, tahun_terbit=@tahun, kategori=@kategori WHERE id_buku=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", txtIdBuku.Text);
                        cmd.Parameters.AddWithValue("@judul", txtJudul.Text);
                        cmd.Parameters.AddWithValue("@pengarang", txtPengarang.Text);
                        cmd.Parameters.AddWithValue("@penerbit", txtPenerbit.Text);
                        cmd.Parameters.AddWithValue("@tahun", txtTahun.Text);
                        cmd.Parameters.AddWithValue("@kategori", txtKategori.Text);
                        // [DIHAPUS] Parameter untuk status tidak lagi diperlukan
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Data buku berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataBuku();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mengupdate data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdBuku.Text))
            {
                MessageBox.Show("Pilih buku yang akan dihapus dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Apakah Anda yakin ingin menghapus buku ini dari daftar?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string query = "DELETE FROM Buku WHERE id_buku=@id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtIdBuku.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Data buku berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataBuku();
                    ClearFields();
                }
                catch (SqlException ex)
                {
                    // Menangani error jika buku tidak bisa dihapus karena sedang dipinjam (Foreign Key constraint)
                    if (ex.Number == 547)
                    {
                        MessageBox.Show("Gagal menghapus: Buku ini sedang dalam proses peminjaman dan tidak bisa dihapus.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Gagal menghapus data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dataGridViewBuku_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = this.dataGridViewBuku.Rows[e.RowIndex];

                    txtIdBuku.Text = row.Cells["id_buku"].Value.ToString();
                    txtJudul.Text = row.Cells["judul"].Value.ToString();
                    txtPengarang.Text = row.Cells["pengarang"].Value.ToString();
                    txtPenerbit.Text = row.Cells["penerbit"].Value.ToString();
                    txtTahun.Text = row.Cells["tahun_terbit"].Value.ToString();
                    txtKategori.Text = row.Cells["kategori"].Value.ToString();
                    // [DIHAPUS] Baris untuk mengisi cmbStatus dihapus
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi error saat memilih baris: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}