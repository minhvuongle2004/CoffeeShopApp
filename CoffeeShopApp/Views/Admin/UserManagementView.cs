using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopAPI.Models;
using CoffeeShopApp.Services;

namespace CoffeeShopApp.Views.Admin
{
    public partial class UserManagementView : Form
    {
        private UserService _userService;
        private List<CoffeeShopAPI.Models.User> _users;
        private CoffeeShopAPI.Models.User _selectedUser;

        public UserManagementView()
        {
            InitializeComponent();
            _userService = new UserService();

            // Thiết lập sự kiện load form
            this.Load += UserManagementView_Load;

            // Thiết lập sự kiện cell click cho DataGridView
            dgvNhanVien.CellClick += DgvNhanVien_CellClick;

            // Khởi tạo giá trị cho ComboBox vai trò
            InitRoleComboBox();

            // Đặt placeholder cho ô tìm kiếm
            if (Controls.Find("txtSearch", true).FirstOrDefault() is TextBox txtSearch)
            {
                txtSearch.Enter += (s, e) => {
                    if (txtSearch.Text == "Tìm kiếm...")
                    {
                        txtSearch.Text = "";
                        txtSearch.ForeColor = Color.Black;
                    }
                };

                txtSearch.Leave += (s, e) => {
                    if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        txtSearch.Text = "Tìm kiếm...";
                        txtSearch.ForeColor = Color.Gray;
                    }
                };

                // Khởi tạo giá trị mặc định cho txtSearch
                txtSearch.Text = "Tìm kiếm...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void InitRoleComboBox()
        {
            // Thêm các vai trò vào ComboBox
            cbVaiTro.Items.Clear();
            cbVaiTro.Items.Add("admin");
            cbVaiTro.Items.Add("cashier");
            cbVaiTro.Items.Add("staff");

            // Chọn giá trị mặc định
            cbVaiTro.SelectedIndex = 2; // Mặc định là "staff"
        }

        private async void UserManagementView_Load(object sender, EventArgs e)
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Lấy danh sách người dùng từ API
                _users = await _userService.GetAllUsersAsync();

                // Hiển thị dữ liệu lên DataGridView
                DisplayUsers(_users);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void DisplayUsers(List<CoffeeShopAPI.Models.User> users)
        {
            // Xóa các binding cũ
            dgvNhanVien.DataSource = null;

            // Đặt binding mới
            dgvNhanVien.DataSource = users;

            // Thiết lập tên hiển thị cho các cột
            if (dgvNhanVien.Columns.Contains("Id"))
                dgvNhanVien.Columns["Id"].HeaderText = "Mã";

            if (dgvNhanVien.Columns.Contains("Fullname"))
                dgvNhanVien.Columns["Fullname"].HeaderText = "Họ và tên";

            if (dgvNhanVien.Columns.Contains("Username"))
                dgvNhanVien.Columns["Username"].HeaderText = "Tên đăng nhập";

            if (dgvNhanVien.Columns.Contains("Role"))
                dgvNhanVien.Columns["Role"].HeaderText = "Vai trò";

            if (dgvNhanVien.Columns.Contains("Phone"))
                dgvNhanVien.Columns["Phone"].HeaderText = "Số điện thoại";

            // Ẩn cột Password nếu có
            if (dgvNhanVien.Columns.Contains("Password"))
                dgvNhanVien.Columns["Password"].Visible = false;
        }

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhanVien.Rows.Count)
            {
                // Lấy thông tin người dùng được chọn
                _selectedUser = new CoffeeShopAPI.Models.User
                {
                    Id = Convert.ToInt32(dgvNhanVien.Rows[e.RowIndex].Cells["Id"].Value),
                    Fullname = dgvNhanVien.Rows[e.RowIndex].Cells["Fullname"].Value.ToString(),
                    Username = dgvNhanVien.Rows[e.RowIndex].Cells["Username"].Value.ToString(),
                    Role = dgvNhanVien.Rows[e.RowIndex].Cells["Role"].Value.ToString(),
                    Phone = dgvNhanVien.Rows[e.RowIndex].Cells["Phone"].Value?.ToString()
                };

                // Hiển thị thông tin lên TextBox
                txtHoTen.Text = _selectedUser.Fullname;
                txtTenDangNhap.Text = _selectedUser.Username;
                txtMatKhau.Text = ""; // Không hiển thị mật khẩu

                // Chọn vai trò trong ComboBox
                switch (_selectedUser.Role.ToLower())
                {
                    case "admin":
                        cbVaiTro.SelectedIndex = 0;
                        break;
                    case "cashier":
                        cbVaiTro.SelectedIndex = 1;
                        break;
                    case "staff":
                        cbVaiTro.SelectedIndex = 2;
                        break;
                    default:
                        cbVaiTro.SelectedIndex = -1;
                        break;
                }

                txtSDT.Text = _selectedUser.Phone;
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateUserData(true))
                return;

            try
            {
                // Tạo đối tượng người dùng mới
                CoffeeShopAPI.Models.User newUser = new CoffeeShopAPI.Models.User
                {
                    Fullname = txtHoTen.Text.Trim(),
                    Username = txtTenDangNhap.Text.Trim(),
                    Password = txtMatKhau.Text.Trim(),
                    Role = cbVaiTro.SelectedItem.ToString(),
                    Phone = string.IsNullOrWhiteSpace(txtSDT.Text) ? null : txtSDT.Text.Trim()
                };

                // Gọi API để thêm người dùng mới
                CoffeeShopAPI.Models.User addedUser = await _userService.RegisterUserAsync(newUser);

                if (addedUser != null)
                {
                    // Tải lại danh sách người dùng
                    await LoadUsers();

                    // Xóa dữ liệu trên form
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn người dùng chưa
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateUserData(false))
                return;

            try
            {
                // Cập nhật thông tin người dùng
                CoffeeShopAPI.Models.User updatedUser = new CoffeeShopAPI.Models.User
                {
                    Id = _selectedUser.Id,
                    Fullname = txtHoTen.Text.Trim(),
                    Username = txtTenDangNhap.Text.Trim(),
                    Role = cbVaiTro.SelectedItem.ToString(),
                    Phone = string.IsNullOrWhiteSpace(txtSDT.Text) ? null : txtSDT.Text.Trim()
                };

                // Gọi API để cập nhật người dùng
                bool updated = await _userService.UpdateUserAsync(_selectedUser.Id, updatedUser);

                if (updated)
                {
                    // Tải lại danh sách người dùng
                    await LoadUsers();

                    // Xóa dữ liệu trên form và reset người dùng đã chọn
                    ClearForm();
                    _selectedUser = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn người dùng chưa
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa người dùng '{_selectedUser.Fullname}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Gọi API để xóa người dùng
                    bool deleted = await _userService.DeleteUserAsync(_selectedUser.Id);

                    if (deleted)
                    {
                        // Tải lại danh sách người dùng
                        await LoadUsers();

                        // Xóa dữ liệu trên form và reset người dùng đã chọn
                        ClearForm();
                        _selectedUser = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (Controls.Find("txtSearch", true).FirstOrDefault() is TextBox txtSearch)
            {
                string searchTerm = txtSearch.Text;
                if (searchTerm == "Tìm kiếm...")
                    searchTerm = "";

                try
                {
                    Cursor = Cursors.WaitCursor;

                    // Tìm kiếm người dùng
                    List<CoffeeShopAPI.Models.User> searchResults = await _userService.SearchUsersAsync(searchTerm);

                    // Hiển thị kết quả tìm kiếm
                    DisplayUsers(searchResults);

                    Cursor = Cursors.Default;

                    // Hiển thị thông báo nếu không có kết quả
                    if (searchResults.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                }
            }
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đã xử lý ở sự kiện DgvNhanVien_CellClick
        }

        private bool ValidateUserData(bool isNewUser)
        {
            // Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // Kiểm tra tên đăng nhập
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            // Kiểm tra mật khẩu khi thêm mới
            if (isNewUser && string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            // Kiểm tra đã chọn vai trò chưa
            if (cbVaiTro.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbVaiTro.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtHoTen.Text = "";
            txtTenDangNhap.Text = "";
            txtMatKhau.Text = "";
            cbVaiTro.SelectedIndex = 2; // Mặc định là "staff"
            txtSDT.Text = "";
            txtHoTen.Focus();
        }
    }
}