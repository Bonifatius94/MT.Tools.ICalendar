using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public class TimeValue : IPropertyValueImpl
    {
        #region Constructor

        public TimeValue() { }

        public TimeValue(TimeSpan time, bool isUtcTime = false)
        {
            Time = time;
            IsUtcTime = IsUtcTime;
        }

        #endregion Constructor

        #region Members

        public bool IsUtcTime { get; private set; }
        public TimeSpan Time { get; private set; }

        public PropertyValueType Type => PropertyValueType.Time;

        #endregion Members

        public void Deserialize(string content)
        {
            content = content.Trim();

            IsUtcTime = (content.Length == 7 && content[4] == 'Z');
            content = content.Substring(0, 6);

            string hourAsString = content.Substring(0, 2);
            string minuteAsString = content.Substring(2, 2);
            string secondAsString = content.Substring(4, 2);

            int hour = int.Parse(hourAsString);
            int minute = int.Parse(minuteAsString);
            int second = int.Parse(secondAsString);

            Time = new TimeSpan(0, hour, minute, second, 0);
        }

        public string Serialize()
        {
            return $"{ Time.Hours }{ Time.Minutes }{ Time.Seconds }{ (IsUtcTime ? "Z" : "") }";
        }
    }
}
