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
            Value = bytes;
        }

        #endregion Constructor

        #region Members

        public byte[] Value { get; private set; }

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
