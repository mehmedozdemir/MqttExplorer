using RabbitMQExplorer.Helpers;
using RabbitMQExplorer.Models;
using RabbitMQExplorer.Services;
using System;
using System.Windows.Forms;

namespace RabbitMQExplorer.Forms
{
    public partial class ProfileManagementForm : Form
    {
        private readonly ProfileService _profileService;
        private ConnectionProfile? _selectedProfile;

        public ProfileManagementForm(ProfileService profileService)
        {
            InitializeComponent();
            _profileService = profileService;
            InitializeUI();
            LoadProfiles();
        }

        private void InitializeUI()
        {
            this.BackColor = UIHelper.Colors.Background;
            UIHelper.StyleButton(btnNew, true);
            UIHelper.StyleButton(btnEdit);
            UIHelper.StyleButton(btnDelete);
            UIHelper.StyleButton(btnSetDefault);
            UIHelper.StyleButton(btnClose);
            UIHelper.StyleDataGridView(dgvProfiles);
        }

        private void LoadProfiles()
        {
            var profiles = _profileService.GetAllProfiles();
            
            // Eğer profil yoksa, boş bir mesaj göster
            if (profiles.Count == 0)
            {
                dgvProfiles.DataSource = null;
                return;
            }
            
            dgvProfiles.DataSource = null;
            dgvProfiles.DataSource = profiles;
            
            // Sütunları yapılandır
            try
            {
                if (dgvProfiles.Columns.Contains("Id"))
                    dgvProfiles.Columns["Id"].Visible = false;
                
                if (dgvProfiles.Columns.Contains("Password"))
                    dgvProfiles.Columns["Password"].Visible = false;
                
                if (dgvProfiles.Columns.Contains("CreatedAt"))
                    dgvProfiles.Columns["CreatedAt"].Visible = false;
                
                if (dgvProfiles.Columns.Contains("Name"))
                {
                    dgvProfiles.Columns["Name"].HeaderText = "Profil Adı";
                    dgvProfiles.Columns["Name"].Width = 150;
                }
                
                if (dgvProfiles.Columns.Contains("HostName"))
                {
                    dgvProfiles.Columns["HostName"].HeaderText = "Sunucu";
                    dgvProfiles.Columns["HostName"].Width = 150;
                }
                
                if (dgvProfiles.Columns.Contains("Port"))
                {
                    dgvProfiles.Columns["Port"].HeaderText = "Port";
                    dgvProfiles.Columns["Port"].Width = 60;
                }
                
                if (dgvProfiles.Columns.Contains("VirtualHost"))
                {
                    dgvProfiles.Columns["VirtualHost"].HeaderText = "VHost";
                    dgvProfiles.Columns["VirtualHost"].Width = 80;
                }
                
                if (dgvProfiles.Columns.Contains("UserName"))
                {
                    dgvProfiles.Columns["UserName"].HeaderText = "Kullanıcı";
                    dgvProfiles.Columns["UserName"].Width = 100;
                }
                
                if (dgvProfiles.Columns.Contains("UseSsl"))
                {
                    dgvProfiles.Columns["UseSsl"].HeaderText = "SSL";
                    dgvProfiles.Columns["UseSsl"].Width = 50;
                }
                
                if (dgvProfiles.Columns.Contains("ManagementPort"))
                {
                    dgvProfiles.Columns["ManagementPort"].HeaderText = "Mgmt Port";
                    dgvProfiles.Columns["ManagementPort"].Width = 80;
                }
                
                if (dgvProfiles.Columns.Contains("IsDefault"))
                {
                    dgvProfiles.Columns["IsDefault"].HeaderText = "Varsayılan";
                    dgvProfiles.Columns["IsDefault"].Width = 90;
                }
                
                if (dgvProfiles.Columns.Contains("LastConnected"))
                {
                    dgvProfiles.Columns["LastConnected"].HeaderText = "Son Bağlantı";
                    dgvProfiles.Columns["LastConnected"].Width = 150;
                }
            }
            catch
            {
                // Sütun ayarlarında hata olursa görmezden gel
            }
        }

        private void dgvProfiles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProfiles.SelectedRows.Count > 0)
            {
                _selectedProfile = dgvProfiles.SelectedRows[0].DataBoundItem as ConnectionProfile;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnSetDefault.Enabled = !(_selectedProfile?.IsDefault ?? false);
            }
            else
            {
                _selectedProfile = null;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSetDefault.Enabled = false;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var profile = new ConnectionProfile();
            var form = new ProfileEditorForm(profile);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _profileService.SaveProfile(profile);
                LoadProfiles();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedProfile == null) return;

            var form = new ProfileEditorForm(_selectedProfile);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _profileService.SaveProfile(_selectedProfile);
                LoadProfiles();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedProfile == null) return;

            var result = UIHelper.ShowConfirmation(
                $"'{_selectedProfile.Name}' profilini silmek istediğinizden emin misiniz?",
                "Profil Sil");

            if (result == DialogResult.Yes)
            {
                _profileService.DeleteProfile(_selectedProfile.Id);
                LoadProfiles();
            }
        }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            if (_selectedProfile == null) return;

            _profileService.SetDefaultProfile(_selectedProfile.Id);
            LoadProfiles();
            UIHelper.ShowSuccess($"'{_selectedProfile.Name}' varsayılan profil olarak ayarlandı.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    partial class ProfileManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvProfiles;
        private Button btnNew;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSetDefault;
        private Button btnClose;
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
            this.dgvProfiles = new DataGridView();
            this.buttonPanel = new Panel();
            this.btnClose = new Button();
            this.btnSetDefault = new Button();
            this.btnDelete = new Button();
            this.btnEdit = new Button();
            this.btnNew = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfiles)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            
            // dgvProfiles
            this.dgvProfiles.Dock = DockStyle.Fill;
            this.dgvProfiles.Location = new System.Drawing.Point(10, 10);
            this.dgvProfiles.Name = "dgvProfiles";
            this.dgvProfiles.Size = new System.Drawing.Size(964, 440);
            this.dgvProfiles.TabIndex = 0;
            this.dgvProfiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.dgvProfiles.SelectionChanged += new EventHandler(this.dgvProfiles_SelectionChanged);
            
            // buttonPanel
            this.buttonPanel.Controls.Add(this.btnClose);
            this.buttonPanel.Controls.Add(this.btnSetDefault);
            this.buttonPanel.Controls.Add(this.btnDelete);
            this.buttonPanel.Controls.Add(this.btnEdit);
            this.buttonPanel.Controls.Add(this.btnNew);
            this.buttonPanel.Dock = DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(10, 450);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Padding = new Padding(0, 10, 0, 0);
            this.buttonPanel.Size = new System.Drawing.Size(964, 60);
            this.buttonPanel.TabIndex = 1;
            
            // btnNew
            this.btnNew.Location = new System.Drawing.Point(0, 10);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(120, 40);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Yeni Profil";
            this.btnNew.Click += new EventHandler(this.btnNew_Click);
            
            // btnEdit
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(130, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(120, 40);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Düzenle";
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            
            // btnDelete
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(260, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Sil";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            
            // btnSetDefault
            this.btnSetDefault.Enabled = false;
            this.btnSetDefault.Location = new System.Drawing.Point(390, 10);
            this.btnSetDefault.Name = "btnSetDefault";
            this.btnSetDefault.Size = new System.Drawing.Size(150, 40);
            this.btnSetDefault.TabIndex = 3;
            this.btnSetDefault.Text = "Varsayılan Yap";
            this.btnSetDefault.Click += new EventHandler(this.btnSetDefault_Click);
            
            // btnClose
            this.btnClose.Location = new System.Drawing.Point(844, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Kapat";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            
            // ProfileManagementForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 520);
            this.Controls.Add(this.dgvProfiles);
            this.Controls.Add(this.buttonPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ProfileManagementForm";
            this.Padding = new Padding(10);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Bağlantı Profilleri Yönetimi";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfiles)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
