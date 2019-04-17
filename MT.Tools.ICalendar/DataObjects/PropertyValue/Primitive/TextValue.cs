using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive
{
    public class TextValue : IPropertyValueImpl
    {
        #region Constructor

        public TextValue() { }

        public TextValue(string value)
        {
            Value = value;
        }

        #endregion Constructor

        #region Members

        public string Value { get; set; }

        #endregion Members

        #region Methods

        // TODO: make sure that line breaks are handled correctly

        public void Deserialize(string content)
        {
            Value = content;
        }

        public string Serialize()
        {
            return Value;
        }

        #endregion Methods
    }
}
