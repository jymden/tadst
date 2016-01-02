using System;
using System.IO;
using System.Windows.Forms;

namespace TADST
{
    public partial class MonitorForm : Form
    {
        private readonly string _watchFile;
        private long _lastFileSize;      
        public bool AutoScrollToBottom { get; set; }
        public bool AutoUpdate { get; set; }
        public bool AlwaysOnTop { get; set; } 
        public Timer MonitorTimer { get; set; }

        public MonitorForm(string watchFile)
        {
            InitializeComponent();
            
            _watchFile = watchFile;
            _lastFileSize = 0;

            AutoUpdate = true;
            AutoScrollToBottom = true;
            AlwaysOnTop = true;

        }
    
        private void SetTimer()
        {
            MonitorTimer = new Timer {Interval = 3000, Enabled = true};
            MonitorTimer.Tick += (TimerEvent);
            MonitorTimer.Start();
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            if (AutoUpdate)
            {
                ReadFile();
            }
        }

        /* 
         private readonly FileSystemWatcher _watcher = new FileSystemWatcher(); 
         
         private void SetWatcher()
         {
             var path = _watchFile;
             int index = path.LastIndexOf("\\", StringComparison.Ordinal);
             if (index > 0)
                 path = path.Substring(0, index);

             var file = _watchFile;
             index++;
             file = file.Substring(index, file.Length - index);

             _watcher.Path = path;
             _watcher.Filter = file;
             _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
             _watcher.Changed += (OnChanged);
             _watcher.EnableRaisingEvents = true;
             _watcher.IncludeSubdirectories = false;
         }

         private void OnChanged(object sender, FileSystemEventArgs e)
         {

                 ReadFile();
            
         }*/

        /// <summary>
        /// Read file and update textbox if the file has been updated.
        /// </summary>
        private void ReadFile()
        {
            MonitorTimer.Stop();

            if (!File.Exists(_watchFile)) return;

            var fileText = GetFileText();
            if (fileText.Length > _lastFileSize)
            {
                _lastFileSize = fileText.Length;

                txtMonitorOutput.Text = fileText;
                Text = "TADST Monitor (" + _watchFile + ") - Updated " + DateTime.Now.ToLongTimeString();

                if (AutoScrollToBottom)
                {
                    ScrollDown();
                }
            }

            MonitorTimer.Start();
        }

        /// <summary>
        /// Get text from file
        /// </summary>
        /// <returns>String with the text from the monitored file</returns>
        private string GetFileText()
        {
            var fileStream = new FileStream(_watchFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var fileReader = new StreamReader(fileStream);

            var fileText = fileReader.ReadToEnd();

            fileReader.Close();
            fileStream.Close();
            return fileText.ToString();
        }

        /// <summary>
        /// Scroll to bottom of textbox
        /// </summary>
        private void ScrollDown()
        {
            var selectionStart = txtMonitorOutput.TextLength - 1;
            txtMonitorOutput.SelectionStart = selectionStart > -1 ? selectionStart : 0;

            txtMonitorOutput.ScrollToCaret();
            txtMonitorOutput.Refresh();
        }

        private void chkWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            txtMonitorOutput.WordWrap = chkWordWrap.Checked;
        }

        private void chkAutoscroll_CheckedChanged(object sender, EventArgs e)
        {
            AutoScrollToBottom = chkAutoscroll.Checked;
        }

        private void chkAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            AutoUpdate = chkAutoUpdate.Checked;
            //_watcher.EnableRaisingEvents = AutoUpdate;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReadFile();
            if (AutoScrollToBottom)
            {
                ScrollDown();
            }
        }

        private void MonitorForm_Shown(object sender, EventArgs e)
        {
            SetTimer();  
            ReadFile();
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = chkTopMost.Checked;
        }

        private void MonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MonitorTimer.Stop();
            MonitorTimer.Enabled = false;
            MonitorTimer.Dispose();         
        }

    }
}
