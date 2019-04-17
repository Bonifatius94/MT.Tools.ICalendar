using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public class DateTimeValue : IPropertyValueImpl
    {
        #region Constructor

        public DateTimeValue() { }

        public DateTimeValue(DateTime datetime, bool isUtcTime = false)
        {
            _date = new DateValue(datetime.Date);
            _time = new TimeValue(datetime.TimeOfDay, IsUtcTime);
        }

        #endregion Constructor

        #region Members

        private DateValue _date;
        private TimeValue _time;

        public DateTime DateTime { get { return _date.Date + _time.Time; } }
        public bool IsUtcTime { get { return _time.IsUtcTime; } }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // separate date from time part
            var parts = content.Split('T', StringSplitOptions.RemoveEmptyEntries);

            // make sure that there is only a date and a time part
            if (parts.Length != 2) { throw new ArgumentException($"Invalid datetime format ({ content }) detected!"); }

            // deserialize date part
            _date = ObjectSerializer.Deserialize<DateValue>(parts[0]);

            // deserialize time part
            _time = ObjectSerializer.Deserialize<TimeValue>(parts[1]);
        }

        public string Serialize()
        {
            return $"{ _date.Serialize() }T{ _time.Serialize() }";
        }

        #endregion Methods
    }
}
