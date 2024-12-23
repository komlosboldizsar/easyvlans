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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            portTable = new System.Windows.Forms.TableLayoutPanel();
            titlePortPortLabel = new System.Windows.Forms.Label();
            titlePortSwitch = new System.Windows.Forms.Label();
            titlePortPortIndex = new System.Windows.Forms.Label();
            titlePortStatus = new System.Windows.Forms.Label();
            titlePortSpeed = new System.Windows.Forms.Label();
            titlePortCurrentVlan = new System.Windows.Forms.Label();
            titlePortSetVlanTo = new System.Windows.Forms.Label();
            titlePortStatusSet = new System.Windows.Forms.Label();
            rowPortSet = new System.Windows.Forms.Button();
            rowPortSwitch = new System.Windows.Forms.Label();
            rowPortPortLabel = new System.Windows.Forms.Label();
            rowPortPortIndex = new System.Windows.Forms.Label();
            rowPortStatusDisplay = new PortStatusDisplay();
            rowPortStatusSpeed = new PortSpeedDisplay();
            rowPortCurrentVlan = new System.Windows.Forms.Label();
            rowPortSetVlanTo = new System.Windows.Forms.ComboBox();
            rowPostStatusSetVlan = new System.Windows.Forms.Label();
            logTextBox = new System.Windows.Forms.RichTextBox();
            portTableContainer = new System.Windows.Forms.Panel();
            switchTableContainer = new System.Windows.Forms.Panel();
            switchTable = new System.Windows.Forms.TableLayoutPanel();
            titleSwitchSwitchName = new System.Windows.Forms.Label();
            titleSwitchPendingChanges = new System.Windows.Forms.Label();
            titleSwitchPersistChanges = new System.Windows.Forms.Label();
            rowSwitchSwitchName = new System.Windows.Forms.Label();
            rowSwitchPendingChanges = new System.Windows.Forms.Label();
            rowSwitchPersistChanges = new System.Windows.Forms.Button();
            titleSwitchStatusRead = new System.Windows.Forms.Label();
            rowSwitchStatusRead = new System.Windows.Forms.Label();
            titleSwitchStatusPersist = new System.Windows.Forms.Label();
            rowSwitchStatusPersist = new System.Windows.Forms.Label();
            bottomPanel = new System.Windows.Forms.Panel();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            githubLink = new System.Windows.Forms.LinkLabel();
            showVerboseLog = new System.Windows.Forms.CheckBox();
            portCollectionButtonContainer = new System.Windows.Forms.FlowLayoutPanel();
            portCollectionButton = new System.Windows.Forms.Button();
            portCollectionButtonContainerContainer = new System.Windows.Forms.Panel();
            portTable.SuspendLayout();
            portTableContainer.SuspendLayout();
            switchTableContainer.SuspendLayout();
            switchTable.SuspendLayout();
            bottomPanel.SuspendLayout();
            portCollectionButtonContainer.SuspendLayout();
            portCollectionButtonContainerContainer.SuspendLayout();
            SuspendLayout();
            // 
            // trayIcon
            // 
            trayIcon.Icon = (System.Drawing.Icon)resources.GetObject("trayIcon.Icon");
            // 
            // portTable
            // 
            portTable.AutoSize = true;
            portTable.ColumnCount = 9;
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            portTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            portTable.Controls.Add(titlePortPortLabel, 0, 0);
            portTable.Controls.Add(titlePortSwitch, 1, 0);
            portTable.Controls.Add(titlePortPortIndex, 2, 0);
            portTable.Controls.Add(titlePortStatus, 3, 0);
            portTable.Controls.Add(titlePortSpeed, 4, 0);
            portTable.Controls.Add(titlePortCurrentVlan, 5, 0);
            portTable.Controls.Add(titlePortSetVlanTo, 6, 0);
            portTable.Controls.Add(titlePortStatusSet, 8, 0);
            portTable.Controls.Add(rowPortSet, 7, 1);
            portTable.Controls.Add(rowPortSwitch, 1, 1);
            portTable.Controls.Add(rowPortPortLabel, 0, 1);
            portTable.Controls.Add(rowPortPortIndex, 2, 1);
            portTable.Controls.Add(rowPortStatusDisplay, 3, 1);
            portTable.Controls.Add(rowPortStatusSpeed, 4, 1);
            portTable.Controls.Add(rowPortCurrentVlan, 5, 1);
            portTable.Controls.Add(rowPortSetVlanTo, 6, 1);
            portTable.Controls.Add(rowPostStatusSetVlan, 8, 1);
            portTable.Dock = System.Windows.Forms.DockStyle.Fill;
            portTable.Location = new System.Drawing.Point(10, 3);
            portTable.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            portTable.Name = "portTable";
            portTable.RowCount = 2;
            portTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            portTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            portTable.Size = new System.Drawing.Size(1128, 70);
            portTable.TabIndex = 0;
            // 
            // titlePortPortLabel
            // 
            titlePortPortLabel.AutoSize = true;
            titlePortPortLabel.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortPortLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortPortLabel.Location = new System.Drawing.Point(3, 0);
            titlePortPortLabel.Name = "titlePortPortLabel";
            titlePortPortLabel.Size = new System.Drawing.Size(72, 35);
            titlePortPortLabel.TabIndex = 0;
            titlePortPortLabel.Text = "Port label";
            titlePortPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortSwitch
            // 
            titlePortSwitch.AutoSize = true;
            titlePortSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortSwitch.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortSwitch.Location = new System.Drawing.Point(103, 0);
            titlePortSwitch.Name = "titlePortSwitch";
            titlePortSwitch.Size = new System.Drawing.Size(52, 35);
            titlePortSwitch.TabIndex = 1;
            titlePortSwitch.Text = "Switch";
            titlePortSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortPortIndex
            // 
            titlePortPortIndex.AutoSize = true;
            titlePortPortIndex.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortPortIndex.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortPortIndex.Location = new System.Drawing.Point(203, 0);
            titlePortPortIndex.Name = "titlePortPortIndex";
            titlePortPortIndex.Size = new System.Drawing.Size(45, 35);
            titlePortPortIndex.TabIndex = 2;
            titlePortPortIndex.Text = "Index";
            titlePortPortIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortStatus
            // 
            titlePortStatus.AutoSize = true;
            titlePortStatus.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortStatus.Location = new System.Drawing.Point(303, 0);
            titlePortStatus.Name = "titlePortStatus";
            titlePortStatus.Size = new System.Drawing.Size(49, 35);
            titlePortStatus.TabIndex = 15;
            titlePortStatus.Text = "Status";
            titlePortStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortSpeed
            // 
            titlePortSpeed.AutoSize = true;
            titlePortSpeed.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortSpeed.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortSpeed.Location = new System.Drawing.Point(388, 0);
            titlePortSpeed.Name = "titlePortSpeed";
            titlePortSpeed.Size = new System.Drawing.Size(51, 35);
            titlePortSpeed.TabIndex = 18;
            titlePortSpeed.Text = "Speed";
            titlePortSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortCurrentVlan
            // 
            titlePortCurrentVlan.AutoSize = true;
            titlePortCurrentVlan.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortCurrentVlan.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortCurrentVlan.Location = new System.Drawing.Point(483, 0);
            titlePortCurrentVlan.Name = "titlePortCurrentVlan";
            titlePortCurrentVlan.Size = new System.Drawing.Size(98, 35);
            titlePortCurrentVlan.TabIndex = 3;
            titlePortCurrentVlan.Text = "Current VLAN";
            titlePortCurrentVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortSetVlanTo
            // 
            titlePortSetVlanTo.AutoSize = true;
            titlePortSetVlanTo.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortSetVlanTo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortSetVlanTo.Location = new System.Drawing.Point(633, 0);
            titlePortSetVlanTo.Name = "titlePortSetVlanTo";
            titlePortSetVlanTo.Size = new System.Drawing.Size(98, 35);
            titlePortSetVlanTo.TabIndex = 4;
            titlePortSetVlanTo.Text = "Set VLAN to...";
            titlePortSetVlanTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titlePortStatusSet
            // 
            titlePortStatusSet.AutoSize = true;
            titlePortStatusSet.Dock = System.Windows.Forms.DockStyle.Left;
            titlePortStatusSet.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titlePortStatusSet.Location = new System.Drawing.Point(933, 0);
            titlePortStatusSet.Name = "titlePortStatusSet";
            titlePortStatusSet.Size = new System.Drawing.Size(82, 35);
            titlePortStatusSet.TabIndex = 14;
            titlePortStatusSet.Text = "Status (set)";
            titlePortStatusSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortSet
            // 
            rowPortSet.AutoSize = true;
            rowPortSet.Dock = System.Windows.Forms.DockStyle.Fill;
            rowPortSet.Location = new System.Drawing.Point(833, 38);
            rowPortSet.Name = "rowPortSet";
            rowPortSet.Size = new System.Drawing.Size(94, 29);
            rowPortSet.TabIndex = 7;
            rowPortSet.Text = "set";
            rowPortSet.UseVisualStyleBackColor = true;
            // 
            // rowPortSwitch
            // 
            rowPortSwitch.AutoSize = true;
            rowPortSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            rowPortSwitch.Location = new System.Drawing.Point(103, 35);
            rowPortSwitch.Name = "rowPortSwitch";
            rowPortSwitch.Size = new System.Drawing.Size(56, 35);
            rowPortSwitch.TabIndex = 8;
            rowPortSwitch.Text = "Main A";
            rowPortSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortPortLabel
            // 
            rowPortPortLabel.AutoSize = true;
            rowPortPortLabel.Dock = System.Windows.Forms.DockStyle.Left;
            rowPortPortLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            rowPortPortLabel.Location = new System.Drawing.Point(3, 35);
            rowPortPortLabel.Name = "rowPortPortLabel";
            rowPortPortLabel.Size = new System.Drawing.Size(65, 35);
            rowPortPortLabel.TabIndex = 9;
            rowPortPortLabel.Text = "DP B.21";
            rowPortPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortPortIndex
            // 
            rowPortPortIndex.AutoSize = true;
            rowPortPortIndex.Dock = System.Windows.Forms.DockStyle.Left;
            rowPortPortIndex.Location = new System.Drawing.Point(203, 35);
            rowPortPortIndex.Name = "rowPortPortIndex";
            rowPortPortIndex.Size = new System.Drawing.Size(25, 35);
            rowPortPortIndex.TabIndex = 10;
            rowPortPortIndex.Text = "21";
            rowPortPortIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortStatusDisplay
            // 
            rowPortStatusDisplay.Dock = System.Windows.Forms.DockStyle.Left;
            rowPortStatusDisplay.Location = new System.Drawing.Point(303, 38);
            rowPortStatusDisplay.Name = "rowPortStatusDisplay";
            rowPortStatusDisplay.Port = null;
            rowPortStatusDisplay.Size = new System.Drawing.Size(75, 29);
            rowPortStatusDisplay.TabIndex = 16;
            // 
            // rowPortStatusSpeed
            // 
            rowPortStatusSpeed.Location = new System.Drawing.Point(388, 38);
            rowPortStatusSpeed.Name = "rowPortStatusSpeed";
            rowPortStatusSpeed.Port = null;
            rowPortStatusSpeed.Size = new System.Drawing.Size(75, 29);
            rowPortStatusSpeed.TabIndex = 17;
            // 
            // rowPortCurrentVlan
            // 
            rowPortCurrentVlan.AutoSize = true;
            rowPortCurrentVlan.Dock = System.Windows.Forms.DockStyle.Left;
            rowPortCurrentVlan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            rowPortCurrentVlan.Location = new System.Drawing.Point(483, 35);
            rowPortCurrentVlan.Name = "rowPortCurrentVlan";
            rowPortCurrentVlan.Size = new System.Drawing.Size(75, 35);
            rowPortCurrentVlan.TabIndex = 11;
            rowPortCurrentVlan.Text = "4 - Vutrix";
            rowPortCurrentVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowPortSetVlanTo
            // 
            rowPortSetVlanTo.Dock = System.Windows.Forms.DockStyle.Fill;
            rowPortSetVlanTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            rowPortSetVlanTo.FormattingEnabled = true;
            rowPortSetVlanTo.Location = new System.Drawing.Point(633, 38);
            rowPortSetVlanTo.Name = "rowPortSetVlanTo";
            rowPortSetVlanTo.Size = new System.Drawing.Size(194, 28);
            rowPortSetVlanTo.TabIndex = 12;
            // 
            // rowPostStatusSetVlan
            // 
            rowPostStatusSetVlan.AutoSize = true;
            rowPostStatusSetVlan.Dock = System.Windows.Forms.DockStyle.Left;
            rowPostStatusSetVlan.Location = new System.Drawing.Point(933, 35);
            rowPostStatusSetVlan.Name = "rowPostStatusSetVlan";
            rowPostStatusSetVlan.Size = new System.Drawing.Size(91, 35);
            rowPostStatusSetVlan.TabIndex = 13;
            rowPostStatusSetVlan.Text = "In progress...";
            rowPostStatusSetVlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logTextBox
            // 
            logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            logTextBox.Location = new System.Drawing.Point(0, 219);
            logTextBox.MinimumSize = new System.Drawing.Size(4, 100);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new System.Drawing.Size(1148, 106);
            logTextBox.TabIndex = 4;
            logTextBox.Text = "";
            // 
            // portTableContainer
            // 
            portTableContainer.AutoSize = true;
            portTableContainer.Controls.Add(portTable);
            portTableContainer.Dock = System.Windows.Forms.DockStyle.Top;
            portTableContainer.Location = new System.Drawing.Point(0, 41);
            portTableContainer.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            portTableContainer.Name = "portTableContainer";
            portTableContainer.Padding = new System.Windows.Forms.Padding(10, 3, 10, 0);
            portTableContainer.Size = new System.Drawing.Size(1148, 73);
            portTableContainer.TabIndex = 5;
            // 
            // switchTableContainer
            // 
            switchTableContainer.AutoSize = true;
            switchTableContainer.Controls.Add(switchTable);
            switchTableContainer.Dock = System.Windows.Forms.DockStyle.Top;
            switchTableContainer.Location = new System.Drawing.Point(0, 114);
            switchTableContainer.Name = "switchTableContainer";
            switchTableContainer.Padding = new System.Windows.Forms.Padding(10, 10, 10, 25);
            switchTableContainer.Size = new System.Drawing.Size(1148, 105);
            switchTableContainer.TabIndex = 1;
            // 
            // switchTable
            // 
            switchTable.AutoSize = true;
            switchTable.ColumnCount = 5;
            switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            switchTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            switchTable.Controls.Add(titleSwitchSwitchName, 0, 0);
            switchTable.Controls.Add(titleSwitchPendingChanges, 1, 0);
            switchTable.Controls.Add(titleSwitchPersistChanges, 2, 0);
            switchTable.Controls.Add(rowSwitchSwitchName, 0, 1);
            switchTable.Controls.Add(rowSwitchPendingChanges, 1, 1);
            switchTable.Controls.Add(rowSwitchPersistChanges, 2, 1);
            switchTable.Controls.Add(titleSwitchStatusRead, 3, 0);
            switchTable.Controls.Add(rowSwitchStatusRead, 3, 1);
            switchTable.Controls.Add(titleSwitchStatusPersist, 4, 0);
            switchTable.Controls.Add(rowSwitchStatusPersist, 4, 1);
            switchTable.Dock = System.Windows.Forms.DockStyle.Fill;
            switchTable.Location = new System.Drawing.Point(10, 10);
            switchTable.Name = "switchTable";
            switchTable.RowCount = 2;
            switchTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            switchTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            switchTable.Size = new System.Drawing.Size(1128, 70);
            switchTable.TabIndex = 0;
            // 
            // titleSwitchSwitchName
            // 
            titleSwitchSwitchName.AutoSize = true;
            titleSwitchSwitchName.Dock = System.Windows.Forms.DockStyle.Left;
            titleSwitchSwitchName.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titleSwitchSwitchName.Location = new System.Drawing.Point(3, 0);
            titleSwitchSwitchName.Name = "titleSwitchSwitchName";
            titleSwitchSwitchName.Size = new System.Drawing.Size(93, 35);
            titleSwitchSwitchName.TabIndex = 0;
            titleSwitchSwitchName.Text = "Switch name";
            titleSwitchSwitchName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSwitchPendingChanges
            // 
            titleSwitchPendingChanges.AutoSize = true;
            titleSwitchPendingChanges.Dock = System.Windows.Forms.DockStyle.Left;
            titleSwitchPendingChanges.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titleSwitchPendingChanges.Location = new System.Drawing.Point(153, 0);
            titleSwitchPendingChanges.Name = "titleSwitchPendingChanges";
            titleSwitchPendingChanges.Size = new System.Drawing.Size(120, 35);
            titleSwitchPendingChanges.TabIndex = 1;
            titleSwitchPendingChanges.Text = "Pending changes";
            titleSwitchPendingChanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSwitchPersistChanges
            // 
            titleSwitchPersistChanges.AutoSize = true;
            titleSwitchPersistChanges.Dock = System.Windows.Forms.DockStyle.Left;
            titleSwitchPersistChanges.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titleSwitchPersistChanges.Location = new System.Drawing.Point(303, 0);
            titleSwitchPersistChanges.Name = "titleSwitchPersistChanges";
            titleSwitchPersistChanges.Size = new System.Drawing.Size(108, 35);
            titleSwitchPersistChanges.TabIndex = 2;
            titleSwitchPersistChanges.Text = "Persist changes";
            titleSwitchPersistChanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchSwitchName
            // 
            rowSwitchSwitchName.AutoSize = true;
            rowSwitchSwitchName.Dock = System.Windows.Forms.DockStyle.Left;
            rowSwitchSwitchName.Location = new System.Drawing.Point(3, 35);
            rowSwitchSwitchName.Name = "rowSwitchSwitchName";
            rowSwitchSwitchName.Size = new System.Drawing.Size(56, 35);
            rowSwitchSwitchName.TabIndex = 3;
            rowSwitchSwitchName.Text = "Main A";
            rowSwitchSwitchName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchPendingChanges
            // 
            rowSwitchPendingChanges.AutoSize = true;
            rowSwitchPendingChanges.Dock = System.Windows.Forms.DockStyle.Left;
            rowSwitchPendingChanges.ForeColor = System.Drawing.SystemColors.ControlDark;
            rowSwitchPendingChanges.Location = new System.Drawing.Point(153, 35);
            rowSwitchPendingChanges.Name = "rowSwitchPendingChanges";
            rowSwitchPendingChanges.Size = new System.Drawing.Size(125, 35);
            rowSwitchPendingChanges.TabIndex = 4;
            rowSwitchPendingChanges.Text = "no ports changed";
            rowSwitchPendingChanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchPersistChanges
            // 
            rowSwitchPersistChanges.AutoSize = true;
            rowSwitchPersistChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            rowSwitchPersistChanges.Location = new System.Drawing.Point(303, 38);
            rowSwitchPersistChanges.Name = "rowSwitchPersistChanges";
            rowSwitchPersistChanges.Size = new System.Drawing.Size(114, 29);
            rowSwitchPersistChanges.TabIndex = 5;
            rowSwitchPersistChanges.Text = "persist";
            rowSwitchPersistChanges.UseVisualStyleBackColor = true;
            // 
            // titleSwitchStatusRead
            // 
            titleSwitchStatusRead.AutoSize = true;
            titleSwitchStatusRead.Dock = System.Windows.Forms.DockStyle.Left;
            titleSwitchStatusRead.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titleSwitchStatusRead.Location = new System.Drawing.Point(423, 0);
            titleSwitchStatusRead.Name = "titleSwitchStatusRead";
            titleSwitchStatusRead.Size = new System.Drawing.Size(93, 35);
            titleSwitchStatusRead.TabIndex = 6;
            titleSwitchStatusRead.Text = "Status (read)";
            titleSwitchStatusRead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchStatusRead
            // 
            rowSwitchStatusRead.AutoSize = true;
            rowSwitchStatusRead.Dock = System.Windows.Forms.DockStyle.Left;
            rowSwitchStatusRead.Location = new System.Drawing.Point(423, 35);
            rowSwitchStatusRead.Name = "rowSwitchStatusRead";
            rowSwitchStatusRead.Size = new System.Drawing.Size(150, 35);
            rowSwitchStatusRead.TabIndex = 7;
            rowSwitchStatusRead.Text = "In progress (11:48:53)";
            rowSwitchStatusRead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleSwitchStatusPersist
            // 
            titleSwitchStatusPersist.AutoSize = true;
            titleSwitchStatusPersist.Dock = System.Windows.Forms.DockStyle.Left;
            titleSwitchStatusPersist.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            titleSwitchStatusPersist.Location = new System.Drawing.Point(603, 0);
            titleSwitchStatusPersist.Name = "titleSwitchStatusPersist";
            titleSwitchStatusPersist.Size = new System.Drawing.Size(106, 35);
            titleSwitchStatusPersist.TabIndex = 8;
            titleSwitchStatusPersist.Text = "Status (persist)";
            titleSwitchStatusPersist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rowSwitchStatusPersist
            // 
            rowSwitchStatusPersist.AutoSize = true;
            rowSwitchStatusPersist.Dock = System.Windows.Forms.DockStyle.Left;
            rowSwitchStatusPersist.Location = new System.Drawing.Point(603, 35);
            rowSwitchStatusPersist.Name = "rowSwitchStatusPersist";
            rowSwitchStatusPersist.Size = new System.Drawing.Size(97, 35);
            rowSwitchStatusPersist.TabIndex = 9;
            rowSwitchStatusPersist.Text = "OK (11:45:01)";
            rowSwitchStatusPersist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bottomPanel
            // 
            bottomPanel.AutoSize = true;
            bottomPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            bottomPanel.Controls.Add(linkLabel1);
            bottomPanel.Controls.Add(githubLink);
            bottomPanel.Controls.Add(showVerboseLog);
            bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            bottomPanel.Location = new System.Drawing.Point(0, 325);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new System.Drawing.Size(1148, 33);
            bottomPanel.TabIndex = 1;
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(282, 6);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            linkLabel1.Size = new System.Drawing.Size(95, 20);
            linkLabel1.TabIndex = 2;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Report issue";
            linkLabel1.LinkClicked += issueReportLinkClicked;
            // 
            // githubLink
            // 
            githubLink.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            githubLink.AutoSize = true;
            githubLink.LinkColor = System.Drawing.Color.Blue;
            githubLink.Location = new System.Drawing.Point(9, 6);
            githubLink.Name = "githubLink";
            githubLink.Size = new System.Drawing.Size(267, 20);
            githubLink.TabIndex = 1;
            githubLink.TabStop = true;
            githubLink.Text = "github.com/komlosboldizsar/easyvlans";
            githubLink.VisitedLinkColor = System.Drawing.Color.Blue;
            githubLink.LinkClicked += githubLinkClickHandler;
            // 
            // showVerboseLog
            // 
            showVerboseLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            showVerboseLog.AutoSize = true;
            showVerboseLog.Location = new System.Drawing.Point(996, 5);
            showVerboseLog.Name = "showVerboseLog";
            showVerboseLog.Size = new System.Drawing.Size(149, 24);
            showVerboseLog.TabIndex = 0;
            showVerboseLog.Text = "Show verbose log";
            showVerboseLog.UseVisualStyleBackColor = true;
            showVerboseLog.CheckedChanged += showVerboseLogCheckedChanged;
            // 
            // portCollectionButtonContainer
            // 
            portCollectionButtonContainer.AutoSize = true;
            portCollectionButtonContainer.Controls.Add(portCollectionButton);
            portCollectionButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            portCollectionButtonContainer.Location = new System.Drawing.Point(0, 5);
            portCollectionButtonContainer.MinimumSize = new System.Drawing.Size(0, 36);
            portCollectionButtonContainer.Name = "portCollectionButtonContainer";
            portCollectionButtonContainer.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            portCollectionButtonContainer.Size = new System.Drawing.Size(1148, 36);
            portCollectionButtonContainer.TabIndex = 1;
            // 
            // portCollectionButton
            // 
            portCollectionButton.AutoSize = true;
            portCollectionButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            portCollectionButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            portCollectionButton.Location = new System.Drawing.Point(13, 3);
            portCollectionButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            portCollectionButton.Name = "portCollectionButton";
            portCollectionButton.Size = new System.Drawing.Size(98, 30);
            portCollectionButton.TabIndex = 0;
            portCollectionButton.Text = "Collection 1";
            portCollectionButton.UseVisualStyleBackColor = true;
            // 
            // portCollectionButtonContainerContainer
            // 
            portCollectionButtonContainerContainer.AutoSize = true;
            portCollectionButtonContainerContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            portCollectionButtonContainerContainer.Controls.Add(portCollectionButtonContainer);
            portCollectionButtonContainerContainer.Dock = System.Windows.Forms.DockStyle.Top;
            portCollectionButtonContainerContainer.Location = new System.Drawing.Point(0, 0);
            portCollectionButtonContainerContainer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            portCollectionButtonContainerContainer.Name = "portCollectionButtonContainerContainer";
            portCollectionButtonContainerContainer.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            portCollectionButtonContainerContainer.Size = new System.Drawing.Size(1148, 41);
            portCollectionButtonContainerContainer.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(1148, 358);
            Controls.Add(logTextBox);
            Controls.Add(bottomPanel);
            Controls.Add(switchTableContainer);
            Controls.Add(portTableContainer);
            Controls.Add(portCollectionButtonContainerContainer);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Location = new System.Drawing.Point(50, 50);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "EasyVLANs";
            Load += MainForm_Load;
            portTable.ResumeLayout(false);
            portTable.PerformLayout();
            portTableContainer.ResumeLayout(false);
            portTableContainer.PerformLayout();
            switchTableContainer.ResumeLayout(false);
            switchTableContainer.PerformLayout();
            switchTable.ResumeLayout(false);
            switchTable.PerformLayout();
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            portCollectionButtonContainer.ResumeLayout(false);
            portCollectionButtonContainer.PerformLayout();
            portCollectionButtonContainerContainer.ResumeLayout(false);
            portCollectionButtonContainerContainer.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Label titlePortStatusSet;
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
        private System.Windows.Forms.FlowLayoutPanel portCollectionButtonContainer;
        private System.Windows.Forms.Button portCollectionButton;
        private System.Windows.Forms.LinkLabel githubLink;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label titleSwitchStatusPersist;
        private System.Windows.Forms.Label rowSwitchStatusPersist;
        private System.Windows.Forms.Panel portCollectionButtonContainerContainer;
        private System.Windows.Forms.Label titlePortStatus;
        private PortStatusDisplay rowPortStatusDisplay;
        private System.Windows.Forms.Label titlePortSpeed;
        private PortSpeedDisplay rowPortStatusSpeed;
    }
}

