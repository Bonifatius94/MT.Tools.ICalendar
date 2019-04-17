using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Other
{
    // TODO: determine what is the difference between this and UriValue

    public class CalendarUserAddressValue : IPropertyValueImpl
    {
        #region Constructor

        public CalendarUserAddressValue() { }

        public CalendarUserAddressValue(Uri uri)
        {
            _uri = new UriValue(uri);
        }

        #endregion Constructor

        #region Members

        private UriValue _uri;

        public Uri Address
        {
            get { return _uri.Value; }
            set { _uri.Value = value; }
        }

        #endregion Members

        #region Methods

        // TODO: implement validation function

        public void Deserialize(string content)
        {
            _uri = ObjectSerializer.Deserialize<UriValue>(content);
        }

        public string Serialize()
        {
            return _uri.Serialize();
        }

        #endregion Methods
    }
}
