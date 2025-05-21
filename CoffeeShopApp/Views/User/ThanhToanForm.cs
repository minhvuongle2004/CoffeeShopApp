using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopAPI.Models;
using CoffeeShopApp.Controls;
using CoffeeShopApp.ServicesUser;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;


namespace CoffeeShopApp.Views.User
{
    public partial class ThanhToanForm : Form
    {
        private Order _currentOrder;
        private CoffeeShopAPI.Models.User _currentUser;
        private Shift _currentShift;
        private HttpClient client;
        private List<(string menuName, int quantity, decimal price, decimal subtotal)> _orderItems;
        private OrderApiClient _apiClient;
        private readonly string baseUrl = "https://localhost:44332/";

        public ThanhToanForm()
        {
            InitializeComponent();
            _apiClient = new OrderApiClient();
        }

        // Constructor nhận thông tin đơn hàng
        public ThanhToanForm(Order order, CoffeeShopAPI.Models.User currentUser, List<(string menuName, int quantity, decimal price, decimal subtotal)> orderItems)
        {
            InitializeComponent();

            _apiClient = new OrderApiClient();
            _currentOrder = order;
            _currentUser = currentUser;
            _orderItems = orderItems;

            // Khởi tạo HttpClient
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Hiển thị thông tin bàn
            lblTableNumberValue.Text = _currentOrder.TableId.ToString();

            // Hiển thị thông tin tổng tiền
            lblTotalAmountValue.Text = _currentOrder.TotalPrice.ToString("N0") + " đ";

            // Hiển thị danh sách món
            LoadOrderItems();

            // Đặt giá trị mặc định cho combobox phương thức thanh toán
            cb_ChonPhuongThuc.SelectedIndex = 0;  // Mặc định "Chuyển khoản"

            // Đăng ký sự kiện cho các nút
            btnPayment.Click += btnPayment_Click;
            btnPrintReceipt.Click += btnPrintReceipt_Click;
            btnDiscount.Click += btnDiscount_Click;

            // Tải thông tin ca làm việc hiện tại
            this.Load += async (s, e) => await LoadCurrentShift();
        }
        //phương thức để tải thông tin ca làm việc hiện tại
        private async Task LoadCurrentShift()
        {
            try
            {
                // Kiểm tra nếu user không có thông tin
                if (_currentUser == null || _currentUser.Id <= 0)
                {
                    MessageBox.Show("Không có thông tin người dùng hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gửi request lấy ca làm việc hiện tại
                HttpResponseMessage response = await client.GetAsync($"api/shift/getCurrentByUserId/{_currentUser.Id}");

                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    _currentShift = JsonConvert.DeserializeObject<CoffeeShopAPI.Models.Shift>(resultJson);
                    Console.WriteLine($"Đã tải thông tin ca làm việc: {_currentShift.Id}");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Không thể tải thông tin ca làm việc: {errorContent}");
                    MessageBox.Show("Không thể tải thông tin ca làm việc. Vui lòng đảm bảo bạn đã mở ca làm việc.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải thông tin ca làm việc: {ex.Message}");
                MessageBox.Show($"Đã xảy ra lỗi khi tải thông tin ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PaymentView_Load(object sender, EventArgs e)
        {
            // Hiển thị form hóa đơn trong panel bên phải
            LoadReceiptPreview();
        }

        private void LoadOrderItems()
        {
            // Xóa toàn bộ dữ liệu cũ
            dgvOrderItems.Rows.Clear();

            // Thêm các món vào DataGridView
            foreach (var item in _orderItems)
            {
                dgvOrderItems.Rows.Add(
                    item.menuName,
                    item.quantity,
                    item.price.ToString("N0"),
                    item.subtotal.ToString("N0")
                );
            }
        }
        private void LoadReceiptPreview()
        {
            try
            {
                // Tạo form hóa đơn
                HoaDonForm receiptForm = new HoaDonForm(_currentOrder, _currentUser, _orderItems);

                // Đặt form là child của panel bên phải
                receiptForm.TopLevel = false;
                receiptForm.FormBorderStyle = FormBorderStyle.None;
                receiptForm.Dock = DockStyle.Fill;

                // Thêm form vào panel
                panelRight.Controls.Clear();
                panelRight.Controls.Add(receiptForm);

                // Hiển thị form
                receiptForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút Thanh toán
        private async void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị xác nhận thanh toán
                DialogResult result = MessageBox.Show(
                    $"Xác nhận thanh toán hoá đơn cho bàn {_currentOrder.TableId} với tổng tiền {_currentOrder.TotalPrice:N0}đ?",
                    "Xác nhận thanh toán",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }

                // Kiểm tra ca làm việc
                if (_currentShift == null)
                {
                    await LoadCurrentShift();
                    if (_currentShift == null)
                    {
                        MessageBox.Show("Không tìm thấy ca làm việc đang mở. Vui lòng mở ca trước khi thanh toán.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Xác định phương thức thanh toán
                bool isCashPayment = cb_ChonPhuongThuc.SelectedIndex == 1; // Index 1 là "Tiền mặt"
                string paymentMethod = isCashPayment ? "Tiền mặt" : "Chuyển khoản";

                // Tạo loading form
                using (var loading = new LoadingForm("Đang xử lý thanh toán..."))
                {
                    loading.Show();
                    Application.DoEvents();

                    try
                    {
                        // Lấy thông tin bàn hiện tại để giữ nguyên tên
                        var table = await _apiClient.GetTableByIdAsync(_currentOrder.TableId);
                        if (table == null)
                        {
                            // Nếu không lấy được thông tin bàn, tạo đối tượng mặc định nhưng giữ nguyên ID
                            table = new Table { Id = _currentOrder.TableId };
                        }

                        // Cập nhật trạng thái bàn thành "empty" nhưng giữ nguyên tên
                        bool tableUpdated = await _apiClient.UpdateTableStatusAsync(table, "empty");

                        if (!tableUpdated)
                        {
                            loading.Close();
                            MessageBox.Show("Cập nhật trạng thái bàn không thành công", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Vẫn tiếp tục xử lý thanh toán
                        }
                        // Cập nhật ca làm việc với thông tin thanh toán
                        var paymentData = new
                        {
                            amount = (double)_currentOrder.TotalPrice,
                            isCash = isCashPayment
                        };

                        string jsonContent = JsonConvert.SerializeObject(paymentData);
                        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        HttpResponseMessage shiftResponse = await client.PutAsync($"api/shift/updateAfterPayment/{_currentShift.Id}", content);

                        if (!shiftResponse.IsSuccessStatusCode)
                        {
                            string errorMessage = await shiftResponse.Content.ReadAsStringAsync();
                            loading.Close();
                            MessageBox.Show($"Không thể cập nhật thông tin ca làm việc: {errorMessage}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Vẫn tiếp tục xử lý thanh toán
                        }

                        // Xóa đơn hàng từ database
                        bool orderDeleted = await _apiClient.DeleteOrderAsync(_currentOrder.Id);

                        loading.Close();

                        if (!orderDeleted)
                        {
                            MessageBox.Show("Không thể xóa đơn hàng. Vui lòng thử lại sau.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Hiển thị thông báo thanh toán thành công
                        MessageBox.Show($"Thanh toán thành công bằng {paymentMethod}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Đóng form với kết quả OK để OrderUC biết cần reload danh sách đơn hàng
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        loading.Close();
                        MessageBox.Show($"Đã xảy ra lỗi khi thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút In hóa đơn
        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo thư mục lưu hóa đơn nếu chưa tồn tại
                string folderPath = Path.Combine(Application.StartupPath, "HoaDon");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Tạo tên file tự động với thời gian hiện tại để tránh trùng lặp
                string fileName = $"HoaDon_{_currentOrder.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string filePath = Path.Combine(folderPath, fileName);

                using (var loading = new LoadingForm("Đang tạo hóa đơn..."))
                {
                    loading.Show();
                    Application.DoEvents();

                    // Tạo file PDF
                    ExportToPdf(filePath);

                    loading.Close();
                }

                //// Hiển thị thông báo thành công
                //DialogResult viewResult = MessageBox.Show(
                //    $"Xuất PDF thành công! File được lưu tại:\n{filePath}\n\nBạn có muốn mở file không?",
                //    "Xuất PDF",
                //    MessageBoxButtons.YesNo,
                //    MessageBoxIcon.Information);

                //if (viewResult == DialogResult.Yes)
                //{
                //    // Mở file PDF bằng ứng dụng mặc định
                //    Process.Start(filePath);
                //}
                // Mở file PDF bằng ứng dụng mặc định
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Phương thức xuất hóa đơn ra file PDF
        private void ExportToPdf(string filePath)
        {
            // Tạo document mới với kích thước A5 dọc
            // Sử dụng PageSize.A5 cho khổ giấy nhỏ hơn để hóa đơn gọn hơn
            iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(420f, 595f); // Tùy chỉnh kích thước trang
            iTextSharp.text.Document document = new iTextSharp.text.Document(pageSize, 20, 20, 20, 20); // Giảm margin

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Cấu hình font chữ Unicode cho tiếng Việt
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Giảm kích thước font để tiết kiệm không gian
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, iTextSharp.text.Font.NORMAL);

                // ----- TIÊU ĐỀ HÓA ĐƠN -----
                Paragraph title = new Paragraph("CAFE DE' MĂNG ĐEN", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 2; // Giảm khoảng cách
                document.Add(title);

                Paragraph address = new Paragraph("126 Nguyễn Chánh, Cầu Giấy, Hà Nội", normalFont);
                address.Alignment = Element.ALIGN_CENTER;
                address.SpacingAfter = 5; // Giảm khoảng cách
                document.Add(address);

                // ----- THÔNG TIN HÓA ĐƠN -----
                // Giảm khoảng cách
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 1f, 2f });
                infoTable.SpacingAfter = 5;

                // Thời gian
                infoTable.AddCell(CreateCompactCell("Thời gian:", normalFont, Element.ALIGN_LEFT, false));
                infoTable.AddCell(CreateCompactCell(_currentOrder.StartTime.ToString("dd.MM.yyyy HH:mm"), normalFont, Element.ALIGN_LEFT, false));

                // Thu ngân
                infoTable.AddCell(CreateCompactCell("Thu ngân:", normalFont, Element.ALIGN_LEFT, false));
                infoTable.AddCell(CreateCompactCell(_currentUser?.Fullname ?? "Nhân viên", normalFont, Element.ALIGN_LEFT, false));

                // Số Bill
                infoTable.AddCell(CreateCompactCell("Số Bill:", normalFont, Element.ALIGN_LEFT, false));
                infoTable.AddCell(CreateCompactCell($"ORD{_currentOrder.Id:D6}", normalFont, Element.ALIGN_LEFT, false));

                document.Add(infoTable);

                // ----- DANH SÁCH SẢN PHẨM -----
                PdfPTable productTable = new PdfPTable(5);
                productTable.WidthPercentage = 100;
                productTable.SetWidths(new float[] { 0.5f, 2.5f, 0.5f, 0.75f, 0.75f });
                productTable.SpacingAfter = 5;

                // Header của bảng sản phẩm
                productTable.AddCell(CreateCompactCell("TT", headerFont, Element.ALIGN_CENTER, true));
                productTable.AddCell(CreateCompactCell("Tên món", headerFont, Element.ALIGN_CENTER, true));
                productTable.AddCell(CreateCompactCell("SL", headerFont, Element.ALIGN_CENTER, true));
                productTable.AddCell(CreateCompactCell("Đ.Giá", headerFont, Element.ALIGN_CENTER, true));
                productTable.AddCell(CreateCompactCell("T.Tiền", headerFont, Element.ALIGN_CENTER, true));

                // Thêm các sản phẩm vào bảng
                int index = 1;
                int totalQuantity = 0;
                foreach (var item in _orderItems)
                {
                    productTable.AddCell(CreateCompactCell(index.ToString(), normalFont, Element.ALIGN_CENTER, false));
                    productTable.AddCell(CreateCompactCell(item.menuName, normalFont, Element.ALIGN_LEFT, false));
                    productTable.AddCell(CreateCompactCell(item.quantity.ToString(), normalFont, Element.ALIGN_CENTER, false));
                    productTable.AddCell(CreateCompactCell(item.price.ToString("N0"), normalFont, Element.ALIGN_RIGHT, false));
                    productTable.AddCell(CreateCompactCell(item.subtotal.ToString("N0"), normalFont, Element.ALIGN_RIGHT, false));

                    index++;
                    totalQuantity += item.quantity;
                }

                document.Add(productTable);

                // ----- TỔNG CỘNG -----
                PdfPTable summaryTable = new PdfPTable(2);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 1f, 1f });
                summaryTable.SpacingAfter = 5;

                // Tổng số lượng
                summaryTable.AddCell(CreateCompactCell("Tổng số lượng:", headerFont, Element.ALIGN_LEFT, false));
                summaryTable.AddCell(CreateCompactCell(totalQuantity.ToString(), headerFont, Element.ALIGN_RIGHT, false));

                // Thành tiền
                summaryTable.AddCell(CreateCompactCell("Thành tiền:", headerFont, Element.ALIGN_LEFT, false));
                summaryTable.AddCell(CreateCompactCell(_currentOrder.TotalPrice.ToString("N0") + "đ", headerFont, Element.ALIGN_RIGHT, false));

                // Thanh toán
                summaryTable.AddCell(CreateCompactCell("Thanh toán:", headerFont, Element.ALIGN_LEFT, false));
                summaryTable.AddCell(CreateCompactCell(_currentOrder.TotalPrice.ToString("N0") + "đ", headerFont, Element.ALIGN_RIGHT, false));

                document.Add(summaryTable);

                // ----- FOOTER -----
                Paragraph taxInfo = new Paragraph("Giá sản phẩm đã bao gồm VAT 8%, đơn GTGT chỉ xuất tại thời điểm mua hàng.", smallFont);
                taxInfo.Alignment = Element.ALIGN_CENTER;
                taxInfo.SpacingAfter = 1;
                document.Add(taxInfo);

                Paragraph websiteInfo = new Paragraph("https://evat.mangden.com", smallFont);
                websiteInfo.Alignment = Element.ALIGN_CENTER;
                websiteInfo.SpacingAfter = 1;
                document.Add(websiteInfo);

                Paragraph contactInfo = new Paragraph("Mọi thắc mắc xin liên hệ 02871 087", smallFont);
                contactInfo.Alignment = Element.ALIGN_CENTER;
                contactInfo.SpacingAfter = 1;
                document.Add(contactInfo);

                Paragraph wifiInfo = new Paragraph("Password Wifi: mangden", smallFont);
                wifiInfo.Alignment = Element.ALIGN_CENTER;
                wifiInfo.SpacingAfter = 5;
                document.Add(wifiInfo);

                // ----- THÔNG TIN THANH TOÁN QR -----
                Paragraph paymentInfo = new Paragraph("THÔNG TIN THANH TOÁN", headerFont);
                paymentInfo.Alignment = Element.ALIGN_CENTER;
                paymentInfo.SpacingAfter = 5;
                document.Add(paymentInfo);

                // Tạo một layout 2 cột cho phần thanh toán: bên trái thông tin, bên phải mã QR
                PdfPTable paymentLayoutTable = new PdfPTable(2);
                paymentLayoutTable.WidthPercentage = 100;
                paymentLayoutTable.SetWidths(new float[] { 1.5f, 1f });

                // Cột 1: Thông tin thanh toán
                PdfPTable paymentDetailsTable = new PdfPTable(2);
                paymentDetailsTable.WidthPercentage = 100;
                paymentDetailsTable.SetWidths(new float[] { 1f, 2f });

                paymentDetailsTable.AddCell(CreateCompactCell("Ngân hàng:", normalFont, Element.ALIGN_LEFT, false));
                paymentDetailsTable.AddCell(CreateCompactCell("VIETINBANK", normalFont, Element.ALIGN_LEFT, false));

                paymentDetailsTable.AddCell(CreateCompactCell("Số tài khoản:", normalFont, Element.ALIGN_LEFT, false));
                paymentDetailsTable.AddCell(CreateCompactCell("106873937060", normalFont, Element.ALIGN_LEFT, false));

                paymentDetailsTable.AddCell(CreateCompactCell("Chủ tài khoản:", normalFont, Element.ALIGN_LEFT, false));
                paymentDetailsTable.AddCell(CreateCompactCell("LE MINH VUONG", normalFont, Element.ALIGN_LEFT, false));

                paymentDetailsTable.AddCell(CreateCompactCell("Số tiền:", normalFont, Element.ALIGN_LEFT, false));
                paymentDetailsTable.AddCell(CreateCompactCell(_currentOrder.TotalPrice.ToString("N0") + "đ", normalFont, Element.ALIGN_LEFT, false));

                // Tạo cell chứa bảng thông tin thanh toán
                PdfPCell paymentDetailsCell = new PdfPCell(paymentDetailsTable);
                paymentDetailsCell.Border = 0;
                paymentDetailsCell.PaddingRight = 10;
                paymentLayoutTable.AddCell(paymentDetailsCell);

                // Cột 2: Mã QR
                // Tạo QR image
                iTextSharp.text.Image qrImage = GenerateQRImageForPdf();
                if (qrImage != null)
                {
                    qrImage.ScaleToFit(100f, 100f);
                    PdfPCell qrCell = new PdfPCell(qrImage);
                    qrCell.Border = 0;
                    qrCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    paymentLayoutTable.AddCell(qrCell);
                }
                else
                {
                    // Nếu không tạo được QR, thêm ô trống
                    PdfPCell emptyQrCell = new PdfPCell(new Phrase(""));
                    emptyQrCell.Border = 0;
                    paymentLayoutTable.AddCell(emptyQrCell);
                }

                document.Add(paymentLayoutTable);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo file PDF: {ex.Message}", ex);
            }
            finally
            {
                document.Close();
            }
        }
        // Phương thức tạo mã QR cho PDF
        private iTextSharp.text.Image GenerateQRImageForPdf()
        {
            try
            {
                // Format số tiền (loại bỏ dấu phẩy và phần thập phân)
                string amountStr = ((long)_currentOrder.TotalPrice).ToString();
                string description = $"Thanh toan don hang #{_currentOrder.Id}";

                // Tạo URL tới API tạo QR
                string url = $"https://img.vietqr.io/image/VIETINBANK-106873937060-compact.png?amount={amountStr}&addInfo={Uri.EscapeDataString(description)}&accountName={Uri.EscapeDataString("LE MINH VUONG")}";

                // Tải ảnh QR từ URL
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    byte[] imageData = webClient.DownloadData(url);
                    iTextSharp.text.Image qrImage = iTextSharp.text.Image.GetInstance(imageData);
                    qrImage.ScaleToFit(150f, 150f);
                    return qrImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo mã QR cho PDF: {ex.Message}");
                return null;
            }
        }
        // Phương thức hỗ trợ tạo cell cho bảng PDF
        private PdfPCell CreateCompactCell(string text, iTextSharp.text.Font font, int alignment, bool isHeader)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 3; // Giảm padding xuống để tiết kiệm không gian

            if (isHeader)
            {
                cell.BackgroundColor = new BaseColor(230, 230, 230);
            }
            else
            {
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.BorderWidthBottom = 0.5f;
            }

            return cell;
        }


        // Xử lý sự kiện khi nhấn nút Giảm giá
        private async void btnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị hộp thoại nhập phần trăm giảm giá
                decimal discountPercent = 0;
                using (var inputDialog = new GuestInputNumberDialog("Nhập % giảm giá (0-100):", "Giảm giá", 0))
                {
                    if (inputDialog.ShowDialog() == DialogResult.OK)
                    {
                        discountPercent = inputDialog.Value;
                        if (discountPercent < 0 || discountPercent > 100)
                        {
                            MessageBox.Show("Giảm giá phải nằm trong khoảng 0-100%", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        return; // Người dùng hủy
                    }
                }

                if (discountPercent <= 0)
                {
                    return; // Không có giảm giá
                }

                // Tính giá trị giảm giá
                decimal originalPrice = _currentOrder.TotalPrice;
                decimal discountAmount = originalPrice * discountPercent / 100;
                decimal finalPrice = originalPrice - discountAmount;

                // Xác nhận giảm giá
                DialogResult result = MessageBox.Show(
                    $"Xác nhận giảm giá {discountPercent}% cho hóa đơn?\n" +
                    $"Giá gốc: {originalPrice:N0}đ\n" +
                    $"Giảm: {discountAmount:N0}đ\n" +
                    $"Giá sau giảm: {finalPrice:N0}đ",
                    "Xác nhận giảm giá",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }

                // Cập nhật giá mới cho đơn hàng
                _currentOrder.TotalPrice = finalPrice;

                // Cập nhật đơn hàng trên server
                bool orderUpdated = await _apiClient.UpdateOrderAsync(_currentOrder);

                if (!orderUpdated)
                {
                    MessageBox.Show("Cập nhật giá đơn hàng không thành công", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật hiển thị
                lblTotalAmountValue.Text = finalPrice.ToString("N0") + " đ";

                // Cập nhật lại hóa đơn bên phải
                LoadReceiptPreview();

                // Hiển thị thông báo thành công
                MessageBox.Show("Đã áp dụng giảm giá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi áp dụng giảm giá: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}