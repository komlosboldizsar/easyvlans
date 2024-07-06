namespace easyvlans.GUI
{
    partial class PortDataDisplay
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
            label = new System.Windows.Forms.Label();
            containerPanel = new System.Windows.Forms.Panel();
            backPanel.SuspendLayout();
            containerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // backPanel
            // 
            backPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            backPanel.Controls.Add(label);
            backPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            backPanel.Location = new System.Drawing.Point(2, 2);
            backPanel.Margin = new System.Windows.Forms.Padding(0);
            backPanel.Name = "backPanel";
            backPanel.Size = new System.Drawing.Size(56, 23);
            backPanel.TabIndex = 0;
            // 
            // statusLabel
            // 
            label.BackColor = System.Drawing.Color.Gold;
            label.Dock = System.Windows.Forms.DockStyle.Fill;
            label.ForeColor = System.Drawing.Color.Black;
            label.Location = new System.Drawing.Point(0, 0);
            label.Name = "statusLabel";
            label.Size = new System.Drawing.Size(54, 21);
            label.TabIndex = 0;
            label.Text = "down";
            label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            label.MouseEnter += label_MouseEnter;
            label.MouseLeave += label_MouseLeave;
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
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Panel containerPanel;
    }
}
