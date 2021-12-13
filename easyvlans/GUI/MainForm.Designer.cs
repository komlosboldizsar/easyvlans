
namespace easyvlans
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.titlePortLabel = new System.Windows.Forms.Label();
            this.titleSwitch = new System.Windows.Forms.Label();
            this.titlePortId = new System.Windows.Forms.Label();
            this.titleCurrentVlan = new System.Windows.Forms.Label();
            this.titleSetVlanTo = new System.Windows.Forms.Label();
            this.rowPersist = new System.Windows.Forms.Button();
            this.rowSet = new System.Windows.Forms.Button();
            this.rowSwitch = new System.Windows.Forms.Label();
            this.rowPortLabel = new System.Windows.Forms.Label();
            this.rowPortId = new System.Windows.Forms.Label();
            this.rowCurrentVlan = new System.Windows.Forms.Label();
            this.rowSetVlanTo = new System.Windows.Forms.ComboBox();
            this.rowState = new System.Windows.Forms.Label();
            this.titleState = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.tableContainer = new System.Windows.Forms.Panel();
            this.table.SuspendLayout();
            this.tableContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.AutoSize = true;
            this.table.ColumnCount = 8;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.Controls.Add(this.titlePortLabel, 0, 0);
            this.table.Controls.Add(this.titleSwitch, 1, 0);
            this.table.Controls.Add(this.titlePortId, 2, 0);
            this.table.Controls.Add(this.titleCurrentVlan, 3, 0);
            this.table.Controls.Add(this.titleSetVlanTo, 4, 0);
            this.table.Controls.Add(this.rowPersist, 6, 1);
            this.table.Controls.Add(this.rowSet, 5, 1);
            this.table.Controls.Add(this.rowSwitch, 1, 1);
            this.table.Controls.Add(this.rowPortLabel, 0, 1);
            this.table.Controls.Add(this.rowPortId, 2, 1);
            this.table.Controls.Add(this.rowCurrentVlan, 3, 1);
            this.table.Controls.Add(this.rowSetVlanTo, 4, 1);
            this.table.Controls.Add(this.rowState, 7, 1);
            this.table.Controls.Add(this.titleState, 7, 0);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(10, 10);
            this.table.Name = "table";
            this.table.RowCount = 2;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.table.Size = new System.Drawing.Size(1041, 70);
            this.table.TabIndex = 0;
            // 
            // titlePortLabel
            // 
            this.titlePortLabel.AutoSize = true;
            this.titlePortLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortLabel.Location = new System.Drawing.Point(3, 0);
            this.titlePortLabel.Name = "titlePortLabel";
            this.titlePortLabel.Size = new System.Drawing.Size(72, 35);
            this.titlePortLabel.TabIndex = 0;
            this.titlePortLabel.Text = "Port label";
            this.titlePortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSwitch
            // 
            this.titleSwitch.AutoSize = true;
            this.titleSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSwitch.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSwitch.Location = new System.Drawing.Point(103, 0);
            this.titleSwitch.Name = "titleSwitch";
            this.titleSwitch.Size = new System.Drawing.Size(52, 35);
            this.titleSwitch.TabIndex = 1;
            this.titleSwitch.Text = "Switch";
            this.titleSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortId
            // 
            this.titlePortId.AutoSize = true;
            this.titlePortId.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortId.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortId.Location = new System.Drawing.Point(203, 0);
            this.titlePortId.Name = "titlePortId";
            this.titlePortId.Size = new System.Drawing.Size(54, 35);
            this.titlePortId.TabIndex = 2;
            this.titlePortId.Text = "Port ID";
            this.titlePortId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleCurrentVlan
            // 
            this.titleCurrentVlan.AutoSize = true;
            this.titleCurrentVlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleCurrentVlan.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleCurrentVlan.Location = new System.Drawing.Point(303, 0);
            this.titleCurrentVlan.Name = "titleCurrentVlan";
            this.titleCurrentVlan.Size = new System.Drawing.Size(98, 35);
            this.titleCurrentVlan.TabIndex = 3;
            this.titleCurrentVlan.Text = "Current VLAN";
            this.titleCurrentVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSetVlanTo
            // 
            this.titleSetVlanTo.AutoSize = true;
            this.titleSetVlanTo.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSetVlanTo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSetVlanTo.Location = new System.Drawing.Point(453, 0);
            this.titleSetVlanTo.Name = "titleSetVlanTo";
            this.titleSetVlanTo.Size = new System.Drawing.Size(98, 35);
            this.titleSetVlanTo.TabIndex = 4;
            this.titleSetVlanTo.Text = "Set VLAN to...";
            this.titleSetVlanTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPersist
            // 
            this.rowPersist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowPersist.Location = new System.Drawing.Point(703, 38);
            this.rowPersist.Name = "rowPersist";
            this.rowPersist.Size = new System.Drawing.Size(94, 29);
            this.rowPersist.TabIndex = 6;
            this.rowPersist.Text = "Persist";
            this.rowPersist.UseVisualStyleBackColor = true;
            // 
            // rowSet
            // 
            this.rowSet.AutoSize = true;
            this.rowSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowSet.Location = new System.Drawing.Point(603, 38);
            this.rowSet.Name = "rowSet";
            this.rowSet.Size = new System.Drawing.Size(94, 29);
            this.rowSet.TabIndex = 7;
            this.rowSet.Text = "Set";
            this.rowSet.UseVisualStyleBackColor = true;
            // 
            // rowSwitch
            // 
            this.rowSwitch.AutoSize = true;
            this.rowSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowSwitch.Location = new System.Drawing.Point(103, 35);
            this.rowSwitch.Name = "rowSwitch";
            this.rowSwitch.Size = new System.Drawing.Size(56, 35);
            this.rowSwitch.TabIndex = 8;
            this.rowSwitch.Text = "Main A";
            this.rowSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortLabel
            // 
            this.rowPortLabel.AutoSize = true;
            this.rowPortLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rowPortLabel.Location = new System.Drawing.Point(3, 35);
            this.rowPortLabel.Name = "rowPortLabel";
            this.rowPortLabel.Size = new System.Drawing.Size(65, 35);
            this.rowPortLabel.TabIndex = 9;
            this.rowPortLabel.Text = "DP B.21";
            this.rowPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortId
            // 
            this.rowPortId.AutoSize = true;
            this.rowPortId.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortId.Location = new System.Drawing.Point(203, 35);
            this.rowPortId.Name = "rowPortId";
            this.rowPortId.Size = new System.Drawing.Size(53, 35);
            this.rowPortId.TabIndex = 10;
            this.rowPortId.Text = "Fa0/21";
            this.rowPortId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowCurrentVlan
            // 
            this.rowCurrentVlan.AutoSize = true;
            this.rowCurrentVlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowCurrentVlan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rowCurrentVlan.Location = new System.Drawing.Point(303, 35);
            this.rowCurrentVlan.Name = "rowCurrentVlan";
            this.rowCurrentVlan.Size = new System.Drawing.Size(77, 35);
            this.rowCurrentVlan.TabIndex = 11;
            this.rowCurrentVlan.Text = "4 (Vutrix)";
            this.rowCurrentVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSetVlanTo
            // 
            this.rowSetVlanTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowSetVlanTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rowSetVlanTo.FormattingEnabled = true;
            this.rowSetVlanTo.Location = new System.Drawing.Point(453, 38);
            this.rowSetVlanTo.Name = "rowSetVlanTo";
            this.rowSetVlanTo.Size = new System.Drawing.Size(144, 28);
            this.rowSetVlanTo.TabIndex = 12;
            // 
            // rowState
            // 
            this.rowState.AutoSize = true;
            this.rowState.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowState.Location = new System.Drawing.Point(803, 35);
            this.rowState.Name = "rowState";
            this.rowState.Size = new System.Drawing.Size(91, 35);
            this.rowState.TabIndex = 13;
            this.rowState.Text = "In progress...";
            this.rowState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleState
            // 
            this.titleState.AutoSize = true;
            this.titleState.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleState.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleState.Location = new System.Drawing.Point(803, 0);
            this.titleState.Name = "titleState";
            this.titleState.Size = new System.Drawing.Size(43, 35);
            this.titleState.TabIndex = 14;
            this.titleState.Text = "State";
            this.titleState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 105);
            this.logTextBox.MinimumSize = new System.Drawing.Size(4, 100);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(1061, 103);
            this.logTextBox.TabIndex = 4;
            this.logTextBox.Text = "";
            // 
            // tableContainer
            // 
            this.tableContainer.AutoSize = true;
            this.tableContainer.Controls.Add(this.table);
            this.tableContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableContainer.Location = new System.Drawing.Point(0, 0);
            this.tableContainer.Name = "tableContainer";
            this.tableContainer.Padding = new System.Windows.Forms.Padding(10, 10, 10, 25);
            this.tableContainer.Size = new System.Drawing.Size(1061, 105);
            this.tableContainer.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1061, 208);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.tableContainer);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "EasyVLANs";
            this.table.ResumeLayout(false);
            this.table.PerformLayout();
            this.tableContainer.ResumeLayout(false);
            this.tableContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.Label titlePortLabel;
        private System.Windows.Forms.Label titleSwitch;
        private System.Windows.Forms.Label titlePortId;
        private System.Windows.Forms.Label titleCurrentVlan;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Label titleSetVlanTo;
        private System.Windows.Forms.Button rowPersist;
        private System.Windows.Forms.Button rowSet;
        private System.Windows.Forms.Label rowSwitch;
        private System.Windows.Forms.Label rowPortLabel;
        private System.Windows.Forms.Label rowPortId;
        private System.Windows.Forms.Label rowCurrentVlan;
        private System.Windows.Forms.ComboBox rowSetVlanTo;
        private System.Windows.Forms.Label rowState;
        private System.Windows.Forms.Panel tableContainer;
        private System.Windows.Forms.Label titleState;
    }
}

