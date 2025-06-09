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
    public partial class FormPeminjaman: Form
    {
        SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=PEMINJAMANBUKU;Integrated Security=True");

        public FormPeminjaman()
        {
            InitializeComponent();
        }

        private void FormPeminjaman_Load(object sender, EventArgs e)
        {

        }
        private void TampilData()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Peminjaman", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Peminjaman VALUES(@id, @idanggota, @idbuku, @tglpinjam, @tglkembali)", conn);
            cmd.Parameters.AddWithValue("@id", txtIDPeminjaman.Text);
            cmd.Parameters.AddWithValue("@idanggota", txtIDAnggota.Text);
            cmd.Parameters.AddWithValue("@idbuku", txtIDBuku.Text);
            cmd.Parameters.AddWithValue("@tglpinjam", dtpTanggalPinjam.Value);
            cmd.Parameters.AddWithValue("@tglkembali", dtpTanggalKembali.Value);
            cmd.ExecuteNonQuery();
            conn.Close();
            TampilData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Peminjaman SET id_anggota=@idanggota, id_buku=@idbuku, tanggal_pinjam=@tglpinjam, tanggal_kembali=@tglkembali WHERE id_peminjaman=@id", conn);
            cmd.Parameters.AddWithValue("@idpeminjaman", txtIDPeminjaman.Text);
            cmd.Parameters.AddWithValue("@idanggota", txtIDAnggota.Text);
            cmd.Parameters.AddWithValue("@idbuku", txtIDBuku.Text);
            cmd.Parameters.AddWithValue("@tglpinjam", dtpTanggalPinjam.Value);
            cmd.Parameters.AddWithValue("@tglkembali", dtpTanggalKembali.Value);
            cmd.ExecuteNonQuery();
            conn.Close();
            TampilData();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Peminjaman WHERE id_peminjaman=@id", conn);
            cmd.Parameters.AddWithValue("@id", txtIDPeminjaman.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            TampilData();
        }

        private void btnTampil_Click(object sender, EventArgs e)
        {
            TampilData();
        }

    }
}
