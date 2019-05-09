using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.Property
{
    public interface ICalendarProperty : ISerializableObject
    {
        #region Members

        string Key { get; set; }
        IPropertyValue Value { get; set; }

        PropertyValueType ValueType { get; }

        #endregion Members
    }

    public interface ICalendarProperty<ValueT> : ICalendarProperty
        where ValueT : IPropertyValue
    {
        #region Members

        ValueT ExplicitValue { get; set; }

        #endregion Members
    }

    public class SimpleCalendarProperty<ValueT> : ICalendarProperty<ValueT>
        where ValueT : IPropertyValue, new()
    {
        #region Constructor

        public SimpleCalendarProperty() { }

        public SimpleCalendarProperty(string key, ValueT value)
        {
            Key = key;
            ExplicitValue = value;
        }

        #endregion Constructor

        #region Members

        public string Key { get; set; }
        public ValueT ExplicitValue { get; set; }

        public IPropertyValue Value { get => ExplicitValue; set => ExplicitValue = (ValueT)value; }

        public PropertyValueType ValueType => Value?.Type ?? PropertyValueType.Custom;

        #endregion Members

        #region Methods

        public KeyValuePair<string, IPropertyValue> AsPair()
        {
            return new KeyValuePair<string, IPropertyValue>(Key, Value);
        }

        public void Deserialize(string content)
        {
            // parse property name
            Key = content.Substring(0, content.IndexOf(':'));

            // parse property value
            string valueContent = content.Substring(Key.Length + 1, content.Length - Key.Length - 1);
            ExplicitValue = ObjectSerializer.Deserialize<ValueT>(valueContent);
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
}
