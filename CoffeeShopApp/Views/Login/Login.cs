using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CoffeeShopAPI.Models; // Sử dụng model User từ project API

namespace CoffeeShopApp.Views.Login
{
    public partial class Login : Form
    {
        private HttpClient client;
        private readonly string baseUrl = "https://localhost:44332/"; // Thay đổi port cho phù hợp

        public Login()
        {
            InitializeComponent();

            // Khởi tạo HttpClient
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Đăng ký các sự kiện
            this.Load += Login_Load;
            txtUsername.Enter += TxtUsername_Enter;
            txtUsername.Leave += TxtUsername_Leave;
            txtPassword.Enter += TxtPassword_Enter;
            txtPassword.Leave += TxtPassword_Leave;
            lnkForgot.LinkClicked += LnkForgot_LinkClicked;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Thay panel thông thường bằng GradientPanel
            Panel oldPanel = pnlLogin;
            GradientPanel newPanel = new GradientPanel();

            // Sao chép thuộc tính và controls
            newPanel.Size = oldPanel.Size;
            newPanel.Location = oldPanel.Location;
            newPanel.Name = oldPanel.Name;

            while (oldPanel.Controls.Count > 0)
            {
                Control control = oldPanel.Controls[0];
                oldPanel.Controls.Remove(control);
                newPanel.Controls.Add(control);
            }

            this.Controls.Remove(oldPanel);
            this.Controls.Add(newPanel);
            this.pnlLogin = newPanel;

            // Thiết lập placeholder
            SetPlaceholder();

            // Tải hình ảnh user icon
            try
            {
                picUser.Image = Image.FromFile("../../Resources/iconuser1.png");
                // Hoặc sử dụng resource từ project
                // picUser.Image = Properties.Resources.user_icon;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Tùy chỉnh giao diện
            btnLogin.BackColor = Color.FromArgb(217, 242, 208);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
        }

        private void SetPlaceholder()
        {
            // Thiết lập placeholder cho username và password
            txtUsername.Text = "USER NAME / ID";
            txtUsername.ForeColor = Color.Gray;

            txtPassword.Text = "PASSWORD";
            txtPassword.ForeColor = Color.Gray;
            txtPassword.UseSystemPasswordChar = false;
        }

        private void TxtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "USER NAME / ID")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void TxtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "USER NAME / ID";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "PASSWORD")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "PASSWORD";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (txtUsername.Text == "USER NAME / ID" || txtPassword.Text == "PASSWORD" ||
                    string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hiển thị loading
                btnLogin.Enabled = false;
                btnLogin.Text = "ĐANG ĐĂNG NHẬP...";
                Application.DoEvents();

                // Tạo đối tượng User để gửi request
                CoffeeShopAPI.Models.User loginUser = new CoffeeShopAPI.Models.User
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Text
                };

                // Chuyển đối tượng thành JSON
                string jsonContent = JsonConvert.SerializeObject(loginUser);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi request đến API
                HttpResponseMessage response = await client.PostAsync("api/user/login", content);

                // Xử lý response
                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu trả về
                    string resultJson = await response.Content.ReadAsStringAsync();
                    CoffeeShopAPI.Models.User loggedInUser = JsonConvert.DeserializeObject<CoffeeShopAPI.Models.User>(resultJson);

                    // Chuyển hướng dựa vào role
                    if (loggedInUser.Role.ToLower() == "admin")
                    {
                        // Chuyển đến trang Admin Dashboard
                        Views.Admin.AdminDashboard adminDashboard = new Views.Admin.AdminDashboard();
                        adminDashboard.Show();
                        this.Hide();
                    }
                    else if (loggedInUser.Role.ToLower() == "cashier" || loggedInUser.Role.ToLower() == "staff")
                    {
                        // Chuyển đến trang ChonCaForm 
                        Views.User.ChonCaForm chonCaForm = new Views.User.ChonCaForm(loggedInUser);
                        chonCaForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show($"Không thể xác định quyền truy cập cho vai trò: {loggedInUser.Role}",
                            "Lỗi quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Lỗi: {response.StatusCode} - {response.ReasonPhrase}",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục trạng thái nút đăng nhập
                btnLogin.Enabled = true;
                btnLogin.Text = "LOGIN";
            }
        }

        private void LnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Chuyển đến trang đăng ký
            Register registerForm = new Register();
            registerForm.Show();
            this.Hide(); // Ẩn form Login hiện tại
        }

        private void LnkForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Xử lý quên mật khẩu
            MessageBox.Show("Chức năng quên mật khẩu sẽ được triển khai sau!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    public class GradientPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(180, 80, 180),  // Màu tím
                Color.FromArgb(100, 130, 200), // Màu xanh
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}