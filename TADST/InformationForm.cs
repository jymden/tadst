using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;

namespace TADST
{
    public partial class InformationForm : Form
    {
        public InformationForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=N6ZR3G33RDHJY");
            Process.Start(sInfo);    
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://www.facebook.com/meadowseverbleeding");
            Process.Start(sInfo);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void InformationForm_Shown(object sender, EventArgs e)
        {
            SystemSounds.Exclamation.Play();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            var sInfo = new ProcessStartInfo("http://forums.bistudio.com/showthread.php?104991");
            Process.Start(sInfo);
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }


    }
}
