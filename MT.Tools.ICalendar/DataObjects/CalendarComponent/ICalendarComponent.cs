using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public enum CalendarComponentType
    {
        Alarm,
        Custom, // iana- or x-comp
        Event,
        FreeBusy,
        Journal,
        Timezone,
        Todo
    }

    public interface ICalendarComponent : ISerializableObject
    {
        #region Members

        CalendarComponentType Type { get; }

        #endregion Members
    }
}
