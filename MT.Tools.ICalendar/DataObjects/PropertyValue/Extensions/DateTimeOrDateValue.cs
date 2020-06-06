using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class DateTimeOrDateValue : DualSwitchValue<DateValue, DateTimeValue>
    {
        #region Constructor

        public DateTimeOrDateValue(DateValue date) : base(date) { }

        public DateTimeOrDateValue(DateTimeValue dateTime) : base(dateTime) { }

        #endregion Constructor
    }
}
