using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class FloatValue : IPropertyValue
    {
        #region Constructor

        public FloatValue() { }

        public FloatValue(float value)
        {
            Value = value;
        }

        public FloatValue(double value)
        {
            Value = Convert.ToSingle(value);
        }

        #endregion Constructor

        #region Members

        public float Value { get; set; }

        public PropertyValueType Type => PropertyValueType.Float;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            Value = float.Parse(content.Trim());
        }

        public string Serialize()
        {
            return Value.ToString();
        }

        #endregion Methods
    }
}
