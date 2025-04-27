using CoffeeShopAPI.Models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CoffeeShopApp.ServicesUser
{
    public class OrderUIFactory
    {
        //Tạo nút danh mục
        public Button CreateCategoryButton(string name, int categoryId, Action<int> onCategoryClick)
        {
            Button btn = new Button
            {
                Text = name,
                Tag = categoryId,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Minion Pro", 16),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            btn.FlatAppearance.BorderSize = 0;

            btn.Click += (s, e) =>
            {
                onCategoryClick(categoryId);
            };

            return btn;
        }
        //Tạo panel hiển thị món ăn
        public Panel CreateMenuPanel(CoffeeShopAPI.Models.Menu menu, Action<CoffeeShopAPI.Models.Menu> onAddToCart)
        {
            Panel panel = new Panel
            {
                Width = 120,
                Height = 120,
                Margin = new Padding(0),
                BackgroundImageLayout = ImageLayout.Stretch,
                Cursor = Cursors.Hand,
                Tag = menu
            };

            // Load hình ảnh từ đường dẫn
            try
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, menu.Image);
                if (File.Exists(imagePath))
                {
                    panel.BackgroundImage = Image.FromFile(imagePath);
                }
            }
            catch { }

            // Tạo label hiển thị tên món
            Label label = new Label
            {
                Text = menu.Name,
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Minion Pro", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(180, Color.White)
            };

            panel.Controls.Add(label);

            // Sự kiện click để thêm vào giỏ
            panel.Click += (s, e) => onAddToCart(menu);
            label.Click += (s, e) => onAddToCart(menu);

            return panel;
        }
        //Tạo dòng hiển thị món trong giỏ hàng
        public TableLayoutPanel CreateCartItemRow(CoffeeShopAPI.Models.Menu menu)
        {
            TableLayoutPanel newRow = new TableLayoutPanel
            {
                ColumnCount = 5,
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
                Tag = menu.Id // lưu để kiểm tra trùng món
            };

            newRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Tên
            newRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F)); // Số lượng
            newRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F)); // Giá
            newRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F)); // Giảm
            newRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F)); // Xóa

            Label lblName = new Label
            {
                Text = menu.Name,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Minion Pro", 16)
            };

            Label lblQuantity = new Label
            {
                Text = "1",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Minion Pro", 16)
            };

            Label lblPrice = new Label
            {
                Text = menu.Price.ToString("N0") + "đ",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Minion Pro", 16)
            };

            // Giảm số lượng
            Button btnDecrease = new Button
            {
                Text = "➖",
                Dock = DockStyle.Fill,
                BackColor = Color.LightYellow,
                Font = new Font("Minion Pro", 16)
            };

            // Xóa toàn bộ dòng
            Button btnRemove = new Button
            {
                Text = "❌",
                Dock = DockStyle.Fill,
                BackColor = Color.LightCoral,
                Font = new Font("Minion Pro", 16)
            };

            newRow.Controls.Add(lblName, 0, 0);
            newRow.Controls.Add(lblQuantity, 1, 0);
            newRow.Controls.Add(lblPrice, 2, 0);
            newRow.Controls.Add(btnDecrease, 3, 0);
            newRow.Controls.Add(btnRemove, 4, 0);

            return newRow;
        }
        //Gán sự kiện cho các nút trong giỏ hàng
        public void SetCartItemEvents(TableLayoutPanel row, Action onDecrease, Action onRemove)
        {
            Button btnDecrease = row.Controls[3] as Button;
            Button btnRemove = row.Controls[4] as Button;

            if (btnDecrease != null && onDecrease != null)
                btnDecrease.Click += (s, e) => onDecrease();

            if (btnRemove != null && onRemove != null)
                btnRemove.Click += (s, e) => onRemove();
        }
    }
}