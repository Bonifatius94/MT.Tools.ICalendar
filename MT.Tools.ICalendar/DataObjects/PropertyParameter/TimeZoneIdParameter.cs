using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class TimeZoneIdParameter : IPropertyParameter
    {
        #region Constructor

        public TimeZoneIdParameter() { }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => throw new NotImplementedException();

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
