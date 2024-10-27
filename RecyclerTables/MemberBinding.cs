using System;
using System.Windows.Forms;

namespace BToolbox.GUI.RecyclerTables
{

    public interface IMemberBinding
    {
        public void Set(Control[] controls);
    }

    public class MemberBinding<TControl> : IMemberBinding
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
