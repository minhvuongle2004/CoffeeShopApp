using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CoffeeShopAPI.Models;
using CoffeeShopApp.Services;

namespace CoffeeShopApp.Views.Admin
{
    public partial class CategoryManagementView : Form
    {
        private CategoryService _categoryService;
        private List<Category> _categories;
        private Category _selectedCategory;

        public CategoryManagementView()
        {
            InitializeComponent();
            _categoryService = new CategoryService();

            // Thiết lập sự kiện load form
            this.Load += CategoryManagementView_Load;

            // Thiết lập sự kiện cell click cho DataGridView
            dgvDanhMuc.CellClick += DgvDanhMuc_CellClick;

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

        private async void CategoryManagementView_Load(object sender, EventArgs e)
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Lấy danh sách danh mục từ API
                _categories = await _categoryService.GetAllCategoriesAsync();

                // Hiển thị dữ liệu lên DataGridView
                DisplayCategories(_categories);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void DisplayCategories(List<Category> categories)
        {
            // Xóa các binding cũ
            dgvDanhMuc.DataSource = null;

            // Đặt binding mới
            dgvDanhMuc.DataSource = categories;

            // Thiết lập tên hiển thị cho các cột
            if (dgvDanhMuc.Columns.Contains("Id"))
                dgvDanhMuc.Columns["Id"].HeaderText = "Mã danh mục";

            if (dgvDanhMuc.Columns.Contains("Name"))
                dgvDanhMuc.Columns["Name"].HeaderText = "Tên danh mục";


        }

        private void DgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDanhMuc.Rows.Count)
            {
                // Lấy thông tin danh mục được chọn
                _selectedCategory = new Category
                {
                    Id = Convert.ToInt32(dgvDanhMuc.Rows[e.RowIndex].Cells["Id"].Value),
                    Name = dgvDanhMuc.Rows[e.RowIndex].Cells["Name"].Value.ToString()
                };

                // Hiển thị thông tin lên TextBox
                txtDanhMuc.Text = _selectedCategory.Name;
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateCategoryData())
                return;

            try
            {
                // Tạo đối tượng danh mục mới
                Category newCategory = new Category
                {
                    Name = txtDanhMuc.Text.Trim()
                };

                // Gọi API để thêm danh mục mới
                Category addedCategory = await _categoryService.AddCategoryAsync(newCategory);

                if (addedCategory != null)
                {
                    // Tải lại danh sách danh mục
                    await LoadCategories();

                    // Xóa dữ liệu trên form
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn danh mục chưa
            if (_selectedCategory == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateCategoryData())
                return;

            try
            {
                // Cập nhật thông tin danh mục
                _selectedCategory.Name = txtDanhMuc.Text.Trim();

                // Gọi API để cập nhật danh mục
                bool updated = await _categoryService.UpdateCategoryAsync(_selectedCategory);

                if (updated)
                {
                    // Tải lại danh sách danh mục
                    await LoadCategories();

                    // Xóa dữ liệu trên form và reset danh mục đã chọn
                    ClearForm();
                    _selectedCategory = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn danh mục chưa
            if (_selectedCategory == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa danh mục '{_selectedCategory.Name}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Gọi API để xóa danh mục
                    bool deleted = await _categoryService.DeleteCategoryAsync(_selectedCategory.Id);

                    if (deleted)
                    {
                        // Tải lại danh sách danh mục
                        await LoadCategories();

                        // Xóa dữ liệu trên form và reset danh mục đã chọn
                        ClearForm();
                        _selectedCategory = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Tìm kiếm danh mục
                List<Category> searchResults = await _categoryService.SearchCategoriesAsync(searchTerm);

                // Hiển thị kết quả tìm kiếm
                DisplayCategories(searchResults);

                Cursor = Cursors.Default;

                // Hiển thị thông báo nếu không có kết quả
                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đã xử lý ở sự kiện DgvDanhMuc_CellClick
        }

        private bool ValidateCategoryData()
        {
            // Kiểm tra tên danh mục
            if (string.IsNullOrWhiteSpace(txtDanhMuc.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDanhMuc.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtDanhMuc.Text = "";
            txtDanhMuc.Focus();
        }
    }
}