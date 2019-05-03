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
        IPropertyValueImpl Value { get; set; }

        PropertyValueType ValueType { get; }

        #endregion Members
    }

    public interface ICalendarProperty<ValueT> : ICalendarProperty
        where ValueT : IPropertyValueImpl
    {
        #region Members

        ValueT ExplicitValue { get; set; }

        #endregion Members
    }

    public class CalendarProperty<ValueT> : ICalendarProperty<ValueT>
        where ValueT : IPropertyValueImpl, new()
    {
        #region Constructor

        public CalendarProperty() { }

        public CalendarProperty(string key, ValueT value)
        {
            Key = key;
            ExplicitValue = value;
        }

        #endregion Constructor

        #region Members

        public string Key { get; set; }
        public ValueT ExplicitValue { get; set; }

        public IPropertyValueImpl Value { get => ExplicitValue as IPropertyValueImpl; set => ExplicitValue = (ValueT)value; }

        public PropertyValueType ValueType => Value?.Type ?? PropertyValueType.Unknown;

        #endregion Members

        #region Methods

        public KeyValuePair<string, IPropertyValueImpl> AsPair()
        {
            return new KeyValuePair<string, IPropertyValueImpl>(Key, Value);
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

    //public class CalendarProperty<ValueT> : ICalendarProperty<ValueT>
    //    where ValueT : IPropertyValueImpl, new()
    //{
    //    #region Constructor

    //    public CalendarProperty() { }

    //    public CalendarProperty(string key, ValueT value)
    //    {
    //        Key = key;
    //        Value = value;
    //    }

    //    #endregion Constructor

    //    #region Members

    //    public string Key { get; set; }
    //    public ValueT Value { get; set; }

    //    public PropertyValueType ValueType => Value?.Type ?? PropertyValueType.Unknown;

    //    #endregion Members

    //    #region Methods

    //    public KeyValuePair<string, ValueT> AsPair()
    //    {
    //        return new KeyValuePair<string, ValueT>(Key, Value);
    //    }

    //    public void Deserialize(string content)
    //    {
    //        // parse property name
    //        Key = content.Substring(0, content.IndexOf(':'));

    //        // parse property value
    //        string valueContent = content.Substring(Key.Length + 1, content.Length - Key.Length - 1);
    //        Value = ObjectSerializer.Deserialize<ValueT>(valueContent);
    //    }

    //    public string Serialize()
    //    {
    //        // make sure that the value is not null
    //        if (Value == null) { throw new NullReferenceException("The value of this property must not be null at serialization!"); }

    //        // serialize the key / value pair and return it
    //        return $"{ Key }:{ Value.Serialize() }";
    //    }

    //    #endregion Methods
    //}

    //public static class CalendarPropertyDeserializer
    //{
    //    #region Methods

    //    public static KeyValuePair<string, IPropertyValueImpl> DeserializeProperty(string content)
    //    {
    //        // parse property name
    //        string key = content.Substring(0, content.IndexOf(':'));

    //        // extract value content
    //        string valueContent = content.Substring(key.Length + 1, content.Length - key.Length - 1);

    //        switch ()
    //        {

    //        }
    //    }

    //    #endregion Methods
    //}
}
