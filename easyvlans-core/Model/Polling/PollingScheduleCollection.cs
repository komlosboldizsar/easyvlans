using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using easyvlans.Model.SwitchOperationMethods;
using System.Xml;

namespace easyvlans.Model.Polling
{
    public class PollingScheduleCollection
    {

        private PollingSchedule _defaultSchedule = null;
        private readonly Dictionary<string, PollingSchedule> _schedulesForMethods = new();

        public void Set(string methodCode, int interval, int offset)
        {
            PollingSchedule schedule = (interval > 0) ? new(interval, offset) : null;
            if (methodCode == null)
                _defaultSchedule = schedule;
            else
                _schedulesForMethods[methodCode] = schedule;
        }

        public PollingSchedule Get(string methodCode)
        {
            if (_schedulesForMethods.TryGetValue(methodCode, out PollingSchedule intervalForMethod))
                return intervalForMethod;
            return _defaultSchedule;
        }

        public static PollingScheduleCollection GetFromXml(XmlNode switchRootNode, DeserializationContext context)
        {
            PollingScheduleCollection pollingIntervals = new();
            foreach (XmlNode pollNode in switchRootNode.SelectNodes(NODE_POLL))
            {
                string methodCode = pollNode.AttributeAsString(ATTRIBUTE_METHOD, context).Get().Value;
                int interval = (int)pollNode.AttributeAsInt(ATTRIBUTE_INTERVAL, context).Mandatory().Min(0).Get().Value;
                int? offset = pollNode.AttributeAsInt(ATTRIBUTE_OFFSET, context).Default(0).Min(0).Get().Value;
                if ((methodCode != null) && !POLLABLE_METHODS.Contains(methodCode))
                {
                    context.Report(DeserializationReportSeverity.Warning, pollNode, $"Polling not possible for method '{methodCode}'.");
                    continue;
                }
                pollingIntervals.Set(methodCode, interval, (int)offset);
            }
            return pollingIntervals;
        }

        public const string NODE_POLL = "poll";
        private const string ATTRIBUTE_METHOD = "method";
        private const string ATTRIBUTE_INTERVAL = "interval";
        private const string ATTRIBUTE_OFFSET = "offset";

        private static readonly string[] POLLABLE_METHODS =
        {
            MethodCodes.METHOD__READ_INTERFACE_STATUS,
            MethodCodes.METHOD__READ_VLAN_MEMBERSHIP
        };

    }

}
