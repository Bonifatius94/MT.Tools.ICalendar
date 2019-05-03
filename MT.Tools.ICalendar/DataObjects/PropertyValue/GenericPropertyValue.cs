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

        /// <summary>
        /// The type of the property value.
        /// </summary>
        public PropertyValueType Type { get; } = PropertyValueType.Unknown;

        /// <summary>
        /// The original serialized content of the property value.
        /// </summary>
        public string SerializedValue { get; private set; }

        #endregion Members

        #region Methods

        /// <summary>
        /// Retrieve the property value deserialized as a specific value type.
        /// </summary>
        /// <param name="type">The type to be deserialized (if null the type from the Type member is used)</param>
        /// <returns>a deserialized property value</returns>
        public IPropertyValueImpl GetValue(PropertyValueType? type = null)
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
                default: throw new ArgumentException("Unknown value type! Therefore the value cannot be converted!");
            }
        }

        #region Cast

        public BinaryValue AsBinary() => GetValue(PropertyValueType.Binary) as BinaryValue;

        public BooleanValue AsBoolean() => GetValue(PropertyValueType.Boolean) as BooleanValue;

        public CalendarUserAddressValue AsCalendarUserAddress() => GetValue(PropertyValueType.CalendarUserAddress) as CalendarUserAddressValue;

        public DateValue AsDate() => GetValue(PropertyValueType.Date) as DateValue;

        public DateTimeValue AsDateTime() => GetValue(PropertyValueType.DateTime) as DateTimeValue;

        public DurationValue AsDuration() => GetValue(PropertyValueType.Duration) as DurationValue;

        public FloatValue AsFloat() => GetValue(PropertyValueType.Float) as FloatValue;

        public IntegerValue AsInt32() => GetValue(PropertyValueType.Integer32) as IntegerValue;

        public PeriodOfTimeValue AsPeriodOfTime() => GetValue(PropertyValueType.PeriodOfTime) as PeriodOfTimeValue;

        public RecurrenceRuleValue AsRecurrenceRule() => GetValue(PropertyValueType.RecurrenceRule) as RecurrenceRuleValue;

        public TextValue AsText() => GetValue(PropertyValueType.Text) as TextValue;

        public TimeValue AsTime() => GetValue(PropertyValueType.Time) as TimeValue;

        public UriValue AsUri() => GetValue(PropertyValueType.Uri) as UriValue;

        public UtcOffsetValue AsUtcOffset() => GetValue(PropertyValueType.UtcOffset) as UtcOffsetValue;

        public WeekdayValue AsWeekday() => GetValue(PropertyValueType.Weekday) as WeekdayValue;

        public EnumValue<EnumT> AsEnum<EnumT>()
            where EnumT : struct, IConvertible
        {
            return ObjectSerializer.Deserialize<EnumValue<EnumT>>(SerializedValue);
        }

        #endregion Cast

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
