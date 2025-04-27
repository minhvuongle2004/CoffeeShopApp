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
    public class MenuService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://localhost:44332/";

        public MenuService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<CoffeeShopAPI.Models.Menu>> GetAllMenusAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/menu/getAll");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CoffeeShopAPI.Models.Menu>>(result);
                }
                return new List<CoffeeShopAPI.Models.Menu>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách menu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<CoffeeShopAPI.Models.Menu>();
            }
        }

        public async Task<CoffeeShopAPI.Models.Menu> AddMenuAsync(CoffeeShopAPI.Models.Menu menu)
        {
            try
            {
                string json = JsonConvert.SerializeObject(menu);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("api/menu/add", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Thêm món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return menu;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi thêm món: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<bool> UpdateMenuAsync(CoffeeShopAPI.Models.Menu menu)
        {
            try
            {
                string json = JsonConvert.SerializeObject(menu);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync("api/menu/update", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi cập nhật món: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"api/menu/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi khi xóa món: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<List<CoffeeShopAPI.Models.Menu>> GetMenuByCategoryAsync(int categoryId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"api/menu/byCategory/{categoryId}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CoffeeShopAPI.Models.Menu>>(result);
                }
                return new List<CoffeeShopAPI.Models.Menu>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách menu theo danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<CoffeeShopAPI.Models.Menu>();
            }
        }

        public async Task<List<CoffeeShopAPI.Models.Menu>> SearchMenusAsync(string searchTerm)
        {
            try
            {
                List<CoffeeShopAPI.Models.Menu> allMenus = await GetAllMenusAsync();
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return allMenus;

                searchTerm = searchTerm.ToLower();
                return allMenus.Where(m =>
                    m.Name.ToLower().Contains(searchTerm)
                ).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<CoffeeShopAPI.Models.Menu>();
            }
        }
    }
}