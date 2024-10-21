using BToolbox.GUI.Forms;
using BToolbox.Logger;
using BToolbox.OneInstance;
using easyvlans.GUI.Helpers;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class MainForm : OneInstanceForm
    {

        private readonly Config _config;
        private readonly string _startupError;

        public MainForm() : base() => InitializeComponent();

        public MainForm(Config config, string parsingError, bool oneInstanceMode, bool hideOnStartup)
            : base(oneInstanceMode, hideOnStartup)
        {
            _config = config;
            _startupError = parsingError;
            InitializeComponent();
            _logRTBManager = new(logTextBox);
        }

        private LogRichTextBoxManager _logRTBManager;

        private void showVerboseLogCheckedChanged(object sender, EventArgs e)
            => _logRTBManager.ShowVerboseLog = showVerboseLog.Checked;

        private void MainForm_Load(object sender, EventArgs e)
        {
            _logRTBManager.Init();
            string errorToShow = _startupError;
            if ((errorToShow == null) && (_config == null))
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
            selectPortCollection(_config.PortCollection);
            createAndShowSwitchesTable();
        }

        private List<FlowLayoutPanel> portCollectionButtonContainers = new();

        private void initCollections()
        {
            if (_config.PortCollectionStructure.Depth == 0)
            {
                portCollectionButtonContainer.Visible = false;
                return;
            }
            else
            {
                portCollectionButtonContainers.Add(portCollectionButtonContainer);
                if (_config.PortCollectionStructure.Depth > 1)
                {
                    for (int level = 1; level < _config.PortCollectionStructure.Depth; level++)
                    {
                        FlowLayoutPanel newLevelContainer = portCollectionButtonContainer.CloneT();
                        portCollectionButtonContainerContainer.Controls.Add(newLevelContainer);
                        portCollectionButtonContainers.Add(newLevelContainer);
                    }
                }
            }
            for (int level = 0; level < _config.PortCollectionStructure.Depth; level++)
            {
                for (int buttonIndex = 0; buttonIndex < _config.PortCollectionStructure.MaxSubCollectionCount[level]; buttonIndex++)
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
            if (_config.Ports.Count == 0)
                return; // Todo...
            portTableManager = new(this, portTable, 1);
            portTableManager.CreateAllRows(_config.PortCollectionStructure.MaxVisibleSize);
        }

        private RecyclerTableLayoutManager<Switch, SwitchRowManager> switchTableManager;

        private void createAndShowSwitchesTable()
        {
            if (_config.Switches.Count == 0)
                return; // Todo...
            switchTableManager = new(this, switchTable, 1);
            switchTableManager.CreateAllRows(_config.Switches.Count);
            switchTableManager.BindItems(_config.Switches.Values);
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
            if (_config.PortCollectionStructure.Depth > portCollection.Level)
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

        private static void openUrl(string url) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });

        private void githubLinkClickHandler(object sender, LinkLabelLinkClickedEventArgs e) => openUrl(URL_GITHUB);
        private void issueReportLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => openUrl(URL_GITHUB_ISSUEREPORT);

        private const string URL_GITHUB = @"http://github.com/komlosboldizsar/easyvlans";
        private const string URL_GITHUB_ISSUEREPORT = @"http://github.com/komlosboldizsar/easyvlans/issues/new";

    }

}
