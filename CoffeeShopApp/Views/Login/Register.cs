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
    public partial class Register : Form
    {
        private HttpClient client;
        private readonly string baseUrl = "https://localhost:44332/"; // Port API

        public Register()
        {
            InitializeComponent();

            // Khởi tạo HttpClient
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Đăng ký các sự kiện
            this.Load += Register_Load;

            // Sự kiện Enter/Leave cho các TextBox
            txtFullname.Enter += TxtFullname_Enter;
            txtFullname.Leave += TxtFullname_Leave;

            txtUsername.Enter += TxtUsername_Enter;
            txtUsername.Leave += TxtUsername_Leave;

            txtPassword.Enter += TxtPassword_Enter;
            txtPassword.Leave += TxtPassword_Leave;

            txtConfirmPassword.Enter += TxtConfirmPassword_Enter;
            txtConfirmPassword.Leave += TxtConfirmPassword_Leave;

            txtPhone.Enter += TxtPhone_Enter;
            txtPhone.Leave += TxtPhone_Leave;

            // Đăng ký sự kiện Click cho Button và LinkLabel
            btnRegister.Click += BtnRegister_Click;
            lnkLogin.LinkClicked += LnkLogin_LinkClicked;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            // Chuyển đổi thành GradientPanel giống như trong Login
            Panel oldPanel = pnlRegister;
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
            this.pnlRegister = newPanel;

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
            btnRegister.BackColor = Color.FromArgb(217, 242, 208);
            txtFullname.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
        }

        private void SetPlaceholder()
        {
            // Thiết lập placeholder cho tất cả các trường
            txtFullname.Text = "FULL NAME";
            txtFullname.ForeColor = Color.Gray;

            txtUsername.Text = "USER NAME / ID";
            txtUsername.ForeColor = Color.Gray;

            txtPassword.Text = "PASSWORD";
            txtPassword.ForeColor = Color.Gray;
            txtPassword.UseSystemPasswordChar = false;

            txtConfirmPassword.Text = "CONFIRM PASSWORD";
            txtConfirmPassword.ForeColor = Color.Gray;
            txtConfirmPassword.UseSystemPasswordChar = false;

            txtPhone.Text = "PHONE";
            txtPhone.ForeColor = Color.Gray;
        }

        #region TextBox Event Handlers
        private void TxtFullname_Enter(object sender, EventArgs e)
        {
            if (txtFullname.Text == "FULL NAME")
            {
                txtFullname.Text = "";
                txtFullname.ForeColor = Color.Black;
            }
        }

        private void TxtFullname_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullname.Text))
            {
                txtFullname.Text = "FULL NAME";
                txtFullname.ForeColor = Color.Gray;
            }
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

        private void TxtConfirmPassword_Enter(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text == "CONFIRM PASSWORD")
            {
                txtConfirmPassword.Text = "";
                txtConfirmPassword.ForeColor = Color.Black;
                txtConfirmPassword.UseSystemPasswordChar = true;
            }
        }

        private void TxtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                txtConfirmPassword.Text = "CONFIRM PASSWORD";
                txtConfirmPassword.ForeColor = Color.Gray;
                txtConfirmPassword.UseSystemPasswordChar = false;
            }
        }

        private void TxtPhone_Enter(object sender, EventArgs e)
        {
            if (txtPhone.Text == "PHONE")
            {
                txtPhone.Text = "";
                txtPhone.ForeColor = Color.Black;
            }
        }

        private void TxtPhone_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                txtPhone.Text = "PHONE";
                txtPhone.ForeColor = Color.Gray;
            }
        }
        #endregion

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (txtFullname.Text == "FULL NAME" || txtUsername.Text == "USER NAME / ID" ||
                    txtPassword.Text == "PASSWORD" || txtConfirmPassword.Text == "CONFIRM PASSWORD")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra mật khẩu xác nhận
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hiển thị loading
                btnRegister.Enabled = false;
                btnRegister.Text = "ĐANG ĐĂNG KÝ...";
                Application.DoEvents();

                // Tạo đối tượng User để gửi request
                string phoneNumber = txtPhone.Text == "PHONE" ? null : txtPhone.Text;

                CoffeeShopAPI.Models.User newUser = new CoffeeShopAPI.Models.User
                {
                    Fullname = txtFullname.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Role = "staff", // Mặc định đăng ký vai trò là staff
                    Phone = phoneNumber
                };

                // Chuyển đối tượng thành JSON
                string jsonContent = JsonConvert.SerializeObject(newUser);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi request đến API
                HttpResponseMessage response = await client.PostAsync("api/user/register", content);

                // Xử lý response
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Chuyển về trang đăng nhập
                    Login loginForm = new Login();
                    loginForm.Show();
                    this.Hide();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi: {errorMessage}", "Đăng ký thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Lỗi: {response.StatusCode} - {response.ReasonPhrase}",
                        "Đăng ký thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục trạng thái nút đăng ký
                btnRegister.Enabled = true;
                btnRegister.Text = "SIGN UP";
            }
        }

        private void LnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Chuyển đến trang đăng nhập
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
    }
}