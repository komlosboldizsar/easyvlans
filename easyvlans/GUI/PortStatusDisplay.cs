using BToolbox.Helpers;
using easyvlans.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class PortStatusDisplay : PortDataDisplay
    {

        public PortStatusDisplay() => InitializeComponent();

        protected override void unsubscribeEvents()
        {
            _port.AdministrativeStatusChanged -= portStatusChangedHandler;
            _port.OperationalStatusChanged -= portStatusChangedHandler;
            _port.AdministrativeStatusStringChanged -= portStatusStringChangedHandler;
            _port.OperationalStatusStringChanged -= portStatusStringChangedHandler;
            _port.LastStatusChangeChanged -= portLastStatusChangeChangedHandler;
        }

        protected override void subscribeEvents()
        {
            _port.AdministrativeStatusChanged += portStatusChangedHandler;
            _port.OperationalStatusChanged += portStatusChangedHandler;
            _port.AdministrativeStatusStringChanged += portStatusStringChangedHandler;
            _port.OperationalStatusStringChanged += portStatusStringChangedHandler;
            _port.LastStatusChangeChanged += portLastStatusChangeChangedHandler;
        }

        private void portStatusChangedHandler(Port item, PortStatus newValue)
        {
            update();
            reshowTooltip();
        }

        private void portStatusStringChangedHandler(Port item, string newValue)
            => reshowTooltip();

        private void portLastStatusChangeChangedHandler(Port item, Port.LastStatusChangeData newValue)
            => reshowTooltip();

        protected override StatusStyle getStyleFromData()
            => _port.AdministrativeStatus switch
            {
                PortStatus.Down => ST_ADM_DOWN,
                PortStatus.Up => getStyleFromOperationalStatus(),
                PortStatus.Unknown => ST_UNKNOWN,
                _ => ST_OTHER
            };

        private StatusStyle getStyleFromOperationalStatus()
            => _port.OperationalStatus switch
            {
                PortStatus.Down => ST_DOWN,
                PortStatus.Up => ST_UP,
                PortStatus.Unknown => ST_UNKNOWN,
                _ => ST_OTHER
            };

        protected override string getTooltipText()
        {
            string toolTipLabel = "Administrative status: " + _port?.AdministrativeStatusString;
            toolTipLabel += "\r\nOperational status: " + _port.OperationalStatusString;
            string lastChangeStr = _port.LastStatusChange?.Timestamp?.ToString("yyyy.MM.dd. HH:mm:ss") ?? string.Empty;
            if (_port.LastStatusChange?.Source != null)
                lastChangeStr += $"({LAST_STATUS_CHANGE_SOURCE_TYPE_CONVERTER.Convert(_port.LastStatusChange.Source)})";
            toolTipLabel += $"\r\nLast change: {lastChangeStr}";
            return toolTipLabel;
        }

        private EnumToStringConverter<Port.LastStatusChangeSourceType> LAST_STATUS_CHANGE_SOURCE_TYPE_CONVERTER = new()
        {
            {  Port.LastStatusChangeSourceType.Absolute, "absolute" },
            {  Port.LastStatusChangeSourceType.BoottimeRelative, "boottime-relative" }
        };

        private static readonly StatusStyle ST_OTHER = new(Color.Gold, Color.Black, () => "other");
        private static readonly StatusStyle ST_ADM_DOWN = new(Color.Maroon, Color.White, () => "a.down");
        private static readonly StatusStyle ST_DOWN = new(Color.Green, Color.White, () => "down");
        private static readonly StatusStyle ST_UP = new(Color.Lime, Color.Black, () => "up");

    }
}
