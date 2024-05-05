using BToolbox.OneInstance;
using easyvlans.GUI.Helpers;
using easyvlans.Logger;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class MainForm : Form
    {

        private readonly Config config;
        private readonly string parsingError;
        private readonly bool oneInstanceMode;
        private readonly bool hideOnStartup;
        private bool hidingOnStartup;

        public MainForm() => InitializeComponent();

        public MainForm(Config config, string parsingError, bool oneInstanceMode, bool hideOnStartup)
        {
            InitializeComponent();
            LogDispatcher.NewLogMessage += newLogMessageHandler;
            this.config = config;
            this.parsingError = parsingError;
            this.oneInstanceMode = oneInstanceMode;
            this.hideOnStartup = hideOnStartup;
            lastWindowStateNotMinimized = WindowState;
            Resize += MainForm_Resize;
            if (oneInstanceMode)
            {
                trayIcon.Visible = true;
                OneInstancePipe.ShowMessageReceived += oneInstanceShowMessageReceived;
            }
            if (hideOnStartup)
            {
                hidingOnStartup = true;
                ShowInTaskbar = false;
                Opacity = 0;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            reloadLogMessages();
            string errorToShow = parsingError;
            if ((errorToShow == null) && (config == null))
                errorToShow = "Couldn't load configuration, reason unknown.";
            if (errorToShow != null)
            {
                rowPortPortLabel.Text = "N/A";
                rowPortPortLabel.ForeColor = Color.Red;
                rowPortSwitch.Text = "N/A";
                rowPortPortIndex.Text = "N/A";
                rowPortCurrentVlan.Text = "N/A";
                rowPortSetVlanTo.Enabled = false;
                rowPortSet.Enabled = false;
                rowPostStatusSetVlan.Text = "N/A";
                rowSwitchSwitchName.Text = "N/A";
                rowSwitchSwitchName.ForeColor = Color.Red;
                rowSwitchPendingChanges.Text = "N/A";
                rowSwitchPersistChanges.Enabled = false;
                rowSwitchStatusRead.Text = "N/A";
                MessageBox.Show(errorToShow, "Initialization error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            initCollections();
            createPortsTable();
            selectPortCollection(config.PortCollection);
            createAndShowSwitchesTable();
            if (hideOnStartup)
            {
                Hide();
                Opacity = 1;
                ShowInTaskbar = true;
                hidingOnStartup = false;
            }
        }

        protected override bool ShowWithoutActivation => hidingOnStartup;

        private List<FlowLayoutPanel> portCollectionButtonContainers = new();

        private void initCollections()
        {
            if (config.PortCollectionStructure.Depth == 0)
            {
                portCollectionButtonContainer.Visible = false;
                return;
            }
            else
            {
                portCollectionButtonContainers.Add(portCollectionButtonContainer);
                if (config.PortCollectionStructure.Depth > 1)
                {
                    for (int level = 1; level < config.PortCollectionStructure.Depth; level++)
                    {
                        FlowLayoutPanel newLevelContainer = portCollectionButtonContainer.CloneT();
                        portCollectionButtonContainerContainer.Controls.Add(newLevelContainer);
                        portCollectionButtonContainers.Add(newLevelContainer);
                    }
                }
            }
            for (int level = 0; level < config.PortCollectionStructure.Depth; level++)
            {
                for (int buttonIndex = 0; buttonIndex < config.PortCollectionStructure.MaxSubCollectionCount[level]; buttonIndex++)
                {
                    bool existingButton = ((level == 0) && (buttonIndex == 0));
                    {
                        Button newPortCollectionButton = existingButton ? portCollectionButton : portCollectionButton.CloneT();
                        newPortCollectionButton.Click += portCollectionButtonClick;
                        if (!existingButton)
                            portCollectionButtonContainers[level].Controls.Add(newPortCollectionButton);
                    }
                }
            }
        }

        private RecyclerTableLayoutManager<Port, PortRowManager> portTableManager;

        private void createPortsTable()
        {
            if (config.Ports.Count == 0)
                return; // Todo...
            portTableManager = new(this, portTable, 1);
            portTableManager.CreateAllRows(config.PortCollectionStructure.MaxVisibleSize);
        }

        private RecyclerTableLayoutManager<Switch, SwitchRowManager> switchTableManager;

        private void createAndShowSwitchesTable()
        {
            if (config.Switches.Count == 0)
                return; // Todo...
            switchTableManager = new(this, switchTable, 1);
            switchTableManager.CreateAllRows(config.Switches.Count);
            switchTableManager.BindItems(config.Switches.Values);
        }

        private void portCollectionButtonClick(object sender, EventArgs e)
            => selectPortCollection(((Button)sender).Tag as PortCollection);

        private void selectPortCollection(PortCollection portCollection)
        {
            // store last selected subcollection
            if (portCollection.Parent != null)
            {
                lastSelectedSubCollections[portCollection.Parent] = portCollection;
            }
            // show active collection on the level of the selected collection
            if (portCollection.Level > 0)
            {
                foreach (Button button in portCollectionButtonContainers[portCollection.Level - 1].Controls.OfType<Button>())
                {
                    bool selected = ReferenceEquals(button.Tag, portCollection); // == gives a CS0252
                    button.BackColor = selected ? ACTIVE_COLLECTION_BUTTON_COLORS[(portCollection.Level - 1) % ACTIVE_COLLECTION_BUTTON_COLORS.Length] : SystemColors.Control;
                    button.ForeColor = selected ? Color.White : SystemColors.ControlText;
                }
            }
            // update levels below the selected collection (subcollections)
            if (config.PortCollectionStructure.Depth > portCollection.Level)
            {
                IEnumerable<PortCollection> subCollections = portCollection.OfType<PortCollection>();
                IEnumerator<PortCollection> subCollectionsEnumerator = subCollections.GetEnumerator();
                foreach (Button button in portCollectionButtonContainers[portCollection.Level].Controls.OfType<Button>())
                {
                    if (subCollectionsEnumerator.MoveNext())
                    {
                        PortCollection subCollection = subCollectionsEnumerator.Current;
                        button.Visible = true;
                        button.Text = subCollection.Title;
                        button.Tag = subCollection;
                    }
                    else
                    {
                        button.Visible = false;
                    }
                }
                // last selected...
                if (!portCollection.RememberLastSelectedSubCollection || !lastSelectedSubCollections.TryGetValue(portCollection, out PortCollection subCollectionToSelect))
                {
                    // ...or default...
                    subCollectionToSelect = subCollections.FirstOrDefault(pc => pc.IsDefault);
                    // ...or first
                    subCollectionToSelect ??= subCollections.FirstOrDefault();
                }
                if (subCollectionToSelect != null)
                    selectPortCollection(subCollectionToSelect);
                else
                    showPortsOfCollection(portCollection);
            }
            else
            {
                showPortsOfCollection(portCollection);
            }
        }

        private void showPortsOfCollection(PortCollection portCollection)
        {
            LinkedList<Port> ports = new();
            PortCollection previousChild = null;
            while (portCollection != null)
            {
                LinkedListNode<Port> addAfter = null;
                foreach (IPortOrPortCollection portOrPortCollection in portCollection)
                {
                    if (portOrPortCollection is Port port)
                        addAfter = (addAfter == null) ? ports.AddFirst(port) : ports.AddAfter(addAfter, port);
                    else if (portOrPortCollection == previousChild)
                        addAfter = ports.Last;
                }
                previousChild = portCollection;
                portCollection = portCollection.Parent;
            }
            portTableManager.BindItems(ports);
        }

        private Color[] ACTIVE_COLLECTION_BUTTON_COLORS = new[] { Color.DarkBlue, Color.DarkRed, Color.DarkGreen };

        private Dictionary<PortCollection, PortCollection> lastSelectedSubCollections = new();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (oneInstanceMode && !closingFromTrayMenu)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
            => Show();

        bool closingFromTrayMenu = false;

        private void trayMenuExit_Click(object sender, EventArgs e)
        {
            closingFromTrayMenu = true;
            Close();
        }

        FormWindowState lastWindowStateNotMinimized;

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                lastWindowStateNotMinimized = WindowState;
        }

        private void oneInstanceShowMessageReceived()
            => this.InvokeIfRequired(() =>
            {
                Show();
                WindowState = lastWindowStateNotMinimized;
                Activate();
            });

        private void newLogMessageHandler(DateTime Timestamp, LogMessageSeverity severity, string message)
            => logTextBox.InvokeIfRequired(() => addLogMessage(Timestamp, severity, message));

        private void addLogMessage(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            if ((severity > MAX_LOG_MSG_SEVERITY_VERBOSE) || (!showVerboseLog.Checked && (severity > MAX_LOG_MSG_SEVERITY_NORMAL)))
                return;
            string textToAdd = $"[{timestamp:HH:mm:ss}] {message}\r\n";
            logTextBox.AppendText(textToAdd);
            int textLength = logTextBox.TextLength;
            int selectionLength = textToAdd.Length;
            int selectionStart = textLength - selectionLength + 1;
            if (selectionStart < 0)
            {
                selectionStart = 0;
                selectionLength = 0;
            }
            logTextBox.Select(selectionStart, selectionLength);
            logTextBox.SelectionColor = logColors[severity];
            logTextBoxScrollToEnd();
        }

        private const LogMessageSeverity MAX_LOG_MSG_SEVERITY_NORMAL = LogMessageSeverity.Info;
        private const LogMessageSeverity MAX_LOG_MSG_SEVERITY_VERBOSE = LogMessageSeverity.Verbose;

        private void logTextBox_Resize(object sender, EventArgs e)
            => logTextBoxScrollToEnd();

        private void logTextBoxScrollToEnd()
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }

        private void showVerboseLogCheckedChanged(object sender, EventArgs e) => reloadLogMessages();

        private void reloadLogMessages()
        {
            logTextBox.Text = string.Empty;
            List<LogMessage> messages = new(LogDispatcher.Messages);
            foreach (LogMessage logMessage in messages)
                addLogMessage(logMessage.Timestamp, logMessage.Severity, logMessage.Message);
        }

        private static readonly Dictionary<LogMessageSeverity, Color> logColors = new()
        {
            { LogMessageSeverity.Error, Color.Red },
            { LogMessageSeverity.Warning, Color.Orange },
            { LogMessageSeverity.Info, Color.Black },
            { LogMessageSeverity.Verbose, Color.Blue },
            { LogMessageSeverity.VerbosePlus, Color.BlueViolet }
        };

        private static void openUrl(string url) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });

        private void githubLinkClickHandler(object sender, LinkLabelLinkClickedEventArgs e) => openUrl(URL_GITHUB);
        private void issueReportLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => openUrl(URL_GITHUB_ISSUEREPORT);

        private const string URL_GITHUB = @"http://github.com/komlosboldizsar/easyvlans";
        private const string URL_GITHUB_ISSUEREPORT = @"http://github.com/komlosboldizsar/easyvlans/issues/new";

    }

}
