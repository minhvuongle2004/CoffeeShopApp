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
                    // Đổi màu tất cả nút khác về trắng
                    foreach (Control c in flowCategory.Controls)
                    {
                        if (c is Button b) b.BackColor = Color.White;
                    }

                    // Tô màu nút đang chọn
                    allBtn.BackColor = Color.LightGray;

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
                            if (c is Button b) b.BackColor = Color.White;
                        }

                        // Tô màu nút đang chọn
                        btn.BackColor = Color.LightGray;

                        await LoadMenuByCategoryAsync(id);
                    });

                    btn.Width = flowCategory.ClientSize.Width;
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

            // Hiển thị hộp thoại nhập số lượng khách
            int totalGuests = 1;
            using (var inputDialog = new OrderInputNumberDialog("Nhập số lượng khách:", "Số lượng khách", totalGuests))
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

                // Thêm chi tiết đơn hàng
                bool allDetailsSaved = await AddOrderDetails(orderId);
                if (allDetailsSaved)
                {
                    // Cập nhật trạng thái bàn thành "full"
                    bool tableUpdated = await apiClient.UpdateTableStatusAsync(currentTable, "full");

                    // Thông báo thành công
                    MessageBox.Show("Đặt món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Quay lại SoDoUC
                    if (parentForm is Views.User.EmployeeView employeeView2)
                    {
                        employeeView2.ShowSoDoUC();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            
        }
    }
}