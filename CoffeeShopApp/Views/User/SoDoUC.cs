// SoDoUC.cs - UserControl hiển thị sơ đồ bàn chỉ phân biệt bàn có khách hay không, icon xám khi có khách (Loại bỏ border, chỉ icon và tên bàn dưới icon)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using CoffeeShopAPI.Models;
using CoffeeShopApp.ServicesUser;
using System.Drawing.Text;
using CoffeeShopApp.Services;

namespace CoffeeShopApp.Views.User
{
    public partial class SoDoUC : UserControl
    {
        private FlowLayoutPanel flowTables;
        private List<Table> allTables = new List<Table>();
        private EmployeeView parent;

        public SoDoUC(EmployeeView parent)
        {
            InitializeComponent();
            this.parent = parent;
            SetupLayout();
            _ = LoadTablesFromApi();
        }

        private void SetupLayout()
        {
            this.Dock = DockStyle.Fill;

            flowTables = new FlowLayoutPanel();
            flowTables.Dock = DockStyle.Fill;
            flowTables.AutoScroll = true;
            flowTables.Padding = new Padding(10);

            this.Controls.Add(flowTables);
        }

        private async Task LoadTablesFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:44332/api/table/getAll";
                    var response = await client.GetStringAsync(apiUrl);
                    allTables = JsonConvert.DeserializeObject<List<Table>>(response);
                    RenderTables(allTables);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bàn: " + ex.Message);
            }
        }

        private void RenderTables(List<Table> tables)
        {
            flowTables.Controls.Clear();
            foreach (var table in tables)
            {
                // Tạo Panel chứa icon và label, loại bỏ border
                Panel tablePanel = new Panel();
                tablePanel.Width = 120;
                tablePanel.Height = 150;
                tablePanel.BackColor = Color.Transparent;
                tablePanel.Margin = new Padding(10);

                // Xử lý icon bàn
                Image original = Properties.Resources.table_icon;
                Bitmap resized = new Bitmap(original, new Size(90, 90)); // Tăng 1.5 lần so với 60x60

                if (table.Status == "full")
                {
                    // Chuyển icon sang xám khi có khách
                    for (int y = 0; y < resized.Height; y++)
                    {
                        for (int x = 0; x < resized.Width; x++)
                        {
                            Color oc = resized.GetPixel(x, y);
                            int gray = (oc.R + oc.G + oc.B) / 3;
                            resized.SetPixel(x, y, Color.FromArgb(oc.A, gray, gray, gray));
                        }
                    }
                }

                PictureBox pic = new PictureBox();
                pic.Image = resized;
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 90;
                pic.Height = 90;
                pic.Top = 0;
                pic.Left = (tablePanel.Width - pic.Width) / 2;
                pic.Cursor = Cursors.Hand;
                pic.Click += (s, e) => BtnTable_Click(table);

                // Tên bàn dưới icon
                Label lblName = new Label();
                lblName.Text = table.TableName;
                lblName.TextAlign = ContentAlignment.MiddleCenter;
                lblName.AutoSize = false;
                lblName.Width = tablePanel.Width;
                lblName.Top = pic.Bottom + 5;
                lblName.Font = new Font("Minion Pro", 13);
                lblName.Click += (s, e) => BtnTable_Click(table); // Thêm sự kiện click cho label

                tablePanel.Controls.Add(pic);
                tablePanel.Controls.Add(lblName);

                flowTables.Controls.Add(tablePanel);
            }
        }

        private void BtnTable_Click(Table table)
        {
            if (table.Status == "full")
            {
                // Nếu bàn có khách (màu xám), hiển thị OrderCardUC dưới dạng popup
                _ = ShowTableOrderPopup(table.Id);
            }
            else
            {
                // Nếu bàn trống, chuyển đến màn hình đặt món như cũ
                parent.LoadUserControl(new OrderView(table));
            }
        }

        private async Task ShowTableOrderPopup(int tableId)
        {
            try
            {
                // Lấy thông tin đơn hàng của bàn từ API
                Order order = await GetTableOrderAsync(tableId);

                if (order == null)
                {
                    MessageBox.Show($"Không tìm thấy đơn hàng cho bàn {tableId}");
                    return;
                }

                // Tạo form popup để hiển thị thông tin đơn hàng
                Form popup = new Form
                {
                    Text = $"Thông tin đơn hàng - Bàn {tableId}",
                    StartPosition = FormStartPosition.CenterParent,
                    Width = 250,
                    Height = 200,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                // Tạo OrderCardUC và thêm vào form
                var orderCard = new OrderCardUC();
                orderCard.SetData(order.TableId, order.Id, order.TotalGuest, order.TotalPrice);
                orderCard.Dock = DockStyle.Fill;

                // Đăng ký sự kiện cho các nút trong OrderCardUC
                orderCard.ViewDetailsClicked += (s, e) =>
                {
                    popup.Close();
                    // Chuyển đến màn hình chi tiết đơn hàng
                    var orderView = new OrderView(new Table { Id = tableId, TableName = $"Bàn {tableId}" });
                    orderView.OrderId = order.Id; // Thiết lập OrderId 
                    parent.LoadUserControl(orderView);
                };

                orderCard.PaymentClicked += (s, e) =>
                {
                    popup.Close(); // Đóng popup trước khi xử lý thanh toán
                    ProcessPayment(order); // Sử dụng phương thức tái sử dụng để xử lý thanh toán
                };

                orderCard.NotifyClicked += (s, e) =>
                {
                    // Gọi phương thức tái sử dụng từ TableOperationService
                    TableOperationService.ProcessTableMoveAsync(order, () => {
                        // Refresh lại danh sách bàn sau khi chuyển bàn thành công
                        LoadTablesFromApi();
                    });
                };

                orderCard.EditClicked += (s, e) =>
                {
                    popup.Close();
                    // Chuyển đến màn hình chỉnh sửa đơn hàng
                    var orderView = new OrderView(new Table { Id = tableId, TableName = $"Bàn {tableId}" });
                    orderView.OrderId = order.Id; // Thiết lập OrderId 
                    parent.LoadUserControl(orderView);
                };

                popup.Controls.Add(orderCard);

                // Thêm nút Đóng vào popup
                Button btnClose = new Button
                {
                    Text = "Đóng",
                    Font = new Font("Minion Pro", 12),
                    Dock = DockStyle.Bottom,
                    Height = 30,
                    BackColor = Color.LightGray
                };
                btnClose.Click += (s, e) => popup.Close();

                popup.Controls.Add(btnClose);
                popup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private async Task<Order> GetTableOrderAsync(int tableId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API để lấy tất cả đơn hàng
                    string apiUrl = "https://localhost:44332/api/order/getAll";
                    var response = await client.GetStringAsync(apiUrl);
                    var orders = JsonConvert.DeserializeObject<List<Order>>(response);

                    // Lọc ra đơn hàng có tableId tương ứng và status chưa thanh toán
                    // Giả sử đơn hàng chưa thanh toán có status là "pending" hoặc null
                    return orders.Find(o => o.TableId == tableId && o.Status != "completed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin đơn hàng: {ex.Message}");
                return null;
            }
        }

        // Phương thức tái sử dụng cho chức năng thanh toán, tạo thành static class để có thể gọi từ bất kỳ đâu
        public async void ProcessPayment(Order order)
        {
            try
            {
                // Lấy thông tin người dùng hiện tại từ form cha
                CoffeeShopAPI.Models.User currentUser = null;
                if (parent != null)
                {
                    currentUser = parent._currentUser;
                }

                if (currentUser == null)
                {
                    MessageBox.Show("Không thể xác định người dùng hiện tại. Vui lòng đăng nhập lại.",
                                   "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hiển thị thông báo đang tải
                using (var loading = new LoadingForm("Đang tải thông tin đơn hàng..."))
                {
                    loading.Show();

                    try
                    {
                        // Tạo instance của OrderApiClient để sử dụng các phương thức API
                        var apiClient = new OrderApiClient();

                        // Lấy thông tin bàn để giữ nguyên tên
                        var table = await apiClient.GetTableByIdAsync(order.TableId);

                        // Lấy chi tiết đơn hàng
                        var orderDetails = await apiClient.GetOrderDetailsAsync(order.Id);

                        // Tạo danh sách các món ăn để hiển thị trong hóa đơn
                        List<(string menuName, int quantity, decimal price, decimal subtotal)> items =
                            new List<(string menuName, int quantity, decimal price, decimal subtotal)>();

                        // Lấy thông tin chi tiết từng món
                        foreach (var detail in orderDetails)
                        {
                            // Lấy thông tin menu item từ detail.MenuId
                            var menuItem = await apiClient.GetMenuByIdAsync(detail.MenuId);
                            if (menuItem != null)
                            {
                                decimal subtotal = detail.Quantity * menuItem.Price;
                                items.Add((menuItem.Name, detail.Quantity, menuItem.Price, subtotal));
                            }
                        }

                        loading.Close();

                        // Tạo và hiển thị form thanh toán
                        var paymentView = new ThanhToanForm(order, currentUser, items);
                        paymentView.ShowDialog();

                        // Sau khi thanh toán xong, reload lại danh sách bàn
                        if (paymentView.DialogResult == DialogResult.OK)
                        {
                            // Refresh lại danh sách bàn
                            _ = LoadTablesFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        loading.Close();
                        MessageBox.Show($"Lỗi khi tải thông tin đơn hàng: {ex.Message}",
                                       "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Static class để chứa phương thức thanh toán có thể tái sử dụng ở nhiều nơi
    public static class PaymentProcessor
    {
        public static async Task ProcessOrderPayment(Order order, CoffeeShopAPI.Models.User currentUser, Action onPaymentCompleted = null)
        {
            try
            {
                if (currentUser == null)
                {
                    MessageBox.Show("Không thể xác định người dùng hiện tại. Vui lòng đăng nhập lại.",
                                   "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hiển thị thông báo đang tải
                using (var loading = new LoadingForm("Đang tải thông tin đơn hàng..."))
                {
                    loading.Show();

                    try
                    {
                        // Tạo instance của OrderApiClient để sử dụng các phương thức API
                        var apiClient = new OrderApiClient();

                        // Lấy thông tin bàn để giữ nguyên tên
                        var table = await apiClient.GetTableByIdAsync(order.TableId);

                        // Lấy chi tiết đơn hàng
                        var orderDetails = await apiClient.GetOrderDetailsAsync(order.Id);

                        // Tạo danh sách các món ăn để hiển thị trong hóa đơn
                        List<(string menuName, int quantity, decimal price, decimal subtotal)> items =
                            new List<(string menuName, int quantity, decimal price, decimal subtotal)>();

                        // Lấy thông tin chi tiết từng món
                        foreach (var detail in orderDetails)
                        {
                            // Lấy thông tin menu item từ detail.MenuId
                            var menuItem = await apiClient.GetMenuByIdAsync(detail.MenuId);
                            if (menuItem != null)
                            {
                                decimal subtotal = detail.Quantity * menuItem.Price;
                                items.Add((menuItem.Name, detail.Quantity, menuItem.Price, subtotal));
                            }
                        }

                        loading.Close();

                        // Tạo và hiển thị form thanh toán
                        var paymentView = new ThanhToanForm(order, currentUser, items);
                        paymentView.ShowDialog();

                        // Sau khi thanh toán xong, thực hiện callback nếu có
                        if (paymentView.DialogResult == DialogResult.OK && onPaymentCompleted != null)
                        {
                            onPaymentCompleted();
                        }
                    }
                    catch (Exception ex)
                    {
                        loading.Close();
                        MessageBox.Show($"Lỗi khi tải thông tin đơn hàng: {ex.Message}",
                                       "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}