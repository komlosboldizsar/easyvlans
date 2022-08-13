using B.XmlDeserializer;
using B.XmlDeserializer.Context;
using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Helpers;
using easyvlans.Logger;

namespace easyvlans.Model.Deserializers
{
    public class ConfigDeserializer
    {

        static ConfigDeserializer() => rootDeserializer = new(createDeserializer(), rootDeserializerContextInitializer);

        private const string FILE_CONFIG = "config.xml";

        public Config LoadConfig()
        {
            if (File.Exists(FILE_CONFIG))
            {
                try
                {
                    Config config = rootDeserializer.Deserialize(FILE_CONFIG, out DeserializationContext context, reportHandler);
                    int infoReportsCount = context.Reports.Count(r => r.Severity == DeserializationReportSeverity.Info);
                    if (infoReportsCount > 0)
                        LogDispatcher.I($"{infoReportsCount} verbose messages from configuration XML deserialization process.");
                    return config;
                }
                catch (DeserializationException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new DeserializationException("An exception was thrown while deserializing configuration XML!", ex);
                }
            }
            else
            {
                throw new DeserializationException($"Couldn't find {FILE_CONFIG}!");
            }
        }

        private void reportHandler(DeserializationContext context, IDeserializationReport report)
        {
            LogMessageSeverity severity = report.Severity switch
            {
                DeserializationReportSeverity.Info => LogMessageSeverity.Verbose,
                DeserializationReportSeverity.Warning => LogMessageSeverity.Warning,
                DeserializationReportSeverity.Error => LogMessageSeverity.Error,
                _ => LogMessageSeverity.Info
            };
            LogDispatcher.Log(severity, $"{report.XmlNode.GetPath()} :: {context.TranslateReportMessage(report)}");
        }

        private static readonly RootDeserializer<Config> rootDeserializer;

        private static IDeserializer<Config, Config> createDeserializer()
        {

            TypedCompositeDeserializer<Config, Config> configDeserializer = new(ConfigTagNames.ROOT, () => new Config());

            TypedCompositeDeserializer<Config.SettingsGroups, Config> settingsDeserializer = new(ConfigTagNames.SETTINGS, () => new Config.SettingsGroups());
            settingsDeserializer.Register(new SnmpSettingsDeserializer(), (settingsGroups, snmpSettings) => settingsGroups.Snmp = snmpSettings);
            configDeserializer.Register(settingsDeserializer, (config, settings) => config.Settings = settings);

            SimpleDictionaryDeserializer<string, Switch, Config> switchesDeserializer = new(ConfigTagNames.SWITCHES, new SwitchDeserializer(), @switch => @switch.ID);
            configDeserializer.Register(switchesDeserializer, (config, switches) => config.Switches = switches);

            SimpleDictionaryDeserializer<int, Vlan, Config> vlansDeserializer = new(ConfigTagNames.VLANS, new VlanDeserializer(), vlan => vlan.ID);
            SimpleDictionaryDeserializer<string, Vlanset, Config> vlansetsDeserializer = new(ConfigTagNames.VLANS, new VlansetDeserializer(), vlanset => vlanset.ID);
            MultiDeserializer<Config> vlansVlansetsDeserializer = new();
            var vlansDeserializerRegistration = vlansVlansetsDeserializer.Register(vlansDeserializer);
            var vlansetsDeserializerRegistration = vlansVlansetsDeserializer.Register(vlansetsDeserializer);
            configDeserializer.Register(vlansVlansetsDeserializer, (config, vlansVlansetsResult) =>
            {
                vlansVlansetsResult.GetForRegistration(vlansDeserializerRegistration, vlans => config.Vlans = vlans);
                vlansVlansetsResult.GetForRegistration(vlansetsDeserializerRegistration, vlansets => config.Vlansets = vlansets);
            });

            HeterogenousListDeserializer<object, Config> portsPortpagesDeserializer = new(ConfigTagNames.PORTS);
            portsPortpagesDeserializer.Register(new PortDeserializer());
            portsPortpagesDeserializer.Register(new PortPageDeserializer());
            configDeserializer.Register(portsPortpagesDeserializer, (config, portsAndPages) =>
            {
                List<Port> ports = new();
                List<PortPage> portPages = new();
                foreach (object portOrPage in portsAndPages)
                {
                    if (portOrPage is Port port)
                    {
                        ports.Add(port);
                    }
                    else if (portOrPage is PortPage portPage)
                    {
                        portPages.Add(portPage);
                        ports.AddRange(portPage);
                    }
                }
                config.Ports = ports;
                config.PortPages = portPages;
            });

            return configDeserializer;

        }

        private static void rootDeserializerContextInitializer(DeserializationContext context)
        {
            context.RegisterTypeName<Switch>("switch");
            context.RegisterTypeName<Port>("port");
            context.RegisterTypeName<PortPage>("page");
            context.RegisterTypeName<Vlan>("vlan");
            context.RegisterTypeName<Vlanset>("vlanset");
        }

    }
}
