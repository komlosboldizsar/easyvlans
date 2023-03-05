using easyvlans.GUI.Helpers;
using easyvlans.Logger;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
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
            showPages();
            createPortsTable();
            showDefaultPortPage();
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
        
        private void showPages()
        {
            if (config.PortPages.Count == 0)
            {
                portPageButtonContainer.Visible = false;
                return;
            }
            int portPageIndex = 0;
            foreach (PortPage portPage in config.PortPages)
            {
                Button newPortPageButton = (portPageIndex > 0) ? portPageButton.CloneT() : portPageButton;
                newPortPageButton.Text = portPage.Title;
                newPortPageButton.Tag = portPage;
                newPortPageButton.Click += portPageButtonClick;
                if (portPageIndex > 0)
                    portPageButtonContainer.Controls.Add(newPortPageButton);
                portPageIndex++;
            }
        }

        private RecyclerTableLayoutManager<Port, PortRowManager> portTableManager;

        private void createPortsTable()
        {
            if (config.Ports.Count == 0)
                return; // Todo...
            int maxPortRowCount = config.Ports.Where(p => p.Page == null).Count() + config.Ports.GroupBy(p => p.Page).Max(gp => gp.Count());
            portTableManager = new(this, portTable, 1);
            portTableManager.CreateAllRows(maxPortRowCount);
        }

        private void showDefaultPortPage()
            => showPortPage(config.PortPages.FirstOrDefault(pp => pp.IsDefault) ?? config.PortPages.FirstOrDefault());

        private RecyclerTableLayoutManager<Switch, SwitchRowManager> switchTableManager;

        private void createAndShowSwitchesTable()
        {
            if (config.Switches.Count == 0)
                return; // Todo...
            switchTableManager = new(this, switchTable, 1);
            switchTableManager.CreateAllRows(config.Switches.Count);
            switchTableManager.BindItems(config.Switches.Values);
        }

        private void portPageButtonClick(object sender, EventArgs e) => showPortPage(((Button)sender).Tag as PortPage);

        private void showPortPage(PortPage portPage)
        {
            foreach (Button btn in portPageButtonContainer.Controls.OfType<Button>())
            {
                bool selected = ReferenceEquals(btn.Tag, portPage); // == gives a CS0252
                btn.BackColor = selected ? Color.DarkBlue : SystemColors.Control;
                btn.ForeColor = selected ? Color.White : SystemColors.ControlText;
            }
            IEnumerable<Port> shownPorts = config.Ports.Where(p => ((p.Page == null) || (p.Page == portPage)));
            portTableManager.BindItems(shownPorts);
        }

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
            => this.InvokeIfRequired(() => {
                Show();
                WindowState = lastWindowStateNotMinimized;
                Activate();
            });

        private void newLogMessageHandler(DateTime Timestamp, LogMessageSeverity severity, string message)
            => logTextBox.InvokeIfRequired(() => addLogMessage(Timestamp, severity, message));

        private void addLogMessage(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            if (!showVerboseLog.Checked && (severity >= LogMessageSeverity.Verbose))
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
            foreach (LogMessage logMessage in LogDispatcher.Messages)
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
