using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Extensions
{
    public class GeoPositionValue : IPropertyValue
    {
        #region Constructor

        public GeoPositionValue() { }

        public GeoPositionValue(FloatValue latitude, FloatValue longitude) { Latitude = latitude; Longitude = longitude; }

        #endregion Constructor

        #region Members

        public PropertyValueType Type => PropertyValueType.Float;

        public FloatValue Latitude { get; set; }
        public FloatValue Longitude { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            var floatValues = content.Trim().Split(';');
            Latitude = ObjectSerializer.Deserialize<FloatValue>(floatValues[0]);
            Longitude = ObjectSerializer.Deserialize<FloatValue>(floatValues[1]);
        }

        public string Serialize()
        {
            return $"{ Latitude.Serialize() };{ Longitude.Serialize() }";
        }

        #endregion Methods
    }
}
