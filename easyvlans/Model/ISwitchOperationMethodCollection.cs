using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace easyvlans.Model
{
    public interface ISwitchOperationMethodCollection : IMethod
    {

        IReadConfigMethod ReadConfigMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }

        internal interface IFactory : IFactory<ISwitchOperationMethodCollection>
        {
            ISwitchOperationMethodCollection GetInstance(XmlNode configNode, Switch @switch);
        }

    }
}
