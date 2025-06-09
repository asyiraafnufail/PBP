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
    public partial class FormPengembalian: Form
    {
        SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=PEMINJAMANBUKU;Integrated Security=True");

        public FormPengembalian()
        {
            InitializeComponent();
        }

        private void FormPengembalian_Load(object sender, EventArgs e)
        {

        }
        private void TampilData()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Pengembalian", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Pengembalian VALUES(@id, @idpeminjaman, @tglkembali, @denda)", conn);
            cmd.Parameters.AddWithValue("@id", txtIDPengembalian.Text);
            cmd.Parameters.AddWithValue("@idpeminjaman", txtIDPeminjaman.Text);
            cmd.Parameters.AddWithValue("@tglkembali", dtpTanggalKembali.Value);
            cmd.Parameters.AddWithValue("@denda", txtDenda.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            TampilData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Pengembalian SET id_peminjaman=@idpeminjaman, tanggal_pengembalian=@tglkembali, denda=@denda WHERE id_pengembalian=@id", conn);
            cmd.Parameters.AddWithValue("@id", txtIDPengembalian.Text);
            cmd.Parameters.AddWithValue("@idpeminjaman", txtIDPeminjaman.Text);
            cmd.Parameters.AddWithValue("@tglkembali", dtpTanggalKembali.Value);
            cmd.Parameters.AddWithValue("@denda", txtDenda.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            TampilData();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Pengembalian WHERE id_pengembalian=@id", conn);
            cmd.Parameters.AddWithValue("@id", txtIDPengembalian.Text);
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
