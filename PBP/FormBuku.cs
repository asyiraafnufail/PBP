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
    public partial class FormBuku: Form
    {
        SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=PEMINJAMANBUKU;Integrated Security=True");
        public FormBuku()
        {
            InitializeComponent();
        }

        private void FormBuku_Load(object sender, EventArgs e)
        {

        }
        private void LoadDataBuku()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Buku", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewBuku.DataSource = dt;
            conn.Close();
        }
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Buku VALUES(@id, @judul, @pengarang, @penerbit, @tahun, @kategori, @status)", conn);
            cmd.Parameters.AddWithValue("@id", txtIdBuku.Text);
            cmd.Parameters.AddWithValue("@judul", txtJudul.Text);
            cmd.Parameters.AddWithValue("@pengarang", txtPengarang.Text);
            cmd.Parameters.AddWithValue("@penerbit", txtPenerbit.Text);
            cmd.Parameters.AddWithValue("@tahun", txtTahun.Text);
            cmd.Parameters.AddWithValue("@kategori", txtKategori.Text);
            cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadDataBuku();
            MessageBox.Show("Data berhasil disimpan");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Buku SET judul=@judul, pengarang=@pengarang, penerbit=@penerbit, tahun_terbit=@tahun, kategori=@kategori, status=@status WHERE id_buku=@id", conn);
            cmd.Parameters.AddWithValue("@id", txtIdBuku.Text);
            cmd.Parameters.AddWithValue("@judul", txtJudul.Text);
            cmd.Parameters.AddWithValue("@pengarang", txtPengarang.Text);
            cmd.Parameters.AddWithValue("@penerbit", txtPenerbit.Text);
            cmd.Parameters.AddWithValue("@tahun", txtTahun.Text);
            cmd.Parameters.AddWithValue("@kategori", txtKategori.Text);
            cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadDataBuku();
            MessageBox.Show("Data berhasil diupdate");
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Buku WHERE id_buku=@id", conn);
            cmd.Parameters.AddWithValue("@id", txtIdBuku.Text);
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadDataBuku();
            MessageBox.Show("Data berhasil dihapus");
        }

        private void dataGridViewBuku_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIdBuku.Text = dataGridViewBuku.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtJudul.Text = dataGridViewBuku.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPengarang.Text = dataGridViewBuku.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPenerbit.Text = dataGridViewBuku.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtTahun.Text = dataGridViewBuku.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtKategori.Text = dataGridViewBuku.Rows[e.RowIndex].Cells[5].Value.ToString();
            cmbStatus.Text = dataGridViewBuku.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {

        }
    }
}
