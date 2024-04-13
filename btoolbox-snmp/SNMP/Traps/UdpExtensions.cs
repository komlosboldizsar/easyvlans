using System.Net;
using System.Net.Sockets;

namespace BToolbox.SNMP
{
    internal static class UdpExtensions
    {
        // @source https://stackoverflow.com/a/15087172
        public static IPEndPoint BestLocalEndPoint(this IPEndPoint remoteIPEndPoint)
        {
            using (Socket testSocket = new(remoteIPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp))
            {
                testSocket.Connect(remoteIPEndPoint);
                return (IPEndPoint)testSocket.LocalEndPoint;
            }
        }
    }
}
