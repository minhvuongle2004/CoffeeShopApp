using CoffeeShopAPI.Models;
using CoffeeShopApp.Controls;
using CoffeeShopApp.ServicesUser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopApp.Views.User
{
    public partial class OrderView : UserControl
    {
        private readonly OrderApiClient apiClient;
        private readonly OrderUIFactory uiFactory;
        private OrderCartManager cartManager;
        private Table currentTable;

        // Thêm thuộc tính OrderId để lưu ID của đơn hàng hiện có
        public int OrderId { get; set; } = -1;
        private Order currentOrder = null;

        // Constructor mặc định
        public OrderView()
        {
            InitializeComponent();

            apiClient = new OrderApiClient();
            uiFactory = new OrderUIFactory();

            InitializeControls();
            cartManager = new OrderCartManager(tableLayoutPanelCartItem, lblTotal);

            _ = LoadDataAsync();
        }

        // Constructor mới nhận thông tin bàn
        public OrderView(Table table)
        {
            InitializeComponent();

            apiClient = new OrderApiClient();
            uiFactory = new OrderUIFactory();

            InitializeControls();
            cartManager = new OrderCartManager(tableLayoutPanelCartItem, lblTotal);

            this.currentTable = table;

            // Hiển thị tên bàn nếu có
            if (currentTable != null)
            {
                lb_table.Text = "Bàn : " + currentTable.TableName;
            }

            _ = LoadDataAsync();
        }

        private void InitializeControls()
        {
            flowCategory.AutoScroll = true;
            flowCategory.FlowDirection = FlowDirection.TopDown;
            flowCategory.WrapContents = false;

            flowMenu.AutoScroll = true;
            flowMenu.FlowDirection = FlowDirection.LeftToRight;
            flowMenu.WrapContents = true;

            panelCart.AutoScroll = true;
            tableLayoutPanelCartItem.AutoScroll = true;
        }

        private async Task LoadDataAsync()
        {
            await LoadCategoryAsync();
            await LoadMenuAsync();

            // Nếu có OrderId thì load thông tin đơn hàng hiện có
            if (OrderId > 0)
            {
                await LoadExistingOrderAsync(OrderId);
            }
        }

        // Phương thức mới để tải thông tin đơn hàng hiện có
        private async Task LoadExistingOrderAsync(int orderId)
        {
            try
            {
                // Hiển thị thông báo đang tải
                var loadingLabel = new Label
                {
                    Text = "Đang tải thông tin đơn hàng...",
                    AutoSize = true,
                    Font = new Font("Minion Pro", 12),
                    Location = new Point(tableLayoutPanelCartItem.Width / 2 - 80, tableLayoutPanelCartItem.Height / 2)
                };
                tableLayoutPanelCartItem.Controls.Add(loadingLabel);

                // Thay đổi nút "Đặt món" thành "Cập nhật đơn hàng"
                btnDatMon.Text = "Cập nhật đơn hàng";

                // Lấy thông tin đơn hàng từ API
                currentOrder = await apiClient.GetOrderByIdAsync(orderId);

                if (currentOrder != null)
                {
                    // Lấy chi tiết đơn hàng
                    var orderDetails = await apiClient.GetOrderDetailsAsync(orderId);

                    // Xóa label đang tải
                    tableLayoutPanelCartItem.Controls.Remove(loadingLabel);

                    // Xóa tất cả các items trong giỏ hàng hiện tại
                    tableLayoutPanelCartItem.Controls.Clear();

                    // Reset tổng tiền về 0
                    lblTotal.Text = "0đ";

                    // Thêm các món ăn từ orderDetails vào giỏ hàng
                    foreach (var detail in orderDetails)
                    {
                        // Lấy thông tin menu item từ detail.MenuId
                        var menuItem = await apiClient.GetMenuByIdAsync(detail.MenuId);
                        if (menuItem != null)
                        {
                            // Thêm vào giỏ hàng số lần tương ứng với quantity
                            for (int i = 0; i < detail.Quantity; i++)
                            {
                                cartManager.AddToCart(menuItem, uiFactory);
                            }
                        }
                    }

                    // Hiển thị thông tin tổng tiền và số khách
                    lblTotal.Text = currentOrder.TotalPrice.ToString("N0") + "đ";

                    // Hiển thị thông báo đã tải xong đơn hàng
                    MessageBox.Show($"Đã tải đơn hàng #{currentOrder.Id} với tổng tiền {currentOrder.TotalPrice:N0}đ",
                                   "Thông tin đơn hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Xóa label đang tải
                    tableLayoutPanelCartItem.Controls.Remove(loadingLabel);

                    MessageBox.Show($"Không tìm thấy đơn hàng có ID: {orderId}",
                                   "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin đơn hàng: {ex.Message}",
                               "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadCategoryAsync()
        {
            try
            {
                var categories = await apiClient.GetAllCategoriesAsync();

                flowCategory.Controls.Clear();

                // Resize lại button theo chiều rộng panel
                flowCategory.Resize += (sender, e) =>
                {
                    foreach (Control control in flowCategory.Controls)
                    {
                        if (control is Button btn)
                            btn.Width = flowCategory.ClientSize.Width;
                    }
                };

                // Tạo nút "Tất cả" nhưng chỉ lưu tham chiếu
                Button allBtn = null;
                allBtn = uiFactory.CreateCategoryButton("Tất cả", 0, async (id) => {
                    // Đổi màu tất cả nút khác về màu mặc định
                    foreach (Control c in flowCategory.Controls)
                    {
                        if (c is Button b) b.BackColor = Color.FromArgb(245, 242, 235);
                    }

                    // Tô màu nút Tất cả khi đang chọn
                    allBtn.BackColor = Color.FromArgb(160, 180, 120);

                    await LoadMenuAsync();
                });

                allBtn.Width = flowCategory.ClientSize.Width;
                flowCategory.Controls.Add(allBtn);
                allBtn.PerformClick(); // Mặc định chọn tất cả khi load form

                foreach (var cat in categories)
                {
                    Button btn = null;
                    int catId = cat.Id; // Lưu ID vào biến cục bộ để sử dụng trong lambda

                    btn = uiFactory.CreateCategoryButton(cat.Name, catId, async (id) => {
                        // Đổi màu tất cả nút khác về trắng
                        foreach (Control c in flowCategory.Controls)
                        {
                            if (c is Button b) b.BackColor = Color.FromArgb(245, 242, 235);
                        }

                        // Tô màu nút đang chọn
                        btn.BackColor = Color.FromArgb(160, 180, 120);

                        await LoadMenuByCategoryAsync(id);
                    });

                    btn.Width = flowCategory.ClientSize.Width;
                    btn.BackColor = Color.FromArgb(245, 242, 235);
                    flowCategory.Controls.Add(btn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }

        private async Task LoadMenuByCategoryAsync(int categoryId)
        {
            try
            {
                var menus = await apiClient.GetMenusByCategoryAsync(categoryId);
                flowMenu.Controls.Clear();

                foreach (var menu in menus)
                {
                    Panel panel = uiFactory.CreateMenuPanel(menu, (selectedMenu) => {
                        cartManager.AddToCart(selectedMenu, uiFactory);
                    });

                    flowMenu.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải menu theo danh mục: " + ex.Message);
            }
        }

        private async Task LoadMenuAsync()
        {
            try
            {
                var menus = await apiClient.GetAllMenusAsync();
                flowMenu.Controls.Clear();

                foreach (var menu in menus)
                {
                    Panel panel = uiFactory.CreateMenuPanel(menu, (selectedMenu) => {
                        cartManager.AddToCart(selectedMenu, uiFactory);
                    });

                    flowMenu.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải menu: " + ex.Message);
            }
        }

        private async void btnDatMon_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem giỏ hàng có trống không
            if (cartManager.IsEmpty())
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng thêm món vào giỏ hàng trước khi đặt.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem đã chọn bàn chưa
            if (currentTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi đặt món.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy thông tin người dùng hiện tại từ form cha
                var parentForm = this.FindForm();
                CoffeeShopAPI.Models.User currentUser = null;

                // Lấy người dùng từ form cha
                if (parentForm is Views.User.EmployeeView employeeView)
                {
                    currentUser = employeeView._currentUser;
                }

                // Nếu không tìm thấy user, hiển thị thông báo lỗi
                if (currentUser == null)
                {
                    MessageBox.Show("Không thể xác định người dùng hiện tại. Vui lòng đăng nhập lại.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra nếu đang cập nhật đơn hàng hiện có
                if (OrderId > 0 && currentOrder != null)
                {
                    await UpdateExistingOrder(currentUser);
                }
                else // Tạo đơn hàng mới
                {
                    await CreateNewOrder(currentUser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức mới để cập nhật đơn hàng hiện có
        private async Task UpdateExistingOrder(CoffeeShopAPI.Models.User currentUser)
        {
            // Xác nhận cập nhật đơn hàng
            DialogResult result = MessageBox.Show(
                $"Xác nhận cập nhật đơn hàng cho bàn {currentTable.TableName} với tổng tiền {cartManager.TotalAmount.ToString("N0")}đ?",
                "Xác nhận cập nhật đơn hàng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            // Cập nhật thông tin đơn hàng
            currentOrder.TotalPrice = cartManager.TotalAmount;

            // Cập nhật đơn hàng trên server
            bool orderUpdated = await apiClient.UpdateOrderAsync(currentOrder);

            if (!orderUpdated)
            {
                MessageBox.Show("Cập nhật đơn hàng không thành công", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xóa các chi tiết đơn hàng cũ
            bool detailsDeleted = await apiClient.DeleteOrderDetailsAsync(OrderId);

            if (!detailsDeleted)
            {
                MessageBox.Show("Xóa chi tiết đơn hàng cũ không thành công", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thêm chi tiết đơn hàng mới
            List<OrderDetail> orderDetails = cartManager.GetOrderDetails(OrderId);
            bool allDetailsSaved = await AddOrderDetails(OrderId);

            if (!allDetailsSaved)
            {
                MessageBox.Show("Có lỗi khi thêm chi tiết đơn hàng mới", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thu thập thông tin cần thiết cho hóa đơn từ giỏ hàng
            List<(string menuName, int quantity, decimal price, decimal subtotal)> receiptItems = GetReceiptItems();

            // Thông báo thành công
            MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Hiển thị form hóa đơn
            using (var receiptForm = new HoaDonForm(currentOrder, currentUser, receiptItems))
            {
                receiptForm.ShowDialog();
            }

            // Quay lại SoDoUC sau khi đóng form hóa đơn
            if (this.ParentForm is Views.User.EmployeeView employeeView)
            {
                employeeView.ShowSoDoUC();
            }
            else
            {
                this.Dispose(); // Đóng control hiện tại
            }
        }

        // Phương thức tạo đơn hàng mới (từ phương thức cũ)
        private async Task CreateNewOrder(CoffeeShopAPI.Models.User currentUser)
        {
            // Hiển thị hộp thoại nhập số lượng khách
            int totalGuests = 1;
            using (var inputDialog = new GuestInputNumberDialog("Nhập số lượng khách:", "Số lượng khách", totalGuests))
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    totalGuests = inputDialog.Value;
                }
                else
                {
                    return; // Người dùng hủy
                }
            }

            // Hiển thị xác nhận đặt món
            DialogResult result = MessageBox.Show(
                $"Xác nhận đặt món cho bàn {currentTable.TableName} với tổng tiền {cartManager.TotalAmount.ToString("N0")}đ?",
                "Xác nhận đặt món",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            // Tạo đối tượng Order
            Order order = new Order
            {
                TableId = currentTable.Id,
                UserId = currentUser.Id,
                TotalPrice = cartManager.TotalAmount,
                TotalGuest = totalGuests,
                Status = "pending",
                StartTime = DateTime.Now
            };

            // Tạo đơn hàng
            int orderId = await apiClient.CreateOrderAsync(order);
            if (orderId <= 0)
            {
                MessageBox.Show("Tạo đơn hàng không thành công", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cập nhật ID của đơn hàng
            order.Id = orderId;

            // Thu thập thông tin cần thiết cho hóa đơn từ giỏ hàng
            List<(string menuName, int quantity, decimal price, decimal subtotal)> receiptItems = GetReceiptItems();

            // Lấy danh sách OrderDetails từ giỏ hàng
            List<OrderDetail> orderDetails = cartManager.GetOrderDetails(orderId);

            // Thêm chi tiết đơn hàng
            bool allDetailsSaved = await AddOrderDetails(orderId);
            if (allDetailsSaved)
            {
                // Cập nhật trạng thái bàn thành "full"
                bool tableUpdated = await apiClient.UpdateTableStatusAsync(currentTable, "full");

                // Thông báo thành công
                MessageBox.Show("Đặt món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Hiển thị form hóa đơn
                using (var receiptForm = new HoaDonForm(order, currentUser, receiptItems))
                {
                    receiptForm.ShowDialog();
                }

                // Quay lại SoDoUC sau khi đóng form hóa đơn
                if (this.ParentForm is Views.User.EmployeeView employeeView)
                {
                    employeeView.ShowSoDoUC();
                }
                else
                {
                    this.Dispose(); // Đóng control hiện tại
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm chi tiết đơn hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức lấy thông tin các món hàng cho hóa đơn
        private List<(string menuName, int quantity, decimal price, decimal subtotal)> GetReceiptItems()
        {
            List<(string menuName, int quantity, decimal price, decimal subtotal)> receiptItems = new List<(string, int, decimal, decimal)>();

            // Duyệt qua các mục trong tableLayoutPanelCartItem để thu thập thông tin
            foreach (Control control in tableLayoutPanelCartItem.Controls)
            {
                if (control is TableLayoutPanel row && row.Tag is int menuId)
                {
                    // Lấy tên sản phẩm (thường ở control đầu tiên)
                    string menuName = "";
                    if (row.Controls[0] is Label lblName)
                    {
                        menuName = lblName.Text;
                    }

                    // Lấy số lượng
                    int quantity = 1;
                    if (row.Controls[1] is Label lblQuantity)
                    {
                        int.TryParse(lblQuantity.Text, out quantity);
                    }

                    // Lấy tổng tiền của sản phẩm này
                    decimal subtotal = 0;
                    if (row.Controls[2] is Label lblPrice)
                    {
                        string priceText = lblPrice.Text.Replace("đ", "").Replace(".", "").Replace(",", "").Trim();
                        decimal.TryParse(priceText, out subtotal);
                    }

                    // Tính đơn giá
                    decimal price = quantity > 0 ? subtotal / quantity : 0;

                    // Thêm vào danh sách
                    receiptItems.Add((menuName, quantity, price, subtotal));
                }
            }

            return receiptItems;
        }

        // Phương thức thêm chi tiết đơn hàng
        private async Task<bool> AddOrderDetails(int orderId)
        {
            try
            {
                bool allSuccess = true;

                List<OrderDetail> details = cartManager.GetOrderDetails(orderId);

                foreach (var detail in details)
                {
                    bool success = await apiClient.AddOrderDetailAsync(detail);
                    if (!success)
                    {
                        allSuccess = false;
                    }
                }

                return allSuccess;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chi tiết đơn hàng: {ex.Message}",
                              "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void lb_table_Click(object sender, EventArgs e)
        {
            // Xử lý khi click vào tên bàn nếu cần
        }

        private void btnHuyHoaDon_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn hủy hóa đơn không?", "Xác nhận",
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Thực hiện các thao tác hủy hóa đơn nếu cần

                // Quay lại SoDoUC
                if (this.ParentForm is EmployeeView employeeView)
                {
                    employeeView.ShowSoDoUC();
                }
            }
        }
    }
}