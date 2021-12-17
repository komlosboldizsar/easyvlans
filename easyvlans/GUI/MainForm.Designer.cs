
namespace easyvlans.GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.portTable = new System.Windows.Forms.TableLayoutPanel();
            this.titlePortPortLabel = new System.Windows.Forms.Label();
            this.titlePortSwitch = new System.Windows.Forms.Label();
            this.titlePortPortId = new System.Windows.Forms.Label();
            this.titlePortCurrentVlan = new System.Windows.Forms.Label();
            this.titlePortSetVlanTo = new System.Windows.Forms.Label();
            this.rowPortSet = new System.Windows.Forms.Button();
            this.rowPortSwitch = new System.Windows.Forms.Label();
            this.rowPortPortLabel = new System.Windows.Forms.Label();
            this.rowPortPortId = new System.Windows.Forms.Label();
            this.rowPortCurrentVlan = new System.Windows.Forms.Label();
            this.rowPortSetVlanTo = new System.Windows.Forms.ComboBox();
            this.rowPortState = new System.Windows.Forms.Label();
            this.titlePortState = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.portTableContainer = new System.Windows.Forms.Panel();
            this.switchTableContainer = new System.Windows.Forms.Panel();
            this.switchTable = new System.Windows.Forms.TableLayoutPanel();
            this.titleSwitchSwitchName = new System.Windows.Forms.Label();
            this.titleSwitchPendingChanges = new System.Windows.Forms.Label();
            this.titleSwitchPersistChanges = new System.Windows.Forms.Label();
            this.rowSwitchSwitchName = new System.Windows.Forms.Label();
            this.rowSwitchPendingChanges = new System.Windows.Forms.Label();
            this.rowSwitchPersistChanges = new System.Windows.Forms.Button();
            this.titleSwitchState = new System.Windows.Forms.Label();
            this.rowSwitchState = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.showVerboseLog = new System.Windows.Forms.CheckBox();
            this.portTable.SuspendLayout();
            this.portTableContainer.SuspendLayout();
            this.switchTableContainer.SuspendLayout();
            this.switchTable.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // portTable
            // 
            this.portTable.AutoSize = true;
            this.portTable.ColumnCount = 7;
            this.portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.portTable.Controls.Add(this.titlePortPortLabel, 0, 0);
            this.portTable.Controls.Add(this.titlePortSwitch, 1, 0);
            this.portTable.Controls.Add(this.titlePortPortId, 2, 0);
            this.portTable.Controls.Add(this.titlePortCurrentVlan, 3, 0);
            this.portTable.Controls.Add(this.titlePortSetVlanTo, 4, 0);
            this.portTable.Controls.Add(this.rowPortSet, 5, 1);
            this.portTable.Controls.Add(this.rowPortSwitch, 1, 1);
            this.portTable.Controls.Add(this.rowPortPortLabel, 0, 1);
            this.portTable.Controls.Add(this.rowPortPortId, 2, 1);
            this.portTable.Controls.Add(this.rowPortCurrentVlan, 3, 1);
            this.portTable.Controls.Add(this.rowPortSetVlanTo, 4, 1);
            this.portTable.Controls.Add(this.rowPortState, 6, 1);
            this.portTable.Controls.Add(this.titlePortState, 6, 0);
            this.portTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portTable.Location = new System.Drawing.Point(10, 10);
            this.portTable.Name = "portTable";
            this.portTable.RowCount = 2;
            this.portTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.portTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.portTable.Size = new System.Drawing.Size(1041, 70);
            this.portTable.TabIndex = 0;
            // 
            // titlePortPortLabel
            // 
            this.titlePortPortLabel.AutoSize = true;
            this.titlePortPortLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortPortLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortPortLabel.Location = new System.Drawing.Point(3, 0);
            this.titlePortPortLabel.Name = "titlePortPortLabel";
            this.titlePortPortLabel.Size = new System.Drawing.Size(72, 35);
            this.titlePortPortLabel.TabIndex = 0;
            this.titlePortPortLabel.Text = "Port label";
            this.titlePortPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortSwitch
            // 
            this.titlePortSwitch.AutoSize = true;
            this.titlePortSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortSwitch.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortSwitch.Location = new System.Drawing.Point(103, 0);
            this.titlePortSwitch.Name = "titlePortSwitch";
            this.titlePortSwitch.Size = new System.Drawing.Size(52, 35);
            this.titlePortSwitch.TabIndex = 1;
            this.titlePortSwitch.Text = "Switch";
            this.titlePortSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortPortId
            // 
            this.titlePortPortId.AutoSize = true;
            this.titlePortPortId.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortPortId.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortPortId.Location = new System.Drawing.Point(203, 0);
            this.titlePortPortId.Name = "titlePortPortId";
            this.titlePortPortId.Size = new System.Drawing.Size(54, 35);
            this.titlePortPortId.TabIndex = 2;
            this.titlePortPortId.Text = "Port ID";
            this.titlePortPortId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortCurrentVlan
            // 
            this.titlePortCurrentVlan.AutoSize = true;
            this.titlePortCurrentVlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortCurrentVlan.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortCurrentVlan.Location = new System.Drawing.Point(303, 0);
            this.titlePortCurrentVlan.Name = "titlePortCurrentVlan";
            this.titlePortCurrentVlan.Size = new System.Drawing.Size(98, 35);
            this.titlePortCurrentVlan.TabIndex = 3;
            this.titlePortCurrentVlan.Text = "Current VLAN";
            this.titlePortCurrentVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortSetVlanTo
            // 
            this.titlePortSetVlanTo.AutoSize = true;
            this.titlePortSetVlanTo.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortSetVlanTo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortSetVlanTo.Location = new System.Drawing.Point(453, 0);
            this.titlePortSetVlanTo.Name = "titlePortSetVlanTo";
            this.titlePortSetVlanTo.Size = new System.Drawing.Size(98, 35);
            this.titlePortSetVlanTo.TabIndex = 4;
            this.titlePortSetVlanTo.Text = "Set VLAN to...";
            this.titlePortSetVlanTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortSet
            // 
            this.rowPortSet.AutoSize = true;
            this.rowPortSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowPortSet.Location = new System.Drawing.Point(603, 38);
            this.rowPortSet.Name = "rowPortSet";
            this.rowPortSet.Size = new System.Drawing.Size(94, 29);
            this.rowPortSet.TabIndex = 7;
            this.rowPortSet.Text = "Set";
            this.rowPortSet.UseVisualStyleBackColor = true;
            // 
            // rowPortSwitch
            // 
            this.rowPortSwitch.AutoSize = true;
            this.rowPortSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortSwitch.Location = new System.Drawing.Point(103, 35);
            this.rowPortSwitch.Name = "rowPortSwitch";
            this.rowPortSwitch.Size = new System.Drawing.Size(56, 35);
            this.rowPortSwitch.TabIndex = 8;
            this.rowPortSwitch.Text = "Main A";
            this.rowPortSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortPortLabel
            // 
            this.rowPortPortLabel.AutoSize = true;
            this.rowPortPortLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortPortLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rowPortPortLabel.Location = new System.Drawing.Point(3, 35);
            this.rowPortPortLabel.Name = "rowPortPortLabel";
            this.rowPortPortLabel.Size = new System.Drawing.Size(65, 35);
            this.rowPortPortLabel.TabIndex = 9;
            this.rowPortPortLabel.Text = "DP B.21";
            this.rowPortPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortPortId
            // 
            this.rowPortPortId.AutoSize = true;
            this.rowPortPortId.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortPortId.Location = new System.Drawing.Point(203, 35);
            this.rowPortPortId.Name = "rowPortPortId";
            this.rowPortPortId.Size = new System.Drawing.Size(53, 35);
            this.rowPortPortId.TabIndex = 10;
            this.rowPortPortId.Text = "Fa0/21";
            this.rowPortPortId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortCurrentVlan
            // 
            this.rowPortCurrentVlan.AutoSize = true;
            this.rowPortCurrentVlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortCurrentVlan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rowPortCurrentVlan.Location = new System.Drawing.Point(303, 35);
            this.rowPortCurrentVlan.Name = "rowPortCurrentVlan";
            this.rowPortCurrentVlan.Size = new System.Drawing.Size(77, 35);
            this.rowPortCurrentVlan.TabIndex = 11;
            this.rowPortCurrentVlan.Text = "4 (Vutrix)";
            this.rowPortCurrentVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortSetVlanTo
            // 
            this.rowPortSetVlanTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowPortSetVlanTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rowPortSetVlanTo.FormattingEnabled = true;
            this.rowPortSetVlanTo.Location = new System.Drawing.Point(453, 38);
            this.rowPortSetVlanTo.Name = "rowPortSetVlanTo";
            this.rowPortSetVlanTo.Size = new System.Drawing.Size(144, 28);
            this.rowPortSetVlanTo.TabIndex = 12;
            // 
            // rowPortState
            // 
            this.rowPortState.AutoSize = true;
            this.rowPortState.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortState.Location = new System.Drawing.Point(703, 35);
            this.rowPortState.Name = "rowPortState";
            this.rowPortState.Size = new System.Drawing.Size(91, 35);
            this.rowPortState.TabIndex = 13;
            this.rowPortState.Text = "In progress...";
            this.rowPortState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortState
            // 
            this.titlePortState.AutoSize = true;
            this.titlePortState.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortState.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortState.Location = new System.Drawing.Point(703, 0);
            this.titlePortState.Name = "titlePortState";
            this.titlePortState.Size = new System.Drawing.Size(43, 35);
            this.titlePortState.TabIndex = 14;
            this.titlePortState.Text = "State";
            this.titlePortState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 185);
            this.logTextBox.MinimumSize = new System.Drawing.Size(4, 100);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(1061, 140);
            this.logTextBox.TabIndex = 4;
            this.logTextBox.Text = "";
            // 
            // portTableContainer
            // 
            this.portTableContainer.AutoSize = true;
            this.portTableContainer.Controls.Add(this.portTable);
            this.portTableContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.portTableContainer.Location = new System.Drawing.Point(0, 0);
            this.portTableContainer.Name = "portTableContainer";
            this.portTableContainer.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.portTableContainer.Size = new System.Drawing.Size(1061, 80);
            this.portTableContainer.TabIndex = 5;
            // 
            // switchTableContainer
            // 
            this.switchTableContainer.AutoSize = true;
            this.switchTableContainer.Controls.Add(this.switchTable);
            this.switchTableContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.switchTableContainer.Location = new System.Drawing.Point(0, 80);
            this.switchTableContainer.Name = "switchTableContainer";
            this.switchTableContainer.Padding = new System.Windows.Forms.Padding(10, 10, 10, 25);
            this.switchTableContainer.Size = new System.Drawing.Size(1061, 105);
            this.switchTableContainer.TabIndex = 1;
            // 
            // switchTable
            // 
            this.switchTable.AutoSize = true;
            this.switchTable.ColumnCount = 4;
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.switchTable.Controls.Add(this.titleSwitchSwitchName, 0, 0);
            this.switchTable.Controls.Add(this.titleSwitchPendingChanges, 1, 0);
            this.switchTable.Controls.Add(this.titleSwitchPersistChanges, 2, 0);
            this.switchTable.Controls.Add(this.rowSwitchSwitchName, 0, 1);
            this.switchTable.Controls.Add(this.rowSwitchPendingChanges, 1, 1);
            this.switchTable.Controls.Add(this.rowSwitchPersistChanges, 2, 1);
            this.switchTable.Controls.Add(this.titleSwitchState, 3, 0);
            this.switchTable.Controls.Add(this.rowSwitchState, 3, 1);
            this.switchTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.switchTable.Location = new System.Drawing.Point(10, 10);
            this.switchTable.Name = "switchTable";
            this.switchTable.RowCount = 2;
            this.switchTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.switchTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.switchTable.Size = new System.Drawing.Size(1041, 70);
            this.switchTable.TabIndex = 0;
            // 
            // titleSwitchSwitchName
            // 
            this.titleSwitchSwitchName.AutoSize = true;
            this.titleSwitchSwitchName.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSwitchSwitchName.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSwitchSwitchName.Location = new System.Drawing.Point(3, 0);
            this.titleSwitchSwitchName.Name = "titleSwitchSwitchName";
            this.titleSwitchSwitchName.Size = new System.Drawing.Size(93, 35);
            this.titleSwitchSwitchName.TabIndex = 0;
            this.titleSwitchSwitchName.Text = "Switch name";
            this.titleSwitchSwitchName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSwitchPendingChanges
            // 
            this.titleSwitchPendingChanges.AutoSize = true;
            this.titleSwitchPendingChanges.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSwitchPendingChanges.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSwitchPendingChanges.Location = new System.Drawing.Point(153, 0);
            this.titleSwitchPendingChanges.Name = "titleSwitchPendingChanges";
            this.titleSwitchPendingChanges.Size = new System.Drawing.Size(120, 35);
            this.titleSwitchPendingChanges.TabIndex = 1;
            this.titleSwitchPendingChanges.Text = "Pending changes";
            this.titleSwitchPendingChanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSwitchPersistChanges
            // 
            this.titleSwitchPersistChanges.AutoSize = true;
            this.titleSwitchPersistChanges.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSwitchPersistChanges.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSwitchPersistChanges.Location = new System.Drawing.Point(303, 0);
            this.titleSwitchPersistChanges.Name = "titleSwitchPersistChanges";
            this.titleSwitchPersistChanges.Size = new System.Drawing.Size(108, 35);
            this.titleSwitchPersistChanges.TabIndex = 2;
            this.titleSwitchPersistChanges.Text = "Persist changes";
            this.titleSwitchPersistChanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchSwitchName
            // 
            this.rowSwitchSwitchName.AutoSize = true;
            this.rowSwitchSwitchName.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowSwitchSwitchName.Location = new System.Drawing.Point(3, 35);
            this.rowSwitchSwitchName.Name = "rowSwitchSwitchName";
            this.rowSwitchSwitchName.Size = new System.Drawing.Size(56, 35);
            this.rowSwitchSwitchName.TabIndex = 3;
            this.rowSwitchSwitchName.Text = "Main A";
            this.rowSwitchSwitchName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchPendingChanges
            // 
            this.rowSwitchPendingChanges.AutoSize = true;
            this.rowSwitchPendingChanges.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowSwitchPendingChanges.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.rowSwitchPendingChanges.Location = new System.Drawing.Point(153, 35);
            this.rowSwitchPendingChanges.Name = "rowSwitchPendingChanges";
            this.rowSwitchPendingChanges.Size = new System.Drawing.Size(125, 35);
            this.rowSwitchPendingChanges.TabIndex = 4;
            this.rowSwitchPendingChanges.Text = "no ports changed";
            this.rowSwitchPendingChanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchPersistChanges
            // 
            this.rowSwitchPersistChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowSwitchPersistChanges.Location = new System.Drawing.Point(303, 38);
            this.rowSwitchPersistChanges.Name = "rowSwitchPersistChanges";
            this.rowSwitchPersistChanges.Size = new System.Drawing.Size(144, 29);
            this.rowSwitchPersistChanges.TabIndex = 5;
            this.rowSwitchPersistChanges.Text = "running -> startup";
            this.rowSwitchPersistChanges.UseVisualStyleBackColor = true;
            // 
            // titleSwitchState
            // 
            this.titleSwitchState.AutoSize = true;
            this.titleSwitchState.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSwitchState.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSwitchState.Location = new System.Drawing.Point(453, 0);
            this.titleSwitchState.Name = "titleSwitchState";
            this.titleSwitchState.Size = new System.Drawing.Size(43, 35);
            this.titleSwitchState.TabIndex = 6;
            this.titleSwitchState.Text = "State";
            this.titleSwitchState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchState
            // 
            this.rowSwitchState.AutoSize = true;
            this.rowSwitchState.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowSwitchState.Location = new System.Drawing.Point(453, 35);
            this.rowSwitchState.Name = "rowSwitchState";
            this.rowSwitchState.Size = new System.Drawing.Size(91, 35);
            this.rowSwitchState.TabIndex = 7;
            this.rowSwitchState.Text = "In progress...";
            this.rowSwitchState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.showVerboseLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 325);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1061, 33);
            this.panel1.TabIndex = 1;
            // 
            // showVerboseLog
            // 
            this.showVerboseLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showVerboseLog.AutoSize = true;
            this.showVerboseLog.Location = new System.Drawing.Point(909, 5);
            this.showVerboseLog.Name = "showVerboseLog";
            this.showVerboseLog.Size = new System.Drawing.Size(149, 24);
            this.showVerboseLog.TabIndex = 0;
            this.showVerboseLog.Text = "Show verbose log";
            this.showVerboseLog.UseVisualStyleBackColor = true;
            this.showVerboseLog.CheckedChanged += new System.EventHandler(this.showVerboseLogCheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1061, 358);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.switchTableContainer);
            this.Controls.Add(this.portTableContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "EasyVLANs";
            this.portTable.ResumeLayout(false);
            this.portTable.PerformLayout();
            this.portTableContainer.ResumeLayout(false);
            this.portTableContainer.PerformLayout();
            this.switchTableContainer.ResumeLayout(false);
            this.switchTableContainer.PerformLayout();
            this.switchTable.ResumeLayout(false);
            this.switchTable.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel portTable;
        private System.Windows.Forms.Label titlePortPortLabel;
        private System.Windows.Forms.Label titlePortSwitch;
        private System.Windows.Forms.Label titlePortPortId;
        private System.Windows.Forms.Label titlePortCurrentVlan;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Label titlePortSetVlanTo;
        private System.Windows.Forms.Button rowPortSet;
        private System.Windows.Forms.Label rowPortSwitch;
        private System.Windows.Forms.Label rowPortPortLabel;
        private System.Windows.Forms.Label rowPortPortId;
        private System.Windows.Forms.Label rowPortCurrentVlan;
        private System.Windows.Forms.ComboBox rowPortSetVlanTo;
        private System.Windows.Forms.Label rowPortState;
        private System.Windows.Forms.Panel portTableContainer;
        private System.Windows.Forms.Label titlePortState;
        private System.Windows.Forms.Panel switchTableContainer;
        private System.Windows.Forms.TableLayoutPanel switchTable;
        private System.Windows.Forms.Label titleSwitchSwitchName;
        private System.Windows.Forms.Label titleSwitchPendingChanges;
        private System.Windows.Forms.Label titleSwitchPersistChanges;
        private System.Windows.Forms.Label rowSwitchSwitchName;
        private System.Windows.Forms.Label rowSwitchPendingChanges;
        private System.Windows.Forms.Button rowSwitchPersistChanges;
        private System.Windows.Forms.Label titleSwitchState;
        private System.Windows.Forms.Label rowSwitchState;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox showVerboseLog;
    }
}

