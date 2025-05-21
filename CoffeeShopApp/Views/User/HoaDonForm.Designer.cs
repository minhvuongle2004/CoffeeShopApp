namespace CoffeeShopApp.Views.User
{
    partial class HoaDonForm
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblShopAddress = new System.Windows.Forms.Label();
            this.lblShopName = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.lblInvoiceDateTimeValue = new System.Windows.Forms.Label();
            this.lblInvoiceDateTime = new System.Windows.Forms.Label();
            this.lblCashierValue = new System.Windows.Forms.Label();
            this.lblBillNumberValue = new System.Windows.Forms.Label();
            this.lblCashier = new System.Windows.Forms.Label();
            this.lblBillNumber = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblPaymentMethodValue = new System.Windows.Forms.Label();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.lblTotalAmountValue = new System.Windows.Forms.Label();
            this.lblTotalQuantityValue = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblTotalQuantity = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblWifiPassword = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblWebsite = new System.Windows.Forms.Label();
            this.pictureBoxQR = new System.Windows.Forms.PictureBox();
            this.lblIncludeTax = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.panelSummary.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHeader.Controls.Add(this.lblShopAddress);
            this.panelHeader.Controls.Add(this.lblShopName);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(404, 74);
            this.panelHeader.TabIndex = 0;
            // 
            // lblShopAddress
            // 
            this.lblShopAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblShopAddress.Font = new System.Drawing.Font("Minion Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShopAddress.Location = new System.Drawing.Point(0, 40);
            this.lblShopAddress.Name = "lblShopAddress";
            this.lblShopAddress.Size = new System.Drawing.Size(402, 26);
            this.lblShopAddress.TabIndex = 1;
            this.lblShopAddress.Text = "126 Nguyễn Chánh, Cầu Giấy, Hà Nội";
            this.lblShopAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblShopName
            // 
            this.lblShopName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblShopName.Font = new System.Drawing.Font("Minion Pro", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShopName.Location = new System.Drawing.Point(0, 0);
            this.lblShopName.Name = "lblShopName";
            this.lblShopName.Size = new System.Drawing.Size(402, 40);
            this.lblShopName.TabIndex = 0;
            this.lblShopName.Text = "CAFE DE\' MĂNG ĐEN";
            this.lblShopName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.lblInvoiceDateTimeValue);
            this.panelInfo.Controls.Add(this.lblInvoiceDateTime);
            this.panelInfo.Controls.Add(this.lblCashierValue);
            this.panelInfo.Controls.Add(this.lblBillNumberValue);
            this.panelInfo.Controls.Add(this.lblCashier);
            this.panelInfo.Controls.Add(this.lblBillNumber);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 74);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(404, 82);
            this.panelInfo.TabIndex = 1;
            // 
            // lblInvoiceDateTimeValue
            // 
            this.lblInvoiceDateTimeValue.AutoSize = true;
            this.lblInvoiceDateTimeValue.Font = new System.Drawing.Font("Minion Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceDateTimeValue.Location = new System.Drawing.Point(100, 6);
            this.lblInvoiceDateTimeValue.Name = "lblInvoiceDateTimeValue";
            this.lblInvoiceDateTimeValue.Size = new System.Drawing.Size(92, 18);
            this.lblInvoiceDateTimeValue.TabIndex = 7;
            this.lblInvoiceDateTimeValue.Text = "16.01.2024 15:14";
            // 
            // lblInvoiceDateTime
            // 
            this.lblInvoiceDateTime.AutoSize = true;
            this.lblInvoiceDateTime.Font = new System.Drawing.Font("Minion Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceDateTime.Location = new System.Drawing.Point(12, 6);
            this.lblInvoiceDateTime.Name = "lblInvoiceDateTime";
            this.lblInvoiceDateTime.Size = new System.Drawing.Size(63, 18);
            this.lblInvoiceDateTime.TabIndex = 6;
            this.lblInvoiceDateTime.Text = "Thời gian: ";
            // 
            // lblCashierValue
            // 
            this.lblCashierValue.AutoSize = true;
            this.lblCashierValue.Font = new System.Drawing.Font("Minion Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashierValue.Location = new System.Drawing.Point(100, 29);
            this.lblCashierValue.Name = "lblCashierValue";
            this.lblCashierValue.Size = new System.Drawing.Size(36, 18);
            this.lblCashierValue.TabIndex = 5;
            this.lblCashierValue.Text = "DELI";
            // 
            // lblBillNumberValue
            // 
            this.lblBillNumberValue.AutoSize = true;
            this.lblBillNumberValue.Font = new System.Drawing.Font("Minion Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillNumberValue.Location = new System.Drawing.Point(100, 52);
            this.lblBillNumberValue.Name = "lblBillNumberValue";
            this.lblBillNumberValue.Size = new System.Drawing.Size(78, 18);
            this.lblBillNumberValue.TabIndex = 3;
            this.lblBillNumberValue.Text = "TA123456789";
            // 
            // lblCashier
            // 
            this.lblCashier.AutoSize = true;
            this.lblCashier.Font = new System.Drawing.Font("Minion Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashier.Location = new System.Drawing.Point(12, 29);
            this.lblCashier.Name = "lblCashier";
            this.lblCashier.Size = new System.Drawing.Size(61, 18);
            this.lblCashier.TabIndex = 2;
            this.lblCashier.Text = "Thu ngân:";
            // 
            // lblBillNumber
            // 
            this.lblBillNumber.AutoSize = true;
            this.lblBillNumber.Font = new System.Drawing.Font("Minion Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillNumber.Location = new System.Drawing.Point(12, 52);
            this.lblBillNumber.Name = "lblBillNumber";
            this.lblBillNumber.Size = new System.Drawing.Size(44, 18);
            this.lblBillNumber.TabIndex = 0;
            this.lblBillNumber.Text = "Số Bill:";
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AllowUserToResizeColumns = false;
            this.dgvProducts.AllowUserToResizeRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProducts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIndex,
            this.colProductName,
            this.colQuantity,
            this.colUnitPrice,
            this.colTotalPrice});
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvProducts.Location = new System.Drawing.Point(0, 156);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersVisible = false;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProducts.Size = new System.Drawing.Size(404, 167);
            this.dgvProducts.TabIndex = 2;
            // 
            // colIndex
            // 
            this.colIndex.FillWeight = 10F;
            this.colIndex.HeaderText = "TT";
            this.colIndex.MinimumWidth = 6;
            this.colIndex.Name = "colIndex";
            this.colIndex.ReadOnly = true;
            // 
            // colProductName
            // 
            this.colProductName.FillWeight = 50F;
            this.colProductName.HeaderText = "Tên món";
            this.colProductName.MinimumWidth = 6;
            this.colProductName.Name = "colProductName";
            this.colProductName.ReadOnly = true;
            // 
            // colQuantity
            // 
            this.colQuantity.FillWeight = 10F;
            this.colQuantity.HeaderText = "SL";
            this.colQuantity.MinimumWidth = 6;
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            // 
            // colUnitPrice
            // 
            this.colUnitPrice.FillWeight = 15F;
            this.colUnitPrice.HeaderText = "Đ.Giá";
            this.colUnitPrice.MinimumWidth = 6;
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.ReadOnly = true;
            // 
            // colTotalPrice
            // 
            this.colTotalPrice.FillWeight = 15F;
            this.colTotalPrice.HeaderText = "T.Tiền";
            this.colTotalPrice.MinimumWidth = 6;
            this.colTotalPrice.Name = "colTotalPrice";
            this.colTotalPrice.ReadOnly = true;
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.lblPaymentMethodValue);
            this.panelSummary.Controls.Add(this.lblPaymentMethod);
            this.panelSummary.Controls.Add(this.lblTotalAmountValue);
            this.panelSummary.Controls.Add(this.lblTotalQuantityValue);
            this.panelSummary.Controls.Add(this.lblTotalAmount);
            this.panelSummary.Controls.Add(this.lblTotalQuantity);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 323);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(404, 88);
            this.panelSummary.TabIndex = 3;
            // 
            // lblPaymentMethodValue
            // 
            this.lblPaymentMethodValue.AutoSize = true;
            this.lblPaymentMethodValue.Font = new System.Drawing.Font("Minion Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentMethodValue.Location = new System.Drawing.Point(323, 58);
            this.lblPaymentMethodValue.Name = "lblPaymentMethodValue";
            this.lblPaymentMethodValue.Size = new System.Drawing.Size(47, 20);
            this.lblPaymentMethodValue.TabIndex = 5;
            this.lblPaymentMethodValue.Text = "58 000";
            this.lblPaymentMethodValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Minion Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentMethod.Location = new System.Drawing.Point(12, 58);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(82, 20);
            this.lblPaymentMethod.TabIndex = 2;
            this.lblPaymentMethod.Text = "Thanh toán:";
            // 
            // lblTotalAmountValue
            // 
            this.lblTotalAmountValue.AutoSize = true;
            this.lblTotalAmountValue.Font = new System.Drawing.Font("Minion Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmountValue.Location = new System.Drawing.Point(323, 35);
            this.lblTotalAmountValue.Name = "lblTotalAmountValue";
            this.lblTotalAmountValue.Size = new System.Drawing.Size(47, 20);
            this.lblTotalAmountValue.TabIndex = 4;
            this.lblTotalAmountValue.Text = "58 000";
            this.lblTotalAmountValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalQuantityValue
            // 
            this.lblTotalQuantityValue.AutoSize = true;
            this.lblTotalQuantityValue.Font = new System.Drawing.Font("Minion Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQuantityValue.Location = new System.Drawing.Point(343, 12);
            this.lblTotalQuantityValue.Name = "lblTotalQuantityValue";
            this.lblTotalQuantityValue.Size = new System.Drawing.Size(16, 20);
            this.lblTotalQuantityValue.TabIndex = 3;
            this.lblTotalQuantityValue.Text = "2";
            this.lblTotalQuantityValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Minion Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(12, 35);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(77, 20);
            this.lblTotalAmount.TabIndex = 1;
            this.lblTotalAmount.Text = "Thành tiền:";
            // 
            // lblTotalQuantity
            // 
            this.lblTotalQuantity.AutoSize = true;
            this.lblTotalQuantity.Font = new System.Drawing.Font("Minion Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQuantity.Location = new System.Drawing.Point(12, 12);
            this.lblTotalQuantity.Name = "lblTotalQuantity";
            this.lblTotalQuantity.Size = new System.Drawing.Size(98, 20);
            this.lblTotalQuantity.TabIndex = 0;
            this.lblTotalQuantity.Text = "Tổng số lượng:";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.lblWifiPassword);
            this.panelFooter.Controls.Add(this.lblContact);
            this.panelFooter.Controls.Add(this.lblWebsite);
            this.panelFooter.Controls.Add(this.pictureBoxQR);
            this.panelFooter.Controls.Add(this.lblIncludeTax);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 411);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(404, 192);
            this.panelFooter.TabIndex = 4;
            // 
            // lblWifiPassword
            // 
            this.lblWifiPassword.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblWifiPassword.Font = new System.Drawing.Font("Minion Pro", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWifiPassword.Location = new System.Drawing.Point(0, 169);
            this.lblWifiPassword.Name = "lblWifiPassword";
            this.lblWifiPassword.Size = new System.Drawing.Size(404, 23);
            this.lblWifiPassword.TabIndex = 4;
            this.lblWifiPassword.Text = "Password Wifi: thecoffeehouse";
            this.lblWifiPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblContact
            // 
            this.lblContact.Font = new System.Drawing.Font("Minion Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.Location = new System.Drawing.Point(12, 104);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(242, 35);
            this.lblContact.TabIndex = 3;
            this.lblContact.Text = "Mọi thắc mắc xin liên hệ 0353234113";
            // 
            // lblWebsite
            // 
            this.lblWebsite.Font = new System.Drawing.Font("Minion Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebsite.Location = new System.Drawing.Point(12, 58);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(242, 35);
            this.lblWebsite.TabIndex = 2;
            this.lblWebsite.Text = "https://evat.thecoffeehouse.com";
            // 
            // pictureBoxQR
            // 
            this.pictureBoxQR.Location = new System.Drawing.Point(260, 42);
            this.pictureBoxQR.Name = "pictureBoxQR";
            this.pictureBoxQR.Size = new System.Drawing.Size(120, 120);
            this.pictureBoxQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxQR.TabIndex = 1;
            this.pictureBoxQR.TabStop = false;
            // 
            // lblIncludeTax
            // 
            this.lblIncludeTax.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIncludeTax.Font = new System.Drawing.Font("Minion Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeTax.Location = new System.Drawing.Point(0, 0);
            this.lblIncludeTax.Name = "lblIncludeTax";
            this.lblIncludeTax.Size = new System.Drawing.Size(404, 35);
            this.lblIncludeTax.TabIndex = 0;
            this.lblIncludeTax.Text = "Giá sản phẩm đã bao gồm VAT 8%, đơn GTGT chỉ xuất tại thời điểm mua hàng. Nếu bạn" +
    " cần xuất hóa đơn, hãy quét QR code hoặc truy cập website:";
            this.lblIncludeTax.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // HoaDonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 603);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Minion Pro", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HoaDonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hóa Đơn Bán Hàng";
            this.panelHeader.ResumeLayout(false);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblShopAddress;
        private System.Windows.Forms.Label lblShopName;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblInvoiceDateTimeValue;
        private System.Windows.Forms.Label lblInvoiceDateTime;
        private System.Windows.Forms.Label lblCashierValue;
        private System.Windows.Forms.Label lblBillNumberValue;
        private System.Windows.Forms.Label lblCashier;
        private System.Windows.Forms.Label lblBillNumber;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblPaymentMethodValue;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.Label lblTotalAmountValue;
        private System.Windows.Forms.Label lblTotalQuantityValue;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblTotalQuantity;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblWifiPassword;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblWebsite;
        private System.Windows.Forms.PictureBox pictureBoxQR;
        private System.Windows.Forms.Label lblIncludeTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalPrice;
    }
}