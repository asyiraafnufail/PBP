using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBP
{
    public partial class FormMenuAdmin: Form
    {
        public FormMenuAdmin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAnggota_Click(object sender, EventArgs e)
        {
            FormAnggota formAnggota = new FormAnggota();
            formAnggota.Show();
        }

        private void btnBuku_Click(object sender, EventArgs e)
        {
            FormBuku formBuku = new FormBuku();
            formBuku.Show();
        }

        private void btnPeminjaman_Click(object sender, EventArgs e)
        {
            FormPeminjaman formPeminjaman = new FormPeminjaman();
            formPeminjaman.Show();
        }
    }
}
