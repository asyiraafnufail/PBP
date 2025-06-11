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
            this.dtpPinjam.Location = new System.Drawing.Point(560, 288);
            this.dtpPinjam.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dtpPinjam.Name = "dtpPinjam";
            this.dtpPinjam.Size = new System.Drawing.Size(396, 31);
            this.dtpPinjam.TabIndex = 0;
            this.dtpPinjam.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dtpKembali
            // 
            this.dtpKembali.Location = new System.Drawing.Point(560, 483);
            this.dtpKembali.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dtpKembali.Name = "dtpKembali";
            this.dtpKembali.Size = new System.Drawing.Size(396, 31);
            this.dtpKembali.TabIndex = 1;
            this.dtpKembali.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 300);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tanggal Pinjam";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 496);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tanggal Kembali";
            // 
            // btnPinjam
            // 
            this.btnPinjam.Location = new System.Drawing.Point(502, 646);
            this.btnPinjam.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnPinjam.Name = "btnPinjam";
            this.btnPinjam.Size = new System.Drawing.Size(192, 75);
            this.btnPinjam.TabIndex = 4;
            this.btnPinjam.Text = "Pinjam";
            this.btnPinjam.UseVisualStyleBackColor = true;
            this.btnPinjam.Click += new System.EventHandler(this.button1_Click);
            // 
            // panelBuku
            // 
            this.panelBuku.AutoScroll = true;
            this.panelBuku.Location = new System.Drawing.Point(1020, 23);
            this.panelBuku.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelBuku.Name = "panelBuku";
            this.panelBuku.Size = new System.Drawing.Size(576, 896);
            this.panelBuku.TabIndex = 5;
            this.panelBuku.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Location = new System.Drawing.Point(1608, 223);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1046, 471);
            this.reportViewer1.TabIndex = 6;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(1610, 740);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(248, 90);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1990, 137);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Buku yang Dipinjam";
            // 
            // DashboardUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2564, 987);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panelBuku);
            this.Controls.Add(this.btnPinjam);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpKembali);
            this.Controls.Add(this.dtpPinjam);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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