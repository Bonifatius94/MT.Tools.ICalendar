using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class BinaryValue : IPropertyValueImpl
    {
        #region Constructor

        public BinaryValue(byte[] bytes)
        {
            Bytes = bytes;
        }

        #endregion Constructor

        #region Members

        public byte[] Bytes { get; private set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // parse BASE64 content
            Bytes = Convert.FromBase64String(content);

            // TODO: check if this works
        }

        public string Serialize()
        {
            // generate BASE64 content
            return Convert.ToBase64String(Bytes);

            // TODO: check if this works
        }

        #endregion Methods
    }
}
