using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public enum PropertyValueType
    {
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
        Weekday
    }

    public interface IPropertyValueImpl : ISerializableObject
    {
        #region Members

        PropertyValueType Type { get; }

        #endregion Members
    }
}
