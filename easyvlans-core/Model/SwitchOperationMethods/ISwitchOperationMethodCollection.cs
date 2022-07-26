﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISwitchOperationMethodCollection : IMethod
    {

        IReadConfigMethod ReadConfigMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }

        public interface IFactory : IFactory<ISwitchOperationMethodCollection>
        {
            ISwitchOperationMethodCollection GetInstance(XmlNode configNode, Switch @switch);
        }

    }
}