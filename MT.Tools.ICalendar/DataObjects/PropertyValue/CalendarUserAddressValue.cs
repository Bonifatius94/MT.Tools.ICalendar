﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class CalendarUserAddressValue : IPropertyValueImpl
    {
        #region Constructor

        public CalendarUserAddressValue(Uri uri)
        {
            Uri = uri;
        }

        #endregion Constructor

        #region Members

        public Uri Uri { get; private set; }

        #endregion Members

        #region Methods

        // TODO: implement validation function

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
