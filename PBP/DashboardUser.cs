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
        // Gunakan satu connection string ini untuk semua operasi database
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
            using (var cmd = new SqlCommand("SELECT id_buku, judul, status FROM Buku", conn))
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

        private void BukuButton_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            _selectedBookId = (int)btn.Tag;

            if (_lastSelectedButton != null)
                _lastSelectedButton.BackColor = SystemColors.ControlLight;

            btn.BackColor = SystemColors.Highlight;
            _lastSelectedButton = btn;
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void SetupReportViewer()
        {
            // 1. Menyiapkan query SQL untuk mengambil data yang akan dilaporkan
            string query = @"SELECT 
                         b.judul, b.pengarang, b.penerbit, b.tahun_terbit, 
                         b.kategori, b.status, p.tanggal_pinjam, p.tanggal_kembali
                     FROM Peminjaman AS p
                     INNER JOIN Buku AS b ON p.id_buku = b.id_buku";

            DataTable dt = new DataTable();

            // 2. Mengambil data dari database dan mengisinya ke dalam DataTable
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            // 3. Membuat sumber data untuk laporan
            // "DataSet1" harus sama dengan nama DataSet yang Anda buat di file Report.rdlc
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            // 4. Mengkonfigurasi ReportViewer
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.ReportPath = @"C:\Users\Asyiraaf\Documents\PABD\Fix\PBP\Report1.rdlc";

            // 5. Merefresh laporan agar data baru ditampilkan
            reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Awal (Tidak Berubah) ---
            if (_selectedBookId < 0)
            {
                MessageBox.Show("Pilih dulu buku yang akan dipinjam.", "Peminjaman", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tPinjam = dtpPinjam.Value.Date;
            DateTime tKembali = dtpKembali.Value.Date;

            if (tPinjam < DateTime.Today)
            {
                MessageBox.Show("Tanggal peminjaman tidak boleh dipilih sebelum tanggal hari ini.",
                                "Tanggal Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hentikan proses jika tidak valid
            }   

            if (tKembali > tPinjam.AddMonths(6))
            {
                MessageBox.Show("Durasi peminjaman tidak boleh lebih dari 6 bulan.",
                                "Durasi Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hentikan proses jika tidak valid
            }

            if (tKembali < tPinjam)
            {
                MessageBox.Show("Tanggal kembali harus sama atau setelah tanggal pinjam.", "Peminjaman", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // --- Akhir Validasi Input ---

            // Menggunakan 'using' untuk memastikan koneksi selalu ditutup, bahkan saat error.
            using (var conn = new SqlConnection(connStr))
            {
                SqlTransaction transaction = null; // Deklarasikan variabel transaksi di sini

                try
                {
                    // 1. Buka koneksi ke database
                    conn.Open();

                    // 2. Mulai sebuah transaksi
                    transaction = conn.BeginTransaction();

                    // Siapkan command untuk dieksekusi di dalam transaksi
                    using (var cmd = new SqlCommand("InsertPeminjaman", conn, transaction)) // Sertakan transaksi di command
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_anggota", _anggotaId);
                        cmd.Parameters.AddWithValue("@id_buku", _selectedBookId);
                        cmd.Parameters.AddWithValue("@tanggal_pinjam", tPinjam);
                        cmd.Parameters.AddWithValue("@tanggal_kembali", tKembali);

                        // Eksekusi perintah. Jika ini gagal, akan loncat ke blok 'catch'
                        cmd.ExecuteNonQuery();
                    }

                    // 3. COMMIT: Jika semua perintah di atas berhasil tanpa error,
                    // simpan perubahan secara permanen ke database.
                    transaction.Commit();

                    // Tampilkan pesan sukses HANYA SETELAH commit berhasil
                    MessageBox.Show("Peminjaman berhasil dicatat!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset tampilan (UI) setelah data dipastikan tersimpan
                    // === RESET SEMUA STATE ===
                    LoadDaftarBuku();
                    _selectedBookId = -1;
                    if (_lastSelectedButton != null)
                    {
                        _lastSelectedButton.BackColor = SystemColors.ControlLight;
                        _lastSelectedButton = null;
                    }
                    dtpPinjam.Value = DateTime.Today;
                    dtpKembali.Value = DateTime.Today;
                    panelBuku.AutoScrollPosition = new Point(0, 0);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Terjadi kesalahan database. Perubahan dibatalkan.\n\nError: {ex.Message}",
                                    "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    try
                    {
                        // 4. ROLLBACK: Jika terjadi error di blok 'try',
                        // batalkan SEMUA perubahan yang sudah terjadi dalam transaksi ini.
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception exRollback)
                    {
                        // Menangani kasus langka di mana proses rollback itu sendiri gagal
                        MessageBox.Show($"Kesalahan kritis saat mencoba melakukan rollback!\n\nError: {exRollback.Message}",
                                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                finally
                {
                    // Koneksi akan ditutup secara otomatis oleh blok 'using'
                    // jadi 'conn.Close()' tidak wajib di sini.
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // 1. Memastikan data laporan adalah yang paling baru
            SetupReportViewer();
            reportViewer1.RefreshReport();

            // 2. Menyiapkan variabel untuk proses render
            string mimeType, encoding, extension;
            string[] streamIds;
            Warning[] warnings;

            // 3. Merender laporan menjadi format "EXCEL" dalam bentuk byte array
            byte[] bytes = reportViewer1.LocalReport.Render(
                format: "EXCEL",
                deviceInfo: null,
                mimeType: out mimeType,
                encoding: out encoding,
                fileNameExtension: out extension,
                streams: out streamIds,
                warnings: out warnings);

            // 4. Menentukan lokasi dan nama file untuk hasil ekspor
            string customFolder = @"C:\Users\Asyiraaf\Documents\PABD\Fix\HasilExport\";
            string fileName = $"LaporanPeminjaman_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
            string path = Path.Combine(customFolder, fileName);

            // 5. Menulis byte array ke dalam file dan menampilkan pesan sukses
            File.WriteAllBytes(path, bytes);
            MessageBox.Show($"File disimpan di:\n{path}",
                            "Export Selesai",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}