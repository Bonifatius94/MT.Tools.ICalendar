using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using MT.Tools.ICalendar.DataObjects.PropertyValue.RecurrenceRule;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class GenericPropertyValue : IPropertyValueImpl
    {
        #region Constructor

        public GenericPropertyValue() { }

        public GenericPropertyValue(string serializedValue, PropertyValueType type = PropertyValueType.Unknown)
        {
            SerializedValue = serializedValue;
            Type = type;
        }

        #endregion Constructor

        #region Members

        public PropertyValueType Type { get; } = PropertyValueType.Unknown;

        public string SerializedValue { get; private set; }

        #endregion Members

        #region Methods

        public object GetValue(PropertyValueType? type = null)
        {
            switch (type ?? Type)
            {
                case PropertyValueType.Binary:              return ObjectSerializer.Deserialize<BinaryValue>(SerializedValue);
                case PropertyValueType.Boolean:             return ObjectSerializer.Deserialize<BooleanValue>(SerializedValue);
                case PropertyValueType.CalendarUserAddress: return ObjectSerializer.Deserialize<CalendarUserAddressValue>(SerializedValue);
                case PropertyValueType.Date:                return ObjectSerializer.Deserialize<DateValue>(SerializedValue);
                case PropertyValueType.DateTime:            return ObjectSerializer.Deserialize<DateTimeValue>(SerializedValue);
                case PropertyValueType.Duration:            return ObjectSerializer.Deserialize<DurationValue>(SerializedValue);
                case PropertyValueType.Float:               return ObjectSerializer.Deserialize<FloatValue>(SerializedValue);
                case PropertyValueType.Integer32:           return ObjectSerializer.Deserialize<IntegerValue>(SerializedValue);
                case PropertyValueType.PeriodOfTime:        return ObjectSerializer.Deserialize<PeriodOfTimeValue>(SerializedValue);
                case PropertyValueType.RecurrenceRule:      return ObjectSerializer.Deserialize<RecurrenceRuleValue>(SerializedValue);
                case PropertyValueType.Text:                return ObjectSerializer.Deserialize<TextValue>(SerializedValue);
                case PropertyValueType.Time:                return ObjectSerializer.Deserialize<TimeValue>(SerializedValue);
                case PropertyValueType.Uri:                 return ObjectSerializer.Deserialize<UriValue>(SerializedValue);
                case PropertyValueType.UtcOffset:           return ObjectSerializer.Deserialize<UtcOffsetValue>(SerializedValue);
                case PropertyValueType.Weekday:             return ObjectSerializer.Deserialize<WeekdayValue>(SerializedValue);
                default: throw new ArgumentException("Unknown value type cannot be converted!");
            }
        }

        public void Deserialize(string content)
        {
            // just save the content and deserialize it later
            SerializedValue = content;
        }

        public string Serialize()
        {
            // return the original content (this works because the content cannot be changed)
            return SerializedValue;
        }

        #endregion Methods
    }
}
