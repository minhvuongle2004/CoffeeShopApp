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
    public class ShiftService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://localhost:44332/";

        public ShiftService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Shift>> GetAllShiftsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/shift/getAll");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Shift>>(result);
                }
                return new List<Shift>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Shift>();
            }
        }

        public async Task<Shift> GetShiftByIdAsync(string id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"api/shift/getById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Shift>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<Shift> CreateShiftAsync(Shift shift)
        {
            try
            {
                string json = JsonConvert.SerializeObject(shift);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("api/shift/create", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Tạo ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return JsonConvert.DeserializeObject<Shift>(result);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi tạo ca làm việc: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<bool> UpdateShiftAsync(string id, Shift shift)
        {
            try
            {
                string json = JsonConvert.SerializeObject(shift);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync($"api/shift/update/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi cập nhật ca làm việc: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> CloseShiftAsync(string id)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsync($"api/shift/close/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đóng ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi đóng ca làm việc: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đóng ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<List<Shift>> SearchShiftsAsync(string searchTerm)
        {
            try
            {
                List<Shift> allShifts = await GetAllShiftsAsync();
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return allShifts;

                searchTerm = searchTerm.ToLower();
                return allShifts.Where(s =>
                    s.Id.ToLower().Contains(searchTerm) ||
                    s.UserId.ToString().Contains(searchTerm) ||
                    s.Username.ToLower().Contains(searchTerm) ||
                    s.Fullname.ToLower().Contains(searchTerm) ||
                    s.Status.ToLower().Contains(searchTerm) ||
                    s.Session.ToLower().Contains(searchTerm) ||
                    s.StartTime.ToString().Contains(searchTerm) ||
                    (s.EndTime.HasValue && s.EndTime.Value.ToString().Contains(searchTerm))
                ).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm ca làm việc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Shift>();
            }
        }
    }
}