using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive
{
    public class BooleanValue : IPropertyValue
    {
        #region Constructor

        public BooleanValue() { }

        public BooleanValue(bool value)
        {
            Value = value;
        }

        #endregion Constructor

        #region Members

        public bool Value { get; set; }

        public PropertyValueType Type => PropertyValueType.Boolean;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            content = content.Trim();

            switch (content)
            {
                case "TRUE":  Value = true;  break;
                case "FALSE": Value = false; break;
                default: throw new ArgumentException("Invalid boolean content ({ content }) detected!");
            }
        }

        public string Serialize()
        {
            return Value ? "TRUE" : "FALSE";
        }

        #endregion Methods
    }
}
