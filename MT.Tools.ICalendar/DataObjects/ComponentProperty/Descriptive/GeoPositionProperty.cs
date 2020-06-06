using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
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

        public GeoPositionProperty(FloatValue latitude, FloatValue longitude) { Latitude = latitude; Longitude = longitude; }

        public GeoPositionProperty(FloatValue latitude, FloatValue longitude, IEnumerable<IPropertyParameter> parameters) { Latitude = latitude; Longitude = longitude; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.GeoPosition;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public FloatValue Latitude { get; set; }
        public FloatValue Longitude { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with GEO
            if (!content.ToUpper().StartsWith("GEO")) { throw new ArgumentException("Invalid geo position detected! Component property needs to start with GEO keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("GEO".Length, content.IndexOf(':') - "GEO".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            var floatValues = valueContent.Split(';');
            Latitude = ObjectSerializer.Deserialize<FloatValue>(floatValues[0]);
            Longitude = ObjectSerializer.Deserialize<FloatValue>(floatValues[1]);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"GEO{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Latitude.Serialize() };{ Longitude.Serialize() }";
        }

        #endregion Methods
    }
}
