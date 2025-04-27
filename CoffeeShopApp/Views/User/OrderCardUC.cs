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
                TextAlign = ContentAlignment.MiddleCenter
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

            string[] icons = { "🧮", "🔔", "📝", "⋯" };
            foreach (string icon in icons)
            {
                Button btn = new Button
                {
                    Text = icon,
                    Font = new Font("Minion Pro", 11),
                    Dock = DockStyle.Fill
                };
                actionPanel.Controls.Add(btn);
            }

            this.Controls.Add(actionPanel);
            this.Controls.Add(infoPanel);
            this.Controls.Add(lblTable);

        }

        public void SetData(int tableId, int orderId, int totalGuest, decimal totalPrice)
        {
            lblTable.Text = $"Bàn A{tableId}";
            lblOrder.Text = $"Order {orderId}";
            lblGuestPrice.Text = $"👤 {totalGuest}\n{totalPrice:N0}";
        }
    }
}
