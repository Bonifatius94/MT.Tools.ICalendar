using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public enum PropertyValueType
    {
        // RFC 5545 types
        Binary,
        Boolean,
        CalendarUserAddress,
        Date,
        Time,
        DateTime,
        Duration,
        Float,
        Integer32,
        PeriodOfTime,
        RecurrenceRule,
        Text,
        Uri,
        UtcOffset,
        Weekday,

        // additional types
        Custom
    }

    public interface IPropertyValue : ISerializableObject
    {
        #region Members

        PropertyValueType Type { get; }

        #endregion Members
    }
}
