using System;
using System.IO;
using System.Windows.Forms;

namespace AdminHunter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath+"dic.txt"))
            {
                MessageBox.Show("Success Load Dictionary . . .", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (MessageBox.Show("Can't Load Dictionary . . .", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
            )
            {
                case DialogResult.Retry:
                    MainForm_Load(null,null);
                    break;
                case DialogResult.Cancel:
                    Application.Exit();
                    break;
            }
        }

        private void btnaout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void btnhuntering_Click(object sender, EventArgs e)
        {
            HuntForm huntForm = new HuntForm();
            huntForm.Show(this);
        }
    }
}
