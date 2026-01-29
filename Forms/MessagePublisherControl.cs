using RabbitMQExplorer.Helpers;
using RabbitMQExplorer.Models;
using RabbitMQExplorer.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RabbitMQExplorer.Forms
{
    public partial class MessagePublisherControl : UserControl
    {
        private readonly RabbitMQService _rabbitMQService;
        private List<ExchangeInfo> _exchanges = new();

        public MessagePublisherControl(RabbitMQService rabbitMQService)
        {
            InitializeComponent();
            _rabbitMQService = rabbitMQService;
            InitializeUI();
            _ = LoadExchangesAsync();
        }

        private void InitializeUI()
        {
            this.BackColor = UIHelper.Colors.Background;
            UIHelper.StyleButton(btnSend, true);
            UIHelper.StyleButton(btnClear);
            UIHelper.StyleButton(btnFormatJson);
            UIHelper.StyleButton(btnAddHeader);
            UIHelper.StyleButton(btnRemoveHeader);
            UIHelper.StyleComboBox(cmbExchange);
            UIHelper.StyleTextBox(txtRoutingKey);
            UIHelper.StyleDataGridView(dgvHeaders);
            
            // Setup headers grid
            dgvHeaders.Columns.Add("Key", "Header Key");
            dgvHeaders.Columns.Add("Value", "Header Value");
            dgvHeaders.Columns[0].Width = 200;
            dgvHeaders.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvHeaders.AllowUserToAddRows = true;
        }

        private async System.Threading.Tasks.Task LoadExchangesAsync()
        {
            try
            {
                _exchanges = await _rabbitMQService.GetExchangesAsync();
                cmbExchange.Items.Clear();
                cmbExchange.Items.Add("(default)");
                
                foreach (var exchange in _exchanges)
                {
                    if (!string.IsNullOrEmpty(exchange.Name))
                    {
                        cmbExchange.Items.Add(exchange.Name);
                    }
                }
                
                cmbExchange.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Exchange'ler yüklenirken hata: {ex.Message}");
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMessage.Text))
                {
                    UIHelper.ShowError("Mesaj içeriği boş olamaz!");
                    return;
                }

                var exchange = cmbExchange.SelectedItem?.ToString() ?? "";
                if (exchange == "(default)") exchange = "";

                var routingKey = txtRoutingKey.Text;
                var message = txtMessage.Text;
                
                // Collect headers
                var headers = new Dictionary<string, object>();
                foreach (DataGridViewRow row in dgvHeaders.Rows)
                {
                    if (row.IsNewRow) continue;
                    var key = row.Cells[0].Value?.ToString();
                    var value = row.Cells[1].Value?.ToString();
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        headers[key] = value;
                    }
                }

                byte deliveryMode = chkPersistent.Checked ? (byte)2 : (byte)1;
                byte priority = (byte)numPriority.Value;
                string contentType = txtContentType.Text;

                UIHelper.ShowLoading(this);
                lblStatus.Text = "Mesaj gönderiliyor...";
                
                await _rabbitMQService.PublishMessageAsync(exchange, routingKey, message, headers, deliveryMode, priority, contentType);
                
                lblStatus.Text = $"Mesaj başarıyla gönderildi! - {DateTime.Now:HH:mm:ss}";
                lblStatus.ForeColor = UIHelper.Colors.Success;
                
                UpdateStats();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Hata: {ex.Message}";
                lblStatus.ForeColor = UIHelper.Colors.Error;
                UIHelper.ShowError($"Mesaj gönderilirken hata:\n{ex.Message}");
            }
            finally
            {
                UIHelper.HideLoading(this);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMessage.Clear();
            txtRoutingKey.Clear();
            dgvHeaders.Rows.Clear();
            txtContentType.Text = "text/plain";
            numPriority.Value = 0;
            chkPersistent.Checked = false;
        }

        private void btnFormatJson_Click(object sender, EventArgs e)
        {
            try
            {
                var formatted = MessageFormatter.FormatJson(txtMessage.Text);
                txtMessage.Text = formatted;
                txtContentType.Text = "application/json";
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"JSON formatlama hatası:\n{ex.Message}");
            }
        }

        private void btnAddHeader_Click(object sender, EventArgs e)
        {
            dgvHeaders.Rows.Add("header-key", "header-value");
        }

        private void btnRemoveHeader_Click(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvHeaders.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        dgvHeaders.Rows.Remove(row);
                    }
                }
            }
        }

        private int _sentCount = 0;
        private void UpdateStats()
        {
            _sentCount++;
            lblSentCount.Text = $"Gönderilen Mesaj: {_sentCount}";
        }
    }

    partial class MessagePublisherControl
    {
        private System.ComponentModel.IContainer components = null;
        private SplitContainer mainSplitContainer;
        private GroupBox grpSettings;
        private Label lblExchange;
        private ComboBox cmbExchange;
        private Label lblRoutingKey;
        private TextBox txtRoutingKey;
        private Label lblContentType;
        private TextBox txtContentType;
        private CheckBox chkPersistent;
        private Label lblPriority;
        private NumericUpDown numPriority;
        private GroupBox grpHeaders;
        private DataGridView dgvHeaders;
        private Panel headerButtonPanel;
        private Button btnAddHeader;
        private Button btnRemoveHeader;
        private GroupBox grpMessage;
        private TextBox txtMessage;
        private Panel messageButtonPanel;
        private Button btnFormatJson;
        private Button btnClear;
        private Button btnSend;
        private Panel statusPanel;
        private Label lblStatus;
        private Label lblSentCount;

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
            this.grpSettings = new GroupBox();
            this.numPriority = new NumericUpDown();
            this.lblPriority = new Label();
            this.chkPersistent = new CheckBox();
            this.txtContentType = new TextBox();
            this.lblContentType = new Label();
            this.txtRoutingKey = new TextBox();
            this.lblRoutingKey = new Label();
            this.cmbExchange = new ComboBox();
            this.lblExchange = new Label();
            this.grpHeaders = new GroupBox();
            this.dgvHeaders = new DataGridView();
            this.headerButtonPanel = new Panel();
            this.btnRemoveHeader = new Button();
            this.btnAddHeader = new Button();
            this.grpMessage = new GroupBox();
            this.txtMessage = new TextBox();
            this.messageButtonPanel = new Panel();
            this.btnSend = new Button();
            this.btnClear = new Button();
            this.btnFormatJson = new Button();
            this.statusPanel = new Panel();
            this.lblSentCount = new Label();
            this.lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPriority)).BeginInit();
            this.grpHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).BeginInit();
            this.headerButtonPanel.SuspendLayout();
            this.grpMessage.SuspendLayout();
            this.messageButtonPanel.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.SuspendLayout();
            
            // mainSplitContainer
            this.mainSplitContainer.Dock = DockStyle.Fill;
            this.mainSplitContainer.Location = new Point(10, 10);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Panel1.Controls.Add(this.grpHeaders);
            this.mainSplitContainer.Panel1.Controls.Add(this.grpSettings);
            this.mainSplitContainer.Panel2.Controls.Add(this.grpMessage);
            this.mainSplitContainer.Size = new Size(980, 650);
            this.mainSplitContainer.SplitterDistance = 450;
            
            // grpSettings
            this.grpSettings.Controls.Add(this.numPriority);
            this.grpSettings.Controls.Add(this.lblPriority);
            this.grpSettings.Controls.Add(this.chkPersistent);
            this.grpSettings.Controls.Add(this.txtContentType);
            this.grpSettings.Controls.Add(this.lblContentType);
            this.grpSettings.Controls.Add(this.txtRoutingKey);
            this.grpSettings.Controls.Add(this.lblRoutingKey);
            this.grpSettings.Controls.Add(this.cmbExchange);
            this.grpSettings.Controls.Add(this.lblExchange);
            this.grpSettings.Dock = DockStyle.Top;
            this.grpSettings.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpSettings.Location = new Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Padding = new Padding(10);
            this.grpSettings.Size = new Size(450, 260);
            this.grpSettings.Text = "Mesaj Ayarları";
            
            int y = 30;
            int spacing = 40;
            
            // lblExchange
            this.lblExchange.AutoSize = true;
            this.lblExchange.Font = new Font("Segoe UI", 9F);
            this.lblExchange.Location = new Point(15, y);
            this.lblExchange.Text = "Exchange:";
            
            // cmbExchange
            this.cmbExchange.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbExchange.Location = new Point(15, y + 20);
            this.cmbExchange.Name = "cmbExchange";
            this.cmbExchange.Size = new Size(410, 25);
            y += spacing + 20;
            
            // lblRoutingKey
            this.lblRoutingKey.AutoSize = true;
            this.lblRoutingKey.Font = new Font("Segoe UI", 9F);
            this.lblRoutingKey.Location = new Point(15, y);
            this.lblRoutingKey.Text = "Routing Key:";
            
            // txtRoutingKey
            this.txtRoutingKey.Location = new Point(15, y + 20);
            this.txtRoutingKey.Name = "txtRoutingKey";
            this.txtRoutingKey.Size = new Size(410, 25);
            y += spacing + 20;
            
            // lblContentType
            this.lblContentType.AutoSize = true;
            this.lblContentType.Font = new Font("Segoe UI", 9F);
            this.lblContentType.Location = new Point(15, y);
            this.lblContentType.Text = "Content Type:";
            
            // txtContentType
            this.txtContentType.Location = new Point(15, y + 20);
            this.txtContentType.Name = "txtContentType";
            this.txtContentType.Size = new Size(250, 25);
            this.txtContentType.Text = "text/plain";
            y += spacing + 20;
            
            // chkPersistent
            this.chkPersistent.AutoSize = true;
            this.chkPersistent.Font = new Font("Segoe UI", 9F);
            this.chkPersistent.Location = new Point(15, y);
            this.chkPersistent.Name = "chkPersistent";
            this.chkPersistent.Text = "Persistent (Kalıcı Mesaj)";
            
            // lblPriority
            this.lblPriority.AutoSize = true;
            this.lblPriority.Font = new Font("Segoe UI", 9F);
            this.lblPriority.Location = new Point(250, y);
            this.lblPriority.Text = "Priority:";
            
            // numPriority
            this.numPriority.Location = new Point(320, y - 2);
            this.numPriority.Maximum = 10;
            this.numPriority.Name = "numPriority";
            this.numPriority.Size = new Size(80, 25);
            
            // grpHeaders
            this.grpHeaders.Controls.Add(this.dgvHeaders);
            this.grpHeaders.Controls.Add(this.headerButtonPanel);
            this.grpHeaders.Dock = DockStyle.Fill;
            this.grpHeaders.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpHeaders.Location = new Point(0, 260);
            this.grpHeaders.Name = "grpHeaders";
            this.grpHeaders.Padding = new Padding(10);
            this.grpHeaders.Size = new Size(450, 390);
            this.grpHeaders.Text = "Headers";
            
            // dgvHeaders
            this.dgvHeaders.Dock = DockStyle.Fill;
            this.dgvHeaders.Location = new Point(10, 26);
            this.dgvHeaders.Name = "dgvHeaders";
            this.dgvHeaders.Size = new Size(430, 314);
            
            // headerButtonPanel
            this.headerButtonPanel.Controls.Add(this.btnRemoveHeader);
            this.headerButtonPanel.Controls.Add(this.btnAddHeader);
            this.headerButtonPanel.Dock = DockStyle.Bottom;
            this.headerButtonPanel.Location = new Point(10, 340);
            this.headerButtonPanel.Padding = new Padding(0, 5, 0, 0);
            this.headerButtonPanel.Size = new Size(430, 40);
            
            // btnAddHeader
            this.btnAddHeader.Location = new Point(0, 5);
            this.btnAddHeader.Name = "btnAddHeader";
            this.btnAddHeader.Size = new Size(100, 30);
            this.btnAddHeader.Text = "Ekle";
            this.btnAddHeader.Click += new EventHandler(this.btnAddHeader_Click);
            
            // btnRemoveHeader
            this.btnRemoveHeader.Location = new Point(110, 5);
            this.btnRemoveHeader.Name = "btnRemoveHeader";
            this.btnRemoveHeader.Size = new Size(100, 30);
            this.btnRemoveHeader.Text = "Sil";
            this.btnRemoveHeader.Click += new EventHandler(this.btnRemoveHeader_Click);
            
            // grpMessage
            this.grpMessage.Controls.Add(this.txtMessage);
            this.grpMessage.Controls.Add(this.messageButtonPanel);
            this.grpMessage.Dock = DockStyle.Fill;
            this.grpMessage.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpMessage.Location = new Point(0, 0);
            this.grpMessage.Name = "grpMessage";
            this.grpMessage.Padding = new Padding(10);
            this.grpMessage.Size = new Size(526, 650);
            this.grpMessage.Text = "Mesaj İçeriği";
            
            // txtMessage
            this.txtMessage.Dock = DockStyle.Fill;
            this.txtMessage.Font = new Font("Consolas", 9.5F);
            this.txtMessage.Location = new Point(10, 26);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = ScrollBars.Both;
            this.txtMessage.Size = new Size(506, 574);
            
            // messageButtonPanel
            this.messageButtonPanel.Controls.Add(this.btnSend);
            this.messageButtonPanel.Controls.Add(this.btnClear);
            this.messageButtonPanel.Controls.Add(this.btnFormatJson);
            this.messageButtonPanel.Dock = DockStyle.Bottom;
            this.messageButtonPanel.Location = new Point(10, 600);
            this.messageButtonPanel.Padding = new Padding(0, 5, 0, 0);
            this.messageButtonPanel.Size = new Size(506, 40);
            
            // btnFormatJson
            this.btnFormatJson.Location = new Point(0, 5);
            this.btnFormatJson.Name = "btnFormatJson";
            this.btnFormatJson.Size = new Size(120, 30);
            this.btnFormatJson.Text = "Format JSON";
            this.btnFormatJson.Click += new EventHandler(this.btnFormatJson_Click);
            
            // btnClear
            this.btnClear.Location = new Point(260, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(120, 30);
            this.btnClear.Text = "Temizle";
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            
            // btnSend
            this.btnSend.Location = new Point(386, 5);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new Size(120, 30);
            this.btnSend.Text = "Gönder";
            this.btnSend.Click += new EventHandler(this.btnSend_Click);
            
            // statusPanel
            this.statusPanel.Controls.Add(this.lblSentCount);
            this.statusPanel.Controls.Add(this.lblStatus);
            this.statusPanel.Dock = DockStyle.Bottom;
            this.statusPanel.Location = new Point(10, 660);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new Size(980, 30);
            
            // lblStatus
            this.lblStatus.Dock = DockStyle.Fill;
            this.lblStatus.Font = new Font("Segoe UI", 9F);
            this.lblStatus.Location = new Point(0, 0);
            this.lblStatus.Padding = new Padding(5);
            this.lblStatus.Size = new Size(680, 30);
            this.lblStatus.Text = "Hazır";
            
            // lblSentCount
            this.lblSentCount.Dock = DockStyle.Right;
            this.lblSentCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblSentCount.Location = new Point(680, 0);
            this.lblSentCount.Padding = new Padding(5);
            this.lblSentCount.Size = new Size(300, 30);
            this.lblSentCount.Text = "Gönderilen Mesaj: 0";
            this.lblSentCount.TextAlign = ContentAlignment.MiddleRight;
            
            // MessagePublisherControl
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusPanel);
            this.Name = "MessagePublisherControl";
            this.Padding = new Padding(10);
            this.Size = new Size(1000, 700);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPriority)).EndInit();
            this.grpHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).EndInit();
            this.headerButtonPanel.ResumeLayout(false);
            this.grpMessage.ResumeLayout(false);
            this.grpMessage.PerformLayout();
            this.messageButtonPanel.ResumeLayout(false);
            this.statusPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
