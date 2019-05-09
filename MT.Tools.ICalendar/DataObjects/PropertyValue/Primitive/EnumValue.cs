using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive
{
    public class EnumValue<EnumT> : IPropertyValue
        where EnumT : struct, IConvertible
    {
        #region Constructor

        public EnumValue() { }

        public EnumValue(bool isCaseSensitive) { _isCaseSensitive = isCaseSensitive; }

        public EnumValue(EnumT value, bool isCaseSensitive = false) : this(isCaseSensitive) { Value = value; }

        #endregion Constructor

        #region Members

        private bool _isCaseSensitive = false;

        public PropertyValueType Type => PropertyValueType.Text;

        public EnumT Value { get; set; }

        #endregion Members

        #region Methods

        private bool isEnum() => typeof(EnumT).IsEnum;

        public void Deserialize(string content)
        {
            // make sure the generic type is an enum
            if (!isEnum()) { throw new InvalidOperationException("The given enum type is not an enum!"); }

            // parse the enum value from content string
            Value = Enum.Parse<EnumT>(content, !_isCaseSensitive);
        }

        public string Serialize()
        {
            return _isCaseSensitive ? Value.ToString() : Value.ToString().ToUpper();
        }

        #endregion Methods
    }
}
