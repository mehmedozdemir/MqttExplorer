using RabbitMQExplorer.Helpers;
using RabbitMQExplorer.Models;
using RabbitMQExplorer.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitMQExplorer.Forms
{
    public partial class BindingBrowserControl : UserControl
    {
        private readonly RabbitMQService _rabbitMQService;
        private List<BindingInfo> _bindings = new();
        private System.Windows.Forms.Timer _refreshTimer;

        public BindingBrowserControl(RabbitMQService rabbitMQService)
        {
            InitializeComponent();
            _rabbitMQService = rabbitMQService;
            InitializeUI();
            
            _refreshTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _refreshTimer.Tick += async (s, e) => await RefreshBindingsAsync();
            _refreshTimer.Start();
            
            _ = RefreshBindingsAsync();
        }

        private void InitializeUI()
        {
            this.BackColor = UIHelper.Colors.Background;
            UIHelper.StyleButton(btnRefresh, true);
            UIHelper.StyleButton(btnClearFilter);
            UIHelper.StyleDataGridView(dgvBindings);
            UIHelper.StyleTextBox(txtSearch);
        }

        private async Task RefreshBindingsAsync()
        {
            try
            {
                lblStatus.Text = "Yükleniyor...";
                lblStatus.ForeColor = UIHelper.Colors.Warning;
                
                _bindings = await _rabbitMQService.GetBindingsAsync();
                ApplyFilter();
                
                lblStatus.Text = $"Toplam {_bindings.Count} binding bulundu - Son güncelleme: {DateTime.Now:HH:mm:ss}";
                lblStatus.ForeColor = UIHelper.Colors.Success;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Hata: {ex.Message}";
                lblStatus.ForeColor = UIHelper.Colors.Error;
            }
        }

        private void ApplyFilter()
        {
            var filtered = _bindings.Where(b => !string.IsNullOrEmpty(b.Source)).AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchText = txtSearch.Text.ToLower();
                filtered = filtered.Where(b => 
                    b.Source.ToLower().Contains(searchText) ||
                    b.Destination.ToLower().Contains(searchText) ||
                    b.RoutingKey.ToLower().Contains(searchText));
            }

            dgvBindings.DataSource = null;
            dgvBindings.DataSource = filtered.ToList();

            if (dgvBindings.Columns.Count > 0)
            {
                dgvBindings.Columns["Source"].HeaderText = "Kaynak Exchange";
                dgvBindings.Columns["Source"].Width = 250;
                dgvBindings.Columns["Destination"].HeaderText = "Hedef";
                dgvBindings.Columns["Destination"].Width = 250;
                dgvBindings.Columns["DestinationType"].HeaderText = "Hedef Tipi";
                dgvBindings.Columns["DestinationType"].Width = 100;
                dgvBindings.Columns["RoutingKey"].HeaderText = "Routing Key";
                dgvBindings.Columns["RoutingKey"].Width = 200;
                
                dgvBindings.Columns["VHost"].Visible = false;
                dgvBindings.Columns["Arguments"].Visible = false;
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshBindingsAsync();
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            ApplyFilter();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

    }

    partial class BindingBrowserControl
    {
        private System.ComponentModel.IContainer components = null;
        private Panel topPanel;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnRefresh;
        private Button btnClearFilter;
        private DataGridView dgvBindings;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvBindings = new DataGridView();
            this.topPanel = new Panel();
            this.btnClearFilter = new Button();
            this.btnRefresh = new Button();
            this.txtSearch = new TextBox();
            this.lblSearch = new Label();
            this.lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBindings)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            
            // topPanel
            this.topPanel.Controls.Add(this.btnClearFilter);
            this.topPanel.Controls.Add(this.btnRefresh);
            this.topPanel.Controls.Add(this.txtSearch);
            this.topPanel.Controls.Add(this.lblSearch);
            this.topPanel.Dock = DockStyle.Top;
            this.topPanel.Location = new Point(10, 10);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new Padding(0, 0, 0, 10);
            this.topPanel.Size = new Size(980, 50);
            this.topPanel.TabIndex = 0;
            
            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Segoe UI", 9.5F);
            this.lblSearch.Location = new Point(0, 12);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Text = "Ara:";
            
            // txtSearch
            this.txtSearch.Location = new Point(50, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(300, 25);
            this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
            
            // btnRefresh
            this.btnRefresh.Location = new Point(360, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(100, 30);
            this.btnRefresh.Text = "Yenile";
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            
            // btnClearFilter
            this.btnClearFilter.Location = new Point(470, 8);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new Size(120, 30);
            this.btnClearFilter.Text = "Filtreyi Temizle";
            this.btnClearFilter.Click += new EventHandler(this.btnClearFilter_Click);
            
            // dgvBindings
            this.dgvBindings.Dock = DockStyle.Fill;
            this.dgvBindings.Location = new Point(10, 60);
            this.dgvBindings.Name = "dgvBindings";
            this.dgvBindings.Size = new Size(980, 600);
            this.dgvBindings.TabIndex = 1;
            
            // lblStatus
            this.lblStatus.Dock = DockStyle.Bottom;
            this.lblStatus.Font = new Font("Segoe UI", 9F);
            this.lblStatus.Location = new Point(10, 660);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new Padding(5);
            this.lblStatus.Size = new Size(980, 30);
            this.lblStatus.Text = "Hazır";
            
            // BindingBrowserControl
            this.Controls.Add(this.dgvBindings);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.lblStatus);
            this.Name = "BindingBrowserControl";
            this.Padding = new Padding(10);
            this.Size = new Size(1000, 700);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBindings)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
