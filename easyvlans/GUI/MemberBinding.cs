using System;
using System.Windows.Forms;

namespace easyvlans.GUI
{

    internal interface IMemberBinding
    {
        public void Set(Control[] controls);
    }

    internal class MemberBinding<TControl> : IMemberBinding
        where TControl : Control
    {
        private int columnIndex;
        private Action<TControl> setter;
        public MemberBinding(int columnIndex, Action<TControl> setter)
        {
            this.columnIndex = columnIndex;
            this.setter = setter;
        }
        public void Set(Control[] controls)
            => setter(controls[columnIndex] as TControl);
    }

}
