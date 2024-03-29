﻿
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.portTable = new System.Windows.Forms.TableLayoutPanel();
            this.titlePortPortLabel = new System.Windows.Forms.Label();
            this.titlePortSwitch = new System.Windows.Forms.Label();
            this.titlePortPortIndex = new System.Windows.Forms.Label();
            this.titlePortCurrentVlan = new System.Windows.Forms.Label();
            this.titlePortSetVlanTo = new System.Windows.Forms.Label();
            this.rowPortSet = new System.Windows.Forms.Button();
            this.rowPortSwitch = new System.Windows.Forms.Label();
            this.rowPortPortLabel = new System.Windows.Forms.Label();
            this.rowPortPortIndex = new System.Windows.Forms.Label();
            this.rowPortCurrentVlan = new System.Windows.Forms.Label();
            this.rowPortSetVlanTo = new System.Windows.Forms.ComboBox();
            this.rowPostStatusSetVlan = new System.Windows.Forms.Label();
            this.titlePortStatus = new System.Windows.Forms.Label();
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
            this.titleSwitchStatusRead = new System.Windows.Forms.Label();
            this.rowSwitchStatusRead = new System.Windows.Forms.Label();
            this.titleSwitchStatusPersist = new System.Windows.Forms.Label();
            this.rowSwitchStatusPersist = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.githubLink = new System.Windows.Forms.LinkLabel();
            this.showVerboseLog = new System.Windows.Forms.CheckBox();
            this.portPageButtonContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.portPageButton = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.portTable.SuspendLayout();
            this.portTableContainer.SuspendLayout();
            this.switchTableContainer.SuspendLayout();
            this.switchTable.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.portPageButtonContainer.SuspendLayout();
            this.trayMenu.SuspendLayout();
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
            this.portTable.Controls.Add(this.titlePortPortIndex, 2, 0);
            this.portTable.Controls.Add(this.titlePortCurrentVlan, 3, 0);
            this.portTable.Controls.Add(this.titlePortSetVlanTo, 4, 0);
            this.portTable.Controls.Add(this.rowPortSet, 5, 1);
            this.portTable.Controls.Add(this.rowPortSwitch, 1, 1);
            this.portTable.Controls.Add(this.rowPortPortLabel, 0, 1);
            this.portTable.Controls.Add(this.rowPortPortIndex, 2, 1);
            this.portTable.Controls.Add(this.rowPortCurrentVlan, 3, 1);
            this.portTable.Controls.Add(this.rowPortSetVlanTo, 4, 1);
            this.portTable.Controls.Add(this.rowPostStatusSetVlan, 6, 1);
            this.portTable.Controls.Add(this.titlePortStatus, 6, 0);
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
            // titlePortPortIndex
            // 
            this.titlePortPortIndex.AutoSize = true;
            this.titlePortPortIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortPortIndex.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortPortIndex.Location = new System.Drawing.Point(203, 0);
            this.titlePortPortIndex.Name = "titlePortPortIndex";
            this.titlePortPortIndex.Size = new System.Drawing.Size(45, 35);
            this.titlePortPortIndex.TabIndex = 2;
            this.titlePortPortIndex.Text = "Index";
            this.titlePortPortIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.rowPortSet.Text = "set";
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
            // rowPortPortIndex
            // 
            this.rowPortPortIndex.AutoSize = true;
            this.rowPortPortIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortPortIndex.Location = new System.Drawing.Point(203, 35);
            this.rowPortPortIndex.Name = "rowPortPortIndex";
            this.rowPortPortIndex.Size = new System.Drawing.Size(25, 35);
            this.rowPortPortIndex.TabIndex = 10;
            this.rowPortPortIndex.Text = "21";
            this.rowPortPortIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortCurrentVlan
            // 
            this.rowPortCurrentVlan.AutoSize = true;
            this.rowPortCurrentVlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPortCurrentVlan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rowPortCurrentVlan.Location = new System.Drawing.Point(303, 35);
            this.rowPortCurrentVlan.Name = "rowPortCurrentVlan";
            this.rowPortCurrentVlan.Size = new System.Drawing.Size(75, 35);
            this.rowPortCurrentVlan.TabIndex = 11;
            this.rowPortCurrentVlan.Text = "4 - Vutrix";
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
            // rowPostStatusSetVlan
            // 
            this.rowPostStatusSetVlan.AutoSize = true;
            this.rowPostStatusSetVlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowPostStatusSetVlan.Location = new System.Drawing.Point(703, 35);
            this.rowPostStatusSetVlan.Name = "rowPostStatusSetVlan";
            this.rowPostStatusSetVlan.Size = new System.Drawing.Size(91, 35);
            this.rowPostStatusSetVlan.TabIndex = 13;
            this.rowPostStatusSetVlan.Text = "In progress...";
            this.rowPostStatusSetVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortStatus
            // 
            this.titlePortStatus.AutoSize = true;
            this.titlePortStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.titlePortStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titlePortStatus.Location = new System.Drawing.Point(703, 0);
            this.titlePortStatus.Name = "titlePortStatus";
            this.titlePortStatus.Size = new System.Drawing.Size(82, 35);
            this.titlePortStatus.TabIndex = 14;
            this.titlePortStatus.Text = "Status (set)";
            this.titlePortStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 231);
            this.logTextBox.MinimumSize = new System.Drawing.Size(4, 100);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(1061, 100);
            this.logTextBox.TabIndex = 4;
            this.logTextBox.Text = "";
            this.logTextBox.Resize += new System.EventHandler(this.logTextBox_Resize);
            // 
            // portTableContainer
            // 
            this.portTableContainer.AutoSize = true;
            this.portTableContainer.Controls.Add(this.portTable);
            this.portTableContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.portTableContainer.Location = new System.Drawing.Point(0, 46);
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
            this.switchTableContainer.Location = new System.Drawing.Point(0, 126);
            this.switchTableContainer.Name = "switchTableContainer";
            this.switchTableContainer.Padding = new System.Windows.Forms.Padding(10, 10, 10, 25);
            this.switchTableContainer.Size = new System.Drawing.Size(1061, 105);
            this.switchTableContainer.TabIndex = 1;
            // 
            // switchTable
            // 
            this.switchTable.AutoSize = true;
            this.switchTable.ColumnCount = 5;
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.switchTable.Controls.Add(this.titleSwitchSwitchName, 0, 0);
            this.switchTable.Controls.Add(this.titleSwitchPendingChanges, 1, 0);
            this.switchTable.Controls.Add(this.titleSwitchPersistChanges, 2, 0);
            this.switchTable.Controls.Add(this.rowSwitchSwitchName, 0, 1);
            this.switchTable.Controls.Add(this.rowSwitchPendingChanges, 1, 1);
            this.switchTable.Controls.Add(this.rowSwitchPersistChanges, 2, 1);
            this.switchTable.Controls.Add(this.titleSwitchStatusRead, 3, 0);
            this.switchTable.Controls.Add(this.rowSwitchStatusRead, 3, 1);
            this.switchTable.Controls.Add(this.titleSwitchStatusPersist, 4, 0);
            this.switchTable.Controls.Add(this.rowSwitchStatusPersist, 4, 1);
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
            this.rowSwitchPersistChanges.AutoSize = true;
            this.rowSwitchPersistChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowSwitchPersistChanges.Location = new System.Drawing.Point(303, 38);
            this.rowSwitchPersistChanges.Name = "rowSwitchPersistChanges";
            this.rowSwitchPersistChanges.Size = new System.Drawing.Size(114, 29);
            this.rowSwitchPersistChanges.TabIndex = 5;
            this.rowSwitchPersistChanges.Text = "persist";
            this.rowSwitchPersistChanges.UseVisualStyleBackColor = true;
            // 
            // titleSwitchStatusRead
            // 
            this.titleSwitchStatusRead.AutoSize = true;
            this.titleSwitchStatusRead.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSwitchStatusRead.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSwitchStatusRead.Location = new System.Drawing.Point(423, 0);
            this.titleSwitchStatusRead.Name = "titleSwitchStatusRead";
            this.titleSwitchStatusRead.Size = new System.Drawing.Size(93, 35);
            this.titleSwitchStatusRead.TabIndex = 6;
            this.titleSwitchStatusRead.Text = "Status (read)";
            this.titleSwitchStatusRead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchStatusRead
            // 
            this.rowSwitchStatusRead.AutoSize = true;
            this.rowSwitchStatusRead.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowSwitchStatusRead.Location = new System.Drawing.Point(423, 35);
            this.rowSwitchStatusRead.Name = "rowSwitchStatusRead";
            this.rowSwitchStatusRead.Size = new System.Drawing.Size(150, 35);
            this.rowSwitchStatusRead.TabIndex = 7;
            this.rowSwitchStatusRead.Text = "In progress (11:48:53)";
            this.rowSwitchStatusRead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSwitchStatusPersist
            // 
            this.titleSwitchStatusPersist.AutoSize = true;
            this.titleSwitchStatusPersist.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleSwitchStatusPersist.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleSwitchStatusPersist.Location = new System.Drawing.Point(603, 0);
            this.titleSwitchStatusPersist.Name = "titleSwitchStatusPersist";
            this.titleSwitchStatusPersist.Size = new System.Drawing.Size(106, 35);
            this.titleSwitchStatusPersist.TabIndex = 8;
            this.titleSwitchStatusPersist.Text = "Status (persist)";
            this.titleSwitchStatusPersist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchStatusPersist
            // 
            this.rowSwitchStatusPersist.AutoSize = true;
            this.rowSwitchStatusPersist.Dock = System.Windows.Forms.DockStyle.Left;
            this.rowSwitchStatusPersist.Location = new System.Drawing.Point(603, 35);
            this.rowSwitchStatusPersist.Name = "rowSwitchStatusPersist";
            this.rowSwitchStatusPersist.Size = new System.Drawing.Size(97, 35);
            this.rowSwitchStatusPersist.TabIndex = 9;
            this.rowSwitchStatusPersist.Text = "OK (11:45:01)";
            this.rowSwitchStatusPersist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bottomPanel
            // 
            this.bottomPanel.AutoSize = true;
            this.bottomPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bottomPanel.Controls.Add(this.linkLabel1);
            this.bottomPanel.Controls.Add(this.githubLink);
            this.bottomPanel.Controls.Add(this.showVerboseLog);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 325);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(1061, 33);
            this.bottomPanel.TabIndex = 1;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(282, 6);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.linkLabel1.Size = new System.Drawing.Size(95, 20);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Report issue";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.issueReportLinkClicked);
            // 
            // githubLink
            // 
            this.githubLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.githubLink.AutoSize = true;
            this.githubLink.LinkColor = System.Drawing.Color.Blue;
            this.githubLink.Location = new System.Drawing.Point(9, 6);
            this.githubLink.Name = "githubLink";
            this.githubLink.Size = new System.Drawing.Size(267, 20);
            this.githubLink.TabIndex = 1;
            this.githubLink.TabStop = true;
            this.githubLink.Text = "github.com/komlosboldizsar/easyvlans";
            this.githubLink.VisitedLinkColor = System.Drawing.Color.Blue;
            this.githubLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.githubLinkClickHandler);
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
            // portPageButtonContainer
            // 
            this.portPageButtonContainer.AutoSize = true;
            this.portPageButtonContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.portPageButtonContainer.Controls.Add(this.portPageButton);
            this.portPageButtonContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.portPageButtonContainer.Location = new System.Drawing.Point(0, 0);
            this.portPageButtonContainer.Name = "portPageButtonContainer";
            this.portPageButtonContainer.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.portPageButtonContainer.Size = new System.Drawing.Size(1061, 46);
            this.portPageButtonContainer.TabIndex = 1;
            // 
            // portPageButton
            // 
            this.portPageButton.AutoSize = true;
            this.portPageButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.portPageButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.portPageButton.Location = new System.Drawing.Point(13, 13);
            this.portPageButton.Name = "portPageButton";
            this.portPageButton.Size = new System.Drawing.Size(63, 30);
            this.portPageButton.TabIndex = 0;
            this.portPageButton.Text = "Page 1";
            this.portPageButton.UseVisualStyleBackColor = true;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "EasyVLANs";
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayMenuExit});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(179, 28);
            // 
            // trayMenuExit
            // 
            this.trayMenuExit.Name = "trayMenuExit";
            this.trayMenuExit.Size = new System.Drawing.Size(178, 24);
            this.trayMenuExit.Text = "Exit EasyVLANs";
            this.trayMenuExit.Click += new System.EventHandler(this.trayMenuExit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1061, 358);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.switchTableContainer);
            this.Controls.Add(this.portTableContainer);
            this.Controls.Add(this.portPageButtonContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(50, 50);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EasyVLANs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.portTable.ResumeLayout(false);
            this.portTable.PerformLayout();
            this.portTableContainer.ResumeLayout(false);
            this.portTableContainer.PerformLayout();
            this.switchTableContainer.ResumeLayout(false);
            this.switchTableContainer.PerformLayout();
            this.switchTable.ResumeLayout(false);
            this.switchTable.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.portPageButtonContainer.ResumeLayout(false);
            this.portPageButtonContainer.PerformLayout();
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel portTable;
        private System.Windows.Forms.Label titlePortPortLabel;
        private System.Windows.Forms.Label titlePortSwitch;
        private System.Windows.Forms.Label titlePortPortIndex;
        private System.Windows.Forms.Label titlePortCurrentVlan;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Label titlePortSetVlanTo;
        private System.Windows.Forms.Button rowPortSet;
        private System.Windows.Forms.Label rowPortSwitch;
        private System.Windows.Forms.Label rowPortPortLabel;
        private System.Windows.Forms.Label rowPortPortIndex;
        private System.Windows.Forms.Label rowPortCurrentVlan;
        private System.Windows.Forms.ComboBox rowPortSetVlanTo;
        private System.Windows.Forms.Label rowPostStatusSetVlan;
        private System.Windows.Forms.Panel portTableContainer;
        private System.Windows.Forms.Label titlePortStatus;
        private System.Windows.Forms.Panel switchTableContainer;
        private System.Windows.Forms.TableLayoutPanel switchTable;
        private System.Windows.Forms.Label titleSwitchSwitchName;
        private System.Windows.Forms.Label titleSwitchPendingChanges;
        private System.Windows.Forms.Label titleSwitchPersistChanges;
        private System.Windows.Forms.Label rowSwitchSwitchName;
        private System.Windows.Forms.Label rowSwitchPendingChanges;
        private System.Windows.Forms.Button rowSwitchPersistChanges;
        private System.Windows.Forms.Label titleSwitchStatusRead;
        private System.Windows.Forms.Label rowSwitchStatusRead;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.CheckBox showVerboseLog;
        private System.Windows.Forms.FlowLayoutPanel portPageButtonContainer;
        private System.Windows.Forms.Button portPageButton;
        private System.Windows.Forms.LinkLabel githubLink;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label titleSwitchStatusPersist;
        private System.Windows.Forms.Label rowSwitchStatusPersist;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem trayMenuExit;
    }
}

