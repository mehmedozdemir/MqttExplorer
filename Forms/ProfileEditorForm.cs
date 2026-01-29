using RabbitMQExplorer.Helpers;
using RabbitMQExplorer.Models;
using System;
using System.Windows.Forms;

namespace RabbitMQExplorer.Forms
{
    public partial class ProfileEditorForm : Form
    {
        private readonly ConnectionProfile _profile;

        public ProfileEditorForm(ConnectionProfile profile)
        {
            InitializeComponent();
            _profile = profile;
            InitializeUI();
            LoadProfile();
        }

        private void InitializeUI()
        {
            this.BackColor = UIHelper.Colors.Background;
            UIHelper.StyleButton(btnSave, true);
            UIHelper.StyleButton(btnCancel);
            UIHelper.StyleTextBox(txtName);
            UIHelper.StyleTextBox(txtHostName);
            UIHelper.StyleTextBox(txtPort);
            UIHelper.StyleTextBox(txtVirtualHost);
            UIHelper.StyleTextBox(txtUserName);
            UIHelper.StyleTextBox(txtPassword);
            UIHelper.StyleTextBox(txtManagementPort);
        }

        private void LoadProfile()
        {
            txtName.Text = _profile.Name;
            txtHostName.Text = _profile.HostName;
            txtPort.Text = _profile.Port.ToString();
            txtVirtualHost.Text = _profile.VirtualHost;
            txtUserName.Text = _profile.UserName;
            txtPassword.Text = _profile.Password;
            chkUseSsl.Checked = _profile.UseSsl;
            txtManagementPort.Text = _profile.ManagementPort.ToString();
            chkIsDefault.Checked = _profile.IsDefault;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                UIHelper.ShowError("Profil adı boş olamaz!");
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHostName.Text))
            {
                UIHelper.ShowError("Sunucu adresi boş olamaz!");
                txtHostName.Focus();
                return;
            }

            if (!int.TryParse(txtPort.Text, out int port) || port <= 0 || port > 65535)
            {
                UIHelper.ShowError("Geçerli bir port numarası girin! (1-65535)");
                txtPort.Focus();
                return;
            }

            if (!int.TryParse(txtManagementPort.Text, out int mgmtPort) || mgmtPort <= 0 || mgmtPort > 65535)
            {
                UIHelper.ShowError("Geçerli bir management port numarası girin! (1-65535)");
                txtManagementPort.Focus();
                return;
            }

            // Save profile
            _profile.Name = txtName.Text.Trim();
            _profile.HostName = txtHostName.Text.Trim();
            _profile.Port = port;
            _profile.VirtualHost = txtVirtualHost.Text.Trim();
            _profile.UserName = txtUserName.Text.Trim();
            _profile.Password = txtPassword.Text;
            _profile.UseSsl = chkUseSsl.Checked;
            _profile.ManagementPort = mgmtPort;
            _profile.IsDefault = chkIsDefault.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    partial class ProfileEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblName;
        private TextBox txtName;
        private Label lblHostName;
        private TextBox txtHostName;
        private Label lblPort;
        private TextBox txtPort;
        private Label lblVirtualHost;
        private TextBox txtVirtualHost;
        private Label lblUserName;
        private TextBox txtUserName;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkUseSsl;
        private Label lblManagementPort;
        private TextBox txtManagementPort;
        private CheckBox chkIsDefault;
        private Button btnSave;
        private Button btnCancel;
        private Panel buttonPanel;

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
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblHostName = new Label();
            this.txtHostName = new TextBox();
            this.lblPort = new Label();
            this.txtPort = new TextBox();
            this.lblVirtualHost = new Label();
            this.txtVirtualHost = new TextBox();
            this.lblUserName = new Label();
            this.txtUserName = new TextBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.chkUseSsl = new CheckBox();
            this.lblManagementPort = new Label();
            this.txtManagementPort = new TextBox();
            this.chkIsDefault = new CheckBox();
            this.buttonPanel = new Panel();
            this.btnCancel = new Button();
            this.btnSave = new Button();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            
            int y = 20;
            int labelHeight = 20;
            int textBoxHeight = 25;
            int spacing = 35;
            
            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblName.Location = new System.Drawing.Point(20, y);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, labelHeight);
            this.lblName.Text = "Profil Adı:";
            
            // txtName
            this.txtName.Location = new System.Drawing.Point(180, y);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(300, textBoxHeight);
            y += spacing;
            
            // lblHostName
            this.lblHostName.AutoSize = true;
            this.lblHostName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHostName.Location = new System.Drawing.Point(20, y);
            this.lblHostName.Name = "lblHostName";
            this.lblHostName.Text = "Sunucu (Host):";
            
            // txtHostName
            this.txtHostName.Location = new System.Drawing.Point(180, y);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(300, textBoxHeight);
            y += spacing;
            
            // lblPort
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPort.Location = new System.Drawing.Point(20, y);
            this.lblPort.Name = "lblPort";
            this.lblPort.Text = "Port:";
            
            // txtPort
            this.txtPort.Location = new System.Drawing.Point(180, y);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, textBoxHeight);
            y += spacing;
            
            // lblVirtualHost
            this.lblVirtualHost.AutoSize = true;
            this.lblVirtualHost.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVirtualHost.Location = new System.Drawing.Point(20, y);
            this.lblVirtualHost.Name = "lblVirtualHost";
            this.lblVirtualHost.Text = "Virtual Host:";
            
            // txtVirtualHost
            this.txtVirtualHost.Location = new System.Drawing.Point(180, y);
            this.txtVirtualHost.Name = "txtVirtualHost";
            this.txtVirtualHost.Size = new System.Drawing.Size(300, textBoxHeight);
            y += spacing;
            
            // lblUserName
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUserName.Location = new System.Drawing.Point(20, y);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Text = "Kullanıcı Adı:";
            
            // txtUserName
            this.txtUserName.Location = new System.Drawing.Point(180, y);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, textBoxHeight);
            y += spacing;
            
            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPassword.Location = new System.Drawing.Point(20, y);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Text = "Şifre:";
            
            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(180, y);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(200, textBoxHeight);
            y += spacing;
            
            // chkUseSsl
            this.chkUseSsl.AutoSize = true;
            this.chkUseSsl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkUseSsl.Location = new System.Drawing.Point(180, y);
            this.chkUseSsl.Name = "chkUseSsl";
            this.chkUseSsl.Text = "SSL/TLS Kullan";
            y += spacing;
            
            // lblManagementPort
            this.lblManagementPort.AutoSize = true;
            this.lblManagementPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblManagementPort.Location = new System.Drawing.Point(20, y);
            this.lblManagementPort.Name = "lblManagementPort";
            this.lblManagementPort.Text = "Management Port:";
            
            // txtManagementPort
            this.txtManagementPort.Location = new System.Drawing.Point(180, y);
            this.txtManagementPort.Name = "txtManagementPort";
            this.txtManagementPort.Size = new System.Drawing.Size(100, textBoxHeight);
            y += spacing;
            
            // chkIsDefault
            this.chkIsDefault.AutoSize = true;
            this.chkIsDefault.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkIsDefault.Location = new System.Drawing.Point(180, y);
            this.chkIsDefault.Name = "chkIsDefault";
            this.chkIsDefault.Text = "Varsayılan profil olarak ayarla";
            y += spacing + 10;
            
            // buttonPanel
            this.buttonPanel.Controls.Add(this.btnCancel);
            this.buttonPanel.Controls.Add(this.btnSave);
            this.buttonPanel.Dock = DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, y);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Padding = new Padding(20, 10, 20, 20);
            this.buttonPanel.Size = new System.Drawing.Size(500, 70);
            
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(240, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.Text = "Kaydet";
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(370, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.Text = "İptal";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            
            // ProfileEditorForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, y + 70);
            this.Controls.Add(this.chkIsDefault);
            this.Controls.Add(this.txtManagementPort);
            this.Controls.Add(this.lblManagementPort);
            this.Controls.Add(this.chkUseSsl);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtVirtualHost);
            this.Controls.Add(this.lblVirtualHost);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtHostName);
            this.Controls.Add(this.lblHostName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.buttonPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileEditorForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Profil Düzenle";
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
