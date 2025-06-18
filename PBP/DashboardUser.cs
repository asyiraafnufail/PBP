using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBP
{
    public partial class DashboardUser : Form
    {
        private bool isLoggingOut = false;

        private readonly int _anggotaId;
        private int _selectedBookId = -1;
        private Button _lastSelectedButton = null;

        private readonly string connStr = "Server=localhost;Database=db_pbp;User ID=root;Password=;";

        public DashboardUser(int anggotaId)
        {
            InitializeComponent();
            _anggotaId = anggotaId;
        }

        private void DashboardUser_Load(object sender, EventArgs e)
        {
            LoadDaftarBuku();
            SetupReportViewer();
        }

        private void LoadDaftarBuku()
        {
            panelBuku.Controls.Clear();
            try
            {
                using (var conn = new MySqlConnection(connStr))
                using (var cmd = new MySqlCommand("SELECT id_buku, judul, status FROM Buku ORDER BY judul ASC", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        int x = 10, y = 10;
                        const int btnWidth = 200, btnHeight = 40, margin = 10;

                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id_buku");
                            string judul = reader.GetString("judul");
                            string status = reader.GetString("status");

                            var btn = new Button
                            {
                                Width = btnWidth,
                                Height = btnHeight,
                                Left = x,
                                Top = y,
                                Text = $"{judul} [{status}]",
                                Tag = id,
                                BackColor = status == "Tersedia" ? SystemColors.ControlLight : SystemColors.ControlDark,
                                Enabled = status == "Tersedia"
                            };
                            btn.Click += BukuButton_Click;
                            panelBuku.Controls.Add(btn);
                            y += btnHeight + margin;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat daftar buku: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BukuButton_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            _selectedBookId = (int)btn.Tag;

            if (_lastSelectedButton != null && _lastSelectedButton.Enabled)
            {
                _lastSelectedButton.BackColor = SystemColors.ControlLight;
            }

            btn.BackColor = SystemColors.Highlight;
            _lastSelectedButton = btn;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void SetupReportViewer()
        {
            try
            {
                DataTable dt = new DataTable();

                string query = @"
                    SELECT b.judul, b.pengarang, b.penerbit, b.tahun_terbit, 
                           b.kategori, b.status, p.tanggal_pinjam, p.tanggal_kembali
                    FROM Peminjaman AS p
                    INNER JOIN Buku AS b ON p.id_buku = b.id_buku";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    da.Fill(dt);
                }

                ReportDataSource rds = new ReportDataSource("DataSet1", dt);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.LocalReport.ReportPath = @"C:\Users\Asyiraaf\Documents\PABD\Fix\PBP\Report1.rdlc";
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyiapkan laporan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_selectedBookId < 0)
            {
                MessageBox.Show("Pilih dulu buku yang akan dipinjam.", "Peminjaman", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tKembali = dtpKembali.Value.Date;

            if (tKembali < DateTime.Today)
            {
                MessageBox.Show("Tanggal kembali tidak boleh di masa lalu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new MySqlConnection(connStr))
            {
                using (var cmd = new MySqlCommand("InsertPeminjaman", conn))
                {
                    MySqlTransaction transaction = null;
                    try
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();
                        cmd.Transaction = transaction;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id_anggota", _anggotaId);
                        cmd.Parameters.AddWithValue("p_id_buku", _selectedBookId);
                        cmd.Parameters.AddWithValue("p_tanggal_kembali", tKembali);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();

                        MessageBox.Show("Peminjaman berhasil dicatat!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDaftarBuku();
                        SetupReportViewer();

                        _selectedBookId = -1;
                        if (_lastSelectedButton != null)
                        {
                            _lastSelectedButton.BackColor = SystemColors.ControlLight;
                            _lastSelectedButton = null;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show($"Gagal melakukan peminjaman:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show($"Terjadi kesalahan umum:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SetupReportViewer();

                string mimeType, encoding, extension;
                Warning[] warnings;
                byte[] bytes = reportViewer1.LocalReport.Render(
                    "EXCEL", null, out mimeType, out encoding, out extension, out _, out warnings);

                string customFolder = @"C:\Users\Asyiraaf\Documents\PABD\Fix\HasilExport\";
                if (!Directory.Exists(customFolder))
                {
                    Directory.CreateDirectory(customFolder);
                }
                string fileName = $"LaporanPeminjaman_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
                string path = Path.Combine(customFolder, fileName);

                File.WriteAllBytes(path, bytes);
                MessageBox.Show($"File berhasil diekspor di:\n{path}", "Export Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mengekspor laporan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DashboardUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !this.isLoggingOut)
            {
                DialogResult result = MessageBox.Show("Apakah Anda yakin ingin keluar dari aplikasi?", "Konfirmasi Keluar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            Form loginForm = Application.OpenForms["FormLogin"];

            if (loginForm != null)
            {
                loginForm.Show();
            }
            else
            {
                FormLogin newLogin = new FormLogin();
                newLogin.Show();
            }

            this.isLoggingOut = true;
            this.Close();
        }
    }
}