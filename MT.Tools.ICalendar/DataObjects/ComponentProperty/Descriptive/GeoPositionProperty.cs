using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class GeoPositionProperty : IComponentProperty
    {
        #region Constructor

        public GeoPositionProperty() { }

        public GeoPositionProperty(GeoPositionValue position) { Position = position; }

        public GeoPositionProperty(GeoPositionValue position, IEnumerable<IPropertyParameter> parameters) { Position = position; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup => "GEO";
        public ComponentPropertyType Type => ComponentPropertyType.GeoPosition;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public GeoPositionValue Position { get; set; }
        public IPropertyValue Value => Position;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with GEO
            if (!content.ToUpper().StartsWith(Markup)) { throw new ArgumentException($"Invalid geo position detected! Component property needs to start with { Markup } keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();
            Position = ObjectSerializer.Deserialize<GeoPositionValue>(valueContent);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Position.Serialize() }";
        }

        #endregion Methods
    }
}
