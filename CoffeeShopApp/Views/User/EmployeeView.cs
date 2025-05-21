// EmployeeView.cs
using CoffeeShopApp.Views.User;
using System;
using System.Windows.Forms;
using CoffeeShopAPI.Models; // Sử dụng model User từ project API

namespace CoffeeShopApp.Views.User
{
    public partial class EmployeeView : Form
    {
        public CoffeeShopAPI.Models.User _currentUser; // Thêm biến để lưu thông tin người dùng

        public EmployeeView()
        {
            InitializeComponent();
            LoadUserControl(new ThongKeUC());
        }
        public CoffeeShopAPI.Models.User CurrentUser { get; private set; }

        // Constructor mới nhận thông tin User
        public EmployeeView(CoffeeShopAPI.Models.User user)
        {
            InitializeComponent();
            _currentUser = user;

            // Tạo ThongKeUC và thiết lập user
            ThongKeUC thongKeUc = new ThongKeUC();
            thongKeUc.SetCurrentUser(user); // Dùng phương thức mới
            LoadUserControl(thongKeUc);

            // Hiển thị fullname của người dùng lên label
            if (_currentUser != null && !string.IsNullOrEmpty(_currentUser.Fullname))
            {
                lblUser.Text = _currentUser.Fullname;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ThongKeUC thongKeUc = new ThongKeUC();
            thongKeUc.SetCurrentUser(_currentUser);
            LoadUserControl(thongKeUc);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            LoadUserControl(new OrderUC(this));
        }

        private void btnSodo_Click(object sender, EventArgs e)
        {
            LoadUserControl(new SoDoUC(this));
        }

        public void LoadUserControl(UserControl uc)
        {
            panelContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelContent.Controls.Add(uc);
        }

        public void ShowSoDoUC()
        {
            LoadUserControl(new SoDoUC(this));
        }

        private void picCloudSync_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện click vào nút đồng bộ cloud nếu cần
        }

        private void lblUser_Click(object sender, EventArgs e)
        {
            // Có thể thêm chức năng hiển thị thông tin chi tiết người dùng khi click vào tên
        }
    }
}