namespace PBP
{
    partial class DashboardUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpPinjam = new System.Windows.Forms.DateTimePicker();
            this.dtpKembali = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPinjam = new System.Windows.Forms.Button();
            this.panelBuku = new System.Windows.Forms.Panel();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnExport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dtpPinjam
            // 
            this.dtpPinjam.Location = new System.Drawing.Point(280, 150);
            this.dtpPinjam.Name = "dtpPinjam";
            this.dtpPinjam.Size = new System.Drawing.Size(200, 20);
            this.dtpPinjam.TabIndex = 0;
            this.dtpPinjam.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dtpKembali
            // 
            this.dtpKembali.Location = new System.Drawing.Point(280, 251);
            this.dtpKembali.Name = "dtpKembali";
            this.dtpKembali.Size = new System.Drawing.Size(200, 20);
            this.dtpKembali.TabIndex = 1;
            this.dtpKembali.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tanggal Pinjam";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tanggal Kembali";
            // 
            // btnPinjam
            // 
            this.btnPinjam.Location = new System.Drawing.Point(251, 336);
            this.btnPinjam.Name = "btnPinjam";
            this.btnPinjam.Size = new System.Drawing.Size(96, 39);
            this.btnPinjam.TabIndex = 4;
            this.btnPinjam.Text = "Pinjam";
            this.btnPinjam.UseVisualStyleBackColor = true;
            this.btnPinjam.Click += new System.EventHandler(this.button1_Click);
            // 
            // panelBuku
            // 
            this.panelBuku.Location = new System.Drawing.Point(510, 12);
            this.panelBuku.Name = "panelBuku";
            this.panelBuku.Size = new System.Drawing.Size(288, 466);
            this.panelBuku.TabIndex = 5;
            this.panelBuku.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Location = new System.Drawing.Point(804, 116);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(524, 246);
            this.reportViewer1.TabIndex = 6;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(805, 385);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(124, 47);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(995, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Buku yang Dipinjam";
            // 
            // DashboardUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 513);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panelBuku);
            this.Controls.Add(this.btnPinjam);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpKembali);
            this.Controls.Add(this.dtpPinjam);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DashboardUser";
            this.Text = "DashboardUser";
            this.Load += new System.EventHandler(this.DashboardUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpPinjam;
        private System.Windows.Forms.DateTimePicker dtpKembali;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPinjam;
        private System.Windows.Forms.Panel panelBuku;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label3;
    }
}