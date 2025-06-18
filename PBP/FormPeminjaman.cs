using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormPeminjaman : Form
    {
        private readonly string connStr = "Server=localhost;Database=db_pbp;User ID=root;Password=;";

        public FormPeminjaman()
        {
            InitializeComponent();
        }

        private void FormPeminjaman_Load(object sender, EventArgs e)
        {
            TampilData();
        }

        private void TampilData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    string query = @"
                        SELECT 
                            p.id_peminjaman, 
                            p.id_anggota, 
                            a.nama AS nama_anggota, 
                            p.id_buku, 
                            b.judul AS judul_buku, 
                            p.tanggal_pinjam, 
                            p.tanggal_kembali 
                        FROM Peminjaman p
                        JOIN Anggota a ON p.id_anggota = a.id_anggota
                        JOIN Buku b ON p.id_buku = b.id_buku
                        ORDER BY p.tanggal_pinjam DESC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data peminjaman: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtIDPeminjaman.Clear();
            txtIDAnggota.Clear();
            txtIDBuku.Clear();
            dtpTanggalPinjam.Value = DateTime.Now;
            dtpTanggalKembali.Value = DateTime.Now;
            txtIDPeminjaman.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDPeminjaman.Text) || string.IsNullOrWhiteSpace(txtIDAnggota.Text) || string.IsNullOrWhiteSpace(txtIDBuku.Text))
            {
                MessageBox.Show("Semua ID (Peminjaman, Anggota, Buku) wajib diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string queryPeminjaman = "INSERT INTO Peminjaman (id_peminjaman, id_anggota, id_buku, tanggal_pinjam, tanggal_kembali) VALUES (@id, @idanggota, @idbuku, @tglpinjam, @tglkembali)";
                    using (MySqlCommand cmdPeminjaman = new MySqlCommand(queryPeminjaman, conn, transaction))
                    {
                        cmdPeminjaman.Parameters.AddWithValue("@id", txtIDPeminjaman.Text);
                        cmdPeminjaman.Parameters.AddWithValue("@idanggota", txtIDAnggota.Text);
                        cmdPeminjaman.Parameters.AddWithValue("@idbuku", txtIDBuku.Text);
                        cmdPeminjaman.Parameters.AddWithValue("@tglpinjam", dtpTanggalPinjam.Value);
                        cmdPeminjaman.Parameters.AddWithValue("@tglkembali", dtpTanggalKembali.Value);
                        cmdPeminjaman.ExecuteNonQuery();
                    }

                    string queryBuku = "UPDATE Buku SET status = 'Dipinjam' WHERE id_buku = @idbuku";
                    using (MySqlCommand cmdBuku = new MySqlCommand(queryBuku, conn, transaction))
                    {
                        cmdBuku.Parameters.AddWithValue("@idbuku", txtIDBuku.Text);
                        cmdBuku.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Data peminjaman berhasil disimpan dan status buku telah diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    TampilData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Transaksi gagal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDPeminjaman.Text))
            {
                MessageBox.Show("Pilih data peminjaman yang akan diupdate.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "UPDATE Peminjaman SET id_anggota=@idanggota, id_buku=@idbuku, tanggal_pinjam=@tglpinjam, tanggal_kembali=@tglkembali WHERE id_peminjaman=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", txtIDPeminjaman.Text);
                        cmd.Parameters.AddWithValue("@idanggota", txtIDAnggota.Text);
                        cmd.Parameters.AddWithValue("@idbuku", txtIDBuku.Text);
                        cmd.Parameters.AddWithValue("@tglpinjam", dtpTanggalPinjam.Value);
                        cmd.Parameters.AddWithValue("@tglkembali", dtpTanggalKembali.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Data peminjaman berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TampilData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mengupdate data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDPeminjaman.Text))
            {
                MessageBox.Show("Pilih data peminjaman yang akan dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Menghapus peminjaman akan mengembalikan status buku menjadi 'Tersedia'. Lanjutkan?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.No)
            {
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string queryPeminjaman = "DELETE FROM Peminjaman WHERE id_peminjaman=@id";
                    using (MySqlCommand cmdPeminjaman = new MySqlCommand(queryPeminjaman, conn, transaction))
                    {
                        cmdPeminjaman.Parameters.AddWithValue("@id", txtIDPeminjaman.Text);
                        cmdPeminjaman.ExecuteNonQuery();
                    }

                    string queryBuku = "UPDATE Buku SET status = 'Tersedia' WHERE id_buku = @idbuku";
                    using (MySqlCommand cmdBuku = new MySqlCommand(queryBuku, conn, transaction))
                    {
                        cmdBuku.Parameters.AddWithValue("@idbuku", txtIDBuku.Text);
                        cmdBuku.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Data peminjaman berhasil dihapus dan status buku telah dikembalikan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    TampilData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Transaksi gagal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    txtIDPeminjaman.Text = row.Cells["id_peminjaman"].Value.ToString();
                    txtIDAnggota.Text = row.Cells["id_anggota"].Value.ToString();
                    txtIDBuku.Text = row.Cells["id_buku"].Value.ToString();
                    dtpTanggalPinjam.Value = Convert.ToDateTime(row.Cells["tanggal_pinjam"].Value);
                    dtpTanggalKembali.Value = Convert.ToDateTime(row.Cells["tanggal_kembali"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi error saat memilih baris: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}