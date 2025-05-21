using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CoffeeShopAPI.Models;

namespace CoffeeShopApp.Views.User
{
    public partial class ThongKeUC : UserControl
    {
        private HttpClient client;
        private readonly string baseUrl = "https://localhost:44332/";
        private CoffeeShopAPI.Models.User _currentUser;
        private CoffeeShopAPI.Models.Shift _currentShift;

        public ThongKeUC()
        {
            InitializeComponent();

            // Khởi tạo HttpClient ngay cả khi không có user
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetCurrentUser(CoffeeShopAPI.Models.User user)
        {
            _currentUser = user;
            if (this.IsHandleCreated && this.Visible)
            {
                // Nếu control đã được tạo và đang hiển thị, load dữ liệu
                LoadCurrentShiftAsync();
            }
        }

        private async void ThongKeUC_Load(object sender, EventArgs e)
        {
            // Nếu đã có current user thì load thông tin ca làm việc
            if (_currentUser != null)
            {
                await LoadCurrentShiftAsync();
            }
            else
            {
                // Hiển thị thông báo hoặc trạng thái mặc định
                DisplayDefaultState();
            }
        }

        private async Task LoadCurrentShiftAsync()
        {
            try
            {
                // Kiểm tra null trước khi sử dụng
                if (_currentUser == null)
                {
                    MessageBox.Show("Không có thông tin người dùng hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra client đã được khởi tạo chưa
                if (client == null)
                {
                    client = new HttpClient();
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }

                Console.WriteLine($"Đang tải ca làm việc cho user_id: {_currentUser.Id}");
                HttpResponseMessage response = await client.GetAsync($"api/shift/getCurrentByUserId/{_currentUser.Id}");

                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kết quả API: {resultJson}");
                    _currentShift = JsonConvert.DeserializeObject<CoffeeShopAPI.Models.Shift>(resultJson);

                    // Hiển thị thông tin ca làm việc
                    DisplayShiftInfo();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi API: {response.StatusCode} - {errorContent}");

                    // Không có ca làm việc đang mở, hiển thị thông báo
                    MessageBox.Show("Không tìm thấy ca làm việc đang mở. Vui lòng mở ca trước khi sử dụng chức năng này.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xử lý chuyển đến form chọn ca
                    // Tuy nhiên, cần cẩn thận khi gọi LoadUserControl để tránh lặp vô hạn
                    Form parentForm = this.FindForm();
                    if (parentForm != null && parentForm is EmployeeView)
                    {
                        parentForm.Hide();
                        ChonCaForm chonCaForm = new ChonCaForm(_currentUser);
                        chonCaForm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                MessageBox.Show($"Đã xảy ra lỗi khi tải thông tin ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisplayDefaultState();
            }
        }

        private void DisplayDefaultState()
        {
            // Hiển thị trạng thái mặc định khi không có thông tin ca làm việc
            maCa.Text = "N/A";
            soDuDau.Text = "0 đ";
            thoiGianBatDau.Text = "N/A";
            caLamViec.Text = "N/A";

            tongChietKhau.Text = "0 đ";
            phiDichVu.Text = "0 đ";
            VAT.Text = "0 đ";
            phieuGiamGia.Text = "0 đ";
            phiVanChuyen.Text = "0 đ";

            doanhThu.Text = "0 đ";
            soHoaDon.Text = "0";
            trungBinhHoaDon.Text = "0 đ";

            thu.Text = "0 đ";
            chi.Text = "0 đ";
            tienTrongKet.Text = "0 đ";

            // Vô hiệu hóa nút kết ca
            btn_KetCa.Enabled = false;
        }

        private void DisplayShiftInfo()
        {
            if (_currentShift != null)
            {
                // Hiển thị thông tin cơ bản
                maCa.Text = _currentShift.Id;
                soDuDau.Text = _currentShift.OpeningCash.ToString("N0") + " đ";
                thoiGianBatDau.Text = _currentShift.StartTime.ToString("dd/MM/yyyy HH:mm:ss");

                // Hiển thị ca làm việc
                string sessionName = "";
                switch (_currentShift.Session)
                {
                    case "morning":
                        sessionName = "Buổi sáng";
                        break;
                    case "afternoon":
                        sessionName = "Buổi chiều";
                        break;
                    case "evening":
                        sessionName = "Buổi tối";
                        break;
                    default:
                        sessionName = _currentShift.Session;
                        break;
                }
                caLamViec.Text = sessionName;

                // Hiển thị các thông tin khác - hiện tại đặt mặc định là 0
                tongChietKhau.Text = "0 đ";
                phiDichVu.Text = "0 đ";
                VAT.Text = "0 đ";
                phieuGiamGia.Text = "0 đ";
                phiVanChuyen.Text = "0 đ";

                // Hiển thị thông tin doanh thu và hóa đơn
                doanhThu.Text = _currentShift.TotalCash.ToString("N0") + " đ";
                soHoaDon.Text = _currentShift.TotalBill.ToString();

                // Tính trung bình hóa đơn
                double avgBill = _currentShift.TotalBill > 0 ? _currentShift.TotalCash / _currentShift.TotalBill : 0;
                trungBinhHoaDon.Text = avgBill.ToString("N0") + " đ";

                // Hiển thị thu và chi - hiện tại đặt mặc định là 0
                thu.Text = "0 đ";
                chi.Text = "0 đ";

                // Hiển thị tiền trong két - bây giờ là closed_cash hoặc opening_cash nếu closed_cash = 0
                double cashInBox = _currentShift.ClosedCash > 0 ? _currentShift.ClosedCash + _currentShift.OpeningCash : _currentShift.OpeningCash;
                tienTrongKet.Text = cashInBox.ToString("N0") + " đ";

                // Kích hoạt nút kết ca
                btn_KetCa.Enabled = true;
            }
            else
            {
                DisplayDefaultState();
            }
        }

        private async void Btn_KetCa_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentShift == null)
                {
                    MessageBox.Show("Không có ca làm việc đang mở để kết thúc.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận kết thúc ca
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn kết thúc ca làm việc hiện tại?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return;

                // Hiển thị loading
                btn_KetCa.Enabled = false;
                btn_KetCa.Text = "ĐANG XỬ LÝ...";
                Application.DoEvents();

                // Gửi request đến API để kết thúc ca
                HttpResponseMessage response = await client.PutAsync($"api/shift/close/{_currentShift.Id}", null);

                // Xử lý response
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Kết thúc ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Chuyển đến form đăng nhập
                    Form parentForm = this.FindForm();
                    if (parentForm != null)
                    {
                        parentForm.Hide();
                        Views.Login.Login loginForm = new Views.Login.Login();
                        loginForm.Show();
                    }
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi: {errorMessage}", "Kết ca thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục trạng thái nút kết ca
                btn_KetCa.Enabled = true;
                btn_KetCa.Text = "Kết ca";
            }
        }
    }
}