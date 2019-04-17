//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MT.Tools.ICalendar.DataObjects.PropertyValue
//{
//    public static class PropertyValueFactory
//    {
//        #region Constructor

//        // Boolean
//        public static IPropertyValueImpl CreateValue(bool value)
//        {
//            return new SimplePropertyValue(value);
//        }

//        // Duration + TimeOfDay
//        public static IPropertyValueImpl CreateValue(TimeSpan value, bool isDuration)
//        {
//            return new SimplePropertyValue(value, isDuration);
//        }

//        // Float
//        public static IPropertyValueImpl CreateValue(float value)
//        {
//            return new SimplePropertyValue(value);
//        }

//        // Integer (32-Bit)
//        public static IPropertyValueImpl CreateValue(int value)
//        {
//            return new SimplePropertyValue(value);
//        }

//        // Text
//        public static IPropertyValueImpl CreateValue(string value)
//        {
//            return new SimplePropertyValue(value);
//        }

//        // Date + DateTime
//        public static IPropertyValueImpl CreateValue(DateTime value, bool isOnlyDate = false)
//        {
//            return new SimplePropertyValue(value, isOnlyDate);
//        }

//        // Binary
//        public static IPropertyValueImpl CreateValue(byte[] bytes)
//        {
//            return new BinaryValue(bytes);
//        }

//        // Calendar User Address
//        public static IPropertyValueImpl CreateValue(Uri uri)
//        {
//            return new CalendarUserAddressValue(uri);
//        }

//        // Period of Time
//        public static IPropertyValueImpl CreateValue(DateTime start, DateTime end)
//        {
//            return new PeriodOfTimeValue(start, end);
//        }

//        // Recurrence Rule
//        public static IPropertyValueImpl CreateValue(RecurrenceRuleValue value)
//        {

//        }

//        // Uri
//        public static IPropertyValueImpl CreateValue(Uri value)
//        {
//            Value = new SimplePropertyValue(value);
//            Type = PropertyValueType.Uri;
//        }

//        // UTC Offset
//        public static IPropertyValueImpl CreateValue(TimeZoneInfo localZone)
//        {

//        }

//        #endregion Constructor

//        #region Members

//        public IPropertyValueImpl Value { get; private set; }
//        public PropertyValueType Type { get; private set; }

//        #endregion Members

//        #region Methods

//        #region Deserialize

//        public void Deserialize(string content)
//        {
//            var type = parseType(content);

//            throw new NotImplementedException();
//        }

//        private PropertyValueType parseType(string content)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion Deserialize

//        #region Serialize

//        public string Serialize()
//        {
//            throw new NotImplementedException();
//        }

//        #endregion Serialize

//        #endregion Methods
//    }
//}
