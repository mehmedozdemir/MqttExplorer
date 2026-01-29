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
    public partial class ExchangeBrowserControl : UserControl
    {
        private readonly RabbitMQService _rabbitMQService;
        private List<ExchangeInfo> _exchanges = new();
        private System.Windows.Forms.Timer _refreshTimer;

        public ExchangeBrowserControl(RabbitMQService rabbitMQService)
        {
            InitializeComponent();
            _rabbitMQService = rabbitMQService;
            InitializeUI();
            
            _refreshTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _refreshTimer.Tick += async (s, e) => await RefreshExchangesAsync();
            _refreshTimer.Start();
            
            _ = RefreshExchangesAsync();
        }

        private void InitializeUI()
        {
            this.BackColor = UIHelper.Colors.Background;
            UIHelper.StyleButton(btnRefresh, true);
            UIHelper.StyleButton(btnClearFilter);
            UIHelper.StyleDataGridView(dgvExchanges);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleDataGridView(dgvExchangeDetails);
            UIHelper.StyleComboBox(cmbTypeFilter);
        }

        private async Task RefreshExchangesAsync()
        {
            try
            {
                lblStatus.Text = "Yükleniyor...";
                lblStatus.ForeColor = UIHelper.Colors.Warning;
                
                _exchanges = await _rabbitMQService.GetExchangesAsync();
                ApplyFilter();
                
                lblStatus.Text = $"Toplam {_exchanges.Count} exchange bulundu - Son güncelleme: {DateTime.Now:HH:mm:ss}";
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
            var filtered = _exchanges.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchText = txtSearch.Text.ToLower();
                filtered = filtered.Where(e => e.Name.ToLower().Contains(searchText));
            }

            if (cmbTypeFilter.SelectedIndex > 0)
            {
                var selectedType = cmbTypeFilter.SelectedItem.ToString();
                filtered = filtered.Where(e => e.Type == selectedType);
            }

            dgvExchanges.DataSource = null;
            dgvExchanges.DataSource = filtered.ToList();

            if (dgvExchanges.Columns.Count > 0)
            {
                dgvExchanges.Columns["Name"].HeaderText = "Exchange Adı";
                dgvExchanges.Columns["Name"].Width = 250;
                dgvExchanges.Columns["Type"].HeaderText = "Tip";
                dgvExchanges.Columns["Type"].Width = 100;
                dgvExchanges.Columns["Durable"].HeaderText = "Kalıcı";
                dgvExchanges.Columns["Durable"].Width = 70;
                dgvExchanges.Columns["AutoDelete"].HeaderText = "Otomatik Sil";
                dgvExchanges.Columns["AutoDelete"].Width = 90;
                dgvExchanges.Columns["Internal"].HeaderText = "İç";
                dgvExchanges.Columns["Internal"].Width = 50;
                dgvExchanges.Columns["MessagePublishIn"].HeaderText = "Gelen Mesaj";
                dgvExchanges.Columns["MessagePublishIn"].Width = 100;
                dgvExchanges.Columns["MessagePublishOut"].HeaderText = "Giden Mesaj";
                dgvExchanges.Columns["MessagePublishOut"].Width = 100;
                
                dgvExchanges.Columns["VHost"].Visible = false;
                dgvExchanges.Columns["Arguments"].Visible = false;
            }
        }

        private void ShowExchangeDetails(ExchangeInfo exchange)
        {
            var details = new List<KeyValuePair<string, string>>
            {
                new("Exchange Adı", exchange.Name),
                new("VirtualHost", exchange.VHost),
                new("Tip", exchange.Type),
                new("Kalıcı (Durable)", exchange.Durable ? "Evet" : "Hayır"),
                new("Otomatik Sil", exchange.AutoDelete ? "Evet" : "Hayır"),
                new("İç (Internal)", exchange.Internal ? "Evet" : "Hayır"),
                new("Gelen Mesaj", exchange.MessagePublishIn.ToString()),
                new("Giden Mesaj", exchange.MessagePublishOut.ToString())
            };

            if (exchange.Arguments.Count > 0)
            {
                details.Add(new("--- Arguments ---", ""));
                foreach (var arg in exchange.Arguments)
                {
                    details.Add(new(arg.Key, arg.Value?.ToString() ?? ""));
                }
            }

            dgvExchangeDetails.DataSource = details;
            dgvExchangeDetails.Columns[0].HeaderText = "Özellik";
            dgvExchangeDetails.Columns[0].Width = 200;
            dgvExchangeDetails.Columns[1].HeaderText = "Değer";
            dgvExchangeDetails.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void dgvExchanges_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvExchanges.SelectedRows.Count > 0)
            {
                var exchange = dgvExchanges.SelectedRows[0].DataBoundItem as ExchangeInfo;
                if (exchange != null)
                {
                    ShowExchangeDetails(exchange);
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshExchangesAsync();
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbTypeFilter.SelectedIndex = 0;
            ApplyFilter();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

    }

    partial class ExchangeBrowserControl
    {
        private System.ComponentModel.IContainer components = null;
        private SplitContainer mainSplitContainer;
        private Panel topPanel;
        private Label lblSearch;
        private TextBox txtSearch;
        private Label lblType;
        private ComboBox cmbTypeFilter;
        private Button btnRefresh;
        private Button btnClearFilter;
        private DataGridView dgvExchanges;
        private GroupBox grpDetails;
        private DataGridView dgvExchangeDetails;
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
            this.mainSplitContainer = new SplitContainer();
            this.dgvExchanges = new DataGridView();
            this.topPanel = new Panel();
            this.cmbTypeFilter = new ComboBox();
            this.lblType = new Label();
            this.btnClearFilter = new Button();
            this.btnRefresh = new Button();
            this.txtSearch = new TextBox();
            this.lblSearch = new Label();
            this.lblStatus = new Label();
            this.grpDetails = new GroupBox();
            this.dgvExchangeDetails = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchanges)).BeginInit();
            this.topPanel.SuspendLayout();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeDetails)).BeginInit();
            this.SuspendLayout();
            
            // mainSplitContainer
            this.mainSplitContainer.Dock = DockStyle.Fill;
            this.mainSplitContainer.Location = new Point(10, 10);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = Orientation.Horizontal;
            this.mainSplitContainer.Panel1.Controls.Add(this.dgvExchanges);
            this.mainSplitContainer.Panel1.Controls.Add(this.topPanel);
            this.mainSplitContainer.Panel1.Controls.Add(this.lblStatus);
            this.mainSplitContainer.Panel2.Controls.Add(this.grpDetails);
            this.mainSplitContainer.Size = new Size(980, 680);
            this.mainSplitContainer.SplitterDistance = 450;
            this.mainSplitContainer.TabIndex = 0;
            
            // topPanel
            this.topPanel.Controls.Add(this.cmbTypeFilter);
            this.topPanel.Controls.Add(this.lblType);
            this.topPanel.Controls.Add(this.btnClearFilter);
            this.topPanel.Controls.Add(this.btnRefresh);
            this.topPanel.Controls.Add(this.txtSearch);
            this.topPanel.Controls.Add(this.lblSearch);
            this.topPanel.Dock = DockStyle.Top;
            this.topPanel.Location = new Point(0, 0);
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
            this.txtSearch.Size = new Size(200, 25);
            this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
            
            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Font = new Font("Segoe UI", 9.5F);
            this.lblType.Location = new Point(260, 12);
            this.lblType.Name = "lblType";
            this.lblType.Text = "Tip:";
            
            // cmbTypeFilter
            this.cmbTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTypeFilter.Items.AddRange(new object[] { "Tümü", "direct", "fanout", "topic", "headers" });
            this.cmbTypeFilter.Location = new Point(295, 10);
            this.cmbTypeFilter.Name = "cmbTypeFilter";
            this.cmbTypeFilter.SelectedIndex = 0;
            this.cmbTypeFilter.Size = new Size(120, 25);
            this.cmbTypeFilter.SelectedIndexChanged += new EventHandler(this.cmbTypeFilter_SelectedIndexChanged);
            
            // btnRefresh
            this.btnRefresh.Location = new Point(425, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(100, 30);
            this.btnRefresh.Text = "Yenile";
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            
            // btnClearFilter
            this.btnClearFilter.Location = new Point(535, 8);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new Size(120, 30);
            this.btnClearFilter.Text = "Filtreyi Temizle";
            this.btnClearFilter.Click += new EventHandler(this.btnClearFilter_Click);
            
            // dgvExchanges
            this.dgvExchanges.Dock = DockStyle.Fill;
            this.dgvExchanges.Location = new Point(0, 50);
            this.dgvExchanges.Name = "dgvExchanges";
            this.dgvExchanges.Size = new Size(980, 370);
            this.dgvExchanges.TabIndex = 1;
            this.dgvExchanges.SelectionChanged += new EventHandler(this.dgvExchanges_SelectionChanged);
            
            // lblStatus
            this.lblStatus.Dock = DockStyle.Bottom;
            this.lblStatus.Font = new Font("Segoe UI", 9F);
            this.lblStatus.Location = new Point(0, 420);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new Padding(5);
            this.lblStatus.Size = new Size(980, 30);
            this.lblStatus.Text = "Hazır";
            
            // grpDetails
            this.grpDetails.Controls.Add(this.dgvExchangeDetails);
            this.grpDetails.Dock = DockStyle.Fill;
            this.grpDetails.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpDetails.Location = new Point(0, 0);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Padding = new Padding(10);
            this.grpDetails.Size = new Size(980, 226);
            this.grpDetails.TabIndex = 0;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Exchange Detayları";
            
            // dgvExchangeDetails
            this.dgvExchangeDetails.Dock = DockStyle.Fill;
            this.dgvExchangeDetails.Location = new Point(10, 26);
            this.dgvExchangeDetails.Name = "dgvExchangeDetails";
            this.dgvExchangeDetails.Size = new Size(960, 190);
            this.dgvExchangeDetails.TabIndex = 0;
            
            // ExchangeBrowserControl
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "ExchangeBrowserControl";
            this.Padding = new Padding(10);
            this.Size = new Size(1000, 700);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchanges)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.grpDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeDetails)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
