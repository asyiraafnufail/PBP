namespace PBP
{
    partial class FormPendaftaran
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
            this.fieldNama = new System.Windows.Forms.TextBox();
            this.fieldAlamat = new System.Windows.Forms.TextBox();
            this.fieldEmail = new System.Windows.Forms.TextBox();
            this.fieldTelepon = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.fieldPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // fieldNama
            // 
            this.fieldNama.Location = new System.Drawing.Point(93, 111);
            this.fieldNama.Name = "fieldNama";
            this.fieldNama.Size = new System.Drawing.Size(324, 20);
            this.fieldNama.TabIndex = 0;
            this.fieldNama.TextChanged += new System.EventHandler(this.fieldNama_TextChanged);
            // 
            // fieldAlamat
            // 
            this.fieldAlamat.Location = new System.Drawing.Point(93, 172);
            this.fieldAlamat.Name = "fieldAlamat";
            this.fieldAlamat.Size = new System.Drawing.Size(324, 20);
            this.fieldAlamat.TabIndex = 1;
            this.fieldAlamat.TextChanged += new System.EventHandler(this.fieldAlamat_TextChanged);
            // 
            // fieldEmail
            // 
            this.fieldEmail.Location = new System.Drawing.Point(93, 305);
            this.fieldEmail.Name = "fieldEmail";
            this.fieldEmail.Size = new System.Drawing.Size(324, 20);
            this.fieldEmail.TabIndex = 2;
            this.fieldEmail.TextChanged += new System.EventHandler(this.fieldEmail_TextChanged);
            // 
            // fieldTelepon
            // 
            this.fieldTelepon.Location = new System.Drawing.Point(93, 241);
            this.fieldTelepon.Name = "fieldTelepon";
            this.fieldTelepon.Size = new System.Drawing.Size(324, 20);
            this.fieldTelepon.TabIndex = 3;
            this.fieldTelepon.TextChanged += new System.EventHandler(this.fieldTelepon_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(549, 253);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Silahkan daftar terlebih dahulu";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(90, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nama";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(90, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Alamat";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(90, 284);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Email";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(90, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Nomor Telepon";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(-1, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(106, 56);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "Kembali";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Lime;
            this.btnRegister.Location = new System.Drawing.Point(93, 412);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(134, 40);
            this.btnRegister.TabIndex = 10;
            this.btnRegister.Text = "Daftar";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(90, 340);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 18);
            this.label6.TabIndex = 12;
            this.label6.Text = "Password";
            // 
            // fieldPassword
            // 
            this.fieldPassword.Location = new System.Drawing.Point(93, 361);
            this.fieldPassword.Name = "fieldPassword";
            this.fieldPassword.PasswordChar = '*';
            this.fieldPassword.Size = new System.Drawing.Size(324, 20);
            this.fieldPassword.TabIndex = 11;
            // 
            // FormPendaftaran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 476);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.fieldPassword);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fieldTelepon);
            this.Controls.Add(this.fieldEmail);
            this.Controls.Add(this.fieldAlamat);
            this.Controls.Add(this.fieldNama);
            this.Name = "FormPendaftaran";
            this.Text = "FormPendaftaran";
            this.Load += new System.EventHandler(this.FormPendaftaran_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fieldNama;
        private System.Windows.Forms.TextBox fieldAlamat;
        private System.Windows.Forms.TextBox fieldEmail;
        private System.Windows.Forms.TextBox fieldTelepon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox fieldPassword;
    }
}