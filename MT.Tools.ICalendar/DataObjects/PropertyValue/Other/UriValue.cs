using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Other
{
    public class UriValue : IPropertyValueImpl
    {
        #region Constructor

        public UriValue() { }

        public UriValue(string uri)
        {
            Value = new Uri(uri);
        }

        public UriValue(Uri uri)
        {
            Value = uri;
        }

        #endregion Constructor

        #region Members

        public Uri Value { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            Value = new Uri(content);
        }

        public string Serialize()
        {
            return Value.ToString();
        }

        #endregion Methods
    }
}
