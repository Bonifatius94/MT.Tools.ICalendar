using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class PeriodOfTimeValue : IPropertyValueImpl
    {
        #region Constructor

        public PeriodOfTimeValue(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        #endregion Constructor

        #region Members

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // TODO: implement logic
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            // TODO: implement logic
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
