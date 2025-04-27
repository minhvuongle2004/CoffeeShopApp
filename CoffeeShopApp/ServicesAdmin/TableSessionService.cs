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
    public class SessionService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://localhost:44332/";

        public SessionService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<TableSession>> GetAllSessionsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/tablesession/getAll");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<TableSession>>(result);
                }
                return new List<TableSession>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<TableSession>();
            }
        }

        public async Task<TableSession> GetSessionByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"api/tablesession/getById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TableSession>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<bool> AddSessionAsync(TableSession session)
        {
            try
            {
                string json = JsonConvert.SerializeObject(session);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("api/tablesession/add", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm phiên bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi thêm phiên bàn: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> UpdateSessionAsync(int id, TableSession session)
        {
            try
            {
                string json = JsonConvert.SerializeObject(session);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync($"api/tablesession/update/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật phiên bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi cập nhật phiên bàn: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"api/tablesession/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa phiên bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi xóa phiên bàn: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Phương thức lấy danh sách bàn
        public async Task<List<Table>> GetAllTablesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/table/getAll");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Table>>(result);
                }
                return new List<Table>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Table>();
            }
        }

        // Phương thức lấy danh sách nhân viên
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
                MessageBox.Show($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<User>();
            }
        }

        // Phương thức tìm kiếm phiên bàn theo tên bàn hoặc tên nhân viên
        public async Task<List<SessionViewModel>> SearchSessionsAsync(List<SessionViewModel> sessions, string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return sessions;

                searchTerm = searchTerm.ToLower();
                return sessions.Where(s =>
                    s.TableName.ToLower().Contains(searchTerm) ||
                    s.UserFullname.ToLower().Contains(searchTerm)
                ).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm phiên bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<SessionViewModel>();
            }
        }
    }

    // View model để hiển thị trên DataGridView kết hợp thông tin từ nhiều bảng
    public class SessionViewModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public string DisplayStatus
        {
            get
            {
                return Status == "active" ? "Đang hoạt động" : "Hoàn thành";
            }
        }
    }
}