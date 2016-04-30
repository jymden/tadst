using System;
using System.Windows.Forms;

namespace TADST
{
    public partial class NewProfileForm : Form
    {
        public string ProfileName { get; set; }

        public NewProfileForm()
        {
            InitializeComponent();
            ProfileName = "";
            btnOk.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ProfileName = txtProfileName.Text;
        }

        /// <summary>
        /// Check if profile name is of valid format
        /// </summary>
        /// <returns>True if valid</returns>
        private bool nameIsValid()
        {
            return (txtProfileName.Text.Trim().Length > 0);
        }

        /// <summary>
        /// Enable ok button if a valid nae is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProfileName_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = nameIsValid();
        }

        /// <summary>
        /// Make sure no illegal characters are put in profile name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProfileName_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == '/' ||
                            e.KeyChar == '\u005C' ||
                            e.KeyChar == ':' ||
                            e.KeyChar == '?' ||
                            e.KeyChar == '*' ||
                            e.KeyChar == '"' ||
                            e.KeyChar == ' ')
                e.Handled = true;
        }

        private void NewProfileForm_Load(object sender, EventArgs e)
        {

        }




    }
}
