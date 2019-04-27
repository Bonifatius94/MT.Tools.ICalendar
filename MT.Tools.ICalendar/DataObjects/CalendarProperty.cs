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
            return $"{ Key }:{ Value.Serialize() }";
        }

        #endregion Methods
    }
}
