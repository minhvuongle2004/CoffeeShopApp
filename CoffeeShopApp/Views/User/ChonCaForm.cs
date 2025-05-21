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
    public partial class ChonCaForm : Form
    {
        private HttpClient client;
        private readonly string baseUrl = "https://localhost:44332/"; // Thay đổi port cho phù hợp
        private CoffeeShopAPI.Models.User _currentUser;

        public ChonCaForm()
        {
            InitializeComponent();
        }

        public ChonCaForm(CoffeeShopAPI.Models.User user)
        {
            InitializeComponent();
            _currentUser = user;

            // Khởi tạo HttpClient
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Load danh sách ca làm việc
            LoadCaLamViec();

            // Đăng ký các sự kiện
            this.Load += ChonCaForm_Load;
            btn_MoCa.Click += Btn_MoCa_Click;

            // Đăng ký sự kiện cho các nút số
            btn_0.Click += Btn_Number_Click;
            btn_1.Click += Btn_Number_Click;
            btn_2.Click += Btn_Number_Click;
            btn_3.Click += Btn_Number_Click;
            btn_4.Click += Btn_Number_Click;
            btn_5.Click += Btn_Number_Click;
            btn_6.Click += Btn_Number_Click;
            btn_7.Click += Btn_Number_Click;
            btn_8.Click += Btn_Number_Click;
            btn_9.Click += Btn_Number_Click;
            btn_000.Click += Btn_Number_Click;
            btn_Delete.Click += Btn_Delete_Click;
        }

        private void ChonCaForm_Load(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã có ca làm việc đang mở chưa
            CheckExistingShift();
        }

        private async void CheckExistingShift()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"api/shift/getCurrentByUserId/{_currentUser.Id}");

                if (response.IsSuccessStatusCode)
                {
                    // Đã có ca làm việc đang mở, chuyển đến EmployeeView
                    string resultJson = await response.Content.ReadAsStringAsync();
                    CoffeeShopAPI.Models.Shift currentShift = JsonConvert.DeserializeObject<CoffeeShopAPI.Models.Shift>(resultJson);

                    MessageBox.Show("Bạn đã có ca làm việc đang mở. Hệ thống sẽ chuyển đến màn hình chính.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    EmployeeView employeeView = new EmployeeView(_currentUser);
                    employeeView.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi - có thể chưa có ca làm việc
                Console.WriteLine($"Lỗi kiểm tra ca làm việc: {ex.Message}");
            }
        }

        private void LoadCaLamViec()
        {
            // Thêm các ca làm việc vào combobox
            cb_ChonCa.Items.Clear();
            cb_ChonCa.Items.Add("Buổi sáng (morning)");
            cb_ChonCa.Items.Add("Buổi chiều (afternoon)");
            cb_ChonCa.Items.Add("Buổi tối (evening)");

            // Chọn ca làm việc mặc định
            cb_ChonCa.SelectedIndex = 0;
        }

        private void Btn_Number_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;

            // Thêm số vào textbox
            if (string.IsNullOrEmpty(tb_TienQuyDauCa.Text))
            {
                tb_TienQuyDauCa.Text = buttonText;
            }
            else
            {
                tb_TienQuyDauCa.Text += buttonText;
            }
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            // Xóa ký tự cuối cùng
            if (!string.IsNullOrEmpty(tb_TienQuyDauCa.Text))
            {
                tb_TienQuyDauCa.Text = tb_TienQuyDauCa.Text.Substring(0, tb_TienQuyDauCa.Text.Length - 1);
            }
        }

        private async void Btn_MoCa_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (cb_ChonCa.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ca làm việc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(tb_TienQuyDauCa.Text))
                {
                    MessageBox.Show("Vui lòng nhập tiền quỹ đầu ca!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin ca làm việc
                string sessionValue = "";
                switch (cb_ChonCa.SelectedIndex)
                {
                    case 0:
                        sessionValue = "morning";
                        break;
                    case 1:
                        sessionValue = "afternoon";
                        break;
                    case 2:
                        sessionValue = "evening";
                        break;
                }

                double openingCash;
                if (!double.TryParse(tb_TienQuyDauCa.Text, out openingCash))
                {
                    MessageBox.Show("Tiền quỹ đầu ca không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hiển thị loading
                btn_MoCa.Enabled = false;
                btn_MoCa.Text = "ĐANG XỬ LÝ...";
                Application.DoEvents();

                // Tạo đối tượng Shift để gửi request
                var newShift = new
                {
                    UserId = _currentUser.Id,
                    OpeningCash = openingCash,
                    Session = sessionValue
                };

                // Hiển thị thông tin gửi đi để debug
                Console.WriteLine($"Gửi request mở ca: UserId={newShift.UserId}, OpeningCash={newShift.OpeningCash}, Session={newShift.Session}");

                // Chuyển đối tượng thành JSON
                string jsonContent = JsonConvert.SerializeObject(newShift);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi request đến API
                HttpResponseMessage response = await client.PostAsync("api/shift/create", content);

                // Hiển thị thông tin response để debug
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status: {response.StatusCode}, Content: {responseContent}");

                // Xử lý response
                if (response.IsSuccessStatusCode)
                {                
                    // Tạo đối tượng Shift từ response nếu có
                    CoffeeShopAPI.Models.Shift createdShift = null;
                    try
                    {
                        createdShift = JsonConvert.DeserializeObject<CoffeeShopAPI.Models.Shift>(responseContent);
                    }
                    catch
                    {
                        // Bỏ qua lỗi deserialization
                    }

                    // Chuyển đến EmployeeView
                    EmployeeView employeeView = new EmployeeView(_currentUser);
                    employeeView.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show($"Lỗi: {responseContent}", "Mở ca thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục trạng thái nút mở ca
                btn_MoCa.Enabled = true;
                btn_MoCa.Text = "Mở ca";
            }
        }
    }
}