using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpMethod : IMethod
    {
        public new interface IFactory<TMethodInterface> : IMethod.IFactory<TMethodInterface>
            where TMethodInterface : ISnmpMethod
        {
            TMethodInterface GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent);
        }
    }
}
