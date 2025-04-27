using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopApp.Views.Admin
{
    public partial class AdminDashboard : Form
    {
        private Button currentButton; // Lưu nút đang được chọn

        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
            LoadForm(new InventoryManagementView());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
            LoadForm(new CategoryManagementView());
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
            LoadForm(new UserManagementView());
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
            LoadForm(new MenuManagementView());
        }

        private void btnSession_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
            LoadForm(new SessionManagementView());
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
            LoadForm(new TableManagementView());
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
            LoadForm(new BillManagementView()); 
        }
        private void LoadForm(Form frm)
        {
            panelMain.Controls.Clear();
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            panelMain.Controls.Add(frm);
            frm.Show();
        }
        private void ActivateButton(Button button)
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(45, 45, 45); // Màu gốc sidebar
            }

            currentButton = button;
            currentButton.BackColor = Color.FromArgb(192, 192, 255); // Màu cam sáng (có thể đổi)
        }

    }
}
