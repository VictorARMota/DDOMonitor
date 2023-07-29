using DDOMonitor.Monitors;
using System;
using System.Windows.Forms;

namespace DDOMonitor
{
    public partial class DDOMonitorForm : Form
    {
        private GroupMonitor groupMonitor;
        private ServerMonitor serverMonitor;
        private PlayerMonitor playerMonitor;
        private static readonly ToastOperator toastOperator = new ToastOperator();

        public DDOMonitorForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                outputLabel.Text = e.Message;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                string server = serverComboBox.SelectedItem.ToString();
                if (String.IsNullOrEmpty(server))
                    throw new Exception("No selected server.");
                server = server.Trim().ToLower();

                string characterName = charTextBox.Text;
                if (String.IsNullOrEmpty(characterName))
                    throw new Exception("Please input character name.");

                // This will Start each monitor at a time.
                // Each monitor task is offset by 15 seconds, as requested by DDOAudit, to avoid overloading.

                serverMonitor = new ServerMonitor(server, toastOperator);

                playerMonitor = new PlayerMonitor(server, characterName, serverMonitor);

                groupMonitor = new GroupMonitor(server, serverMonitor, playerMonitor, toastOperator);

                outputLabel.Text = "Monitor Started.";
            }
            catch (Exception ex)
            {
                outputLabel.Text = ex.Message;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (groupMonitor != null)
                    groupMonitor.Stop();

                if (playerMonitor != null)
                    playerMonitor.Stop();

                if (serverMonitor != null)
                    serverMonitor.Stop();

                outputLabel.Text = "Monitor Stopped.";
            }
            catch (Exception ex)
            {
                outputLabel.Text = ex.Message;
            }
        }

        private void ConfigButton_Click(object sender, EventArgs e)
        {
            ConfigurationForm configurationForm = new ConfigurationForm();
            configurationForm.ShowDialog();
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.ddoaudit.com/");
        }
    }
}