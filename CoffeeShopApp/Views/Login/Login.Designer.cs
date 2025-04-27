using System.Windows.Forms;

namespace CoffeeShopApp.Views.Login
{
    partial class Login
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
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.lnkForgot = new System.Windows.Forms.LinkLabel();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.picUser = new System.Windows.Forms.PictureBox();
            this.pnlLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlLogin.Controls.Add(this.label1);
            this.pnlLogin.Controls.Add(this.lnkRegister);
            this.pnlLogin.Controls.Add(this.lnkForgot);
            this.pnlLogin.Controls.Add(this.chkRemember);
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Controls.Add(this.txtPassword);
            this.pnlLogin.Controls.Add(this.txtUsername);
            this.pnlLogin.Controls.Add(this.picUser);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 0);
            this.pnlLogin.Margin = new System.Windows.Forms.Padding(2);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(322, 402);
            this.pnlLogin.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Minion Pro SmBd", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 26);
            this.label1.TabIndex = 8;
            this.label1.Text = "ĐĂNG NHẬP TÀI KHOẢN";
            // 
            // lnkRegister
            // 
            this.lnkRegister.ActiveLinkColor = System.Drawing.Color.Blue;
            this.lnkRegister.AutoSize = true;
            this.lnkRegister.BackColor = System.Drawing.Color.Transparent;
            this.lnkRegister.Font = new System.Drawing.Font("Minion Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkRegister.LinkArea = new System.Windows.Forms.LinkArea(19, 12);
            this.lnkRegister.LinkColor = System.Drawing.Color.DarkBlue;
            this.lnkRegister.Location = new System.Drawing.Point(76, 348);
            this.lnkRegister.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(170, 19);
            this.lnkRegister.TabIndex = 7;
            this.lnkRegister.TabStop = true;
            this.lnkRegister.Text = "Chưa có tài khoản? Đăng ký ngay";
            this.lnkRegister.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkRegister.UseCompatibleTextRendering = true;
            this.lnkRegister.VisitedLinkColor = System.Drawing.Color.DarkBlue;
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkRegister_LinkClicked);
            // 
            // lnkForgot
            // 
            this.lnkForgot.ActiveLinkColor = System.Drawing.Color.Blue;
            this.lnkForgot.AutoSize = true;
            this.lnkForgot.BackColor = System.Drawing.Color.Transparent;
            this.lnkForgot.Font = new System.Drawing.Font("Minion Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkForgot.LinkColor = System.Drawing.Color.DarkBlue;
            this.lnkForgot.Location = new System.Drawing.Point(153, 258);
            this.lnkForgot.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkForgot.Name = "lnkForgot";
            this.lnkForgot.Size = new System.Drawing.Size(152, 16);
            this.lnkForgot.TabIndex = 6;
            this.lnkForgot.TabStop = true;
            this.lnkForgot.Text = "FORGET THE PASSWORD ?";
            this.lnkForgot.VisitedLinkColor = System.Drawing.Color.DarkBlue;
            // 
            // chkRemember
            // 
            this.chkRemember.AutoSize = true;
            this.chkRemember.BackColor = System.Drawing.Color.Transparent;
            this.chkRemember.Font = new System.Drawing.Font("Minion Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemember.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkRemember.Location = new System.Drawing.Point(30, 258);
            this.chkRemember.Margin = new System.Windows.Forms.Padding(2);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(111, 20);
            this.chkRemember.TabIndex = 4;
            this.chkRemember.Text = "REMEMBER ME";
            this.chkRemember.UseVisualStyleBackColor = false;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Silver;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Minion Pro", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.Black;
            this.btnLogin.Location = new System.Drawing.Point(30, 298);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(262, 37);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.Location = new System.Drawing.Point(30, 209);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(262, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "PASSWORD";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.ForeColor = System.Drawing.Color.Gray;
            this.txtUsername.Location = new System.Drawing.Point(30, 152);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Multiline = true;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(262, 32);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Text = "USER NAME / ID";
            // 
            // picUser
            // 
            this.picUser.BackColor = System.Drawing.Color.Transparent;
            this.picUser.Location = new System.Drawing.Point(131, 73);
            this.picUser.Margin = new System.Windows.Forms.Padding(2);
            this.picUser.Name = "picUser";
            this.picUser.Size = new System.Drawing.Size(60, 65);
            this.picUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUser.TabIndex = 0;
            this.picUser.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 402);
            this.Controls.Add(this.pnlLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.PictureBox picUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.LinkLabel lnkForgot;
        private System.Windows.Forms.LinkLabel lnkRegister;
        private Label label1;
    }
}