﻿using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class DirectoryEntryReferenceParameter : IPropertyParameter
    {
        #region Constructor

        public DirectoryEntryReferenceParameter() { }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.DirectoryEntryReference;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
