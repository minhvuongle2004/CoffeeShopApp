// SoDoUC.cs - UserControl hiển thị sơ đồ bàn chỉ phân biệt bàn có khách hay không, icon xám khi có khách (Loại bỏ border, chỉ icon và tên bàn dưới icon)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using CoffeeShopAPI.Models;

namespace CoffeeShopApp.Views.User
{
    public partial class SoDoUC : UserControl
    {
        private FlowLayoutPanel flowTables;
        private TextBox txtSearch;
        private Button btnSearch;

        private List<Table> allTables = new List<Table>();
        private EmployeeView parent;

        public SoDoUC(EmployeeView parent)
        {
            InitializeComponent();
            this.parent = parent;
            SetupLayout();
            _ = LoadTablesFromApi();
        }

        private void SetupLayout()
        {
            this.Dock = DockStyle.Fill;

            txtSearch = new TextBox();
            txtSearch.Text = "Nhập tên bàn...";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Width = 200;
            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "Nhập tên bàn...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };
            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Nhập tên bàn...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            btnSearch = new Button();
            btnSearch.Text = "Tìm kiếm";
            btnSearch.Click += BtnSearch_Click;

            FlowLayoutPanel topPanel = new FlowLayoutPanel();
            topPanel.Height = 50;
            topPanel.Dock = DockStyle.Top;
            topPanel.Controls.Add(txtSearch);
            topPanel.Controls.Add(btnSearch);

            flowTables = new FlowLayoutPanel();
            flowTables.Dock = DockStyle.Fill;
            flowTables.AutoScroll = true;
            flowTables.Padding = new Padding(10);

            this.Controls.Add(flowTables);
            this.Controls.Add(topPanel);
        }

        private async Task LoadTablesFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:44332/api/table/getAll";
                    var response = await client.GetStringAsync(apiUrl);
                    allTables = JsonConvert.DeserializeObject<List<Table>>(response);
                    RenderTables(allTables);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bàn: " + ex.Message);
            }
        }

        private void RenderTables(List<Table> tables)
        {
            flowTables.Controls.Clear();
            foreach (var table in tables)
            {
                // Tạo Panel chứa icon và label, loại bỏ border
                Panel tablePanel = new Panel();
                tablePanel.Width = 120;
                tablePanel.Height = 150;
                tablePanel.BackColor = Color.Transparent;
                tablePanel.Margin = new Padding(10);

                // Xử lý icon bàn
                Image original = Properties.Resources.table_icon;
                Bitmap resized = new Bitmap(original, new Size(90, 90)); // Tăng 1.5 lần so với 60x60

                if (table.Status == "full")
                {
                    // Chuyển icon sang xám khi có khách
                    for (int y = 0; y < resized.Height; y++)
                    {
                        for (int x = 0; x < resized.Width; x++)
                        {
                            Color oc = resized.GetPixel(x, y);
                            int gray = (oc.R + oc.G + oc.B) / 3;
                            resized.SetPixel(x, y, Color.FromArgb(oc.A, gray, gray, gray));
                        }
                    }
                }

                PictureBox pic = new PictureBox();
                pic.Image = resized;
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 90;
                pic.Height = 90;
                pic.Top = 0;
                pic.Left = (tablePanel.Width - pic.Width) / 2;
                pic.Cursor = Cursors.Hand;
                pic.Click += (s, e) => BtnTable_Click(table);

                // Tên bàn dưới icon
                Label lblName = new Label();
                lblName.Text = table.TableName;
                lblName.TextAlign = ContentAlignment.MiddleCenter;
                lblName.AutoSize = false;
                lblName.Width = tablePanel.Width;
                lblName.Top = pic.Bottom + 5;
                lblName.Font = new Font("Minion Pro", 13);
                lblName.Click += (s, e) => BtnTable_Click(table); // Thêm sự kiện click cho label

                tablePanel.Controls.Add(pic);
                tablePanel.Controls.Add(lblName);

                flowTables.Controls.Add(tablePanel);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = allTables.FindAll(t => t.TableName.ToLower().Contains(keyword));
            RenderTables(filtered);
        }

        private void BtnTable_Click(Table table)
        {
            // Truyền thông tin bàn sang OrderView
            parent.LoadUserControl(new OrderView(table));
        }
    }
}