using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CoffeeShopAPI.Models;
using CoffeeShopApp.ServicesUser;
using CoffeeShopApp.Services;

namespace CoffeeShopApp.Views.User
{
    public partial class MoveTableForm : Form
    {
        private FlowLayoutPanel flowTables;
        private List<Table> allTables;
        private Order currentOrder;
        private OrderApiClient apiClient;

        public MoveTableForm(Order currentOrder, List<Table> allTables)
        {
            this.currentOrder = currentOrder;
            this.allTables = allTables;
            apiClient = new OrderApiClient();
            
            SetupLayout();
            RenderTables();
        }

        private void SetupLayout()
        {
            this.Text = $"Chọn bàn để chuyển đơn hàng #{currentOrder.Id}";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            flowTables = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            Label lblTitle = new Label
            {
                Text = $"Chọn bàn để chuyển đơn hàng từ Bàn {currentOrder.TableId}",
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Minion Pro", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(flowTables);
            this.Controls.Add(lblTitle);
        }

        private void RenderTables()
        {
            if (allTables == null || allTables.Count == 0)
            {
                Label lblNoTables = new Label
                {
                    Text = "Không có bàn nào khả dụng",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Minion Pro", 14)
                };
                this.Controls.Add(lblNoTables);
                return;
            }

            foreach (var table in allTables)
            {
                // Bỏ qua bàn hiện tại
                if (table.Id == currentOrder.TableId)
                    continue;

                // Tạo Panel chứa icon và label
                Panel tablePanel = new Panel
                {
                    Width = 120,
                    Height = 150,
                    BackColor = Color.Transparent,
                    Margin = new Padding(10)
                };

                // Xử lý icon bàn
                Image original = Properties.Resources.table_icon;
                Bitmap resized = new Bitmap(original, new Size(90, 90));

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

                PictureBox pic = new PictureBox
                {
                    Image = resized,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 90,
                    Height = 90,
                    Top = 0,
                    Left = (tablePanel.Width - 90) / 2,
                    Cursor = Cursors.Hand
                };
                pic.Click += (s, e) => TableSelected(table);

                // Tên bàn dưới icon
                Label lblName = new Label
                {
                    Text = table.TableName,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Width = tablePanel.Width,
                    Top = pic.Bottom + 5,
                    Font = new Font("Minion Pro", 13)
                };
                lblName.Click += (s, e) => TableSelected(table);

                // Thêm trạng thái
                Label lblStatus = new Label
                {
                    Text = table.Status == "full" ? "Có khách" : "Trống",
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Width = tablePanel.Width,
                    Top = lblName.Bottom + 5,
                    Font = new Font("Minion Pro", 10),
                    ForeColor = table.Status == "full" ? Color.Red : Color.Green
                };

                tablePanel.Controls.Add(pic);
                tablePanel.Controls.Add(lblName);
                tablePanel.Controls.Add(lblStatus);

                flowTables.Controls.Add(tablePanel);
            }
        }

        private async void TableSelected(Table targetTable)
        {
            try
            {
                bool success;
                
                // Nếu bàn đích có khách (đã có đơn hàng)
                if (targetTable.Status == "full")
                {
                    DialogResult result = MessageBox.Show(
                        $"Bàn {targetTable.TableName} đã có khách. Bạn có muốn gộp đơn hàng không?",
                        "Xác nhận gộp bàn",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        // Sử dụng phương thức từ lớp tiện ích để gộp bàn
                        success = await TableOperationService.MergeTablesAsync(currentOrder, targetTable, apiClient);
                    }
                    else
                    {
                        return; // Người dùng đã hủy thao tác
                    }
                }
                else // Bàn đích trống
                {
                    // Sử dụng phương thức từ lớp tiện ích để chuyển bàn
                    success = await TableOperationService.MoveTableAsync(currentOrder, targetTable, apiClient);
                }
                
                if (success)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}