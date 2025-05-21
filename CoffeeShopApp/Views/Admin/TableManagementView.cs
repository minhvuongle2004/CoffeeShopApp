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
    public partial class TableManagementView : Form
    {
        private TableService _tableService;
        private List<Table> _tables;
        private Table _selectedTable;

        public TableManagementView()
        {
            InitializeComponent();
            _tableService = new TableService();

            // Thiết lập sự kiện load form
            this.Load += TableManagementView_Load;

            // Thiết lập sự kiện cell click cho DataGridView
            dgvBan.CellClick += DgvBan_CellClick;

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

            // Thiết lập sự kiện cho các button
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnSearch.Click += btnSearch_Click;

            // Thiết lập giá trị mặc định cho ComboBox
            if (cbTrangThai.Items.Count > 0)
                cbTrangThai.SelectedIndex = 1; // Mặc định là "Trống"

            // Khởi tạo giá trị mặc định cho txtSearch
            txtSearch.Text = "Tìm kiếm...";
            txtSearch.ForeColor = Color.Gray;
        }

        private async void TableManagementView_Load(object sender, EventArgs e)
        {
            await LoadTables();
        }

        private async Task LoadTables()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Lấy danh sách bàn từ API
                _tables = await _tableService.GetAllTablesAsync();

                // Hiển thị dữ liệu lên DataGridView
                DisplayTables(_tables);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void DisplayTables(List<Table> tables)
        {
            // Xóa dữ liệu cũ
            dgvBan.Rows.Clear();

            // Thêm dữ liệu mới vào DataGridView
            int index = 1;
            foreach (var table in tables)
            {
                // Chuyển đổi trạng thái từ tiếng Anh sang tiếng Việt để hiển thị
                string trangThai = table.Status.ToLower() == "full" ? "Đầy" : "Trống";

                dgvBan.Rows.Add(index, table.TableName, trangThai);

                // Lưu thông tin Id trong Tag của hàng
                dgvBan.Rows[dgvBan.Rows.Count - 1].Tag = table.Id;

                index++;
            }
        }

        private void DgvBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvBan.Rows.Count)
            {
                // Lấy ID của bàn từ Tag của hàng được chọn
                int tableId = (int)dgvBan.Rows[e.RowIndex].Tag;

                // Tìm bàn có ID tương ứng trong danh sách
                _selectedTable = _tables.FirstOrDefault(t => t.Id == tableId);

                if (_selectedTable != null)
                {
                    // Hiển thị thông tin lên các control
                    txtTenBan.Text = _selectedTable.TableName;

                    // Đặt giá trị cho ComboBox trạng thái
                    cbTrangThai.SelectedIndex = _selectedTable.Status.ToLower() == "full" ? 0 : 1;
                }
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateTableData())
                return;

            try
            {
                // Tạo đối tượng bàn mới
                Table newTable = new Table
                {
                    TableName = txtTenBan.Text.Trim(),
                    // Chuyển đổi giá trị ComboBox sang giá trị phù hợp với API
                    Status = cbTrangThai.SelectedIndex == 0 ? "full" : "empty"
                };

                // Gọi API để thêm bàn mới
                Table addedTable = await _tableService.AddTableAsync(newTable);

                if (addedTable != null)
                {
                    // Tải lại danh sách bàn
                    await LoadTables();

                    // Xóa dữ liệu trên form
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn bàn chưa
            if (_selectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateTableData())
                return;

            try
            {
                // Cập nhật thông tin bàn
                int id = _selectedTable.Id;
                _selectedTable.TableName = txtTenBan.Text.Trim();
                _selectedTable.Status = cbTrangThai.SelectedIndex == 0 ? "full" : "empty";

                // Gọi API để cập nhật bàn
                bool updated = await _tableService.UpdateTableAsync(id, _selectedTable);

                if (updated)
                {
                    // Tải lại danh sách bàn
                    await LoadTables();

                    // Xóa dữ liệu trên form và reset bàn đã chọn
                    ClearForm();
                    _selectedTable = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn bàn chưa
            if (_selectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa bàn '{_selectedTable.TableName}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Gọi API để xóa bàn
                    bool deleted = await _tableService.DeleteTableAsync(_selectedTable.Id);

                    if (deleted)
                    {
                        // Tải lại danh sách bàn
                        await LoadTables();

                        // Xóa dữ liệu trên form và reset bàn đã chọn
                        ClearForm();
                        _selectedTable = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Tìm kiếm bàn
                List<Table> searchResults = await _tableService.SearchTablesAsync(searchTerm);

                // Hiển thị kết quả tìm kiếm
                DisplayTables(searchResults);

                Cursor = Cursors.Default;

                // Hiển thị thông báo nếu không có kết quả
                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private bool ValidateTableData()
        {
            // Kiểm tra tên bàn
            if (string.IsNullOrWhiteSpace(txtTenBan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBan.Focus();
                return false;
            }

            // Kiểm tra trạng thái
            if (cbTrangThai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTrangThai.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtTenBan.Text = "";
            cbTrangThai.SelectedIndex = 1; // Mặc định là "Trống"
            txtTenBan.Focus();
        }
    }
}