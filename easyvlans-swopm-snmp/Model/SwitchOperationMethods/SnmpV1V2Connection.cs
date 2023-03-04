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
        protected readonly IPEndPoint _ipEndPoint;
        protected readonly OctetString _readCommunityString;
        protected readonly OctetString _writeCommunityString;

        public SnmpV1V2Connection(Switch @switch, string ip, int port, string communityStrings)
        {
            Switch = @switch;
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
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
        }

        public async Task<List<Variable>> WalkAsync(string objectIdentifierStr)
        {
            try
            {
                string transactionId = GenerateTransactionId();
                LogDispatcher.V($"[{transactionId}] Walking from {objectIdentifierStr} using SNMP{Version}...");
                List<Variable> variables = await DoWalkAsync(objectIdentifierStr);
                int i = 0;
                foreach (Variable variable in variables)
                    LogDispatcher.VV($"[{transactionId}:{i++}] OID: [{variable.Id}], value: [{variable.Data.ToPrettyString()}]");
                LogDispatcher.V($"[{transactionId}] Walking from {objectIdentifierStr} using SNMP{Version} ready, got {variables.Count} variables.");
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

        public abstract string Version { get; }

        private const int TRANSACTION_ID_LENGTH = 4;
        private string GenerateTransactionId() => RandomStringGenerator.RandomString(TRANSACTION_ID_LENGTH);

    }
}
