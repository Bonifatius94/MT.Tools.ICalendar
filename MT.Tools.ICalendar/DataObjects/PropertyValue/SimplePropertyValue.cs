using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class SimplePropertyValue : IPropertyValueImpl
    {
        #region Constructor

        // Empty
        public SimplePropertyValue() { }

        // Boolean
        public SimplePropertyValue(bool value)
        {
            Value = value;
            Type = PropertyValueType.Boolean;
        }

        // Float
        public SimplePropertyValue(float value)
        {
            Value = value;
            Type = PropertyValueType.Float;
        }

        // Integer (32-Bit)
        public SimplePropertyValue(int value)
        {
            Value = value;
            Type = PropertyValueType.Integer32;
        }

        // Text
        public SimplePropertyValue(string value)
        {
            Value = value;
            Type = PropertyValueType.Text;
        }

        // Duration + TimeOfDay
        public SimplePropertyValue(TimeSpan value, bool isDuration = false)
        {
            Value = value;
            Type = isDuration ? PropertyValueType.Duration : PropertyValueType.Time;
        }

        // Date + DateTime
        public SimplePropertyValue(DateTime value, bool isOnlyDate = false)
        {
            Value = isOnlyDate ? value.Date : value;
            Type = isOnlyDate ? PropertyValueType.Date : PropertyValueType.DateTime;
        }

        #endregion Constructor

        #region Members

        public object Value { get; private set; }
        public PropertyValueType Type { get; private set; }

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
