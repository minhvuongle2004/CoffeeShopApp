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
    public partial class InventoryManagementView : Form
    {
        private InventoryService _inventoryService;
        private List<Inventory> _inventoryItems;
        private Inventory _selectedInventory;

        public InventoryManagementView()
        {
            InitializeComponent();
            _inventoryService = new InventoryService();

            // Thiết lập sự kiện load form
            this.Load += InventoryManagementView_Load;

            // Thiết lập sự kiện cell click cho DataGridView
            dgvInventory.CellClick += DgvInventory_CellClick;

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

        private async void InventoryManagementView_Load(object sender, EventArgs e)
        {
            await LoadInventory();
        }

        private async Task LoadInventory()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Lấy danh sách nguyên liệu từ API
                _inventoryItems = await _inventoryService.GetAllInventoryAsync();

                // Hiển thị dữ liệu lên DataGridView
                DisplayInventory(_inventoryItems);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void DisplayInventory(List<Inventory> inventoryItems)
        {
            // Xóa các binding cũ
            dgvInventory.DataSource = null;

            // Đặt binding mới
            dgvInventory.DataSource = inventoryItems;

            // Thiết lập tên hiển thị cho các cột
            if (dgvInventory.Columns.Contains("Id"))
                dgvInventory.Columns["Id"].HeaderText = "Mã nguyên liệu";

            if (dgvInventory.Columns.Contains("Name"))
                dgvInventory.Columns["Name"].HeaderText = "Tên nguyên liệu";

            if (dgvInventory.Columns.Contains("Stock"))
                dgvInventory.Columns["Stock"].HeaderText = "Tồn kho";

            if (dgvInventory.Columns.Contains("Unit"))
                dgvInventory.Columns["Unit"].HeaderText = "Đơn vị";

            if (dgvInventory.Columns.Contains("UpdatedAt"))
                dgvInventory.Columns["UpdatedAt"].HeaderText = "Ngày cập nhật";
        }

        private void DgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvInventory.Rows.Count)
            {
                // Lấy thông tin nguyên liệu được chọn
                _selectedInventory = new Inventory
                {
                    Id = Convert.ToInt32(dgvInventory.Rows[e.RowIndex].Cells["Id"].Value),
                    Name = dgvInventory.Rows[e.RowIndex].Cells["Name"].Value.ToString(),
                    Stock = Convert.ToInt32(dgvInventory.Rows[e.RowIndex].Cells["Stock"].Value),
                    Unit = dgvInventory.Rows[e.RowIndex].Cells["Unit"].Value.ToString(),
                    UpdatedAt = Convert.ToDateTime(dgvInventory.Rows[e.RowIndex].Cells["UpdatedAt"].Value)
                };

                // Hiển thị thông tin lên TextBox
                txtTenNguyenLieu.Text = _selectedInventory.Name;
                txtTonKho.Text = _selectedInventory.Stock.ToString();
                txtDonVi.Text = _selectedInventory.Unit;
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateInventoryData())
                return;

            try
            {
                // Tạo đối tượng nguyên liệu mới
                Inventory newInventory = new Inventory
                {
                    Name = txtTenNguyenLieu.Text.Trim(),
                    Stock = int.Parse(txtTonKho.Text.Trim()),
                    Unit = txtDonVi.Text.Trim(),
                    UpdatedAt = DateTime.Now
                };

                // Gọi API để thêm nguyên liệu mới
                Inventory addedInventory = await _inventoryService.AddInventoryAsync(newInventory);

                if (addedInventory != null)
                {
                    // Tải lại danh sách nguyên liệu
                    await LoadInventory();

                    // Xóa dữ liệu trên form
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn nguyên liệu chưa
            if (_selectedInventory == null)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateInventoryData())
                return;

            try
            {
                // Cập nhật thông tin nguyên liệu
                _selectedInventory.Name = txtTenNguyenLieu.Text.Trim();
                _selectedInventory.Stock = int.Parse(txtTonKho.Text.Trim());
                _selectedInventory.Unit = txtDonVi.Text.Trim();
                _selectedInventory.UpdatedAt = DateTime.Now;

                // Gọi API để cập nhật nguyên liệu
                bool updated = await _inventoryService.UpdateInventoryAsync(_selectedInventory);

                if (updated)
                {
                    // Tải lại danh sách nguyên liệu
                    await LoadInventory();

                    // Xóa dữ liệu trên form và reset nguyên liệu đã chọn
                    ClearForm();
                    _selectedInventory = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn nguyên liệu chưa
            if (_selectedInventory == null)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nguyên liệu '{_selectedInventory.Name}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Gọi API để xóa nguyên liệu
                    bool deleted = await _inventoryService.DeleteInventoryAsync(_selectedInventory.Id);

                    if (deleted)
                    {
                        // Tải lại danh sách nguyên liệu
                        await LoadInventory();

                        // Xóa dữ liệu trên form và reset nguyên liệu đã chọn
                        ClearForm();
                        _selectedInventory = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Tìm kiếm nguyên liệu
                List<Inventory> searchResults = await _inventoryService.SearchInventoryAsync(searchTerm);

                // Hiển thị kết quả tìm kiếm
                DisplayInventory(searchResults);

                Cursor = Cursors.Default;

                // Hiển thị thông báo nếu không có kết quả
                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đã xử lý ở sự kiện DgvInventory_CellClick
        }

        private bool ValidateInventoryData()
        {
            // Kiểm tra tên nguyên liệu
            if (string.IsNullOrWhiteSpace(txtTenNguyenLieu.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNguyenLieu.Focus();
                return false;
            }

            // Kiểm tra tồn kho
            if (string.IsNullOrWhiteSpace(txtTonKho.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng tồn kho!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTonKho.Focus();
                return false;
            }

            // Kiểm tra tồn kho có phải là số không
            if (!int.TryParse(txtTonKho.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Số lượng tồn kho phải là số không âm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTonKho.Focus();
                return false;
            }

            // Kiểm tra đơn vị
            if (string.IsNullOrWhiteSpace(txtDonVi.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn vị tính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonVi.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtTenNguyenLieu.Text = "";
            txtTonKho.Text = "";
            txtDonVi.Text = "";
            txtTenNguyenLieu.Focus();
        }
    }
}