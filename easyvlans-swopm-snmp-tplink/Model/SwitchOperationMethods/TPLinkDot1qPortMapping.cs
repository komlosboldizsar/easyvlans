using B.XmlDeserializer;
using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal record PortMapping(int LocalIndex, int SnmpIndex, int SimpleId, TPLinkDot1qThreePartPortId ThreePartId, int Count)
    {

        public int LocalIndexToSimpleId(int localIndex)
            => SimpleId + (localIndex - LocalIndex);

        public int SnmpIndexToLocalIndex(int snmpIndex)
            => LocalIndex + (snmpIndex - SnmpIndex);

        public int LocalIndexToSnmpIndex(int localIndex)
            => SnmpIndex + (localIndex - LocalIndex);

        public TPLinkDot1qThreePartPortId LocalIndexToThreePartId(int localIndex)
            => ThreePartId with { Counter = ThreePartId.Counter + (localIndex - LocalIndex) };

    }
}
