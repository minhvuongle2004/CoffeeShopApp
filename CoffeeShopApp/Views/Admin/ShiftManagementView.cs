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
    public partial class ShiftManagementView : Form
    {
        private ShiftService _shiftService;
        private UserService _userService;
        private List<Shift> _shifts;
        private List<CoffeeShopAPI.Models.User> _users;
        private Shift _selectedShift;

        // Dictionary để ánh xạ giữa giá trị hiển thị và giá trị lưu trữ
        private Dictionary<string, string> _sessionMapping = new Dictionary<string, string>
        {
            { "Sáng", "morning" },
            { "Chiều", "afternoon" },
            { "Tối", "evening" }
        };

        private Dictionary<string, string> _statusMapping = new Dictionary<string, string>
        {
            { "Đang mở", "open" },
            { "Đã đóng", "closed" }
        };

        public ShiftManagementView()
        {
            InitializeComponent();
            _shiftService = new ShiftService();
            _userService = new UserService();

            // Thiết lập sự kiện load form
            this.Load += ShiftManagementView_Load;

            // Thiết lập sự kiện cell click cho DataGridView
            dgvNhanVien.CellClick += DgvNhanVien_CellClick;

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

            // Khởi tạo giá trị mặc định cho txtSearch
            txtSearch.Text = "Tìm kiếm...";
            txtSearch.ForeColor = Color.Gray;

            // Thiết lập các ComboBox
            SetupComboBoxes();
        }

        private void SetupComboBoxes()
        {
            // Thiết lập ComboBox cho Session (ca làm việc)
            cb_Session.Items.Clear();
            cb_Session.Items.Add("Sáng");
            cb_Session.Items.Add("Chiều");
            cb_Session.Items.Add("Tối");

            // Thiết lập ComboBox cho Status (trạng thái)
            cb_Status.Items.Clear();
            cb_Status.Items.Add("Đang mở");
            cb_Status.Items.Add("Đã đóng");

            // Mặc định chọn giá trị đầu tiên
            if (cb_Session.Items.Count > 0)
                cb_Session.SelectedIndex = 0;

            if (cb_Status.Items.Count > 0)
                cb_Status.SelectedIndex = 0;
        }

        private async void ShiftManagementView_Load(object sender, EventArgs e)
        {
            await LoadUsers();
            await LoadShifts();
        }

        private async Task LoadUsers()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Lấy danh sách người dùng từ API
                _users = await _userService.GetAllUsersAsync();

                // Thiết lập ComboBox cho User
                cb_User.Items.Clear();
                cb_User.DisplayMember = "Text";
                cb_User.ValueMember = "Value";

                foreach (var user in _users)
                {
                    cb_User.Items.Add(new { Text = user.Fullname, Value = user.Id });
                }

                // Mặc định chọn giá trị đầu tiên nếu có
                if (cb_User.Items.Count > 0)
                    cb_User.SelectedIndex = 0;

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private async Task LoadShifts()
        {
            try
            {
                // Hiển thị thông báo đang tải
                Cursor = Cursors.WaitCursor;

                // Lấy danh sách ca làm việc từ API
                _shifts = await _shiftService.GetAllShiftsAsync();

                // Hiển thị dữ liệu lên DataGridView
                DisplayShifts(_shifts);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void DisplayShifts(List<Shift> shifts)
        {
            // Xóa dữ liệu cũ
            dgvNhanVien.Rows.Clear();

            // Thêm dữ liệu mới vào DataGridView
            int index = 1;
            foreach (var shift in shifts)
            {
                // Chuyển đổi Session và Status từ tiếng Anh sang tiếng Việt
                string session = "";
                if (shift.Session == "morning") session = "Sáng";
                else if (shift.Session == "afternoon") session = "Chiều";
                else if (shift.Session == "evening") session = "Tối";

                string status = shift.Status == "open" ? "Đang mở" : "Đã đóng";

                // Format thời gian
                string startTime = shift.StartTime.ToString("dd/MM/yyyy HH:mm:ss");
                string endTime = shift.EndTime.HasValue ? shift.EndTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";

                dgvNhanVien.Rows.Add(
                    index,
                    shift.Fullname,  // Hiển thị tên người dùng thay vì UserId
                    session,
                    startTime,
                    endTime,
                    shift.OpeningCash.ToString("N0") + " VNĐ",
                    shift.ClosedCash.ToString("N0") + " VNĐ",
                    shift.TotalCash.ToString("N0") + " VNĐ",
                    shift.TotalBill,
                    status
                );

                // Lưu thông tin Id trong Tag của hàng
                dgvNhanVien.Rows[dgvNhanVien.Rows.Count - 1].Tag = shift.Id;

                index++;
            }
        }

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhanVien.Rows.Count)
            {
                // Lấy ID của ca làm việc từ Tag của hàng được chọn
                string shiftId = dgvNhanVien.Rows[e.RowIndex].Tag.ToString();

                // Tìm ca làm việc có ID tương ứng trong danh sách
                _selectedShift = _shifts.FirstOrDefault(s => s.Id == shiftId);

                if (_selectedShift != null)
                {
                    // Hiển thị thông tin lên các control
                    // Chọn người dùng trong ComboBox
                    for (int i = 0; i < cb_User.Items.Count; i++)
                    {
                        dynamic item = cb_User.Items[i];
                        if (item.Value == _selectedShift.UserId)
                        {
                            cb_User.SelectedIndex = i;
                            break;
                        }
                    }

                    // Chọn Session (ca làm việc)
                    if (_selectedShift.Session == "morning") cb_Session.SelectedIndex = 0;
                    else if (_selectedShift.Session == "afternoon") cb_Session.SelectedIndex = 1;
                    else if (_selectedShift.Session == "evening") cb_Session.SelectedIndex = 2;

                    // Thiết lập DateTimePicker cho thời gian bắt đầu và kết thúc
                    dt_Start.Value = _selectedShift.StartTime;
                    if (_selectedShift.EndTime.HasValue)
                        dt_End.Value = _selectedShift.EndTime.Value;
                    else
                        dt_End.Value = DateTime.Now;

                    // Thiết lập các TextBox
                    tb_OpeningCash.Text = _selectedShift.OpeningCash.ToString();
                    ttb_ClosedCash.Text = _selectedShift.ClosedCash.ToString();
                    tb_TotalCash.Text = _selectedShift.TotalCash.ToString();
                    tb_TotalBill.Text = _selectedShift.TotalBill.ToString();

                    // Thiết lập ComboBox Status
                    cb_Status.SelectedIndex = _selectedShift.Status == "open" ? 0 : 1;
                }
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateShiftData())
                return;

            try
            {
                // Lấy Id của người dùng đã chọn
                dynamic selectedUser = cb_User.SelectedItem;
                int userId = selectedUser.Value;

                // Kiểm tra xem đã có ca làm việc nào đang mở chưa
                bool hasOpenShift = _shifts.Any(s => s.Status == "open");
                if (hasOpenShift)
                {
                    MessageBox.Show("Đã có ca làm việc đang mở. Vui lòng đóng ca làm việc đó trước khi tạo ca mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy giá trị Session
                string session = GetSessionValue(cb_Session.SelectedItem.ToString());

                // Tạo đối tượng ca làm việc mới
                Shift newShift = new Shift
                {
                    UserId = userId,
                    StartTime = dt_Start.Value,
                    OpeningCash = double.Parse(tb_OpeningCash.Text),
                    TotalCash = 0,
                    TotalBill = 0,
                    Status = "open",
                    Session = session
                };

                // Gọi API để tạo ca làm việc mới
                Shift addedShift = await _shiftService.CreateShiftAsync(newShift);

                if (addedShift != null)
                {
                    // Tải lại danh sách ca làm việc
                    await LoadShifts();

                    // Xóa dữ liệu trên form
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn ca làm việc chưa
            if (_selectedShift == null)
            {
                MessageBox.Show("Vui lòng chọn ca làm việc cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và xác thực dữ liệu đầu vào
            if (!ValidateShiftData())
                return;

            try
            {
                // Lấy Id của người dùng đã chọn
                dynamic selectedUser = cb_User.SelectedItem;
                int userId = selectedUser.Value;

                // Kiểm tra nếu đang thay đổi trạng thái từ đóng sang mở
                string newStatus = GetStatusValue(cb_Status.SelectedItem.ToString());
                if (_selectedShift.Status == "closed" && newStatus == "open")
                {
                    // Kiểm tra xem đã có ca làm việc nào đang mở chưa
                    bool hasOpenShift = _shifts.Any(s => s.Status == "open" && s.Id != _selectedShift.Id);
                    if (hasOpenShift)
                    {
                        MessageBox.Show("Đã có ca làm việc đang mở. Chỉ được phép có một ca làm việc mở tại một thời điểm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Lấy giá trị Session
                string session = GetSessionValue(cb_Session.SelectedItem.ToString());

                // Cập nhật thông tin ca làm việc
                string id = _selectedShift.Id;
                _selectedShift.UserId = userId;
                _selectedShift.StartTime = dt_Start.Value;
                _selectedShift.EndTime = _selectedShift.Status == "closed" ? _selectedShift.EndTime : null;
                _selectedShift.OpeningCash = double.Parse(tb_OpeningCash.Text);
                _selectedShift.ClosedCash = double.Parse(ttb_ClosedCash.Text);
                _selectedShift.TotalCash = double.Parse(tb_TotalCash.Text);
                _selectedShift.TotalBill = int.Parse(tb_TotalBill.Text);
                _selectedShift.Status = newStatus;
                _selectedShift.Session = session;

                // Gọi API để cập nhật ca làm việc
                bool updated = await _shiftService.UpdateShiftAsync(id, _selectedShift);

                if (updated)
                {
                    // Nếu đang thay đổi trạng thái từ mở sang đóng, gọi API đóng ca
                    if (_selectedShift.Status == "open" && newStatus == "closed")
                    {
                        await _shiftService.CloseShiftAsync(id);
                    }

                    // Tải lại danh sách ca làm việc
                    await LoadShifts();

                    // Xóa dữ liệu trên form và reset ca làm việc đã chọn
                    ClearForm();
                    _selectedShift = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            // Không cho phép xóa ca làm việc trong ví dụ này vì có thể ảnh hưởng đến dữ liệu hóa đơn
            MessageBox.Show("Không thể xóa ca làm việc để đảm bảo tính toàn vẹn dữ liệu. Nếu cần, hãy đóng ca làm việc thay vì xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;
            if (searchTerm == "Tìm kiếm...")
                searchTerm = "";

            try
            {
                Cursor = Cursors.WaitCursor;

                // Tìm kiếm ca làm việc
                List<Shift> searchResults = await _shiftService.SearchShiftsAsync(searchTerm);

                // Hiển thị kết quả tìm kiếm
                DisplayShifts(searchResults);

                Cursor = Cursors.Default;

                // Hiển thị thông báo nếu không có kết quả
                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private bool ValidateShiftData()
        {
            // Kiểm tra người dùng
            if (cb_User.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_User.Focus();
                return false;
            }

            // Kiểm tra ca làm việc
            if (cb_Session.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn ca làm việc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_Session.Focus();
                return false;
            }

            // Kiểm tra tiền đầu ca
            if (string.IsNullOrWhiteSpace(tb_OpeningCash.Text) || !double.TryParse(tb_OpeningCash.Text, out _))
            {
                MessageBox.Show("Vui lòng nhập số tiền đầu ca hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_OpeningCash.Focus();
                return false;
            }

            // Kiểm tra tổng tiền mặt (chỉ khi đang chỉnh sửa)
            if (_selectedShift != null && (string.IsNullOrWhiteSpace(ttb_ClosedCash.Text) || !double.TryParse(ttb_ClosedCash.Text, out _)))
            {
                MessageBox.Show("Vui lòng nhập tổng tiền mặt hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ttb_ClosedCash.Focus();
                return false;
            }

            // Kiểm tra doanh thu (chỉ khi đang chỉnh sửa)
            if (_selectedShift != null && (string.IsNullOrWhiteSpace(tb_TotalCash.Text) || !double.TryParse(tb_TotalCash.Text, out _)))
            {
                MessageBox.Show("Vui lòng nhập doanh thu hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_TotalCash.Focus();
                return false;
            }

            // Kiểm tra số hóa đơn (chỉ khi đang chỉnh sửa)
            if (_selectedShift != null && (string.IsNullOrWhiteSpace(tb_TotalBill.Text) || !int.TryParse(tb_TotalBill.Text, out _)))
            {
                MessageBox.Show("Vui lòng nhập số hóa đơn hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_TotalBill.Focus();
                return false;
            }

            // Kiểm tra trạng thái
            if (cb_Status.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_Status.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            // Đặt lại giá trị mặc định cho các control
            if (cb_User.Items.Count > 0)
                cb_User.SelectedIndex = 0;

            if (cb_Session.Items.Count > 0)
                cb_Session.SelectedIndex = 0;

            if (cb_Status.Items.Count > 0)
                cb_Status.SelectedIndex = 0;

            dt_Start.Value = DateTime.Now;
            dt_End.Value = DateTime.Now;

            tb_OpeningCash.Text = "0";
            ttb_ClosedCash.Text = "0";
            tb_TotalCash.Text = "0";
            tb_TotalBill.Text = "0";

            // Reset selected shift
            _selectedShift = null;
        }

        // Hàm chuyển đổi từ giá trị hiển thị sang giá trị lưu trữ
        private string GetSessionValue(string displayValue)
        {
            if (_sessionMapping.ContainsKey(displayValue))
                return _sessionMapping[displayValue];
            return "morning"; // Giá trị mặc định
        }

        private string GetStatusValue(string displayValue)
        {
            if (_statusMapping.ContainsKey(displayValue))
                return _statusMapping[displayValue];
            return "open"; // Giá trị mặc định
        }

        // Hàm chuyển đổi từ giá trị lưu trữ sang giá trị hiển thị
        private string GetSessionDisplay(string value)
        {
            foreach (var pair in _sessionMapping)
            {
                if (pair.Value == value)
                    return pair.Key;
            }
            return "Sáng"; // Giá trị mặc định
        }

        private string GetStatusDisplay(string value)
        {
            foreach (var pair in _statusMapping)
            {
                if (pair.Value == value)
                    return pair.Key;
            }
            return "Đang mở"; // Giá trị mặc định
        }
    }
}