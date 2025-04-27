namespace CoffeeShopApp.Views.Admin
{
    partial class AdminDashboard
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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.ptbUser = new System.Windows.Forms.PictureBox();
            this.lbUser = new System.Windows.Forms.Label();
            this.panelUser = new System.Windows.Forms.Panel();
            this.btnInventory = new System.Windows.Forms.Button();
            this.btnCategory = new System.Windows.Forms.Button();
            this.btnUser = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnSession = new System.Windows.Forms.Button();
            this.btnTable = new System.Windows.Forms.Button();
            this.btnBill = new System.Windows.Forms.Button();
            this.panelSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbUser)).BeginInit();
            this.panelUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panelSidebar.Controls.Add(this.btnBill);
            this.panelSidebar.Controls.Add(this.btnTable);
            this.panelSidebar.Controls.Add(this.btnSession);
            this.panelSidebar.Controls.Add(this.btnMenu);
            this.panelSidebar.Controls.Add(this.btnUser);
            this.panelSidebar.Controls.Add(this.btnCategory);
            this.panelSidebar.Controls.Add(this.btnInventory);
            this.panelSidebar.Controls.Add(this.panelUser);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(250, 668);
            this.panelSidebar.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.LightGray;
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(250, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(811, 100);
            this.panelHeader.TabIndex = 0;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(250, 100);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(811, 568);
            this.panelMain.TabIndex = 1;
            // 
            // ptbUser
            // 
            this.ptbUser.Location = new System.Drawing.Point(75, 20);
            this.ptbUser.Name = "ptbUser";
            this.ptbUser.Size = new System.Drawing.Size(100, 100);
            this.ptbUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbUser.TabIndex = 0;
            this.ptbUser.TabStop = false;
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUser.ForeColor = System.Drawing.Color.White;
            this.lbUser.Location = new System.Drawing.Point(70, 130);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(115, 22);
            this.lbUser.TabIndex = 1;
            this.lbUser.Text = "Lê Minh Vương";
            // 
            // panelUser
            // 
            this.panelUser.Controls.Add(this.ptbUser);
            this.panelUser.Controls.Add(this.lbUser);
            this.panelUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUser.Location = new System.Drawing.Point(0, 0);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(250, 180);
            this.panelUser.TabIndex = 2;
            // 
            // btnInventory
            // 
            this.btnInventory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnInventory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInventory.FlatAppearance.BorderSize = 0;
            this.btnInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInventory.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInventory.ForeColor = System.Drawing.Color.White;
            this.btnInventory.Location = new System.Drawing.Point(0, 180);
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnInventory.Size = new System.Drawing.Size(250, 60);
            this.btnInventory.TabIndex = 3;
            this.btnInventory.Text = "📦 Nguyên liệu";
            this.btnInventory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInventory.UseVisualStyleBackColor = false;
            this.btnInventory.Click += new System.EventHandler(this.btnInventory_Click);
            // 
            // btnCategory
            // 
            this.btnCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnCategory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCategory.FlatAppearance.BorderSize = 0;
            this.btnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCategory.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCategory.ForeColor = System.Drawing.Color.White;
            this.btnCategory.Location = new System.Drawing.Point(0, 240);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnCategory.Size = new System.Drawing.Size(250, 60);
            this.btnCategory.TabIndex = 4;
            this.btnCategory.Text = "📂 Danh mục";
            this.btnCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCategory.UseVisualStyleBackColor = false;
            this.btnCategory.Click += new System.EventHandler(this.btnCategory_Click);
            // 
            // btnUser
            // 
            this.btnUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUser.FlatAppearance.BorderSize = 0;
            this.btnUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUser.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUser.ForeColor = System.Drawing.Color.White;
            this.btnUser.Location = new System.Drawing.Point(0, 300);
            this.btnUser.Name = "btnUser";
            this.btnUser.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnUser.Size = new System.Drawing.Size(250, 60);
            this.btnUser.TabIndex = 5;
            this.btnUser.Text = "👤 Nhân viên";
            this.btnUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUser.UseVisualStyleBackColor = false;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.ForeColor = System.Drawing.Color.White;
            this.btnMenu.Location = new System.Drawing.Point(0, 360);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenu.Size = new System.Drawing.Size(250, 60);
            this.btnMenu.TabIndex = 6;
            this.btnMenu.Text = "🍽 Menu";
            this.btnMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnSession
            // 
            this.btnSession.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnSession.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSession.FlatAppearance.BorderSize = 0;
            this.btnSession.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSession.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSession.ForeColor = System.Drawing.Color.White;
            this.btnSession.Location = new System.Drawing.Point(0, 420);
            this.btnSession.Name = "btnSession";
            this.btnSession.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnSession.Size = new System.Drawing.Size(250, 60);
            this.btnSession.TabIndex = 7;
            this.btnSession.Text = "🪑 Phiên bàn";
            this.btnSession.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSession.UseVisualStyleBackColor = false;
            this.btnSession.Click += new System.EventHandler(this.btnSession_Click);
            // 
            // btnTable
            // 
            this.btnTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTable.FlatAppearance.BorderSize = 0;
            this.btnTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTable.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTable.ForeColor = System.Drawing.Color.White;
            this.btnTable.Location = new System.Drawing.Point(0, 480);
            this.btnTable.Name = "btnTable";
            this.btnTable.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnTable.Size = new System.Drawing.Size(250, 60);
            this.btnTable.TabIndex = 8;
            this.btnTable.Text = "📋 Bàn";
            this.btnTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTable.UseVisualStyleBackColor = false;
            this.btnTable.Click += new System.EventHandler(this.btnTable_Click);
            // 
            // btnBill
            // 
            this.btnBill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnBill.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBill.FlatAppearance.BorderSize = 0;
            this.btnBill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBill.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBill.ForeColor = System.Drawing.Color.White;
            this.btnBill.Location = new System.Drawing.Point(0, 540);
            this.btnBill.Name = "btnBill";
            this.btnBill.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnBill.Size = new System.Drawing.Size(250, 60);
            this.btnBill.TabIndex = 9;
            this.btnBill.Text = "💳 Hóa đơn thanh toán";
            this.btnBill.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBill.UseVisualStyleBackColor = false;
            this.btnBill.Click += new System.EventHandler(this.btnBill_Click);
            // 
            // AdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 668);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSidebar);
            this.Name = "AdminDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdminDashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelSidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbUser)).EndInit();
            this.panelUser.ResumeLayout(false);
            this.panelUser.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.PictureBox ptbUser;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Panel panelUser;
        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Button btnCategory;
        private System.Windows.Forms.Button btnUser;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnSession;
        private System.Windows.Forms.Button btnTable;
        private System.Windows.Forms.Button btnBill;
    }
}