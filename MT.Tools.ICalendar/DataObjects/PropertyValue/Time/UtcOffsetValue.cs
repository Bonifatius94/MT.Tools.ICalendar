using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public class UtcOffsetValue : IPropertyValueImpl
    {
        #region Constructor

        public UtcOffsetValue() { }

        public UtcOffsetValue(TimeZoneInfo zone)
        {
            UtcOffset = zone.BaseUtcOffset;
        }

        public UtcOffsetValue(TimeSpan utcOffset)
        {
            UtcOffset = utcOffset;
        }

        #endregion Constructor

        #region Members

        private TimeSpan UtcOffset { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // remove '+' sign
            content = content.Replace("+", "");

            // parse this using timespan
            UtcOffset = TimeSpan.Parse(content);
        }

        public string Serialize()
        {
            return UtcOffset.ToString("hh\\:mm\\");
        }

        #endregion Methods
    }
}
