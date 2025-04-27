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
    public partial class SessionManagementView : Form
    {
        private SessionService _sessionService;
        private List<TableSession> _sessions;
        private List<Table> _tables;
        private List<CoffeeShopAPI.Models.User> _users;
        private TableSession _selectedSession;
        private List<SessionViewModel> _sessionViewModels;

        public SessionManagementView()
        {
            InitializeComponent();
            _sessionService = new SessionService();

            // Thiết lập sự kiện load form
            this.Load += SessionManagementView_Load;

            // Thiết lập sự kiện cell click cho DataGridView
            dgvPhienBan.CellClick += DgvPhienBan_CellClick;

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

            // Thiết lập sự kiện cho ComboBox trạng thái
            cbTrangThai.SelectedIndexChanged += CbTrangThai_SelectedIndexChanged;
        }

        private void CbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra nếu trạng thái là "Hoàn thành" thì cho phép chọn ngày kết thúc
            if (cbTrangThai.SelectedItem != null && cbTrangThai.SelectedItem.ToString() == "Hoàn thành")
            {
                dtEnd.Enabled = true;
                if (dtEnd.Value < dtStart.Value)
                {
                    dtEnd.Value = DateTime.Now;
                }
            }
            else
            {
                dtEnd.Enabled = false;
                dtEnd.Value = DateTime.Now;
            }
        }

        private async void SessionManagementView_Load(object sender, EventArgs e)
        {
            // Thiết lập ban đầu cho điều khiển ngày giờ
            dtStart.Format = DateTimePickerFormat.Custom;
            dtStart.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            dtEnd.Format = DateTimePickerFormat.Custom;
            dtEnd.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            dtStart.Value = DateTime.Now;
            dtEnd.Value = DateTime.Now;
            dtEnd.Enabled = false;

            // Thiết lập ComboBox trạng thái
            cbTrangThai.Items.Clear();
            cbTrangThai.Items.Add("Đang hoạt động");
            cbTrangThai.Items.Add("Hoàn thành");
            cbTrangThai.SelectedIndex = 0;

            // Tải dữ liệu
            await LoadSessionData();
        }

        private async Task LoadSessionData()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Tải danh sách bàn
                _tables = await _sessionService.GetAllTablesAsync();
                LoadTableComboBox(_tables);

                // Tải danh sách nhân viên
                _users = await _sessionService.GetAllUsersAsync();
                LoadUserComboBox(_users);

                // Tải danh sách phiên bàn
                _sessions = await _sessionService.GetAllSessionsAsync();

                // Tạo danh sách view model cho hiển thị
                _sessionViewModels = CreateSessionViewModels(_sessions, _tables, _users);

                // Hiển thị dữ liệu lên DataGridView
                DisplaySessions(_sessionViewModels);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void LoadTableComboBox(List<Table> tables)
        {
            cbTenBan.DataSource = null;
            cbTenBan.DisplayMember = "TableName";
            cbTenBan.ValueMember = "Id";
            cbTenBan.DataSource = tables;
        }

        private void LoadUserComboBox(List<CoffeeShopAPI.Models.User> users)
        {
            cbNhanVien.DataSource = null;
            cbNhanVien.DisplayMember = "Fullname";
            cbNhanVien.ValueMember = "Id";
            cbNhanVien.DataSource = users;
        }

        private List<SessionViewModel> CreateSessionViewModels(List<TableSession> sessions, List<Table> tables, List<CoffeeShopAPI.Models.User> users)
        {
            var viewModels = new List<SessionViewModel>();

            foreach (var session in sessions)
            {
                var table = tables.FirstOrDefault(t => t.Id == session.TableId);
                var user = users.FirstOrDefault(u => u.Id == session.UserId);

                viewModels.Add(new SessionViewModel
                {
                    Id = session.Id,
                    TableId = session.TableId,
                    TableName = table?.TableName ?? "Không xác định",
                    UserId = session.UserId,
                    UserFullname = user?.Fullname ?? "Không xác định",
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    Status = session.Status
                });
            }

            return viewModels;
        }

        private void DisplaySessions(List<SessionViewModel> sessions)
        {
            // Xóa các binding cũ
            dgvPhienBan.DataSource = null;

            // Đặt binding mới
            dgvPhienBan.DataSource = sessions;

            // Thiết lập tên hiển thị cho các cột
            if (dgvPhienBan.Columns.Contains("Id"))
                dgvPhienBan.Columns["Id"].HeaderText = "Mã phiên";

            if (dgvPhienBan.Columns.Contains("TableName"))
                dgvPhienBan.Columns["TableName"].HeaderText = "Tên bàn";

            if (dgvPhienBan.Columns.Contains("UserFullname"))
                dgvPhienBan.Columns["UserFullname"].HeaderText = "Nhân viên";

            if (dgvPhienBan.Columns.Contains("StartTime"))
                dgvPhienBan.Columns["StartTime"].HeaderText = "Thời gian bắt đầu";

            if (dgvPhienBan.Columns.Contains("EndTime"))
                dgvPhienBan.Columns["EndTime"].HeaderText = "Thời gian kết thúc";

            if (dgvPhienBan.Columns.Contains("DisplayStatus"))
                dgvPhienBan.Columns["DisplayStatus"].HeaderText = "Trạng thái";

            // Ẩn các cột không cần hiển thị
            if (dgvPhienBan.Columns.Contains("TableId"))
                dgvPhienBan.Columns["TableId"].Visible = false;

            if (dgvPhienBan.Columns.Contains("UserId"))
                dgvPhienBan.Columns["UserId"].Visible = false;

            if (dgvPhienBan.Columns.Contains("Status"))
                dgvPhienBan.Columns["Status"].Visible = false;
        }

        private void DgvPhienBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvPhienBan.Rows.Count)
            {
                // Lấy thông tin phiên bàn được chọn từ view model
                var sessionViewModel = dgvPhienBan.Rows[e.RowIndex].DataBoundItem as SessionViewModel;
                if (sessionViewModel != null)
                {
                    // Tìm phiên bàn gốc từ danh sách
                    _selectedSession = _sessions.FirstOrDefault(s => s.Id == sessionViewModel.Id);
                    if (_selectedSession != null)
                    {
                        // Hiển thị thông tin lên form
                        cbTenBan.SelectedValue = _selectedSession.TableId;
                        cbNhanVien.SelectedValue = _selectedSession.UserId;
                        dtStart.Value = _selectedSession.StartTime;

                        if (_selectedSession.EndTime.HasValue)
                        {
                            dtEnd.Value = _selectedSession.EndTime.Value;
                        }
                        else
                        {
                            dtEnd.Value = DateTime.Now;
                        }

                        if (_selectedSession.Status == "active")
                        {
                            cbTrangThai.SelectedIndex = 0; // "Đang hoạt động"
                            dtEnd.Enabled = false;
                        }
                        else
                        {
                            cbTrangThai.SelectedIndex = 1; // "Hoàn thành"
                            dtEnd.Enabled = true;
                        }
                    }
                }
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateSessionData())
                return;

            try
            {
                // Tạo đối tượng phiên bàn mới
                TableSession newSession = new TableSession
                {
                    TableId = (int)cbTenBan.SelectedValue,
                    UserId = (int)cbNhanVien.SelectedValue,
                    StartTime = dtStart.Value,
                    Status = cbTrangThai.SelectedIndex == 0 ? "active" : "completed"
                };

                // Nếu trạng thái là hoàn thành, thêm thời gian kết thúc
                if (cbTrangThai.SelectedIndex == 1)
                {
                    newSession.EndTime = dtEnd.Value;
                }

                // Gọi API để thêm phiên bàn mới
                bool added = await _sessionService.AddSessionAsync(newSession);

                if (added)
                {
                    // Tải lại danh sách phiên bàn
                    await LoadSessionData();

                    // Xóa dữ liệu trên form
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn phiên bàn chưa
            if (_selectedSession == null)
            {
                MessageBox.Show("Vui lòng chọn phiên bàn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateSessionData())
                return;

            try
            {
                // Cập nhật thông tin phiên bàn
                _selectedSession.TableId = (int)cbTenBan.SelectedValue;
                _selectedSession.UserId = (int)cbNhanVien.SelectedValue;
                _selectedSession.StartTime = dtStart.Value;
                _selectedSession.Status = cbTrangThai.SelectedIndex == 0 ? "active" : "completed";

                // Nếu trạng thái là hoàn thành, cập nhật thời gian kết thúc
                if (cbTrangThai.SelectedIndex == 1)
                {
                    _selectedSession.EndTime = dtEnd.Value;
                }
                else
                {
                    _selectedSession.EndTime = null;
                }

                // Gọi API để cập nhật phiên bàn
                bool updated = await _sessionService.UpdateSessionAsync(_selectedSession.Id, _selectedSession);

                if (updated)
                {
                    // Tải lại danh sách phiên bàn
                    await LoadSessionData();

                    // Xóa dữ liệu trên form và reset phiên bàn đã chọn
                    ClearForm();
                    _selectedSession = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn phiên bàn chưa
            if (_selectedSession == null)
            {
                MessageBox.Show("Vui lòng chọn phiên bàn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            var sessionViewModel = _sessionViewModels.FirstOrDefault(s => s.Id == _selectedSession.Id);
            string tableName = sessionViewModel != null ? sessionViewModel.TableName : "không xác định";

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phiên bàn '{tableName}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Gọi API để xóa phiên bàn
                    bool deleted = await _sessionService.DeleteSessionAsync(_selectedSession.Id);

                    if (deleted)
                    {
                        // Tải lại danh sách phiên bàn
                        await LoadSessionData();

                        // Xóa dữ liệu trên form và reset phiên bàn đã chọn
                        ClearForm();
                        _selectedSession = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Tìm kiếm phiên bàn
                var searchResults = await _sessionService.SearchSessionsAsync(_sessionViewModels, searchTerm);

                // Hiển thị kết quả tìm kiếm
                DisplaySessions(searchResults);

                Cursor = Cursors.Default;

                // Hiển thị thông báo nếu không có kết quả
                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void dgvPhienBan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đã xử lý ở sự kiện DgvPhienBan_CellClick
        }

        private bool ValidateSessionData()
        {
            // Kiểm tra bàn
            if (cbTenBan.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTenBan.Focus();
                return false;
            }

            // Kiểm tra nhân viên
            if (cbNhanVien.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbNhanVien.Focus();
                return false;
            }

            // Kiểm tra thời gian bắt đầu
            if (dtStart.Value > DateTime.Now)
            {
                MessageBox.Show("Thời gian bắt đầu không được lớn hơn thời gian hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtStart.Focus();
                return false;
            }

            // Kiểm tra thời gian kết thúc nếu trạng thái là hoàn thành
            if (cbTrangThai.SelectedIndex == 1)
            {
                if (dtEnd.Value <= dtStart.Value)
                {
                    MessageBox.Show("Thời gian kết thúc phải lớn hơn thời gian bắt đầu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtEnd.Focus();
                    return false;
                }
            }

            return true;
        }

        private void ClearForm()
        {
            if (cbTenBan.Items.Count > 0)
                cbTenBan.SelectedIndex = 0;

            if (cbNhanVien.Items.Count > 0)
                cbNhanVien.SelectedIndex = 0;

            dtStart.Value = DateTime.Now;
            dtEnd.Value = DateTime.Now;
            cbTrangThai.SelectedIndex = 0;
            dtEnd.Enabled = false;
        }
    }
}