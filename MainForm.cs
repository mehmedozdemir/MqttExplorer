using RabbitMQExplorer.Forms;
using RabbitMQExplorer.Helpers;
using RabbitMQExplorer.Models;
using RabbitMQExplorer.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitMQExplorer
{
    public partial class MainForm : Form
    {
        private readonly RabbitMQService _rabbitMQService;
        private readonly ProfileService _profileService;

        public MainForm()
        {
            InitializeComponent();
            _rabbitMQService = new RabbitMQService();
            _profileService = new ProfileService();

            InitializeUI();
            LoadProfiles();
        }

        private void InitializeUI()
        {
            // Apply modern styling
            this.BackColor = UIHelper.Colors.Background;
            leftPanel.BackColor = UIHelper.Colors.Surface;
            
            UIHelper.StyleButton(btnConnect, true);
            UIHelper.StyleButton(btnDisconnect);
            UIHelper.StyleButton(btnManageProfiles);
            UIHelper.StyleButton(btnQueues);
            UIHelper.StyleButton(btnExchanges);
            UIHelper.StyleButton(btnBindings);
            UIHelper.StyleButton(btnPublisher);
            UIHelper.StyleButton(btnConsumer);
            
            UIHelper.StyleComboBox(cmbProfiles);
        }

        private void LoadProfiles()
        {
            var profiles = _profileService.GetAllProfiles();
            cmbProfiles.Items.Clear();
            
            foreach (var profile in profiles)
            {
                cmbProfiles.Items.Add(profile);
            }

            cmbProfiles.DisplayMember = "Name";

            var defaultProfile = _profileService.GetDefaultProfile();
            if (defaultProfile != null)
            {
                cmbProfiles.SelectedItem = defaultProfile;
            }
            else if (cmbProfiles.Items.Count > 0)
            {
                cmbProfiles.SelectedIndex = 0;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (cmbProfiles.SelectedItem == null)
            {
                UIHelper.ShowError("Lütfen bir bağlantı profili seçin!");
                return;
            }

            var profile = (ConnectionProfile)cmbProfiles.SelectedItem;
            
            try
            {
                UIHelper.ShowLoading(this);
                statusLabel.Text = "Bağlanıyor...";
                
                await _rabbitMQService.ConnectAsync(profile);
                
                connectionStatusLabel.Text = $"Bağlı: {profile.Name} ({profile.HostName}:{profile.Port})";
                connectionStatusLabel.ForeColor = UIHelper.Colors.Success;
                statusLabel.Text = "Bağlantı başarılı";
                
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                cmbProfiles.Enabled = false;
                
                EnableNavigationButtons(true);
                
                _profileService.SaveProfile(profile); // Update last connected
                
                UIHelper.ShowSuccess($"{profile.Name} bağlantısı başarılı!");
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Bağlantı hatası:\n{ex.Message}");
                statusLabel.Text = "Bağlantı başarısız";
            }
            finally
            {
                UIHelper.HideLoading(this);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _rabbitMQService.Disconnect();
                
                connectionStatusLabel.Text = "Bağlantı Yok";
                connectionStatusLabel.ForeColor = UIHelper.Colors.TextSecondary;
                statusLabel.Text = "Bağlantı kesildi";
                
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                cmbProfiles.Enabled = true;
                
                EnableNavigationButtons(false);
                
                // Close all tabs
                mainTabControl.TabPages.Clear();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Bağlantı kesme hatası:\n{ex.Message}");
            }
        }

        private void EnableNavigationButtons(bool enabled)
        {
            btnQueues.Enabled = enabled;
            btnExchanges.Enabled = enabled;
            btnBindings.Enabled = enabled;
            btnPublisher.Enabled = enabled;
            btnConsumer.Enabled = enabled;
        }

        private void btnManageProfiles_Click(object sender, EventArgs e)
        {
            var form = new ProfileManagementForm(_profileService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProfiles();
            }
        }

        private void btnQueues_Click(object sender, EventArgs e)
        {
            OpenOrFocusTab("Kuyruklar", () => new QueueBrowserControl(_rabbitMQService));
        }

        private void btnExchanges_Click(object sender, EventArgs e)
        {
            OpenOrFocusTab("Exchange'ler", () => new ExchangeBrowserControl(_rabbitMQService));
        }

        private void btnBindings_Click(object sender, EventArgs e)
        {
            OpenOrFocusTab("Binding'ler", () => new BindingBrowserControl(_rabbitMQService));
        }

        private void btnPublisher_Click(object sender, EventArgs e)
        {
            OpenOrFocusTab("Mesaj Gönder", () => new MessagePublisherControl(_rabbitMQService));
        }

        private void btnConsumer_Click(object sender, EventArgs e)
        {
            var tabName = $"Mesaj Dinle {mainTabControl.TabPages.Count + 1}";
            OpenOrFocusTab(tabName, () => new MessageConsumerControl(_rabbitMQService), allowMultiple: true);
        }

        private void OpenOrFocusTab(string tabName, Func<UserControl> controlFactory, bool allowMultiple = false)
        {
            if (!allowMultiple)
            {
                // Check if tab already exists
                foreach (TabPage tab in mainTabControl.TabPages)
                {
                    if (tab.Text == tabName)
                    {
                        mainTabControl.SelectedTab = tab;
                        return;
                    }
                }
            }

            // Create new tab
            var tabPage = new TabPage(tabName);
            var control = controlFactory();
            control.Dock = DockStyle.Fill;
            tabPage.Controls.Add(control);
            mainTabControl.TabPages.Add(tabPage);
            mainTabControl.SelectedTab = tabPage;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _rabbitMQService?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
