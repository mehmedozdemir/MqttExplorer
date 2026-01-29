namespace RabbitMQExplorer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnManageProfiles = new System.Windows.Forms.Button();
            this.cmbProfiles = new System.Windows.Forms.ComboBox();
            this.lblProfile = new System.Windows.Forms.Label();
            this.navigationPanel = new System.Windows.Forms.Panel();
            this.btnConsumer = new System.Windows.Forms.Button();
            this.btnPublisher = new System.Windows.Forms.Button();
            this.btnBindings = new System.Windows.Forms.Button();
            this.btnExchanges = new System.Windows.Forms.Button();
            this.btnQueues = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.connectionGroupBox.SuspendLayout();
            this.navigationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.mainStatusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.mainSplitContainer);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1400, 735);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(1400, 785);
            this.toolStripContainer.TabIndex = 0;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.mainToolStrip);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.connectionStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 0);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(1400, 22);
            this.mainStatusStrip.TabIndex = 0;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Hazır";
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(1346, 17);
            this.connectionStatusLabel.Spring = true;
            this.connectionStatusLabel.Text = "Bağlantı Yok";
            this.connectionStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.leftPanel);
            this.mainSplitContainer.Panel1MinSize = 250;
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.mainTabControl);
            this.mainSplitContainer.Size = new System.Drawing.Size(1400, 735);
            this.mainSplitContainer.SplitterDistance = 280;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.navigationPanel);
            this.leftPanel.Controls.Add(this.connectionGroupBox);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Padding = new System.Windows.Forms.Padding(10);
            this.leftPanel.Size = new System.Drawing.Size(280, 735);
            this.leftPanel.TabIndex = 0;
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Controls.Add(this.btnDisconnect);
            this.connectionGroupBox.Controls.Add(this.btnConnect);
            this.connectionGroupBox.Controls.Add(this.btnManageProfiles);
            this.connectionGroupBox.Controls.Add(this.cmbProfiles);
            this.connectionGroupBox.Controls.Add(this.lblProfile);
            this.connectionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.connectionGroupBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.connectionGroupBox.Location = new System.Drawing.Point(10, 10);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.connectionGroupBox.Size = new System.Drawing.Size(260, 180);
            this.connectionGroupBox.TabIndex = 0;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Bağlantı";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(135, 135);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(115, 35);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "Bağlantıyı Kes";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(10, 135);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(115, 35);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Bağlan";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnManageProfiles
            // 
            this.btnManageProfiles.Location = new System.Drawing.Point(10, 90);
            this.btnManageProfiles.Name = "btnManageProfiles";
            this.btnManageProfiles.Size = new System.Drawing.Size(240, 35);
            this.btnManageProfiles.TabIndex = 2;
            this.btnManageProfiles.Text = "Profilleri Yönet...";
            this.btnManageProfiles.UseVisualStyleBackColor = true;
            this.btnManageProfiles.Click += new System.EventHandler(this.btnManageProfiles_Click);
            // 
            // cmbProfiles
            // 
            this.cmbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProfiles.FormattingEnabled = true;
            this.cmbProfiles.Location = new System.Drawing.Point(10, 52);
            this.cmbProfiles.Name = "cmbProfiles";
            this.cmbProfiles.Size = new System.Drawing.Size(240, 25);
            this.cmbProfiles.TabIndex = 1;
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblProfile.Location = new System.Drawing.Point(10, 30);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(87, 15);
            this.lblProfile.TabIndex = 0;
            this.lblProfile.Text = "Bağlantı Profili:";
            // 
            // navigationPanel
            // 
            this.navigationPanel.Controls.Add(this.btnConsumer);
            this.navigationPanel.Controls.Add(this.btnPublisher);
            this.navigationPanel.Controls.Add(this.btnBindings);
            this.navigationPanel.Controls.Add(this.btnExchanges);
            this.navigationPanel.Controls.Add(this.btnQueues);
            this.navigationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanel.Location = new System.Drawing.Point(10, 190);
            this.navigationPanel.Name = "navigationPanel";
            this.navigationPanel.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.navigationPanel.Size = new System.Drawing.Size(260, 535);
            this.navigationPanel.TabIndex = 1;
            // 
            // btnConsumer
            // 
            this.btnConsumer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConsumer.Enabled = false;
            this.btnConsumer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnConsumer.Location = new System.Drawing.Point(0, 180);
            this.btnConsumer.Name = "btnConsumer";
            this.btnConsumer.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnConsumer.Size = new System.Drawing.Size(260, 40);
            this.btnConsumer.TabIndex = 4;
            this.btnConsumer.Text = "📨 Mesaj Dinle (Consumer)";
            this.btnConsumer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConsumer.UseVisualStyleBackColor = true;
            this.btnConsumer.Click += new System.EventHandler(this.btnConsumer_Click);
            // 
            // btnPublisher
            // 
            this.btnPublisher.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPublisher.Enabled = false;
            this.btnPublisher.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPublisher.Location = new System.Drawing.Point(0, 140);
            this.btnPublisher.Name = "btnPublisher";
            this.btnPublisher.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPublisher.Size = new System.Drawing.Size(260, 40);
            this.btnPublisher.TabIndex = 3;
            this.btnPublisher.Text = "📤 Mesaj Gönder (Publisher)";
            this.btnPublisher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPublisher.UseVisualStyleBackColor = true;
            this.btnPublisher.Click += new System.EventHandler(this.btnPublisher_Click);
            // 
            // btnBindings
            // 
            this.btnBindings.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBindings.Enabled = false;
            this.btnBindings.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBindings.Location = new System.Drawing.Point(0, 100);
            this.btnBindings.Name = "btnBindings";
            this.btnBindings.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnBindings.Size = new System.Drawing.Size(260, 40);
            this.btnBindings.TabIndex = 2;
            this.btnBindings.Text = "🔗 Binding\'ler";
            this.btnBindings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBindings.UseVisualStyleBackColor = true;
            this.btnBindings.Click += new System.EventHandler(this.btnBindings_Click);
            // 
            // btnExchanges
            // 
            this.btnExchanges.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnExchanges.Enabled = false;
            this.btnExchanges.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnExchanges.Location = new System.Drawing.Point(0, 60);
            this.btnExchanges.Name = "btnExchanges";
            this.btnExchanges.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnExchanges.Size = new System.Drawing.Size(260, 40);
            this.btnExchanges.TabIndex = 1;
            this.btnExchanges.Text = "📮 Exchange\'ler";
            this.btnExchanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExchanges.UseVisualStyleBackColor = true;
            this.btnExchanges.Click += new System.EventHandler(this.btnExchanges_Click);
            // 
            // btnQueues
            // 
            this.btnQueues.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnQueues.Enabled = false;
            this.btnQueues.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnQueues.Location = new System.Drawing.Point(0, 20);
            this.btnQueues.Name = "btnQueues";
            this.btnQueues.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnQueues.Size = new System.Drawing.Size(260, 40);
            this.btnQueues.TabIndex = 0;
            this.btnQueues.Text = "📦 Kuyruklar (Queues)";
            this.btnQueues.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQueues.UseVisualStyleBackColor = true;
            this.btnQueues.Click += new System.EventHandler(this.btnQueues_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1116, 735);
            this.mainTabControl.TabIndex = 0;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Location = new System.Drawing.Point(3, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(1397, 25);
            this.mainToolStrip.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 785);
            this.Controls.Add(this.toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RabbitMQ Explorer";
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            this.navigationPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatusLabel;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.ComboBox cmbProfiles;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Button btnManageProfiles;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Panel navigationPanel;
        private System.Windows.Forms.Button btnQueues;
        private System.Windows.Forms.Button btnExchanges;
        private System.Windows.Forms.Button btnBindings;
        private System.Windows.Forms.Button btnPublisher;
        private System.Windows.Forms.Button btnConsumer;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.ToolStrip mainToolStrip;
    }
}
