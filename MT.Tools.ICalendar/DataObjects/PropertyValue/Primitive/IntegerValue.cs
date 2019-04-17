using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive
{
    public class IntegerValue : IPropertyValueImpl
    {
        #region Constructor

        public IntegerValue() { }

        public IntegerValue(int value)
        {
            Value = value;
        }

        public IntegerValue(long value)
        {
            Value = Convert.ToInt32(value);
        }

        #endregion Constructor

        #region Members

        public int Value { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            Value = int.Parse(content.Trim());
        }

        public string Serialize()
        {
            return Value.ToString();
        }

        #endregion Methods
    }
}
