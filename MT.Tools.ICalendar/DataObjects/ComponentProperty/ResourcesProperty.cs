using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class ResourcesProperty : IComponentProperty
    {
        #region Constructor

        public ResourcesProperty() { }

        public ResourcesProperty(IEnumerable<TextValue> resources) { Resources = resources; }

        public ResourcesProperty(IEnumerable<TextValue> resources, IEnumerable<IPropertyParameter> parameters) { Resources = resources; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Categories;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public LanguageParameter Language => Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).FirstOrDefault() as LanguageParameter;

        public AlternateTextRepresentationParamter AltTextRep =>
            Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).FirstOrDefault() as AlternateTextRepresentationParamter;

        public IEnumerable<TextValue> Resources { get; private set; } = new List<TextValue>();

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CATEGORIES
            if (!content.ToUpper().StartsWith("RESOURCES")) { throw new ArgumentException("Invalid resources detected! Component property needs to start with RESOURCES keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("RESOURCES".Length, content.IndexOf(':') - "RESOURCES".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Resources = content.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => ObjectSerializer.Deserialize<TextValue>(x)).ToList();

            // make sure that the language parameter only occurs once
            if (Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! Language parameter needs to be unique!"); }
            if (Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! AlternateTextRepresentation parameter needs to be unique!"); }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            string valuesContent = Resources.Select(x => x.Value).Aggregate((x, y) => x + "," + y);
            return $"RESOURCES{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ valuesContent }";
        }

        #endregion Methods
    }
}
