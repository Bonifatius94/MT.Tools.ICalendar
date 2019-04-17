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

    public class PeriodOfTimeValue : IPropertyValueImpl
    {
        #region Constructor

        public PeriodOfTimeValue() { }

        public PeriodOfTimeValue(DateTime start, DateTime end)
        {
            _start = new DateTimeValue(start);
            _end = new DateTimeValue(end);
        }

        public PeriodOfTimeValue(DateTime start, TimeSpan duration)
        {
            _start = new DateTimeValue(start);
            _end = new DateTimeValue(start + duration);
        }

        #endregion Constructor

        #region Members

        private PeriodOfTimeValueType _type;

        private DateTimeValue _start;
        private DateTimeValue _end;
        private DurationValue _duration;

        public DateTime Start { get { return _start.DateTime; } }
        public DateTime End { get { return (_type == PeriodOfTimeValueType.Duration) ? (_start.DateTime + _duration.Duration) : _end.DateTime; } }
        public TimeSpan Duration { get { return (_type == PeriodOfTimeValueType.Duration) ? _duration.Duration : (_end.DateTime - _start.DateTime); ; } }

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
            _start = new DateTimeValue();
            _start.Deserialize(parts[0]);

            // determine the type of the second part
            _type = parts[1].Contains('P') ? PeriodOfTimeValueType.Duration : PeriodOfTimeValueType.FixEnd;

            if (_type == PeriodOfTimeValueType.Duration)
            {
                // deserialize duration
                _duration = new DurationValue();
                _duration.Deserialize(parts[1]);
            }
            else
            {
                // deserialize end
                _end = new DateTimeValue();
                _end.Deserialize(parts[1]);
            }
        }

        public string Serialize()
        {
            string start = new DateTimeValue(Start).Serialize();
            string end = (_type == PeriodOfTimeValueType.Duration) ? new DurationValue(Duration).Serialize() : new DateTimeValue(End).Serialize();

            return $"{ start }/{ end }";
        }

        #endregion Methods
    }
}
