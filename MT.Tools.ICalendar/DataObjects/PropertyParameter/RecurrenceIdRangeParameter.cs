using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class RecurrenceIdRangeParameter : IPropertyParameter
    {
        //#region Constructor

        //public RecurrenceIdRangeParameter() { }

        //#endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.RecurrenceIdentifierRange;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that parameter and value are correct (there is only one possible value)
            if (!content.ToUpper().Equals("RANGE=THISANDFUTURE")) { throw new ArgumentException("Invalid recurrence id range parameter detected!"); }
        }

        public string Serialize()
        {
            return "RANGE=THISANDFUTURE";
        }

        #endregion Methods
    }
}
