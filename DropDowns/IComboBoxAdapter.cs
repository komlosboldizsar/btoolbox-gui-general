using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.GUI.DropDowns
{

    public interface IComboBoxAdapter : IListSource, ICloneable
    {

        bool ContainsNull { get; }

        public interface IItemProxy
        {
            object ObjValue { get; }
            string Label { get; }
        }

    }

}
