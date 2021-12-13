using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.GUI.Helpers.DropDowns
{

    public interface IComboBoxAdapter : IListSource, ICloneable
    {
        bool ContainsNull { get; }
    }

}
