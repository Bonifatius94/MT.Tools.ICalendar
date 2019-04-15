using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class PropertyValueFactory
    {
        #region Constructor

        // Boolean
        public IPropertyValueImpl CreateValue(bool value)
        {
            return new SimplePropertyValue(value);
        }

        // Duration + TimeOfDay
        public IPropertyValueImpl CreateValue(TimeSpan value, bool isDuration)
        {
            return new SimplePropertyValue(value, isDuration);
        }

        // Float
        public IPropertyValueImpl CreateValue(float value)
        {
            return new SimplePropertyValue(value);
        }

        // Integer (32-Bit)
        public IPropertyValueImpl CreateValue(int value)
        {
            return new SimplePropertyValue(value);
        }

        // Text
        public IPropertyValueImpl CreateValue(string value)
        {
            return new SimplePropertyValue(value);
        }

        // Date + DateTime
        public IPropertyValueImpl CreateValue(DateTime value, bool isOnlyDate = false)
        {
            return new SimplePropertyValue(value, isOnlyDate);
        }

        // Binary
        public IPropertyValueImpl CreateValue(byte[] bytes)
        {
            return new BinaryValue(bytes);
        }

        // Calendar User Address
        public IPropertyValueImpl CreateValue(CalendarUserAddressValue value)
        {

        }

        // Period of Time
        public IPropertyValueImpl CreateValue(PeriodOfTimeValue value)
        {

        }

        // Recurrence Rule
        public IPropertyValueImpl CreateValue(RecurrenceRuleValue value)
        {

        }

        // Uri
        public IPropertyValueImpl CreateValue(Uri value)
        {
            Value = new SimplePropertyValue(value);
            Type = PropertyValueType.Uri;
        }

        // UTC Offset
        public IPropertyValueImpl CreateValue(TimeZoneInfo localZone)
        {

        }

        #endregion Constructor

        #region Members

        public IPropertyValueImpl Value { get; private set; }
        public PropertyValueType Type { get; private set; }

        #endregion Members

        #region Methods

        #region Deserialize

        public void Deserialize(string content)
        {
            var type = parseType(content);

            throw new NotImplementedException();
        }

        private PropertyValueType parseType(string content)
        {
            throw new NotImplementedException();
        }

        #endregion Deserialize

        #region Serialize

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Serialize

        #endregion Methods
    }
}
