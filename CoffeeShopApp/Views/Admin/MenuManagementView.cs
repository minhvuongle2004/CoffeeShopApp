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
using CoffeeShopApp.Services;

namespace CoffeeShopApp.Views.Admin
{
    public partial class MenuManagementView : Form
    {
        private MenuService _menuService;
        private CategoryService _categoryService;
        private List<CoffeeShopAPI.Models.Menu> _menus;
        private List<Category> _categories;
        private CoffeeShopAPI.Models.Menu _selectedMenu;
        private string _selectedImagePath;
        public MenuManagementView()
        {
            InitializeComponent();
            _menuService = new MenuService();
            _categoryService = new CategoryService();

            ptbHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
            // Thiết lập sự kiện load form
            this.Load += MenuManagementView_Load;

            // Thiết lập sự kiện cell click cho DataGridView
            dgvMenu.CellClick += DgvMenu_CellClick;

            // Đặt placeholder cho ô tìm kiếm
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

        private async void MenuManagementView_Load(object sender, EventArgs e)
        {
            await LoadCategories();
            await LoadMenus();
        }

        private async Task LoadCategories()
        {
            try
            {
                // Lấy danh sách danh mục từ API
                _categories = await _categoryService.GetAllCategoriesAsync();

                // Reset combobox
                cbDanhMuc.DataSource = null;
                cbDanhMuc.Items.Clear();

                // Hiển thị danh mục vào ComboBox
                cbDanhMuc.DataSource = _categories;
                cbDanhMuc.DisplayMember = "Name";
                cbDanhMuc.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadMenus()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Lấy danh sách món từ API
                _menus = await _menuService.GetAllMenusAsync();

                // Hiển thị dữ liệu lên DataGridView
                DisplayMenus(_menus);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void DisplayMenus(List<CoffeeShopAPI.Models.Menu> menus)
        {
            // Xóa các binding cũ
            dgvMenu.DataSource = null;

            // Tạo một bảng dữ liệu mới để hiển thị tên danh mục thay vì id
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên món", typeof(string));
            dt.Columns.Add("Danh mục", typeof(string));
            dt.Columns.Add("Giá", typeof(decimal));
            dt.Columns.Add("Hình ảnh", typeof(string));
            dt.Columns.Add("CategoryId", typeof(int));

            foreach (var menu in menus)
            {
                string categoryName = _categories.FirstOrDefault(c => c.Id == menu.CategoryId)?.Name ?? "Không xác định";
                dt.Rows.Add(menu.Id, menu.Name, categoryName, menu.Price, menu.Image, menu.CategoryId);
            }

            // Đặt binding mới
            dgvMenu.DataSource = dt;

            // Ẩn cột CategoryId
            if (dgvMenu.Columns.Contains("CategoryId"))
                dgvMenu.Columns["CategoryId"].Visible = false;

            // Định dạng cột giá tiền
            if (dgvMenu.Columns.Contains("Giá"))
                dgvMenu.Columns["Giá"].DefaultCellStyle.Format = "N0";
        }

        private void DgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvMenu.Rows.Count)
            {
                DataGridViewRow row = dgvMenu.Rows[e.RowIndex];

                // Lấy thông tin món được chọn
                _selectedMenu = new CoffeeShopAPI.Models.Menu
                {
                    Id = Convert.ToInt32(row.Cells["Id"].Value),
                    Name = row.Cells["Tên món"].Value.ToString(),
                    CategoryId = Convert.ToInt32(row.Cells["CategoryId"].Value),
                    Price = Convert.ToDecimal(row.Cells["Giá"].Value),
                    Image = row.Cells["Hình ảnh"].Value?.ToString()
                };

                // Hiển thị thông tin lên TextBox và ComboBox
                txtTenDoUong.Text = _selectedMenu.Name;
                txtGiaTien.Text = _selectedMenu.Price.ToString();

                // Chọn danh mục trong ComboBox
                for (int i = 0; i < cbDanhMuc.Items.Count; i++)
                {
                    if ((cbDanhMuc.Items[i] as Category).Id == _selectedMenu.CategoryId)
                    {
                        cbDanhMuc.SelectedIndex = i;
                        break;
                    }
                }

                // Hiển thị hình ảnh nếu có
                // Phần xử lý hiển thị hình ảnh trong DgvMenu_CellClick
                // Hiển thị hình ảnh nếu có
                if (!string.IsNullOrEmpty(_selectedMenu.Image))
                {
                    try
                    {
                        string fullPath = Path.Combine(Application.StartupPath, _selectedMenu.Image);
                        if (File.Exists(fullPath))
                        {
                            // Giải phóng tài nguyên của hình ảnh cũ nếu có
                            if (ptbHinhAnh.Image != null)
                            {
                                var temp = ptbHinhAnh.Image;
                                ptbHinhAnh.Image = null;
                                temp.Dispose();
                            }

                            // Đọc hình ảnh và hiển thị
                            using (var stream = new MemoryStream(File.ReadAllBytes(fullPath)))
                            {
                                ptbHinhAnh.Image = Image.FromStream(stream);
                            }

                            // Đặt SizeMode để hình ảnh vừa khít với PictureBox
                            ptbHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        else
                        {
                            ptbHinhAnh.Image = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi hiển thị hình ảnh: {ex.Message}");
                        ptbHinhAnh.Image = null;
                    }
                }
                else
                {
                    ptbHinhAnh.Image = null;
                }
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateMenuData())
                return;

            try
            {
                // Copy hình ảnh vào thư mục debug/image nếu có
                string imagePath = null;
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    imagePath = CopyImageToDebugFolder(_selectedImagePath);
                }

                // Tạo đối tượng món mới
                CoffeeShopAPI.Models.Menu newMenu = new CoffeeShopAPI.Models.Menu
                {
                    Name = txtTenDoUong.Text.Trim(),
                    CategoryId = (int)cbDanhMuc.SelectedValue,
                    Price = decimal.Parse(txtGiaTien.Text.Trim()),
                    Image = imagePath // Sử dụng đường dẫn hình ảnh đã copy
                };

                // Gọi API để thêm món mới
                CoffeeShopAPI.Models.Menu addedMenu = await _menuService.AddMenuAsync(newMenu);

                if (addedMenu != null)
                {
                    // Tải lại danh sách món
                    await LoadMenus();

                    // Xóa dữ liệu trên form
                    ClearForm();
                    _selectedImagePath = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn món chưa
            if (_selectedMenu == null)
            {
                MessageBox.Show("Vui lòng chọn món cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateMenuData())
                return;

            try
            {
                string imagePath = _selectedMenu.Image; // Giữ nguyên đường dẫn hình cũ nếu không chọn hình mới

                // Xử lý hình ảnh mới nếu có
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    // Xóa hình ảnh cũ trước khi thêm hình mới
                    DeleteOldImage(_selectedMenu.Image);

                    // Copy hình ảnh mới vào thư mục debug/image
                    imagePath = CopyImageToDebugFolder(_selectedImagePath);
                }

                // Cập nhật thông tin món
                _selectedMenu.Name = txtTenDoUong.Text.Trim();
                _selectedMenu.CategoryId = (int)cbDanhMuc.SelectedValue;
                _selectedMenu.Price = decimal.Parse(txtGiaTien.Text.Trim());
                _selectedMenu.Image = imagePath;

                // Gọi API để cập nhật món
                bool updated = await _menuService.UpdateMenuAsync(_selectedMenu);

                if (updated)
                {
                    // Tải lại danh sách món
                    await LoadMenus();

                    // Xóa dữ liệu trên form và reset món đã chọn
                    ClearForm();
                    _selectedMenu = null;
                    _selectedImagePath = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn món chưa
            if (_selectedMenu == null)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa món '{_selectedMenu.Name}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Lưu lại đường dẫn hình ảnh để xóa sau
                    string imagePath = _selectedMenu.Image;

                    // Gọi API để xóa món
                    bool deleted = await _menuService.DeleteMenuAsync(_selectedMenu.Id);

                    if (deleted)
                    {
                        // Xóa hình ảnh liên quan
                        DeleteOldImage(imagePath);

                        // Tải lại danh sách món
                        await LoadMenus();

                        // Xóa dữ liệu trên form và reset món đã chọn
                        ClearForm();
                        _selectedMenu = null;
                        _selectedImagePath = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;
            if (searchTerm == "Tìm kiếm...")
                searchTerm = "";

            try
            {
                Cursor = Cursors.WaitCursor;

                // Tìm kiếm món
                List<CoffeeShopAPI.Models.Menu> searchResults = await _menuService.SearchMenusAsync(searchTerm);

                // Hiển thị kết quả tìm kiếm
                DisplayMenus(searchResults);

                Cursor = Cursors.Default;

                // Hiển thị thông báo nếu không có kết quả
                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Chọn hình ảnh";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Lưu đường dẫn hình ảnh được chọn
                        _selectedImagePath = openFileDialog.FileName;

                        // Giải phóng tài nguyên của hình ảnh cũ nếu có
                        if (ptbHinhAnh.Image != null)
                        {
                            var temp = ptbHinhAnh.Image;
                            ptbHinhAnh.Image = null;
                            temp.Dispose();
                        }

                        // Đọc hình ảnh và hiển thị
                        using (var stream = new MemoryStream(File.ReadAllBytes(_selectedImagePath)))
                        {
                            ptbHinhAnh.Image = Image.FromStream(stream);
                        }

                        // Đặt SizeMode để hình ảnh vừa khít với PictureBox
                        ptbHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tải hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _selectedImagePath = null;
                        ptbHinhAnh.Image = null;
                    }
                }
            }
        }
        // Thêm phương thức mới để xử lý việc copy hình ảnh vào thư mục debug/image
        private string CopyImageToDebugFolder(string sourcePath)
        {
            try
            {
                if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
                    return null;

                // Tạo đường dẫn đến thư mục image trong debug
                string debugImageFolder = Path.Combine(Application.StartupPath, "image");

                // Kiểm tra thư mục image đã tồn tại chưa, nếu chưa thì tạo mới
                if (!Directory.Exists(debugImageFolder))
                {
                    Directory.CreateDirectory(debugImageFolder);
                }

                // Lấy tên file từ đường dẫn
                string fileName = Path.GetFileName(sourcePath);

                // Đường dẫn đích để lưu file
                string destinationPath = Path.Combine(debugImageFolder, fileName);

                // Copy file hình ảnh
                File.Copy(sourcePath, destinationPath, true);

                // Trả về đường dẫn tương đối để lưu vào database
                return Path.Combine("image/", fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi copy hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        // Thêm phương thức để xóa hình ảnh cũ
        private void DeleteOldImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string fullPath = Path.Combine(Application.StartupPath, imagePath);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Chỉ ghi log lỗi, không hiển thị thông báo để không làm gián đoạn quy trình
                Console.WriteLine($"Lỗi khi xóa hình ảnh cũ: {ex.Message}");
            }
        }

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đã xử lý ở sự kiện DgvMenu_CellClick
        }

        private bool ValidateMenuData()
        {
            // Kiểm tra tên món
            if (string.IsNullOrWhiteSpace(txtTenDoUong.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDoUong.Focus();
                return false;
            }

            // Kiểm tra danh mục
            if (cbDanhMuc.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbDanhMuc.Focus();
                return false;
            }

            // Kiểm tra giá tiền
            if (string.IsNullOrWhiteSpace(txtGiaTien.Text))
            {
                MessageBox.Show("Vui lòng nhập giá tiền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaTien.Focus();
                return false;
            }

            decimal price;
            if (!decimal.TryParse(txtGiaTien.Text, out price) || price <= 0)
            {
                MessageBox.Show("Giá tiền không hợp lệ! Vui lòng nhập số dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaTien.Focus();
                return false;
            }

            return true;
        }
        private void ClearForm()
        {
            txtTenDoUong.Text = "";
            txtGiaTien.Text = "";
            if (cbDanhMuc.Items.Count > 0)
                cbDanhMuc.SelectedIndex = 0;
            ptbHinhAnh.Image = null;
            _selectedImagePath = null; // Reset biến lưu đường dẫn hình ảnh được chọn
            txtTenDoUong.Focus();
        }

        private void ptbHinhAnh_Click(object sender, EventArgs e)
        {

        }
    }
}