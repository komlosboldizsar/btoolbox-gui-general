﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.GUI.DropDowns
{

    public interface IComboBoxAdapterFactory
    {
        IComboBoxAdapter GetOne();
    }

}
