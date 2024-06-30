namespace easyvlans.GUI
{
    partial class PortStatusDisplay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backPanel = new System.Windows.Forms.Panel();
            statusLabel = new System.Windows.Forms.Label();
            containerPanel = new System.Windows.Forms.Panel();
            backPanel.SuspendLayout();
            containerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // backPanel
            // 
            backPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            backPanel.Controls.Add(statusLabel);
            backPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            backPanel.Location = new System.Drawing.Point(2, 2);
            backPanel.Margin = new System.Windows.Forms.Padding(0);
            backPanel.Name = "backPanel";
            backPanel.Size = new System.Drawing.Size(56, 23);
            backPanel.TabIndex = 0;
            // 
            // statusLabel
            // 
            statusLabel.BackColor = System.Drawing.Color.Gold;
            statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            statusLabel.ForeColor = System.Drawing.Color.Black;
            statusLabel.Location = new System.Drawing.Point(0, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(54, 21);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "down";
            statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            statusLabel.MouseEnter += statusLabel_MouseEnter;
            statusLabel.MouseLeave += statusLabel_MouseLeave;
            // 
            // containerPanel
            // 
            containerPanel.Controls.Add(backPanel);
            containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            containerPanel.Location = new System.Drawing.Point(0, 0);
            containerPanel.Name = "containerPanel";
            containerPanel.Padding = new System.Windows.Forms.Padding(2);
            containerPanel.Size = new System.Drawing.Size(60, 27);
            containerPanel.TabIndex = 1;
            // 
            // PortStatusDisplay
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(containerPanel);
            Name = "PortStatusDisplay";
            Size = new System.Drawing.Size(60, 27);
            backPanel.ResumeLayout(false);
            containerPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel backPanel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Panel containerPanel;
    }
}
