using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DDOMonitor
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
            LoadSavedConfig();
        }

        private void LoadSavedConfig()
        {
            Dictionary<string, bool> config = Utils.LoadConfig();
            foreach (Control c in Controls)
            {
                if (c is CheckBox)
                {
                    if (config.TryGetValue(c.Name, out bool active))
                        ((CheckBox)c).Checked = active;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, bool> config = new Dictionary<string, bool>();
            foreach (Control c in Controls)
            {
                if (c is CheckBox)
                {
                    config.Add(c.Name, c.Enabled);
                }
            }

            Utils.SaveConfig(config);
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
