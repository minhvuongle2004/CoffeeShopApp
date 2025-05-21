using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeShopApp.Views.User
{
    public partial class OrderCardUC : UserControl
    {
        private Label lblTable;
        private Label lblOrder;
        private Label lblGuestPrice;
        private TableLayoutPanel actionPanel;

        // Sự kiện tùy chỉnh khi nhấn các nút hành động
        public event EventHandler ViewDetailsClicked;
        public event EventHandler PaymentClicked;
        public event EventHandler NotifyClicked;
        public event EventHandler EditClicked;

        public OrderCardUC()
        {
            InitializeComponent();
            BuildLayout();
        }

        private void BuildLayout()
        {
            this.Width = 200;
            this.Height = 130;
            this.BorderStyle = BorderStyle.FixedSingle;

            lblTable = new Label
            {
                Text = "Bàn A1",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Minion Pro", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(160, 180, 120)
            };

            Panel infoPanel = new Panel
            {
                Height = 40,
                Dock = DockStyle.Top
            };

            lblOrder = new Label
            {
                Text = "Order 1",
                Dock = DockStyle.Left,
                Width = 80,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Minion Pro", 9)
            };

            lblGuestPrice = new Label
            {
                Text = "👤 3\n85.000",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Minion Pro", 9)
            };

            infoPanel.Controls.Add(lblOrder);
            infoPanel.Controls.Add(lblGuestPrice);

            actionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1
            };

            for (int i = 0; i < 4; i++)
                actionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            // Tạo các nút với tooltip và sự kiện
            AddActionButton("🧮", "Thanh toán", (s, e) => PaymentClicked?.Invoke(this, EventArgs.Empty));
            AddActionButton("⮂", "Chuyển bàn", (s, e) => NotifyClicked?.Invoke(this, EventArgs.Empty));
            AddActionButton("📝", "Sửa đơn", (s, e) => EditClicked?.Invoke(this, EventArgs.Empty));
            AddActionButton("⋯", "Xem chi tiết", (s, e) => ViewDetailsClicked?.Invoke(this, EventArgs.Empty));

            this.Controls.Add(actionPanel);
            this.Controls.Add(infoPanel);
            this.Controls.Add(lblTable);
        }

        private void AddActionButton(string icon, string tooltip, EventHandler clickHandler)
        {
            Button btn = new Button
            {
                Text = icon,
                Font = new Font("Minion Pro", 11),
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand
            };

            // Thêm tooltip
            ToolTip tip = new ToolTip();
            tip.SetToolTip(btn, tooltip);

            // Thêm sự kiện click
            btn.Click += clickHandler;

            // Định dạng nút
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.WhiteSmoke;

            // Hiệu ứng hover
            btn.MouseEnter += (s, e) => btn.BackColor = Color.LightGray;
            btn.MouseLeave += (s, e) => btn.BackColor = Color.WhiteSmoke;

            actionPanel.Controls.Add(btn);
        }

        public void SetData(int tableId, int orderId, int totalGuest, decimal totalPrice)
        {
            lblTable.Text = $"Bàn A{tableId}";
            lblOrder.Text = $"Order {orderId}";
            lblGuestPrice.Text = $"👤 {totalGuest}\n{totalPrice:N0} đ";
        }
    }
}