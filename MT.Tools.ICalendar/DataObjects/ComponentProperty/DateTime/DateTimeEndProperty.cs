﻿using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class DateTimeEndProperty : IComponentProperty
    {
        #region Constructor

        public DateTimeEndProperty() { }

        public DateTimeEndProperty(DateTimeValue dateTime) { DateTime = dateTime; }

        public DateTimeEndProperty(DateTimeValue dateTime, IEnumerable<IPropertyParameter> parameters) { DateTime = dateTime; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup => "DTEND";
        public ComponentPropertyType Type => ComponentPropertyType.DateTimeEnd;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public ValueTypeParameter ValueType => Parameters.FirstOrDefault(x => x.GetType() == typeof(ValueTypeParameter)) as ValueTypeParameter;
        public TimeZoneIdParameter TimezoneId => Parameters.FirstOrDefault(x => x.GetType() == typeof(TimeZoneIdParameter)) as TimeZoneIdParameter;

        public DateTimeOrDateValue DateTime { get; set; }
        public IPropertyValue Value => DateTime;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with DTEND
            if (!content.ToUpper().StartsWith(Markup)) { throw new ArgumentException($"Invalid date-time end detected! Component property needs to start with { Markup } keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();
            
            // deserialize datetime
            if ()
            {

            }
            // deserialize only date without time
            else
            {

            }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ DateTime }";
        }

        #endregion Methods
    }

    public class DateTimeEndValue : IPropertyValue
    {
        #region Constructor

        public DateTimeEndValue() { }

        #endregion Constructor

        #region Members

        public PropertyValueType Type => throw new NotImplementedException();

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
