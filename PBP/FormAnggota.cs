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
    public partial class FormAnggota: Form
    {
        SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=PEMINJAMANBUKU;Integrated Security=True");

        public FormAnggota()
        {
            InitializeComponent();
        }

        private void FormAnggota_Load(object sender, EventArgs e)
        {

        }
        private void LoadData()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Anggota", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Anggota (id_anggota, nama, alamat, email, no_telepon) VALUES (@id, @nama, @alamat, @email, @telp)", conn);
            cmd.Parameters.AddWithValue("@id", txtID.Text);
            cmd.Parameters.AddWithValue("@nama", txtNama.Text);
            cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@telp", txtTelepon.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadData();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Anggota WHERE id_anggota=@id", conn);
            cmd.Parameters.AddWithValue("@id", txtID.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Anggota SET nama=@nama, alamat=@alamat, email=@email, no_telepon=@telp WHERE id_anggota=@id", conn);
            cmd.Parameters.AddWithValue("@id", txtID.Text);
            cmd.Parameters.AddWithValue("@nama", txtNama.Text);
            cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@telp", txtTelepon.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadData();
        }
    }
}
