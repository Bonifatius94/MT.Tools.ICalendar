using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class LanguageParameter : IPropertyParameter
    {
        #region Constructor

        public LanguageParameter() { }

        //public LanguageParameter() { }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.Language;

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
