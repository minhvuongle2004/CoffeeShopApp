using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopAPI.Models;
using QRCoder;

namespace CoffeeShopApp.Views.User
{
    public partial class HoaDonForm : Form
    {
        // Constructor mặc định (giữ lại để tương thích với code hiện có)
        public HoaDonForm()
        {
            InitializeComponent();
        }

        // Constructor mới nhận dữ liệu hiển thị hóa đơn
        public HoaDonForm(Order order, CoffeeShopAPI.Models.User cashier, List<(string menuName, int quantity, decimal price, decimal subtotal)> items)
        {
            InitializeComponent();

            // Hiển thị thông tin hóa đơn
            lblInvoiceDateTimeValue.Text = order.StartTime.ToString("dd.MM.yyyy HH:mm");
            lblCashierValue.Text = cashier?.Fullname ?? "Nhân viên";
            lblBillNumberValue.Text = $"ORD{order.Id:D6}";  // Format: ORD000001

            // Hiển thị chi tiết sản phẩm trong DataGridView
            int index = 1;
            foreach (var item in items)
            {
                dgvProducts.Rows.Add(
                    index,               // Số thứ tự
                    item.menuName,       // Tên món
                    item.quantity,       // Số lượng
                    item.price.ToString("N0"), // Đơn giá
                    item.subtotal.ToString("N0") // Thành tiền
                );
                index++;
            }

            // Hiển thị thông tin tổng
            int totalQuantity = items.Sum(item => item.quantity);
            lblTotalQuantityValue.Text = totalQuantity.ToString();
            lblTotalAmountValue.Text = order.TotalPrice.ToString("N0");
            lblPaymentMethodValue.Text = order.TotalPrice.ToString("N0"); // Hiển thị tổng tiền

            // Tạo mã QR thanh toán và hiển thị
            GenerateVietQRImage("106873937060", "VIETINBANK", "LE MINH VUONG", order.TotalPrice, $"Thanh toan don hang #{order.Id}");
        }

        // Phương thức tạo mã VietQR
        private void GenerateVietQRImage(string accountNumber, string bankCode, string accountName, decimal amount, string description)
        {
            try
            {
                // Format số tiền (loại bỏ dấu phẩy và phần thập phân)
                string amountStr = ((long)amount).ToString();

                // Tạo URL tới API tạo QR
                string url = $"https://img.vietqr.io/image/{bankCode}-{accountNumber}-compact.png?amount={amountStr}&addInfo={Uri.EscapeDataString(description)}&accountName={Uri.EscapeDataString(accountName)}";

                // Tải ảnh QR từ URL
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    byte[] imageData = webClient.DownloadData(url);
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        pictureBoxQR.Image = System.Drawing.Image.FromStream(ms);
                        pictureBoxQR.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã QR: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}