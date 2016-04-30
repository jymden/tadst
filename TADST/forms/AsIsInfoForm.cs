using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;

namespace TADST
{
    public partial class AsIsInfoForm : Form
    {
        /// <summary>
        /// Profile folder, for the 'open folder' button
        /// </summary>
        private readonly string _folder;

        public AsIsInfoForm(string folder)
        {
            _folder = folder;
            InitializeComponent();
        }

        private void BtnOpenFolderClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(_folder);
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void AsIsInfoShown(object sender, EventArgs e)
        {
            SystemSounds.Asterisk.Play();
            var myIcon = new System.Drawing.Icon(System.Drawing.SystemIcons.Information, 32, 32);
            lblIcon.Image = myIcon.ToBitmap();
        }
    }
}
