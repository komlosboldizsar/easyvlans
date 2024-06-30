using easyvlans.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class PortStatusDisplay : UserControl
    {

        public PortStatusDisplay()
        {
            InitializeComponent();
            displayStyle(ST_UNKNOWN);
        }

        private readonly ToolTip toolTip = new();

        private Port _port;
        public Port Port
        {
            get => _port;
            set
            {
                if (value == _port)
                    return;
                if (_port != null)
                {
                    _port.AdministrativeStatusChanged -= portStatusChangedHandler;
                    _port.OperationalStatusChanged -= portStatusChangedHandler;
                    _port.AdministrativeStatusStringChanged -= portStatusStringChangedHandler;
                    _port.OperationalStatusStringChanged -= portStatusStringChangedHandler;
                    _port.LastStatusChangeChanged -= portLastStatusChangeChangedHandler;
                }
                _port = value;
                if (_port != null)
                {
                    _port.AdministrativeStatusChanged += portStatusChangedHandler;
                    _port.OperationalStatusChanged += portStatusChangedHandler;
                    _port.AdministrativeStatusStringChanged += portStatusStringChangedHandler;
                    _port.OperationalStatusStringChanged += portStatusStringChangedHandler;
                    _port.LastStatusChangeChanged += portLastStatusChangeChangedHandler;
                    update();
                }
            }
        }

        private void portStatusChangedHandler(Port item, PortStatus newValue)
        {
            update();
            reshowTooltip();
        }

        private void portStatusStringChangedHandler(Port item, string newValue)
            => reshowTooltip();

        private void portLastStatusChangeChangedHandler(Port item, DateTime? newValue)
            => reshowTooltip();

        private void update()
            => displayStyle(getStyleFromAdministrativeStatus());

        private StatusStyle getStyleFromAdministrativeStatus()
        {
            return _port.AdministrativeStatus switch
            {
                PortStatus.Down => ST_ADM_DOWN,
                PortStatus.Up => getStyleFromOperationalStatus(),
                PortStatus.Unknown => ST_UNKNOWN,
                _ => ST_OTHER
            };
        }

        private StatusStyle getStyleFromOperationalStatus()
        {
            return _port.OperationalStatus switch
            {
                PortStatus.Down => ST_DOWN,
                PortStatus.Up => ST_UP,
                PortStatus.Unknown => ST_UNKNOWN,
                _ => ST_OTHER
            };
        }

        bool tooltipShown = true;

        private void statusLabel_MouseEnter(object sender, EventArgs e)
        {
            tooltipShown = true;
            showTooltip();
        }

        private void statusLabel_MouseLeave(object sender, EventArgs e)
        {
            tooltipShown = false;
            toolTip.Hide(statusLabel);
        }

        private void showTooltip()
        {
            string toolTipLabel = "Administrative status: " + _port?.AdministrativeStatusString;
            toolTipLabel += "\r\nOperational status: " + _port.OperationalStatusString;
            string lastChangeStr = _port.LastStatusChange?.ToString("yyyy.MM.dd. HH:mm:ss") ?? string.Empty;
            toolTipLabel += $"\r\nLast change: {lastChangeStr}";
            toolTip.Show(toolTipLabel, statusLabel);
        }

        private void reshowTooltip()
        {
            if (!tooltipShown)
                return;
            showTooltip();
        }

        private record StatusStyle(Color Background, Color Foreground, string Str);
        private static readonly StatusStyle ST_UNKNOWN = new(Color.Silver, Color.Black, "unknw");
        private static readonly StatusStyle ST_OTHER = new(Color.Gold, Color.Black, "other");
        private static readonly StatusStyle ST_ADM_DOWN = new(Color.Maroon, Color.White, "a.down");
        private static readonly StatusStyle ST_DOWN = new(Color.Green, Color.White, "down");
        private static readonly StatusStyle ST_UP = new(Color.Lime, Color.Black, "up");

        private void displayStyle(StatusStyle statusStyle)
        {
            statusLabel.BackColor = statusStyle.Background;
            statusLabel.ForeColor = statusStyle.Foreground;
            statusLabel.Text = statusStyle.Str;
        }

    }
}
