using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class DateTimeValue : IPropertyValueImpl
    {
        #region Constructor

        // Date + DateTime + Time
        public PropertyValue(DateTime value, PropertyValueType type)
        {
            switch (type)
            {
                case PropertyValueType.Date:
                    Value = value.Date;
                    break;
                case PropertyValueType.Time:
                    Value = value.TimeOfDay;
                    break;
                case PropertyValueType.DateTime:
                    Value = value;
                    break;
                default:
                    throw new ArgumentException("Illegal value type detected! The type needs to be Date, Time or DateTime!");
            }
            
            Value = value;
            Type = type;
        }

        #endregion Constructor

        #region Members

        public DateTime Value { get; private set; }

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
