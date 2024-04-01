using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.SNMP
{
    public class TrapEnterprise
    {
        public ObjectIdentifier EnterpriseBase { get; }
        public int SpecificCode { get; }
        public ObjectIdentifier EnterpriseFull { get; }

        public TrapEnterprise(string enterpriseBase, int specificCode)
        {
            EnterpriseBase = new(enterpriseBase);
            SpecificCode = specificCode;
            EnterpriseFull = new($"{enterpriseBase}.{specificCode}");
        }
    }
}
