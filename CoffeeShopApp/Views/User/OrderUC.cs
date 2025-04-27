using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopAPI.Models;

namespace CoffeeShopApp.Views.User
{
    public partial class OrderUC : UserControl
    {
        private FlowLayoutPanel flowPanel;

        public OrderUC()
        {
            InitializeComponent();
            BuildLayout();
            LoadOrdersAsync(); // gọi khi khởi tạo
        }

        private void BuildLayout()
        {
            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10)
            };

            this.Controls.Add(flowPanel);
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API
                    var response = await client.GetAsync("https://localhost:44332/api/order/getAll");
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();

                    // Deserialize
                    var orders = JsonConvert.DeserializeObject<List<Order>>(json);

                    // Tạo từng OrderCardUC
                    foreach (var order in orders)
                    {
                        var card = new OrderCardUC();
                        card.SetData(order.TableId, order.Id, order.TotalGuest, order.TotalPrice);
                        flowPanel.Controls.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải đơn hàng: " + ex.Message);
            }
        }
    }
}
