using System;
using System.Collections.Generic;

namespace MT.Tools.ICalendar.DataObjects
{
    public enum CalendarPropertyType
    {
        CalendarScale,
        Method,
        ProductId,
        Version,
        Custom
    }

    //public interface ICalendarProperty : ISerializableObject
    //{
    //    #region Members

    //    CalendarPropertyType Type { get; }

    //    #endregion Members
    //}

    // TODO: check if it is useful to create property classes for each of the four possibilities
    public class CalendarProperty : ISerializableObject
    {
        #region Constructor

        public CalendarProperty() { }

        public CalendarProperty(string key, string value) { Key = key; Value = value; }

        #endregion Constructor

        #region Members

        public const string PROPERTY_CALSCALE = "CALSCALE";
        public const string PROPERTY_METHOD = "METHOD";
        public const string PROPERTY_PRODID = "PRODID";
        public const string PROPERTY_VERSION = "VERSION";

        private static readonly Dictionary<string, CalendarPropertyType> PROPERTY_FLAGS = new Dictionary<string, CalendarPropertyType>()
        {
            { PROPERTY_CALSCALE, CalendarPropertyType.CalendarScale },
            { PROPERTY_METHOD,   CalendarPropertyType.Method        },
            { PROPERTY_PRODID,   CalendarPropertyType.ProductId     },
            { PROPERTY_VERSION,  CalendarPropertyType.Version       }
        };

        public CalendarPropertyType Type => PROPERTY_FLAGS.ContainsKey(Key) ? PROPERTY_FLAGS[Key] : CalendarPropertyType.Custom;

        public string Key { get; set; }
        public string Value { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            var pair = content.Split(':');
            Key = pair[0];
            Value = pair[1];
        }

        public string Serialize()
        {
            return $"{ Key }:{ Value }";
        }

        #endregion Methods
    }
}
