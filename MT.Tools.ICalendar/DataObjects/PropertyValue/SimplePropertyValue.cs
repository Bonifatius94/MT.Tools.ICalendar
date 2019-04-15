using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class SimplePropertyValue : IPropertyValueImpl
    {
        #region Constructor

        // Boolean
        public SimplePropertyValue(bool value)
        {

        }

        // Duration + TimeOfDay
        public SimplePropertyValue(TimeSpan value, bool isDuration = false)
        {

        }

        // Float
        public SimplePropertyValue(float value)
        {

        }

        // Integer (32-Bit)
        public SimplePropertyValue(int value)
        {

        }

        // Text
        public SimplePropertyValue(string value)
        {

        }

        // Date + DateTime
        public SimplePropertyValue(DateTime value, bool isOnlyDate = false)
        {
            
        }

        #endregion Constructor

        #region Members

        public object Value { get; private set; }
        public PropertyValueType Type { get; private set; }

        #endregion Members

        #region Methods

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public void Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
