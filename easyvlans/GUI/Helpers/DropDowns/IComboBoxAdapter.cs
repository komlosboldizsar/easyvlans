using System;
using System.ComponentModel;

namespace easyvlans.GUI.Helpers.DropDowns
{

    public interface IComboBoxAdapter : IListSource, ICloneable
    {
        bool ContainsNull { get; }
    }

}
