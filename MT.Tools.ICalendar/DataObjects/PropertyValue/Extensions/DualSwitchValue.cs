using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public abstract class DualSwitchValue<Value1T, Value2T> : IPropertyValue
        where Value1T : IPropertyValue
        where Value2T : IPropertyValue
    {
        #region Constructor

        // The empty constructor may not be used to ensure that ObjectSerializer cannot call the deserialize function by accident.
        private DualSwitchValue() { }

        public DualSwitchValue(Value1T value) { Value = value; }

        public DualSwitchValue(Value2T value) { Value = value; }

        #endregion Constructor

        #region Members

        public IPropertyValue Value { get; }
        public PropertyValueType Type => Value.Type;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // unfortunately there is not factory for property values because the standard value types are not flagged, 
            // so noone really knows which value type actually fits. If a factory somehow exists, this exception can be removed!
            throw new InvalidOperationException("DualSwitchValue mustn't be used that way. Use the constructor for putting the data instead!");
        }

        public string Serialize()
        {
            return Value.Serialize();
        }

        #endregion Methods
    }
}
