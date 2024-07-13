using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BToolbox.GUI.Tables
{
    public abstract class CustomDataGridViewCustomColumnTypeDescriptor
    {
        public abstract DataGridViewColumn CreateColumn();
        public abstract DataGridViewCell CreateCell();
        public virtual void BeforeAddToRow(DataGridViewCell cell) { }
        public virtual void AfterAddToRow(DataGridViewCell cell) { }
        public virtual void Initialize(DataGridViewCell cell) { }
        public virtual void Update(DataGridViewCell cell) { }
    }
}
