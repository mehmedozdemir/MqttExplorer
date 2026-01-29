using System;
using System.Drawing;
using System.Windows.Forms;

namespace RabbitMQExplorer.Helpers
{
    public static class UIHelper
    {
        // Modern color palette
        public static class Colors
        {
            public static Color Primary = ColorTranslator.FromHtml("#2196F3");
            public static Color PrimaryDark = ColorTranslator.FromHtml("#1976D2");
            public static Color PrimaryLight = ColorTranslator.FromHtml("#BBDEFB");
            public static Color Accent = ColorTranslator.FromHtml("#FF5722");
            public static Color Success = ColorTranslator.FromHtml("#4CAF50");
            public static Color Warning = ColorTranslator.FromHtml("#FF9800");
            public static Color Error = ColorTranslator.FromHtml("#F44336");
            public static Color Background = ColorTranslator.FromHtml("#FAFAFA");
            public static Color Surface = Color.White;
            public static Color TextPrimary = ColorTranslator.FromHtml("#212121");
            public static Color TextSecondary = ColorTranslator.FromHtml("#757575");
            public static Color Border = ColorTranslator.FromHtml("#E0E0E0");
        }

        public static void StyleButton(Button button, bool isPrimary = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.BorderSize = 0;
            
            if (isPrimary)
            {
                button.BackColor = Colors.Primary;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = Colors.PrimaryDark;
            }
            else
            {
                button.BackColor = Colors.Surface;
                button.ForeColor = Colors.TextPrimary;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Colors.Border;
                button.FlatAppearance.MouseOverBackColor = Colors.Background;
            }
        }

        public static void StylePanel(Panel panel, bool elevated = false)
        {
            panel.BackColor = Colors.Surface;
            
            if (elevated)
            {
                panel.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Colors.Surface;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Header style
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Colors.Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colors.PrimaryDark;
            dgv.ColumnHeadersHeight = 40;

            // Cell style
            dgv.DefaultCellStyle.BackColor = Colors.Surface;
            dgv.DefaultCellStyle.ForeColor = Colors.TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Colors.PrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = Colors.TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.RowTemplate.Height = 35;

            // Alternating row color
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Colors.Background;
        }

        public static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI", 9.5F);
        }

        public static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = new Font("Segoe UI", 9.5F);
        }

        public static void ShowLoading(Control parent)
        {
            var loadingPanel = new Panel
            {
                Name = "loadingPanel",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(128, 255, 255, 255),
                Visible = true
            };

            var label = new Label
            {
                Text = "Yükleniyor...",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Colors.Primary,
                AutoSize = true
            };

            label.Location = new Point(
                (loadingPanel.Width - label.Width) / 2,
                (loadingPanel.Height - label.Height) / 2
            );

            loadingPanel.Controls.Add(label);
            parent.Controls.Add(loadingPanel);
            loadingPanel.BringToFront();
            parent.Refresh();
        }

        public static void HideLoading(Control parent)
        {
            var loadingPanel = parent.Controls.Find("loadingPanel", false);
            if (loadingPanel.Length > 0)
            {
                parent.Controls.Remove(loadingPanel[0]);
                loadingPanel[0].Dispose();
            }
        }

        public static void ShowError(string message, string title = "Hata")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowSuccess(string message, string title = "Başarılı")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowConfirmation(string message, string title = "Onay")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
