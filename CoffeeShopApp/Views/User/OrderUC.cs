using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopAPI.Models;
using CoffeeShopApp.ServicesUser;
using CoffeeShopApp.Services;

namespace CoffeeShopApp.Views.User
{
    public partial class OrderUC : UserControl
    {
        private FlowLayoutPanel flowPanel;
        private EmployeeView parent;  // Thêm tham chiếu đến form cha

        public OrderUC(EmployeeView parent = null)
        {
            InitializeComponent();
            this.parent = parent;  // Lưu tham chiếu đến form cha
            BuildLayout();
            LoadOrdersAsync(); // gọi khi khởi tạo
        }

        private void BuildLayout()
        {
            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10)
            };

            this.Controls.Add(flowPanel);
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                // Hiển thị thông báo "Đang tải..."
                Label lblLoading = new Label
                {
                    Text = "Đang tải dữ liệu...",
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Font = new System.Drawing.Font("Minion Pro", 14)
                };
                this.Controls.Add(lblLoading);

                // Tạo instance của OrderApiClient để sử dụng các phương thức API
                var apiClient = new OrderApiClient();

                // Lấy danh sách đơn hàng
                var orders = await apiClient.GetAllOrdersAsync();

                // Xóa thông báo "Đang tải..."
                this.Controls.Remove(lblLoading);
                lblLoading.Dispose();

                // Nếu không có đơn hàng nào
                if (orders == null || orders.Count == 0)
                {
                    Label lblNoOrders = new Label
                    {
                        Text = "Không có đơn hàng nào",
                        Dock = DockStyle.Fill,
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                        Font = new System.Drawing.Font("Minion Pro", 14)
                    };
                    this.Controls.Add(lblNoOrders);
                    return;
                }

                // Tạo từng OrderCardUC
                foreach (var order in orders)
                {
                    var card = new OrderCardUC();
                    card.SetData(order.TableId, order.Id, order.TotalGuest, order.TotalPrice);

                    // Đăng ký các sự kiện click cho OrderCardUC
                    card.ViewDetailsClicked += (s, e) => OrderCard_ViewDetails(order);
                    card.EditClicked += (s, e) => OrderCard_Edit(order);
                    card.PaymentClicked += (s, e) => OrderCard_Payment(order);
                    card.NotifyClicked += (s, e) => OrderCard_Notify(order);

                    // Thêm margin để tạo khoảng cách giữa các card
                    card.Margin = new Padding(10);

                    flowPanel.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải đơn hàng: " + ex.Message);
            }
        }

        // Xử lý sự kiện khi nhấn nút Xem chi tiết
        private void OrderCard_ViewDetails(Order order)
        {
            if (parent != null)
            {
                // Lấy thông tin bàn từ order
                var table = new Table { Id = order.TableId, TableName = $"Bàn {order.TableId}" };

                // Tạo OrderView và thiết lập OrderId
                var orderView = new OrderView(table);
                orderView.OrderId = order.Id;

                // Hiển thị OrderView
                parent.LoadUserControl(orderView);
            }
            else
            {
                MessageBox.Show($"Xem chi tiết đơn hàng #{order.Id} cho bàn {order.TableId}");
            }
        }

        // Xử lý sự kiện khi nhấn nút Sửa đơn
        private void OrderCard_Edit(Order order)
        {
            if (parent != null)
            {
                // Lấy thông tin bàn từ order
                var table = new Table { Id = order.TableId, TableName = $"Bàn {order.TableId}" };

                // Tạo OrderView và thiết lập OrderId
                var orderView = new OrderView(table);
                orderView.OrderId = order.Id;

                // Hiển thị OrderView
                parent.LoadUserControl(orderView);
            }
            else
            {
                MessageBox.Show($"Sửa đơn hàng #{order.Id} cho bàn {order.TableId}");
            }
        }

        // Xử lý sự kiện khi nhấn nút Thanh toán
        private async void OrderCard_Payment(Order order)
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

                        // Sau khi thanh toán xong, reload lại danh sách đơn hàng
                        if (paymentView.DialogResult == DialogResult.OK)
                        {
                            // Refresh lại danh sách đơn hàng
                            flowPanel.Controls.Clear();
                            LoadOrdersAsync();
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

        // Xử lý sự kiện khi nhấn nút Thông báo
        private void OrderCard_Notify(Order order)
        {
            // Gọi phương thức tái sử dụng từ TableOperationService
            TableOperationService.ProcessTableMoveAsync(order, () => {
                flowPanel.Controls.Clear();
                LoadOrdersAsync();
            });
        }
    }
}