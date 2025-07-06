using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormBuku : Form
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["PBP.Properties.Settings.PBPConnectionString"].ConnectionString;
        private bool isNavigatingBack = false;

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
            txtIdBuku.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {

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
                    using (SqlCommand cmd = new SqlCommand("UpdateBuku", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_id_buku", txtIdBuku.Text);
                        cmd.Parameters.AddWithValue("@p_judul", txtJudul.Text);
                        cmd.Parameters.AddWithValue("@p_pengarang", txtPengarang.Text);
                        cmd.Parameters.AddWithValue("@p_penerbit", txtPenerbit.Text);
                        cmd.Parameters.AddWithValue("@p_tahun", txtTahun.Text);
                        cmd.Parameters.AddWithValue("@p_kategori", txtKategori.Text);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi error saat memilih baris: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormBuku_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !isNavigatingBack)
            {
                DialogResult result = MessageBox.Show("Apakah Anda yakin ingin keluar dari aplikasi?", "Konfirmasi Keluar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            FormMenuAdmin adminDashboard = Application.OpenForms.OfType<FormMenuAdmin>().FirstOrDefault();
            if (adminDashboard != null)
            {
                adminDashboard.Show();
            }

            this.isNavigatingBack = true;
            this.Close();
        }
    }
}