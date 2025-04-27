namespace CoffeeShopApp.Views.User
{
    partial class EmployeeView
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.picCloudSync = new System.Windows.Forms.PictureBox();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnOrder = new System.Windows.Forms.Button();
            this.btnSodo = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.panelSidebar.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCloudSync)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.Controls.Add(this.flowLayoutPanel1);
            this.panelSidebar.Controls.Add(this.btnHome);
            this.panelSidebar.Controls.Add(this.btnOrder);
            this.panelSidebar.Controls.Add(this.btnSodo);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(947, 123);
            this.panelSidebar.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.picCloudSync);
            this.flowLayoutPanel1.Controls.Add(this.lblUser);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(590, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 39, 40, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(357, 123);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // picCloudSync
            // 
            this.picCloudSync.Image = global::CoffeeShopApp.Properties.Resources.cloud;
            this.picCloudSync.Location = new System.Drawing.Point(4, 43);
            this.picCloudSync.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picCloudSync.Name = "picCloudSync";
            this.picCloudSync.Size = new System.Drawing.Size(57, 40);
            this.picCloudSync.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCloudSync.TabIndex = 5;
            this.picCloudSync.TabStop = false;
            this.picCloudSync.Click += new System.EventHandler(this.picCloudSync_Click);
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Minion Pro", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(4, 38);
            this.btnHome.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(139, 45);
            this.btnHome.TabIndex = 2;
            this.btnHome.Text = "🏠 Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnOrder
            // 
            this.btnOrder.FlatAppearance.BorderSize = 0;
            this.btnOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrder.Font = new System.Drawing.Font("Minion Pro", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrder.Location = new System.Drawing.Point(137, 38);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(148, 45);
            this.btnOrder.TabIndex = 3;
            this.btnOrder.Text = " 📝 Order";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // btnSodo
            // 
            this.btnSodo.FlatAppearance.BorderSize = 0;
            this.btnSodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSodo.Font = new System.Drawing.Font("Minion Pro", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSodo.Location = new System.Drawing.Point(280, 38);
            this.btnSodo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSodo.Name = "btnSodo";
            this.btnSodo.Size = new System.Drawing.Size(139, 45);
            this.btnSodo.TabIndex = 4;
            this.btnSodo.Text = "🗺️ Sơ đồ";
            this.btnSodo.UseVisualStyleBackColor = true;
            this.btnSodo.Click += new System.EventHandler(this.btnSodo_Click);
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 123);
            this.panelContent.Margin = new System.Windows.Forms.Padding(4);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(947, 348);
            this.panelContent.TabIndex = 1;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Minion Pro", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(69, 39);
            this.lblUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Padding = new System.Windows.Forms.Padding(50, 10, 0, 0);
            this.lblUser.Size = new System.Drawing.Size(99, 41);
            this.lblUser.TabIndex = 6;
            this.lblUser.Text = "👤 ";
            this.lblUser.Click += new System.EventHandler(this.lblUser_Click);
            // 
            // EmployeeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 471);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSidebar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EmployeeView";
            this.Text = "EmployeeView";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelSidebar.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCloudSync)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.Button btnSodo;
        private System.Windows.Forms.PictureBox picCloudSync;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel panelContent;
    }
}