using easyvlans.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class PortSpeedDisplay : UserControl
    {

        public PortSpeedDisplay()
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
                    _port.SpeedChanged -= portSpeedChangedHandler;
                    _port.LastStatusChangeChanged -= portLastStatusChangeChangedHandler;
                }
                _port = value;
                if (_port != null)
                {
                    _port.SpeedChanged += portSpeedChangedHandler;
                    _port.LastStatusChangeChanged += portLastStatusChangeChangedHandler;
                    update();
                }
            }
        }

        private void portSpeedChangedHandler(Port item, long? newValue)
        {
            update();
            reshowTooltip();
        }

        private void portLastStatusChangeChangedHandler(Port item, DateTime? newValue)
            => reshowTooltip();

        private void update()
            => displayStyle(getStyleFromSpeed());

        private StatusStyle getStyleFromSpeed()
            => getStyleForSpeed(_port.Speed);

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
            string speedStr = string.Empty;
            if (_port.Speed != null)
            {
                speedStr = $"{_port.Speed:N0}";
                if ((long)_port.Speed > 100_000)
                {
                    string speedReadableStr = $"{(_port.Speed / 1_000_000):N1} Mbps";
                    if ((long)_port.Speed > 1_000_000_000)
                        speedReadableStr += $", {(_port.Speed / 1_000_000_000):N1} Gbps";
                    speedStr += " (" + speedReadableStr + ")";
                }
            }
            string toolTipLabel = "Bits per second: " + speedStr;
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
        private static readonly StatusStyle ST_ZERO = new(Color.Maroon, Color.White, "0");
        private static readonly StatusStyle ST_OTHER = new(Color.PaleTurquoise, Color.Black, "other");
        private static readonly StatusStyle ST_10M = new(Color.Gold, Color.Black, "10M");
        private static readonly StatusStyle ST_100M = new(Color.Green, Color.White, "100M");
        private static readonly StatusStyle ST_1G = new(Color.Lime, Color.Black, "1G");
        private static readonly StatusStyle ST_10G = new(Color.Lime, Color.Black, "10G");

        private void displayStyle(StatusStyle statusStyle)
        {
            statusLabel.BackColor = statusStyle.Background;
            statusLabel.ForeColor = statusStyle.Foreground;
            statusLabel.Text = statusStyle.Str;
        }

        private StatusStyle getStyleForSpeed(long? speed)
        {
            if (speed == null)
                return ST_UNKNOWN;
            if ((speed >= MIN_ZERO) && (speed <= MAX_ZERO))
                return ST_ZERO;
            if ((speed >= MIN_10M) && (speed <= MAX_10M))
                return ST_10M;
            if ((speed >= MIN_100M) && (speed <= MAX_100M))
                return ST_100M;
            if ((speed >= MIN_1G) && (speed <= MAX_1G))
                return ST_1G;
            if ((speed >= MIN_10G) && (speed <= MAX_10G))
                return ST_10G;
            return ST_OTHER;
        }

        private static readonly long MIN_ZERO = 0;
        private static readonly long MAX_ZERO = 10;
        private static readonly long MIN_10M = 9_400_000;
        private static readonly long MAX_10M = 10_600_000;
        private static readonly long MIN_100M = 94_000_000;
        private static readonly long MAX_100M = 106_000_000;
        private static readonly long MIN_1G = 920_000_000;
        private static readonly long MAX_1G = 1_080_000_000;
        private static readonly long MIN_10G = 9_200_000_000;
        private static readonly long MAX_10G = 10_800_000_000;

    }
}
