using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopAPI.Models;
using Newtonsoft.Json;

namespace CoffeeShopApp.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://localhost:44332/";

        public UserService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/user/getAll");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<User>>(result);
                }
                return new List<User>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<User>();
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"api/user/getById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("api/user/register", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return user;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi thêm người dùng: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync($"api/user/update/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi cập nhật người dùng: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"api/user/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi xóa người dùng: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<List<User>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                List<User> allUsers = await GetAllUsersAsync();
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return allUsers;

                searchTerm = searchTerm.ToLower();
                return allUsers.Where(u =>
                    u.Id.ToString().Contains(searchTerm) ||
                    u.Fullname.ToLower().Contains(searchTerm) ||
                    u.Username.ToLower().Contains(searchTerm) ||
                    u.Role.ToLower().Contains(searchTerm) ||
                    (u.Phone != null && u.Phone.ToLower().Contains(searchTerm))
                ).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<User>();
            }
        }
    }
}