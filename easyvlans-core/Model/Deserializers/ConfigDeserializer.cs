using B.XmlDeserializer;
using B.XmlDeserializer.Context;
using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Helpers;
using easyvlans.Logger;
using easyvlans.Model.Remote;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Model.Deserializers
{
    public class ConfigDeserializer
    {

        static ConfigDeserializer()
        {
            Deserializer = createDeserializer();
            RootDeserializer = new(Deserializer, rootDeserializerContextInitializer);
        }

        public Config LoadConfig(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    Config config = RootDeserializer.Deserialize(fileName, out DeserializationContext context, reportHandler);
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
                throw new DeserializationException($"Couldn't find {fileName}!");
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

        public static readonly TypedCompositeDeserializer<Config, Config> Deserializer;
        public static readonly RootDeserializer<Config> RootDeserializer;

        private static TypedCompositeDeserializer<Config, Config> createDeserializer()
        {

            TypedCompositeDeserializer<Config, Config> configDeserializer = new(ConfigTagNames.ROOT, () => new Config());

            configDeserializer.Register(RemoteMethodsDeserializer.Instance, (config, remotes) => config.Remotes = remotes);

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
            foreach (KeyValuePair<Type, string> registeredTypeName in registeredTypeNames)
                context.RegisterTypeName(registeredTypeName.Key, registeredTypeName.Value);
            context.RegisterTypeName<Switch>("switch");
            context.RegisterTypeName<Port>("port");
            context.RegisterTypeName<PortPage>("page");
            context.RegisterTypeName<Vlan>("vlan");
            context.RegisterTypeName<Vlanset>("vlanset");
            context.RegisterTypeName<ISwitchOperationMethodCollection>("Switch operation method collection");
        }

        private static Dictionary<Type, string> registeredTypeNames = new();

        public static void RegisterTypeName<T>(string typeName)
            => registeredTypeNames.Add(typeof(T), typeName);

    }
}
