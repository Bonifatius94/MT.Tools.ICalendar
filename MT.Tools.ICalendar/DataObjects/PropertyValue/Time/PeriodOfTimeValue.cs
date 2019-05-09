using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public enum PeriodOfTimeValueType
    {
        Duration,
        FixEnd
    }

    public class PeriodOfTimeValue : IPropertyValue
    {
        #region Constructor

        public PeriodOfTimeValue() { }

        public PeriodOfTimeValue(DateTime start, TimeSpan duration)
        {
            _type = PeriodOfTimeValueType.Duration;
            _start = new DateTimeValue(start);
            _end = new DateTimeValue(start + duration);
            _duration = new DurationValue(duration);
        }

        public PeriodOfTimeValue(DateTime start, DateTime end)
        {
            _type = PeriodOfTimeValueType.FixEnd;
            _start = new DateTimeValue(start);
            _end = new DateTimeValue(end);
            _duration = new DurationValue(end - start);
        }

        #endregion Constructor

        #region Members

        private PeriodOfTimeValueType _type;

        private DateTimeValue _start;
        private DateTimeValue _end;
        private DurationValue _duration;

        public DateTime Start { get { return _start.DateTime; } }
        public DateTime End { get { return _end.DateTime; } }
        public TimeSpan Duration { get { return _duration.Duration; } }

        public PropertyValueType Type => PropertyValueType.PeriodOfTime;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // retrieve the two parts of the period
            var parts = content.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // make sure there are exactly two parts
            if (parts?.Count() != 2) { throw new ArgumentException($"Invalid period of time content ({ content }) detected!"); }

            // deserialize start
            _start = ObjectSerializer.Deserialize<DateTimeValue>(parts[0]);

            // determine the type of the second part
            _type = parts[1].Contains('P') ? PeriodOfTimeValueType.Duration : PeriodOfTimeValueType.FixEnd;

            if (_type == PeriodOfTimeValueType.Duration)
            {
                // deserialize duration value and set end
                _duration = ObjectSerializer.Deserialize<DurationValue>(parts[1]);
                _end = new DateTimeValue(Start + Duration);
            }
            else
            {
                // deserialize end value and set duration
                _end = ObjectSerializer.Deserialize<DateTimeValue>(parts[1]);
                _duration = new DurationValue(End - Start);
            }
        }

        public string Serialize()
        {
            return $"{ _start.Serialize() }/{ ((_type == PeriodOfTimeValueType.Duration) ? _duration.Serialize() : _end.Serialize()) }";
        }

        #endregion Methods
    }
}
