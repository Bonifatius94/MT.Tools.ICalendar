using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public class DurationValue : IPropertyValueImpl
    {
        #region Constructor

        public DurationValue() { }

        public DurationValue(TimeSpan duration)
        {
            Duration = duration;
        }

        #endregion Constructor

        #region Members

        public TimeSpan Duration { get; private set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // remove '+' sign
            content = content.Replace("+", "");

            // check if '-' is set


            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
