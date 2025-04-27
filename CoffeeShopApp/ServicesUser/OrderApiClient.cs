using CoffeeShopAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopApp.ServicesUser
{
    public class OrderApiClient
    {
        private readonly string baseUrl;

        public OrderApiClient(string baseUrl = "https://localhost:44332/api/")
        {
            this.baseUrl = baseUrl;
        }
        //Lấy danh sách danh mục
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(baseUrl + "category/getAll");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<Category>>(jsonData);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi tải danh mục!");
                        return new List<Category>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi API category: " + ex.Message);
                return new List<Category>();
            }
        }
        //Lấy tất cả các món
        public async Task<List<CoffeeShopAPI.Models.Menu>> GetAllMenusAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(baseUrl + "menu/getAll");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<CoffeeShopAPI.Models.Menu>>(jsonData);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi tải menu!");
                        return new List<CoffeeShopAPI.Models.Menu>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi API menu: " + ex.Message);
                return new List<CoffeeShopAPI.Models.Menu>();
            }
        }
        //Lấy món theo danh mục
        public async Task<List<CoffeeShopAPI.Models.Menu>> GetMenusByCategoryAsync(int categoryId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(baseUrl + $"menu/byCategory/{categoryId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<CoffeeShopAPI.Models.Menu>>(jsonData);
                    }
                    else
                    {
                        return new List<CoffeeShopAPI.Models.Menu>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi API lọc menu: " + ex.Message);
                return new List<CoffeeShopAPI.Models.Menu>();
            }
        }
        //Tạo đơn hàng mới
        public async Task<int> CreateOrderAsync(Order order)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonOrder = JsonConvert.SerializeObject(order);
                    var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(baseUrl + "order/add", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(responseContent, out int orderId))
                        {
                            return orderId;
                        }
                        else
                        {
                            MessageBox.Show($"Không thể parse ID từ response: {responseContent}", "Lỗi");
                            return -1;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi tạo đơn hàng: {response.StatusCode} - {response.ReasonPhrase}",
                                      "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        //Thêm chi tiết đơn hàng
        public async Task<bool> AddOrderDetailAsync(OrderDetail detail)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonDetail = JsonConvert.SerializeObject(detail);
                    var content = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(baseUrl + "orderDetail/add", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"API trả về lỗi: {response.StatusCode}\nNội dung: {errorContent}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý một mục trong giỏ hàng: {ex.Message}");
                return false;
            }
        }
        //Cập nhật trạng thái bàn
        public async Task<bool> UpdateTableStatusAsync(Table table, string status)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var tableUpdate = new
                    {
                        Id = table.Id,
                        TableName = table.TableName,
                        Status = status
                    };

                    var jsonTableUpdate = JsonConvert.SerializeObject(tableUpdate);
                    var content = new StringContent(jsonTableUpdate, Encoding.UTF8, "application/json");

                    Console.WriteLine($"API URL: {baseUrl}table/update/{table.Id}");
                    Console.WriteLine($"Request Body: {jsonTableUpdate}");

                    var response = await client.PutAsync(baseUrl + $"table/update/{table.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi khi cập nhật trạng thái bàn: {response.StatusCode} - {response.ReasonPhrase}\nNội dung lỗi: {errorContent}",
                                      "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái bàn: {ex.Message}",
                              "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
    }
}