using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeShopApp.Views.User
{
    public partial class OrderInputNumberDialog : Form
    {
        private Label lblPrompt;
        private NumericUpDown numericValue;
        private Button btnOK;
        private Button btnCancel;

        public int Value => (int)numericValue.Value;

        public OrderInputNumberDialog(string promptText, string title, int defaultValue = 1)
        {
            InitializeComponent();

            // Thiết lập thông tin
            this.Text = title;
            this.lblPrompt.Text = promptText;
            this.numericValue.Value = defaultValue;
        }

        private void InitializeComponent()
        {
            this.lblPrompt = new Label();
            this.numericValue = new NumericUpDown();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.lblPrompt.Location = new Point(12, 20);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new Size(119, 16);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "Nhập giá trị:";
            // 
            // numericValue
            // 
            this.numericValue.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.numericValue.Location = new Point(12, 50);
            this.numericValue.Name = "numericValue";
            this.numericValue.Size = new Size(270, 23);
            this.numericValue.TabIndex = 1;
            this.numericValue.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericValue.Minimum = 0;
            this.numericValue.Maximum = 1000;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new Point(62, 90);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(90, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new Point(162, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(90, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // OrderInputNumberDialog
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(300, 140);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.numericValue);
            this.Controls.Add(this.lblPrompt);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderInputNumberDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Nhập giá trị";
            ((System.ComponentModel.ISupportInitialize)(this.numericValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}