using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeShopApp.Views.User
{
    public partial class LoadingForm : Form
    {
        private Label lblMessage;
        private PictureBox pictureBoxLoading;

        public LoadingForm(string message)
        {
            InitializeComponents();
            this.lblMessage.Text = message;
        }

        private void InitializeComponents()
        {
            this.lblMessage = new Label();
            this.pictureBoxLoading = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = DockStyle.Bottom;
            this.lblMessage.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new Point(0, 80);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(300, 30);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Đang tải dữ liệu...";
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.Dock = DockStyle.Fill;
            this.pictureBoxLoading.Location = new Point(0, 0);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new Size(300, 80);
            this.pictureBoxLoading.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBoxLoading.TabIndex = 1;
            this.pictureBoxLoading.TabStop = false;
            // 
            // LoadingForm
            // 
            this.ClientSize = new Size(300, 110);
            this.Controls.Add(this.pictureBoxLoading);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "LoadingForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Đang tải";
            this.Load += new EventHandler(this.LoadingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            // Thiết lập hiệu ứng cho form
            this.Opacity = 0.9;
            this.BackColor = Color.White;

            // Tạo border cho form
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));

            // Thiết lập hiệu ứng loadding animation nếu cần
            // Ví dụ: bạn có thể sử dụng một hình ảnh gif hoặc tạo một animation đơn giản
            Application.DoEvents();
        }

        // Import hàm từ user32.dll để tạo form bo tròn
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
    }
}