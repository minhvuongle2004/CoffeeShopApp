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

        // Tạo HttpClient mới cho mỗi request để tránh lỗi null
        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(30);
            return client;
        }

        // Lấy danh sách danh mục
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                using (var client = CreateClient())
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

        // Lấy tất cả các món
        public async Task<List<CoffeeShopAPI.Models.Menu>> GetAllMenusAsync()
        {
            try
            {
                using (var client = CreateClient())
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

        // Lấy món theo danh mục
        public async Task<List<CoffeeShopAPI.Models.Menu>> GetMenusByCategoryAsync(int categoryId)
        {
            try
            {
                using (var client = CreateClient())
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

        // Tạo đơn hàng mới
        public async Task<int> CreateOrderAsync(Order order)
        {
            try
            {
                using (var client = CreateClient())
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

        // Thêm chi tiết đơn hàng
        public async Task<bool> AddOrderDetailAsync(OrderDetail detail)
        {
            try
            {
                using (var client = CreateClient())
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

        // Cập nhật trạng thái bàn
        public async Task<bool> UpdateTableStatusAsync(Table table, string status)
        {
            try
            {
                using (var client = CreateClient())
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

        // Phương thức mới: Lấy thông tin đơn hàng theo ID
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            try
            {
                using (var client = CreateClient())
                {
                    // Gọi API lấy tất cả đơn hàng
                    var response = await client.GetAsync(baseUrl + "order/getAll");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<Order>>(json);

                    // Lọc ra đơn hàng có ID tương ứng
                    return orders.Find(o => o.Id == orderId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin đơn hàng: {ex.Message}");
                return null;
            }
        }

        // Phương thức mới: Lấy chi tiết đơn hàng theo ID đơn hàng
        public async Task<List<OrderDetail>> GetOrderDetailsAsync(int orderId)
        {
            try
            {
                using (var client = CreateClient())
                {
                    // Sử dụng đúng tên route từ API controller
                    var response = await client.GetAsync(baseUrl + $"orderDetail/getByOrder/{orderId}");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<OrderDetail>>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy chi tiết đơn hàng: {ex.Message}");
                return new List<OrderDetail>();
            }
        }

        // Phương thức mới: Cập nhật đơn hàng
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                using (var client = CreateClient())
                {
                    var content = new StringContent(
                        JsonConvert.SerializeObject(order),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await client.PutAsync(baseUrl + "order/update", content);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật đơn hàng: {ex.Message}");
                return false;
            }
        }

        // Phương thức mới: Xóa tất cả chi tiết đơn hàng theo ID đơn hàng
        public async Task<bool> DeleteOrderDetailsAsync(int orderId)
        {
            try
            {
                using (var client = CreateClient())
                {
                    // Sử dụng đúng tên route từ API controller
                    var response = await client.DeleteAsync(baseUrl + $"orderDetail/deleteByOrderId/{orderId}");
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chi tiết đơn hàng: {ex.Message}");
                return false;
            }
        }

        // Phương thức mới: Lấy thông tin menu item theo ID
        public async Task<CoffeeShopAPI.Models.Menu> GetMenuByIdAsync(int menuId)
        {
            try
            {
                using (var client = CreateClient())
                {
                    // Thử gọi API endpoint getById/{id} trong MenuController nếu có
                    var response = await client.GetAsync(baseUrl + $"menu/getById/{menuId}");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<CoffeeShopAPI.Models.Menu>(json);
                }
            }
            catch (Exception ex)
            {
                // Nếu không có API endpoint, thử lấy tất cả menu và lọc theo ID
                try
                {
                    var allMenus = await GetAllMenusAsync();
                    return allMenus.Find(m => m.Id == menuId);
                }
                catch
                {
                    MessageBox.Show($"Lỗi khi lấy thông tin menu: {ex.Message}");
                    return null;
                }
            }
        }
        // Lấy tất cả đơn hàng
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                using (var client = CreateClient())
                {
                    var response = await client.GetAsync(baseUrl + "order/getAll");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<Order>>(jsonData);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi tải danh sách đơn hàng!");
                        return new List<Order>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi API khi lấy danh sách đơn hàng: " + ex.Message);
                return new List<Order>();
            }
        }
        // Phương thức xóa đơn hàng theo ID
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"{baseUrl}/order/delete/{orderId}";

                    // Gửi yêu cầu xóa đơn hàng
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    // Kiểm tra kết quả
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa đơn hàng: {ex.Message}");
                return false;
            }
        }
        // Phương thức lấy thông tin bàn theo ID
        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"{baseUrl}/table/getById/{tableId}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<Table>(jsonResponse);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy thông tin bàn: {ex.Message}");
                return null;
            }
        }

        // Phương thức để chuyển bàn
        public async Task<bool> MoveTableAsync(int orderId, int sourceTableId, int targetTableId)
        {
            try
            {
                using (var client = CreateClient())
                {
                    var request = new
                    {
                        OrderId = orderId,
                        SourceTableId = sourceTableId,
                        TargetTableId = targetTableId
                    };

                    var content = new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await client.PutAsync(baseUrl + "order/moveTable", content);

                    // Kiểm tra và log kết quả
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Move table failed: {errorMessage}");
                    }

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chuyển bàn: {ex.Message}");
                return false;
            }
        }

        // Phương thức để gộp đơn hàng
        public async Task<bool> MergeOrdersAsync(int sourceOrderId, int targetOrderId,
                                                int sourceTableId, int targetTableId)
        {
            try
            {
                using (var client = CreateClient())
                {
                    var request = new
                    {
                        SourceOrderId = sourceOrderId,
                        TargetOrderId = targetOrderId,
                        SourceTableId = sourceTableId,
                        TargetTableId = targetTableId
                    };

                    var content = new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await client.PostAsync(baseUrl + "order/mergeOrders", content);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gộp đơn hàng: {ex.Message}");
                return false;
            }
        }
        public async Task<List<Table>> GetAllTablesAsync()
        {
            try
            {
                using (var client = CreateClient())
                {
                    var response = await client.GetAsync(baseUrl + "table/getAll");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<Table>>(jsonData);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi tải danh sách bàn!");
                        return new List<Table>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi API khi lấy danh sách bàn: " + ex.Message);
                return new List<Table>();
            }
        }

        // phương thức cập nhật chi tiết đơn hàng
        public async Task<bool> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                using (var client = CreateClient())
                {
                    var content = new StringContent(
                        JsonConvert.SerializeObject(orderDetail),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await client.PutAsync(baseUrl + "orderDetail/update", content);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chi tiết đơn hàng: {ex.Message}");
                return false;
            }
        }
    }
}