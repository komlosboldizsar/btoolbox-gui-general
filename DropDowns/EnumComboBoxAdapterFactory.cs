﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.GUI.DropDowns
{

    public class EnumComboBoxAdapterFactory<T> : IComboBoxAdapterFactory
        where T : Enum
    {

        Dictionary<T, string> translations;

        public EnumComboBoxAdapterFactory(Dictionary<T, string> translations = null)
            => this.translations = translations;

        public EnumComboBoxAdapter<T> GetOneT() => new EnumComboBoxAdapter<T>(translations);
        public IComboBoxAdapter GetOne() => GetOneT();

    }

}
