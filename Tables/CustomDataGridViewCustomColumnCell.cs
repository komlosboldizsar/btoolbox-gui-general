using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BToolbox.GUI.Tables
{
    public class CustomDataGridViewCustomColumnCell : DataGridViewCell
    {
        public virtual void BeforeAddToRow() { }
        public virtual void AfterAddToRow() { }
    }
}
