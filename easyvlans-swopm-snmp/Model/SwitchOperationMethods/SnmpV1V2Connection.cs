using BToolbox.SNMP;
using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal abstract class SnmpV1V2Connection : ISnmpConnection
    {

        public Switch Switch { get; }
        private string _ipEndPointString;
        protected readonly IPEndPoint _ipEndPoint;
        protected readonly OctetString _readCommunityString;
        protected readonly OctetString _writeCommunityString;
        protected readonly int? _trapPort;
        protected readonly string _trapCommunityString;
        protected readonly bool _trapVersionStrict;
        protected readonly TrapReceiver _trapReceiver;

        public SnmpV1V2Connection(Switch @switch, string ip, int port, string communityStrings, int? trapPort, string trapCommunityString, bool trapVersionStrict)
        {
            Switch = @switch;
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _ipEndPointString = _ipEndPoint.ToString();
            string[] communityStringParts = communityStrings.Split(':');
            if (communityStringParts.Length > 1)
            {
                _readCommunityString = new OctetString(communityStringParts[0]);
                _writeCommunityString = new OctetString(communityStringParts[1]);
            }
            else
            {
                _readCommunityString = _writeCommunityString = new OctetString(communityStrings);
            }

            if ((trapPort != null) && (trapCommunityString != null))
            {
                _trapPort = trapPort;
                _trapCommunityString = trapCommunityString;
                _trapVersionStrict = trapVersionStrict;
                _trapReceiver = MultiportTrapReceiver.GetForPort((int)trapPort);
            }
        }

        public async Task<IList<Variable>> GetAsync(IEnumerable<string> objectIdentifierStrs)
        {
            try
            {
                int variableCount = objectIdentifierStrs.Count();
                string transactionId = GenerateTransactionId();
                LogDispatcher.VV($"[{transactionId}] Getting {variableCount} variables @ {_ipEndPointString} using SNMP{Version}...");
                IList<Variable> variables = await DoGetAsync(objectIdentifierStrs);
                int i = 0;
                foreach (Variable variable in variables)
                    LogDispatcher.VV($"[{transactionId}:{i++}] OID: [{variable.Id}], value: [{variable.Data.ToPrettyString()}]");
                LogDispatcher.VV($"[{transactionId}] Getting {variableCount} variables using SNMP{Version} ready, got {variables.Count} variables.");
                return variables;
            }
            catch (ErrorException ex)
            {
                ISnmpPdu pdu = ex.Body?.Pdu();
                if (pdu != null)
                    LogDispatcher.E($"SNMP{Version} error when walking, status: [{pdu.ErrorStatus}], index: [{pdu.ErrorIndex}]");
                throw ex;
            }
        }

        public async Task<List<Variable>> WalkAsync(string objectIdentifierStr)
        {
            try
            {
                string transactionId = GenerateTransactionId();
                LogDispatcher.VV($"[{transactionId}] Walking from {objectIdentifierStr} @ {_ipEndPointString} using SNMP{Version}...");
                List<Variable> variables = await DoWalkAsync(objectIdentifierStr);
                int i = 0;
                foreach (Variable variable in variables)
                    LogDispatcher.VV($"[{transactionId}:{i++}] OID: [{variable.Id}], value: [{variable.Data.ToPrettyString()}]");
                LogDispatcher.VV($"[{transactionId}] Walking from {objectIdentifierStr} using SNMP{Version} ready, got {variables.Count} variables.");
                return variables;
            }
            catch (ErrorException ex)
            {
                ISnmpPdu pdu = ex.Body?.Pdu();
                if (pdu != null)
                    LogDispatcher.E($"SNMP{Version} error when walking, status: [{pdu.ErrorStatus}], index: [{pdu.ErrorIndex}]");
                throw ex;
            }
        }

        protected abstract Task<IList<Variable>> DoGetAsync(IEnumerable<string> objectIdentifierStrs);
        protected abstract Task<List<Variable>> DoWalkAsync(string objectIdentifierStr);

        public async Task SetAsync(List<Variable> variables)
        {
            try
            {
                string transactionId = GenerateTransactionId();
                LogDispatcher.V($"[{transactionId}] Setting {variables.Count} variables using SNMP{Version}...");
                int i = 0;
                foreach (Variable variable in variables)
                    LogDispatcher.VV($"[{transactionId}:{i++}] OID: [{variable.Id}], value: [{variable.Data.ToPrettyString()}]");
                await DoSetAsync(variables);
                LogDispatcher.V($"[{transactionId}] Setting {variables.Count} variables using SNMP{Version} ready.");
            }
            catch (ErrorException ex)
            {
                ISnmpPdu pdu = ex.Body?.Pdu();
                if (pdu != null)
                    LogDispatcher.E($"SNMP{Version} error when setting, status: [{pdu.ErrorStatus}], index: [{pdu.ErrorIndex}]");
                throw ex;
            }
        }

        protected abstract Task DoSetAsync(List<Variable> variables);

        public abstract void SubscribeForTrap(ITrapSubscriber subscriber, GenericCode v1GenericCode, int? v1SpecificCode, string v1EnterpriseFilter, ObjectIdentifier v2TrapOid);

        public void SubscribeForTrap(ITrapSubscriber subscriber, TrapDescriptor descriptor)
            => SubscribeForTrap(subscriber, descriptor.V1GenericCode, descriptor.V1SpecificCode, descriptor.V1EnterpriseFilter, descriptor.V2TrapOid);

        public abstract string Version { get; }
        public abstract VersionCode VersionCode { get; }

        private const int TRANSACTION_ID_LENGTH = 4;
        private string GenerateTransactionId() => RandomStringGenerator.RandomString(TRANSACTION_ID_LENGTH);

    }
}
