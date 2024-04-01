using Lextm.SharpSnmpLib;

namespace BToolbox.SNMP
{
    public class SnmpErrorCodeException : Exception
    {

        public ErrorCode ErrorCode { get; private init; }

        public SnmpErrorCodeException(ErrorCode errorCode)
            : base()
            => ErrorCode = errorCode;

        public SnmpErrorCodeException(ErrorCode errorCode, string message)
            : base(message)
            => ErrorCode = errorCode;

        public SnmpErrorCodeException(ErrorCode errorCode, string message, Exception innerException)
            : base(message, innerException)
            => ErrorCode = errorCode;

    }
}
