using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive
{
    public class TextValue : IPropertyValue
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

        public PropertyValueType Type => PropertyValueType.Text;

        #endregion Members

        #region Methods

        // TODO: make sure that line breaks are handled correctly

        // TODO: implement regex checking for safe-chars (see pages 10 / 11)

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
