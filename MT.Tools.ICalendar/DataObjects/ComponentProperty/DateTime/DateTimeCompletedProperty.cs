using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class DateTimeCompletedProperty : IComponentProperty
    {
        #region Constructor

        public DateTimeCompletedProperty() { }

        public DateTimeCompletedProperty(DateTimeValue dateTime) { DateTime = dateTime; }

        public DateTimeCompletedProperty(DateTimeValue dateTime, IEnumerable<IPropertyParameter> parameters) { DateTime = dateTime; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup => "COMPLETED";
        public ComponentPropertyType Type => ComponentPropertyType.Classification;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public DateTimeValue DateTime { get; set; }
        public IPropertyValue Value => DateTime;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with COMPLETED
            if (!content.ToUpper().StartsWith(Markup)) { throw new ArgumentException($"Invalid completed detected! Component property needs to start with { Markup } keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();
            DateTime = ObjectSerializer.Deserialize<DateTimeValue>(valueContent);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ DateTime }";
        }

        #endregion Methods
    }
}
