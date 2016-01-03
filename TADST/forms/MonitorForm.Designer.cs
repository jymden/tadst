namespace TADST
{
    partial class MonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMonitorOutput = new System.Windows.Forms.RichTextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkAutoUpdate = new System.Windows.Forms.CheckBox();
            this.chkAutoscroll = new System.Windows.Forms.CheckBox();
            this.chkWordWrap = new System.Windows.Forms.CheckBox();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtMonitorOutput
            // 
            this.txtMonitorOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMonitorOutput.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtMonitorOutput.Location = new System.Drawing.Point(12, 41);
            this.txtMonitorOutput.Name = "txtMonitorOutput";
            this.txtMonitorOutput.ReadOnly = true;
            this.txtMonitorOutput.Size = new System.Drawing.Size(490, 370);
            this.txtMonitorOutput.TabIndex = 1;
            this.txtMonitorOutput.Text = "";
            this.txtMonitorOutput.WordWrap = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(437, 417);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(65, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkAutoUpdate
            // 
            this.chkAutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoUpdate.AutoSize = true;
            this.chkAutoUpdate.Checked = true;
            this.chkAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoUpdate.Location = new System.Drawing.Point(345, 421);
            this.chkAutoUpdate.Name = "chkAutoUpdate";
            this.chkAutoUpdate.Size = new System.Drawing.Size(86, 17);
            this.chkAutoUpdate.TabIndex = 9;
            this.chkAutoUpdate.Text = "Auto Update";
            this.chkAutoUpdate.UseVisualStyleBackColor = true;
            this.chkAutoUpdate.CheckedChanged += new System.EventHandler(this.chkAutoUpdate_CheckedChanged);
            // 
            // chkAutoscroll
            // 
            this.chkAutoscroll.AutoSize = true;
            this.chkAutoscroll.Checked = true;
            this.chkAutoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoscroll.Location = new System.Drawing.Point(99, 16);
            this.chkAutoscroll.Name = "chkAutoscroll";
            this.chkAutoscroll.Size = new System.Drawing.Size(72, 17);
            this.chkAutoscroll.TabIndex = 8;
            this.chkAutoscroll.Text = "Autoscroll";
            this.chkAutoscroll.UseVisualStyleBackColor = true;
            this.chkAutoscroll.CheckedChanged += new System.EventHandler(this.chkAutoscroll_CheckedChanged);
            // 
            // chkWordWrap
            // 
            this.chkWordWrap.AutoSize = true;
            this.chkWordWrap.Location = new System.Drawing.Point(12, 16);
            this.chkWordWrap.Name = "chkWordWrap";
            this.chkWordWrap.Size = new System.Drawing.Size(81, 17);
            this.chkWordWrap.TabIndex = 7;
            this.chkWordWrap.Text = "Word Wrap";
            this.chkWordWrap.UseVisualStyleBackColor = true;
            this.chkWordWrap.CheckedChanged += new System.EventHandler(this.chkWordWrap_CheckedChanged);
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Checked = true;
            this.chkTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTopMost.Location = new System.Drawing.Point(177, 16);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(92, 17);
            this.chkTopMost.TabIndex = 10;
            this.chkTopMost.Text = "Always on top";
            this.chkTopMost.UseVisualStyleBackColor = true;
            this.chkTopMost.CheckedChanged += new System.EventHandler(this.chkTopMost_CheckedChanged);
            // 
            // MonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 452);
            this.Controls.Add(this.chkTopMost);
            this.Controls.Add(this.chkAutoUpdate);
            this.Controls.Add(this.chkAutoscroll);
            this.Controls.Add(this.chkWordWrap);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtMonitorOutput);
            this.MinimumSize = new System.Drawing.Size(286, 140);
            this.Name = "MonitorForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "TADST Monitor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitorForm_FormClosing);
            this.Shown += new System.EventHandler(this.MonitorForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtMonitorOutput;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkAutoUpdate;
        private System.Windows.Forms.CheckBox chkAutoscroll;
        private System.Windows.Forms.CheckBox chkWordWrap;
        private System.Windows.Forms.CheckBox chkTopMost;
    }
}