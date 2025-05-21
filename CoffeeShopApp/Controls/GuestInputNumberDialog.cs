using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeShopApp.Controls
{
    public class GuestInputNumberDialog : Form
    {
        private NumericUpDown numericUpDown;
        private Button btnOK;
        private Button btnCancel;
        private Label lblPrompt;

        public int Value { get { return (int)numericUpDown.Value; } }

        public GuestInputNumberDialog(string prompt, string title, int defaultValue = 1)
        {
            Text = title;
            Size = new Size(300, 150);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            lblPrompt = new Label
            {
                Text = prompt,
                Location = new Point(10, 20),
                Size = new Size(280, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            numericUpDown = new NumericUpDown
            {
                Location = new Point(10, 45),
                Size = new Size(280, 25),
                Minimum = 1,
                Maximum = 100,
                Value = defaultValue
            };

            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(100, 80),
                Size = new Size(80, 30)
            };

            btnCancel = new Button
            {
                Text = "Hủy",
                DialogResult = DialogResult.Cancel,
                Location = new Point(190, 80),
                Size = new Size(80, 30)
            };

            Controls.Add(lblPrompt);
            Controls.Add(numericUpDown);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }
    }
}