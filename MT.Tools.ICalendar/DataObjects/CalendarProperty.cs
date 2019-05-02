using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
{
    public class CalendarProperty<ValueT> : ISerializableObject
        where ValueT : IPropertyValueImpl, new()
    {
        #region Constructor

        public CalendarProperty() { }

        public CalendarProperty(string key, ValueT value)
        {
            Key = key;
            Value = value;
        }

        #endregion Constructor

        #region Members

        public string Key { get; set; }
        public ValueT Value { get; set; }

        #endregion Members

        #region Methods

        public KeyValuePair<string, ValueT> AsPair()
        {
            return new KeyValuePair<string, ValueT>(Key, Value);
        }

        public void Deserialize(string content)
        {
            // parse property name
            Key = content.Substring(0, content.IndexOf(':'));

            // parse property value
            string valueContent = content.Substring(Key.Length + 1, content.Length - Key.Length - 1);
            Value = ObjectSerializer.Deserialize<ValueT>(valueContent);
        }

        public string Serialize()
        {
            // make sure that the value is not null
            if (Value == null) { throw new NullReferenceException("The value of this property must not be null at serialization!"); }

            // serialize the key / value pair and return it
            return $"{ Key }:{ Value.Serialize() }";
        }

        #endregion Methods
    }

    public class CalendarProperty : CalendarProperty<GenericPropertyValue>
    {
        #region Constructor

        public CalendarProperty() { }

        public CalendarProperty(string key, GenericPropertyValue value) : base(key, value) { }

        public CalendarProperty(string key, string serializedValue, PropertyValueType type = PropertyValueType.Unknown)
            : this(key, new GenericPropertyValue(serializedValue, type)) { }

        #endregion Constructor

        #region Methods

        public object GetValue(PropertyValueType? type = null)
        {
            return Value.GetValue(type);
        }

        public ValueT Cast<ValueT>() where ValueT : IPropertyValueImpl, new()
        {
            return ObjectSerializer.Deserialize<ValueT>(Value.SerializedValue);
        }

        public bool TryCast<ValueT>(out ValueT value) where ValueT : IPropertyValueImpl, new()
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
