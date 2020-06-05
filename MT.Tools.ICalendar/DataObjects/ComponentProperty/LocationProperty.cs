using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class LocationProperty : IComponentProperty
    {
        #region Constructor

        public LocationProperty() { }

        public LocationProperty(TextValue location) { Location = location; }

        public LocationProperty(TextValue location, IEnumerable<IPropertyParameter> parameters) { Location = location; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Location;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public LanguageParameter Language => Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).FirstOrDefault() as LanguageParameter;

        public AlternateTextRepresentationParamter AltTextRep =>
            Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).FirstOrDefault() as AlternateTextRepresentationParamter;

        public TextValue Location { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CATEGORIES
            if (!content.ToUpper().StartsWith("LOCATION")) { throw new ArgumentException("Invalid location detected! Component property needs to start with LOCATION keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("LOCATION".Length, content.IndexOf(':') - "LOCATION".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Location = ObjectSerializer.Deserialize<TextValue>(valueContent);

            // make sure that the language and altrep parameters only occur once
            if (Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! Language parameter needs to be unique!"); }
            if (Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! AlternateTextRepresentation parameter needs to be unique!"); }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"LOCATION{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Location.Serialize() }";
        }

        #endregion Methods
    }
}
