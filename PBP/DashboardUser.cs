using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PBP
{
    public partial class DashboardUser : Form
    {
        private bool isLoggingOut = false;
        private readonly int _anggotaId;
        private int _selectedBookId = -1;
        private Button _lastSelectedButton = null;

        private readonly string connStr = ConfigurationManager.ConnectionStrings["PBP.Properties.Settings.PBPConnectionString"].ConnectionString;

        public DashboardUser(int anggotaId)
        {
            InitializeComponent();
            _anggotaId = anggotaId;
        }

        private void DashboardUser_Load(object sender, EventArgs e)
        {
            dtpPinjam.MinDate = DateTime.Today;
            dtpKembali.MinDate = DateTime.Today;
            LoadDaftarBuku();
            SetupReportViewer();
        }

        private void btnPinjam_Click(object sender, EventArgs e)
        {
            if (_selectedBookId < 0)
            {
                MessageBox.Show("Pilih dulu buku yang akan dipinjam.", "Peminjaman", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tKembali = dtpKembali.Value.Date;

            if (tKembali < DateTime.Today)
            {
                MessageBox.Show("Tanggal kembali tidak boleh sebelum hari ini.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    using (var cmd = new SqlCommand("InsertPeminjaman", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_anggota", _anggotaId);
                        cmd.Parameters.AddWithValue("@id_buku", _selectedBookId);
                        cmd.Parameters.AddWithValue("@tanggal_kembali", tKembali);
                        cmd.ExecuteNonQuery();
                    }
                }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal melakukan peminjaman:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDaftarBuku()
        {
            panelBuku.Controls.Clear();
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("SELECT id_buku, judul, status FROM Buku ORDER BY judul ASC", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        int x = 10, y = 10;
                        const int btnWidth = 200, btnHeight = 40, margin = 10;

                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id_buku"]);
                            string judul = reader["judul"].ToString();
                            string status = reader["status"].ToString();

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

        private void SetupReportViewer()
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"SELECT b.judul, b.pengarang, b.penerbit, b.tahun_terbit, 
                               b.kategori, b.status, p.tanggal_pinjam, p.tanggal_kembali
                        FROM Peminjaman AS p
                        INNER JOIN Buku AS b ON p.id_buku = b.id_buku"; // <--- Query dioptimalkan oleh Indeks
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(dt);
                }

                ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                string exePath = Path.GetDirectoryName(Application.ExecutablePath);
                string reportPath = Path.Combine(exePath, "Report1.rdlc");
                reportViewer1.LocalReport.ReportPath = reportPath;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyiapkan laporan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SetupReportViewer();
                string mimeType, encoding, extension;
                Warning[] warnings;
                byte[] bytes = reportViewer1.LocalReport.Render("EXCEL", null, out mimeType, out encoding, out extension, out _, out warnings);
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
                btnTestConnection newLogin = new btnTestConnection();
                newLogin.Show();
            }
            this.isLoggingOut = true;
            this.Close();
        }
    }
}