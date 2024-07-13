using BToolbox.GUI.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BToolbox.GUI.Tables
{
    public abstract class CustomDataGridViewDragHandler<TRowItem> : DragHandler<CustomDataGridViewDragSourceEventArgs<TRowItem>>
    { }
}
