using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQExplorer.Helpers;
using RabbitMQExplorer.Models;
using RabbitMQExplorer.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitMQExplorer.Forms
{
    public partial class MessageConsumerControl : UserControl
    {
        private readonly RabbitMQService _rabbitMQService;
        private List<QueueInfo> _queues = new();
        private List<MessageInfo> _messages = new();
        private EventingBasicConsumer? _consumer;
        private string? _consumerTag;
        private bool _isConsuming = false;

        public MessageConsumerControl(RabbitMQService rabbitMQService)
        {
            InitializeComponent();
            _rabbitMQService = rabbitMQService;
            InitializeUI();
            _ = LoadQueuesAsync();
        }

        private void InitializeUI()
        {
            this.BackColor = UIHelper.Colors.Background;
            UIHelper.StyleButton(btnStartStop, true);
            UIHelper.StyleButton(btnClear);
            UIHelper.StyleButton(btnFormatMessage);
            UIHelper.StyleComboBox(cmbQueue);
            UIHelper.StyleDataGridView(dgvMessages);
            
            txtMessageDetails.Font = new Font("Consolas", 9.5F);
            txtMessageDetails.ReadOnly = true;
        }

        private async Task LoadQueuesAsync()
        {
            try
            {
                _queues = await _rabbitMQService.GetQueuesAsync();
                cmbQueue.Items.Clear();
                
                foreach (var queue in _queues)
                {
                    cmbQueue.Items.Add(queue.Name);
                }
                
                if (cmbQueue.Items.Count > 0)
                {
                    cmbQueue.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Kuyruklar yüklenirken hata: {ex.Message}");
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_isConsuming)
            {
                StopConsuming();
            }
            else
            {
                StartConsuming();
            }
        }

        private void StartConsuming()
        {
            try
            {
                if (cmbQueue.SelectedItem == null)
                {
                    UIHelper.ShowError("Lütfen bir kuyruk seçin!");
                    return;
                }

                var queueName = cmbQueue.SelectedItem.ToString();
                if (string.IsNullOrEmpty(queueName))
                {
                    UIHelper.ShowError("Kuyruk adı boş olamaz!");
                    return;
                }

                var channel = _rabbitMQService.GetChannel();
                if (channel == null)
                {
                    UIHelper.ShowError("RabbitMQ kanalı bulunamadı!");
                    return;
                }

                _consumer = new EventingBasicConsumer(channel);
                _consumer.Received += OnMessageReceived;

                bool autoAck = chkAutoAck.Checked;
                _consumerTag = channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: _consumer);

                _isConsuming = true;
                btnStartStop.Text = "Dinlemeyi Durdur";
                btnStartStop.BackColor = UIHelper.Colors.Error;
                cmbQueue.Enabled = false;
                chkAutoAck.Enabled = false;
                
                lblStatus.Text = $"'{queueName}' kuyruğu dinleniyor...";
                lblStatus.ForeColor = UIHelper.Colors.Success;
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Dinleme başlatılırken hata:\n{ex.Message}");
                lblStatus.Text = "Hata oluştu";
                lblStatus.ForeColor = UIHelper.Colors.Error;
            }
        }

        private void StopConsuming()
        {
            try
            {
                var channel = _rabbitMQService.GetChannel();
                if (channel != null && !string.IsNullOrEmpty(_consumerTag))
                {
                    channel.BasicCancel(_consumerTag);
                }

                if (_consumer != null)
                {
                    _consumer.Received -= OnMessageReceived;
                    _consumer = null;
                }

                _isConsuming = false;
                _consumerTag = null;
                btnStartStop.Text = "Dinlemeyi Başlat";
                btnStartStop.BackColor = UIHelper.Colors.Primary;
                cmbQueue.Enabled = true;
                chkAutoAck.Enabled = true;
                
                lblStatus.Text = "Dinleme durduruldu";
                lblStatus.ForeColor = UIHelper.Colors.TextSecondary;
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Dinleme durdurulurken hata:\n{ex.Message}");
            }
        }

        private void OnMessageReceived(object? sender, BasicDeliverEventArgs e)
        {
            try
            {
                // This runs on a background thread, so we need to invoke to UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => OnMessageReceived(sender, e)));
                    return;
                }

                var body = Encoding.UTF8.GetString(e.Body.ToArray());
                
                var message = new MessageInfo
                {
                    Body = body,
                    ReceivedAt = DateTime.Now,
                    RoutingKey = e.RoutingKey,
                    Exchange = e.Exchange,
                    Queue = cmbQueue.SelectedItem?.ToString() ?? "",
                    Redelivered = e.Redelivered,
                    DeliveryTag = e.DeliveryTag,
                    ContentType = e.BasicProperties.ContentType ?? "text/plain",
                    ContentEncoding = e.BasicProperties.ContentEncoding ?? "UTF-8",
                    DeliveryMode = e.BasicProperties.DeliveryMode,
                    Priority = e.BasicProperties.Priority,
                    CorrelationId = e.BasicProperties.CorrelationId,
                    ReplyTo = e.BasicProperties.ReplyTo,
                    Expiration = e.BasicProperties.Expiration,
                    MessageId = e.BasicProperties.MessageId,
                    Type = e.BasicProperties.Type,
                    UserId = e.BasicProperties.UserId,
                    AppId = e.BasicProperties.AppId
                };

                if (e.BasicProperties.Headers != null)
                {
                    foreach (var header in e.BasicProperties.Headers)
                    {
                        message.Headers[header.Key] = header.Value;
                    }
                }

                _messages.Insert(0, message);
                
                // Limit message history
                if (_messages.Count > 1000)
                {
                    _messages = _messages.Take(1000).ToList();
                }

                RefreshMessageList();
                UpdateStats();

                // Manual acknowledgment if not auto-ack
                if (!chkAutoAck.Checked && _rabbitMQService.GetChannel() is IModel channel)
                {
                    channel.BasicAck(e.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Mesaj işleme hatası: {ex.Message}";
                lblStatus.ForeColor = UIHelper.Colors.Error;
            }
        }

        private void RefreshMessageList()
        {
            var selectedIndex = dgvMessages.SelectedRows.Count > 0 ? dgvMessages.SelectedRows[0].Index : -1;
            
            dgvMessages.DataSource = null;
            dgvMessages.DataSource = _messages.ToList();

            if (dgvMessages.Columns.Count > 0)
            {
                dgvMessages.Columns["MessageId"].Visible = false;
                dgvMessages.Columns["Body"].Visible = false;
                dgvMessages.Columns["Headers"].Visible = false;
                dgvMessages.Columns["CorrelationId"].Visible = false;
                dgvMessages.Columns["ReplyTo"].Visible = false;
                dgvMessages.Columns["Expiration"].Visible = false;
                dgvMessages.Columns["Type"].Visible = false;
                dgvMessages.Columns["UserId"].Visible = false;
                dgvMessages.Columns["AppId"].Visible = false;
                dgvMessages.Columns["ContentEncoding"].Visible = false;
                dgvMessages.Columns["DeliveryTag"].Visible = false;
                
                dgvMessages.Columns["ReceivedAt"].HeaderText = "Alındığı Zaman";
                dgvMessages.Columns["ReceivedAt"].Width = 150;
                dgvMessages.Columns["Exchange"].HeaderText = "Exchange";
                dgvMessages.Columns["Exchange"].Width = 150;
                dgvMessages.Columns["RoutingKey"].HeaderText = "Routing Key";
                dgvMessages.Columns["RoutingKey"].Width = 150;
                dgvMessages.Columns["Queue"].HeaderText = "Queue";
                dgvMessages.Columns["Queue"].Width = 150;
                dgvMessages.Columns["ContentType"].HeaderText = "Content Type";
                dgvMessages.Columns["ContentType"].Width = 120;
                dgvMessages.Columns["DeliveryMode"].HeaderText = "Delivery";
                dgvMessages.Columns["DeliveryMode"].Width = 70;
                dgvMessages.Columns["Priority"].HeaderText = "Priority";
                dgvMessages.Columns["Priority"].Width = 60;
                dgvMessages.Columns["Redelivered"].HeaderText = "Redelivered";
                dgvMessages.Columns["Redelivered"].Width = 90;
            }

            if (selectedIndex >= 0 && selectedIndex < dgvMessages.Rows.Count)
            {
                dgvMessages.Rows[selectedIndex].Selected = true;
            }
        }

        private void UpdateStats()
        {
            lblMessageCount.Text = $"Toplam Mesaj: {_messages.Count}";
        }

        private void dgvMessages_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMessages.SelectedRows.Count > 0)
            {
                var message = dgvMessages.SelectedRows[0].DataBoundItem as MessageInfo;
                if (message != null)
                {
                    ShowMessageDetails(message);
                }
            }
        }

        private void ShowMessageDetails(MessageInfo message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("========== MESAJ BİLGİLERİ ==========");
            sb.AppendLine($"Alındığı Zaman: {message.ReceivedAt:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"Exchange: {message.Exchange}");
            sb.AppendLine($"Routing Key: {message.RoutingKey}");
            sb.AppendLine($"Queue: {message.Queue}");
            sb.AppendLine($"Content Type: {message.ContentType}");
            sb.AppendLine($"Delivery Mode: {(message.DeliveryMode == 2 ? "Persistent" : "Non-Persistent")}");
            sb.AppendLine($"Priority: {message.Priority}");
            sb.AppendLine($"Redelivered: {(message.Redelivered ? "Yes" : "No")}");
            
            if (!string.IsNullOrEmpty(message.MessageId))
                sb.AppendLine($"Message ID: {message.MessageId}");
            if (!string.IsNullOrEmpty(message.CorrelationId))
                sb.AppendLine($"Correlation ID: {message.CorrelationId}");
            if (!string.IsNullOrEmpty(message.ReplyTo))
                sb.AppendLine($"Reply To: {message.ReplyTo}");
            if (!string.IsNullOrEmpty(message.Type))
                sb.AppendLine($"Type: {message.Type}");
            if (!string.IsNullOrEmpty(message.UserId))
                sb.AppendLine($"User ID: {message.UserId}");
            if (!string.IsNullOrEmpty(message.AppId))
                sb.AppendLine($"App ID: {message.AppId}");

            if (message.Headers.Count > 0)
            {
                sb.AppendLine("\n========== HEADERS ==========");
                foreach (var header in message.Headers)
                {
                    sb.AppendLine($"{header.Key}: {header.Value}");
                }
            }

            sb.AppendLine("\n========== MESAJ İÇERİĞİ ==========");
            sb.AppendLine(message.Body);

            txtMessageDetails.Text = sb.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _messages.Clear();
            RefreshMessageList();
            txtMessageDetails.Clear();
            UpdateStats();
        }

        private void btnFormatMessage_Click(object sender, EventArgs e)
        {
            if (dgvMessages.SelectedRows.Count > 0)
            {
                var message = dgvMessages.SelectedRows[0].DataBoundItem as MessageInfo;
                if (message != null)
                {
                    var formatted = MessageFormatter.FormatMessage(message.Body, message.ContentType);
                    
                    var sb = new StringBuilder(txtMessageDetails.Text);
                    var bodyIndex = sb.ToString().IndexOf("========== MESAJ İÇERİĞİ ==========");
                    if (bodyIndex >= 0)
                    {
                        sb.Length = bodyIndex;
                        sb.AppendLine("========== MESAJ İÇERİĞİ (Formatted) ==========");
                        sb.AppendLine(formatted);
                        txtMessageDetails.Text = sb.ToString();
                    }
                }
            }
        }

    }

    partial class MessageConsumerControl
    {
        private System.ComponentModel.IContainer components = null;
        private SplitContainer mainSplitContainer;
        private GroupBox grpSettings;
        private Label lblQueue;
        private ComboBox cmbQueue;
        private CheckBox chkAutoAck;
        private Button btnStartStop;
        private DataGridView dgvMessages;
        private GroupBox grpDetails;
        private TextBox txtMessageDetails;
        private Panel detailsButtonPanel;
        private Button btnFormatMessage;
        private Button btnClear;
        private Panel statusPanel;
        private Label lblStatus;
        private Label lblMessageCount;

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
            this.dgvMessages = new DataGridView();
            this.grpSettings = new GroupBox();
            this.btnStartStop = new Button();
            this.chkAutoAck = new CheckBox();
            this.cmbQueue = new ComboBox();
            this.lblQueue = new Label();
            this.grpDetails = new GroupBox();
            this.txtMessageDetails = new TextBox();
            this.detailsButtonPanel = new Panel();
            this.btnClear = new Button();
            this.btnFormatMessage = new Button();
            this.statusPanel = new Panel();
            this.lblMessageCount = new Label();
            this.lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMessages)).BeginInit();
            this.grpSettings.SuspendLayout();
            this.grpDetails.SuspendLayout();
            this.detailsButtonPanel.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.SuspendLayout();
            
            // mainSplitContainer
            this.mainSplitContainer.Dock = DockStyle.Fill;
            this.mainSplitContainer.Location = new Point(10, 10);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = Orientation.Horizontal;
            this.mainSplitContainer.Panel1.Controls.Add(this.dgvMessages);
            this.mainSplitContainer.Panel1.Controls.Add(this.grpSettings);
            this.mainSplitContainer.Panel2.Controls.Add(this.grpDetails);
            this.mainSplitContainer.Size = new Size(980, 650);
            this.mainSplitContainer.SplitterDistance = 380;
            
            // grpSettings
            this.grpSettings.Controls.Add(this.btnStartStop);
            this.grpSettings.Controls.Add(this.chkAutoAck);
            this.grpSettings.Controls.Add(this.cmbQueue);
            this.grpSettings.Controls.Add(this.lblQueue);
            this.grpSettings.Dock = DockStyle.Top;
            this.grpSettings.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpSettings.Location = new Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Padding = new Padding(10);
            this.grpSettings.Size = new Size(980, 120);
            this.grpSettings.Text = "Consumer Ayarları";
            
            // lblQueue
            this.lblQueue.AutoSize = true;
            this.lblQueue.Font = new Font("Segoe UI", 9F);
            this.lblQueue.Location = new Point(15, 30);
            this.lblQueue.Text = "Kuyruk:";
            
            // cmbQueue
            this.cmbQueue.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbQueue.Location = new Point(15, 50);
            this.cmbQueue.Name = "cmbQueue";
            this.cmbQueue.Size = new Size(400, 25);
            
            // chkAutoAck
            this.chkAutoAck.AutoSize = true;
            this.chkAutoAck.Checked = true;
            this.chkAutoAck.CheckState = CheckState.Checked;
            this.chkAutoAck.Font = new Font("Segoe UI", 9F);
            this.chkAutoAck.Location = new Point(430, 52);
            this.chkAutoAck.Name = "chkAutoAck";
            this.chkAutoAck.Text = "Auto ACK (Otomatik Onay)";
            
            // btnStartStop
            this.btnStartStop.Location = new Point(15, 80);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new Size(180, 30);
            this.btnStartStop.Text = "Dinlemeyi Başlat";
            this.btnStartStop.Click += new EventHandler(this.btnStartStop_Click);
            
            // dgvMessages
            this.dgvMessages.Dock = DockStyle.Fill;
            this.dgvMessages.Location = new Point(0, 120);
            this.dgvMessages.Name = "dgvMessages";
            this.dgvMessages.Size = new Size(980, 260);
            this.dgvMessages.SelectionChanged += new EventHandler(this.dgvMessages_SelectionChanged);
            
            // grpDetails
            this.grpDetails.Controls.Add(this.txtMessageDetails);
            this.grpDetails.Controls.Add(this.detailsButtonPanel);
            this.grpDetails.Dock = DockStyle.Fill;
            this.grpDetails.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpDetails.Location = new Point(0, 0);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Padding = new Padding(10);
            this.grpDetails.Size = new Size(980, 266);
            this.grpDetails.Text = "Mesaj Detayları";
            
            // txtMessageDetails
            this.txtMessageDetails.Dock = DockStyle.Fill;
            this.txtMessageDetails.Location = new Point(10, 26);
            this.txtMessageDetails.Multiline = true;
            this.txtMessageDetails.Name = "txtMessageDetails";
            this.txtMessageDetails.ScrollBars = ScrollBars.Both;
            this.txtMessageDetails.Size = new Size(960, 190);
            
            // detailsButtonPanel
            this.detailsButtonPanel.Controls.Add(this.btnClear);
            this.detailsButtonPanel.Controls.Add(this.btnFormatMessage);
            this.detailsButtonPanel.Dock = DockStyle.Bottom;
            this.detailsButtonPanel.Location = new Point(10, 216);
            this.detailsButtonPanel.Padding = new Padding(0, 5, 0, 0);
            this.detailsButtonPanel.Size = new Size(960, 40);
            
            // btnFormatMessage
            this.btnFormatMessage.Location = new Point(0, 5);
            this.btnFormatMessage.Name = "btnFormatMessage";
            this.btnFormatMessage.Size = new Size(150, 30);
            this.btnFormatMessage.Text = "Format Mesaj";
            this.btnFormatMessage.Click += new EventHandler(this.btnFormatMessage_Click);
            
            // btnClear
            this.btnClear.Location = new Point(160, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(150, 30);
            this.btnClear.Text = "Geçmişi Temizle";
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            
            // statusPanel
            this.statusPanel.Controls.Add(this.lblMessageCount);
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
            
            // lblMessageCount
            this.lblMessageCount.Dock = DockStyle.Right;
            this.lblMessageCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblMessageCount.Location = new Point(680, 0);
            this.lblMessageCount.Padding = new Padding(5);
            this.lblMessageCount.Size = new Size(300, 30);
            this.lblMessageCount.Text = "Toplam Mesaj: 0";
            this.lblMessageCount.TextAlign = ContentAlignment.MiddleRight;
            
            // MessageConsumerControl
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusPanel);
            this.Name = "MessageConsumerControl";
            this.Padding = new Padding(10);
            this.Size = new Size(1000, 700);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMessages)).EndInit();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.detailsButtonPanel.ResumeLayout(false);
            this.statusPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
