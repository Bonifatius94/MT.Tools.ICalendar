using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.Property
{
    public class GenericCalendarProperty : SimpleCalendarProperty<GenericPropertyValue>
    {
        #region Constructor

        public GenericCalendarProperty() { }

        public GenericCalendarProperty(string key, GenericPropertyValue value) : base(key, value) { }

        public GenericCalendarProperty(string key, string serializedValue, PropertyValueType type = PropertyValueType.Custom)
            : this(key, new GenericPropertyValue(serializedValue, type)) { }

        #endregion Constructor

        #region Methods

        public object GetValue(PropertyValueType? type = null)
        {
            return ExplicitValue.GetValue(type);
        }

        public ValueT Cast<ValueT>() where ValueT : IPropertyValue, new()
        {
            return ObjectSerializer.Deserialize<ValueT>(ExplicitValue.SerializedValue);
        }

        public bool TryCast<ValueT>(out ValueT value) where ValueT : IPropertyValue, new()
        {
            // init ret and value for cast failure
            bool ret = false;
            value = default(ValueT);

            try
            {
                // try to cast the content
                value = Cast<ValueT>();

                // conversion successful!
                ret = true;
            }
            catch (Exception) { /* nothing to do here ... */ }

            return ret;
        }

        #endregion Methods
    }
}
