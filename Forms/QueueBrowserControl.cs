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
    public partial class QueueBrowserControl : UserControl
    {
        private readonly RabbitMQService _rabbitMQService;
        private List<QueueInfo> _queues = new();
        private System.Windows.Forms.Timer _refreshTimer;

        public QueueBrowserControl(RabbitMQService rabbitMQService)
        {
            InitializeComponent();
            _rabbitMQService = rabbitMQService;
            InitializeUI();
            
            _refreshTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _refreshTimer.Tick += async (s, e) => await RefreshQueuesAsync();
            _refreshTimer.Start();
            
            _ = RefreshQueuesAsync();
        }

        private void InitializeUI()
        {
            this.BackColor = UIHelper.Colors.Background;
            UIHelper.StyleButton(btnRefresh, true);
            UIHelper.StyleButton(btnClearFilter);
            UIHelper.StyleDataGridView(dgvQueues);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleDataGridView(dgvQueueDetails);
        }

        private async Task RefreshQueuesAsync()
        {
            try
            {
                lblStatus.Text = "Yükleniyor...";
                lblStatus.ForeColor = UIHelper.Colors.Warning;
                
                _queues = await _rabbitMQService.GetQueuesAsync();
                ApplyFilter();
                
                lblStatus.Text = $"Toplam {_queues.Count} kuyruk bulundu - Son güncelleme: {DateTime.Now:HH:mm:ss}";
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
            var filtered = _queues.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchText = txtSearch.Text.ToLower();
                filtered = filtered.Where(q => q.Name.ToLower().Contains(searchText));
            }

            dgvQueues.DataSource = null;
            dgvQueues.DataSource = filtered.ToList();

            if (dgvQueues.Columns.Count > 0)
            {
                dgvQueues.Columns["Name"].HeaderText = "Kuyruk Adı";
                dgvQueues.Columns["Name"].Width = 250;
                dgvQueues.Columns["Messages"].HeaderText = "Toplam Mesaj";
                dgvQueues.Columns["Messages"].Width = 100;
                dgvQueues.Columns["MessagesReady"].HeaderText = "Hazır";
                dgvQueues.Columns["MessagesReady"].Width = 80;
                dgvQueues.Columns["MessagesUnacknowledged"].HeaderText = "Onaysız";
                dgvQueues.Columns["MessagesUnacknowledged"].Width = 80;
                dgvQueues.Columns["Consumers"].HeaderText = "Consumer";
                dgvQueues.Columns["Consumers"].Width = 80;
                dgvQueues.Columns["State"].HeaderText = "Durum";
                dgvQueues.Columns["State"].Width = 100;
                dgvQueues.Columns["Durable"].HeaderText = "Kalıcı";
                dgvQueues.Columns["Durable"].Width = 60;
                dgvQueues.Columns["AutoDelete"].HeaderText = "Otomatik Sil";
                dgvQueues.Columns["AutoDelete"].Width = 90;
                
                dgvQueues.Columns["VHost"].Visible = false;
                dgvQueues.Columns["Exclusive"].Visible = false;
                dgvQueues.Columns["Arguments"].Visible = false;
                dgvQueues.Columns["DeadLetterExchange"].Visible = false;
                dgvQueues.Columns["DeadLetterRoutingKey"].Visible = false;
                dgvQueues.Columns["MessageTtl"].Visible = false;
                dgvQueues.Columns["MaxLength"].Visible = false;
            }
        }

        private void ShowQueueDetails(QueueInfo queue)
        {
            var details = new List<KeyValuePair<string, string>>
            {
                new("Kuyruk Adı", queue.Name),
                new("VirtualHost", queue.VHost),
                new("Durum", queue.State),
                new("Toplam Mesaj", queue.Messages.ToString()),
                new("Hazır Mesajlar", queue.MessagesReady.ToString()),
                new("Onaysız Mesajlar", queue.MessagesUnacknowledged.ToString()),
                new("Consumer Sayısı", queue.Consumers.ToString()),
                new("Kalıcı (Durable)", queue.Durable ? "Evet" : "Hayır"),
                new("Otomatik Sil", queue.AutoDelete ? "Evet" : "Hayır"),
                new("Exclusive", queue.Exclusive ? "Evet" : "Hayır")
            };

            if (!string.IsNullOrEmpty(queue.DeadLetterExchange))
                details.Add(new("Dead Letter Exchange", queue.DeadLetterExchange));

            if (!string.IsNullOrEmpty(queue.DeadLetterRoutingKey))
                details.Add(new("Dead Letter Routing Key", queue.DeadLetterRoutingKey));

            if (queue.MessageTtl.HasValue)
                details.Add(new("Message TTL", $"{queue.MessageTtl.Value} ms"));

            if (queue.MaxLength.HasValue)
                details.Add(new("Max Length", queue.MaxLength.Value.ToString()));

            if (queue.Arguments.Count > 0)
            {
                details.Add(new("--- Arguments ---", ""));
                foreach (var arg in queue.Arguments)
                {
                    details.Add(new(arg.Key, arg.Value?.ToString() ?? ""));
                }
            }

            dgvQueueDetails.DataSource = details;
            dgvQueueDetails.Columns[0].HeaderText = "Özellik";
            dgvQueueDetails.Columns[0].Width = 200;
            dgvQueueDetails.Columns[1].HeaderText = "Değer";
            dgvQueueDetails.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void dgvQueues_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvQueues.SelectedRows.Count > 0)
            {
                var queue = dgvQueues.SelectedRows[0].DataBoundItem as QueueInfo;
                if (queue != null)
                {
                    ShowQueueDetails(queue);
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshQueuesAsync();
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

    partial class QueueBrowserControl
    {
        private System.ComponentModel.IContainer components = null;
        private SplitContainer mainSplitContainer;
        private Panel topPanel;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnRefresh;
        private Button btnClearFilter;
        private DataGridView dgvQueues;
        private GroupBox grpDetails;
        private DataGridView dgvQueueDetails;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_refreshTimer != null)
                {
                    _refreshTimer.Stop();
                    _refreshTimer.Dispose();
                    _refreshTimer = null;
                }
                
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainSplitContainer = new SplitContainer();
            this.dgvQueues = new DataGridView();
            this.topPanel = new Panel();
            this.btnClearFilter = new Button();
            this.btnRefresh = new Button();
            this.txtSearch = new TextBox();
            this.lblSearch = new Label();
            this.lblStatus = new Label();
            this.grpDetails = new GroupBox();
            this.dgvQueueDetails = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueues)).BeginInit();
            this.topPanel.SuspendLayout();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueueDetails)).BeginInit();
            this.SuspendLayout();
            
            // mainSplitContainer
            this.mainSplitContainer.Dock = DockStyle.Fill;
            this.mainSplitContainer.Location = new Point(10, 10);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = Orientation.Horizontal;
            this.mainSplitContainer.Panel1.Controls.Add(this.dgvQueues);
            this.mainSplitContainer.Panel1.Controls.Add(this.topPanel);
            this.mainSplitContainer.Panel1.Controls.Add(this.lblStatus);
            this.mainSplitContainer.Panel2.Controls.Add(this.grpDetails);
            this.mainSplitContainer.Size = new Size(980, 680);
            this.mainSplitContainer.SplitterDistance = 450;
            this.mainSplitContainer.TabIndex = 0;
            
            // topPanel
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
            
            // dgvQueues
            this.dgvQueues.Dock = DockStyle.Fill;
            this.dgvQueues.Location = new Point(0, 50);
            this.dgvQueues.Name = "dgvQueues";
            this.dgvQueues.Size = new Size(980, 370);
            this.dgvQueues.TabIndex = 1;
            this.dgvQueues.SelectionChanged += new EventHandler(this.dgvQueues_SelectionChanged);
            
            // lblStatus
            this.lblStatus.Dock = DockStyle.Bottom;
            this.lblStatus.Font = new Font("Segoe UI", 9F);
            this.lblStatus.Location = new Point(0, 420);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new Padding(5);
            this.lblStatus.Size = new Size(980, 30);
            this.lblStatus.Text = "Hazır";
            
            // grpDetails
            this.grpDetails.Controls.Add(this.dgvQueueDetails);
            this.grpDetails.Dock = DockStyle.Fill;
            this.grpDetails.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpDetails.Location = new Point(0, 0);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Padding = new Padding(10);
            this.grpDetails.Size = new Size(980, 226);
            this.grpDetails.TabIndex = 0;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Kuyruk Detayları";
            
            // dgvQueueDetails
            this.dgvQueueDetails.Dock = DockStyle.Fill;
            this.dgvQueueDetails.Location = new Point(10, 26);
            this.dgvQueueDetails.Name = "dgvQueueDetails";
            this.dgvQueueDetails.Size = new Size(960, 190);
            this.dgvQueueDetails.TabIndex = 0;
            
            // QueueBrowserControl
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "QueueBrowserControl";
            this.Padding = new Padding(10);
            this.Size = new Size(1000, 700);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueues)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.grpDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueueDetails)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
