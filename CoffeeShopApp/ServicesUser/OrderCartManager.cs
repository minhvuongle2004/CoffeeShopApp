using CoffeeShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeShopApp.ServicesUser
{
    public class OrderCartManager
    {
        public decimal TotalAmount { get; private set; } = 0;
        private TableLayoutPanel cartContainer;
        private Label lblTotal;

        public OrderCartManager(TableLayoutPanel cartContainer, Label lblTotal)
        {
            this.cartContainer = cartContainer;
            this.lblTotal = lblTotal;
        }
        //Thêm món vào giỏ hàng
        public void AddToCart(CoffeeShopAPI.Models.Menu menu, OrderUIFactory uiFactory)
        {
            // Kiểm tra xem món đã có trong giỏ chưa
            foreach (Control control in cartContainer.Controls)
            {
                if (control is TableLayoutPanel row && row.Tag is int menuId && menuId == menu.Id)
                {
                    // Đã tồn tại: tăng số lượng và cập nhật giá
                    Label lblQuantity = row.Controls[1] as Label;
                    Label lblPrice = row.Controls[2] as Label;

                    int currentQuantity = int.Parse(lblQuantity.Text);
                    currentQuantity++;
                    lblQuantity.Text = currentQuantity.ToString();

                    decimal totalPrice = menu.Price * currentQuantity;
                    lblPrice.Text = totalPrice.ToString("N0") + "đ";

                    TotalAmount += menu.Price;
                    UpdateTotal();
                    return;
                }
            }

            // Nếu chưa tồn tại món → tạo UI trước (không có sự kiện)
            TableLayoutPanel newRow = uiFactory.CreateCartItemRow(menu);

            // Thêm vào container
            cartContainer.Controls.Add(newRow, 0, cartContainer.RowCount);
            cartContainer.RowCount++;

            // Sau đó mới gán sự kiện
            Label lblQuantityRef = newRow.Controls[1] as Label;
            Label lblPriceRef = newRow.Controls[2] as Label;

            // Gán sự kiện cho nút giảm và nút xóa
            Button btnDecrease = newRow.Controls[3] as Button;
            Button btnRemove = newRow.Controls[4] as Button;

            // Sự kiện nút giảm số lượng
            btnDecrease.Click += (s, e) => {
                int quantity = int.Parse(lblQuantityRef.Text);
                if (quantity > 1)
                {
                    quantity--;
                    lblQuantityRef.Text = quantity.ToString();
                    lblPriceRef.Text = (menu.Price * quantity).ToString("N0") + "đ";
                    TotalAmount -= menu.Price;
                }
                else
                {
                    cartContainer.Controls.Remove(newRow);
                    TotalAmount -= menu.Price;
                }
                UpdateTotal();
            };

            // Sự kiện nút xóa món
            btnRemove.Click += (s, e) => {
                int quantity = int.Parse(lblQuantityRef.Text);
                cartContainer.Controls.Remove(newRow);
                TotalAmount -= menu.Price * quantity;
                UpdateTotal();
            };

            // Cập nhật tổng tiền
            TotalAmount += menu.Price;
            UpdateTotal();
        }
        //Cập nhật tổng tiền
        public void UpdateTotal()
        {
            lblTotal.Text = "Tổng: " + TotalAmount.ToString("N0") + "đ";
            lblTotal.Font = new Font("Minion Pro", 16);
        }
        //Kiểm tra giỏ hàng có trống không
        public bool IsEmpty()
        {
            return cartContainer.Controls.Count == 0;
        }
        //Lấy danh sách chi tiết đơn hàng
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            List<OrderDetail> details = new List<OrderDetail>();

            foreach (Control control in cartContainer.Controls)
            {
                if (control is TableLayoutPanel row && row.Tag is int menuId)
                {
                    // Lấy thông tin từ mỗi dòng trong giỏ hàng
                    Label lblQuantity = row.Controls[1] as Label;
                    int quantity = int.Parse(lblQuantity.Text);

                    // Lấy giá từ label
                    Label lblPrice = row.Controls[2] as Label;
                    string originalPriceText = lblPrice.Text;
                    string priceText = originalPriceText.Replace("đ", "").Replace(".", "").Replace(",", "").Trim();

                    decimal subtotal;
                    if (!decimal.TryParse(priceText, out subtotal))
                    {
                        MessageBox.Show($"Không thể chuyển đổi giá '{originalPriceText}' thành số. Sau khi xử lý: '{priceText}'");
                        continue;
                    }

                    OrderDetail detail = new OrderDetail
                    {
                        OrderId = orderId,
                        MenuId = menuId,
                        Quantity = quantity,
                        Subtotal = subtotal
                    };

                    details.Add(detail);
                }
            }

            return details;
        }
        //Xóa tất cả món trong giỏ hàng
        public void Clear()
        {
            cartContainer.Controls.Clear();
            cartContainer.RowCount = 0;
            TotalAmount = 0;
            UpdateTotal();
        }
    }
}