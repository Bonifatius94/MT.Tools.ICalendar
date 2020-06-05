using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public class AlarmComponent : ICalendarComponent
    {
        public CalendarComponentType Type => CalendarComponentType.Alarm;

        public void Deserialize(string content)
        {
            // TODO: implement component
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            // TODO: implement component
            throw new NotImplementedException();
        }
    }
}
