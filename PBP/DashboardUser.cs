using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        private readonly int _anggotaId;
        private int _selectedBookId = -1;
        private Button _lastSelectedButton = null;
        string connStr = @"Data Source=DESKTOP-A3U4VR2\SQLEXPRESS;Initial Catalog=PBP;Integrated Security=True";
        public DashboardUser(int anggotaId)
        {
            InitializeComponent();
            _anggotaId = anggotaId;
        }

        private void DashboardUser_Load(object sender, EventArgs e)
        {
            LoadDaftarBuku();
            SetupReportViewer();
            this.reportViewer1.RefreshReport();
        }

        private void LoadDaftarBuku()
        {
            panelBuku.Controls.Clear();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(
                "SELECT id_buku, judul, status FROM Buku", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int x = 10, y = 10;
                    const int btnWidth = 200, btnHeight = 40, margin = 10;

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string judul = reader.GetString(1);
                        string status = reader.GetString(2);

                        var btn = new Button
                        {
                            Width = btnWidth,
                            Height = btnHeight,
                            Left = x,
                            Top = y,
                            Text = $"{judul} [{status}]",
                            Tag = id,
                            BackColor = status == "Tersedia"
                                        ? SystemColors.ControlLight
                                        : SystemColors.ControlDark,
                            Enabled = status == "Tersedia"
                        };
                        btn.Click += BukuButton_Click;

                        panelBuku.Controls.Add(btn);
                        y += btnHeight + margin;
                    }
                }
            }
        }

        private void BukuButton_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            _selectedBookId = (int)btn.Tag;

            // highlight pilihan
            if (_lastSelectedButton != null)
                _lastSelectedButton.BackColor = SystemColors.ControlLight;

            btn.BackColor = SystemColors.Highlight;
            _lastSelectedButton = btn;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SetupReportViewer()
        {
            string query = @"SELECT 
                            b.judul, 
                            b.pengarang, 
                            b.penerbit, 
                            b.tahun_terbit, 
                            b.kategori, 
                            b.status,
                            p.tanggal_pinjam,
                            p.tanggal_kembali
                        FROM Peminjaman AS p
                        INNER JOIN Buku AS b 
                            ON p.id_buku = b.id_buku";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.LocalReport.ReportPath = @"C:\Users\Asyiraaf\Documents\PABD\Fix\PBP\Report1.rdlc";
            reportViewer1.RefreshReport();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_selectedBookId < 0)
            {
                MessageBox.Show("Pilih dulu buku yang akan dipinjam.",
                                "Peminjaman", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tPinjam = dtpPinjam.Value.Date;
            DateTime tKembali = dtpKembali.Value.Date;

            if (tKembali < tPinjam)
            {
                MessageBox.Show("Tanggal kembali harus sama atau setelah tanggal pinjam.",
                                "Peminjaman", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = @"Data Source=MSI\ABRA;Initial Catalog=PBP;Integrated Security=True;";

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("InsertPeminjaman", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_anggota", _anggotaId);
                cmd.Parameters.AddWithValue("@id_buku", _selectedBookId);
                cmd.Parameters.AddWithValue("@tanggal_pinjam", tPinjam);
                cmd.Parameters.AddWithValue("@tanggal_kembali", tKembali);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Peminjaman berhasil dicatat!",
                                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // === RESET SEMUA STATE ===
                    // 1. Reload buku agar status ter-update
                    LoadDaftarBuku();

                    // 2. Reset pilihan buku
                    _selectedBookId = -1;
                    if (_lastSelectedButton != null)
                    {
                        _lastSelectedButton.BackColor = SystemColors.ControlLight;
                        _lastSelectedButton = null;
                    }

                    // 3. Reset tanggal pickers ke hari ini
                    dtpPinjam.Value = DateTime.Today;
                    dtpKembali.Value = DateTime.Today;

                    // 4. Scroll panelBuku ke atas (opsional)
                    panelBuku.AutoScrollPosition = new Point(0, 0);
                    // ===========================

                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Gagal melakukan peminjaman:\n{ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SetupReportViewer();
            reportViewer1.RefreshReport();
            string mimeType, encoding, extension;
            string[] streamIds;
            Warning[] warnings;
            byte[] bytes = reportViewer1.LocalReport.Render(
                format: "EXCEL",
                deviceInfo: null,
                mimeType: out mimeType,
                encoding: out encoding,
                fileNameExtension: out extension,
                streams: out streamIds,
                warnings: out warnings);

            string customFolder = @"C:\Users\Asyiraaf\Documents\PABD\Fix\HasilExport\";

            string fileName = $"LaporanPeminjaman_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
            string path = Path.Combine(customFolder, fileName);

            File.WriteAllBytes(path, bytes);
            MessageBox.Show($"File disimpan di:\n{path}",
                            "Export Selesai",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}
