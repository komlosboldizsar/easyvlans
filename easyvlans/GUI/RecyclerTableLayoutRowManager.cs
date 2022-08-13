using System.Windows.Forms;

namespace easyvlans.GUI
{
    internal abstract partial class RecyclerTableLayoutRowManager<TItem>
        where TItem : class
    {

        protected abstract IMemberBinding[] MemberBindings { get; }
        protected TableLayoutPanel Table { get; private set; }
        protected Control[] Controls { get; private set; }

        public void Init(TableLayoutPanel table)
        {
            Table = table;
        }

        public void SetMembers(Control[] controls)
        {
            Controls = controls;
            foreach (IMemberBinding memberBinding in MemberBindings)
                memberBinding.Set(controls);
        }

        public virtual void SubscribeControlEvents() { }

        public bool Shown
        {
            set
            {
                foreach (Control control in Controls)
                    control.Visible = value;
            }
        }

        private TItem _item;
        public TItem Item
        {
            get => _item;
            set
            {
                if (value == _item)
                    return;
                if (_item != null)
                    DebindItem();
                _item = value;
                if (_item != null)
                {
                    BindItem();
                    Shown = true;
                }
                else
                {
                    Shown = false;
                }
            }
        }

        protected abstract void DebindItem();
        protected abstract void BindItem();

    }
}
