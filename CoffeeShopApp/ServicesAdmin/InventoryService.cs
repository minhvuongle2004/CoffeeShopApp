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
    public class InventoryService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://localhost:44332/";

        public InventoryService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/inventory/getAll");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Inventory>>(result);
                }
                return new List<Inventory>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Inventory>();
            }
        }

        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            try
            {
                string json = JsonConvert.SerializeObject(inventory);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("api/inventory/add", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Thêm nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return inventory;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi thêm nguyên liệu: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<bool> UpdateInventoryAsync(Inventory inventory)
        {
            try
            {
                string json = JsonConvert.SerializeObject(inventory);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync("api/inventory/update", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi cập nhật nguyên liệu: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> DeleteInventoryAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"api/inventory/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi xóa nguyên liệu: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<List<Inventory>> SearchInventoryAsync(string searchTerm)
        {
            try
            {
                List<Inventory> allInventory = await GetAllInventoryAsync();
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return allInventory;

                searchTerm = searchTerm.ToLower();
                return allInventory.Where(i =>
                    i.Id.ToString().Contains(searchTerm) ||
                    i.Name.ToLower().Contains(searchTerm) ||
                    i.Unit.ToLower().Contains(searchTerm)
                ).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Inventory>();
            }
        }
    }
}