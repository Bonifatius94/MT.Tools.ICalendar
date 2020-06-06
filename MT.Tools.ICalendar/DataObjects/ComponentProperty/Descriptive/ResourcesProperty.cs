using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
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

        public ResourcesProperty(TextListValue resources) { Resources = resources; }

        public ResourcesProperty(TextListValue resources, IEnumerable<IPropertyParameter> parameters) { Resources = resources; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup => "RESOURCES";
        public ComponentPropertyType Type => ComponentPropertyType.Resources;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public LanguageParameter Language => Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).FirstOrDefault() as LanguageParameter;

        public AlternateTextRepresentationParamter AltTextRep =>
            Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).FirstOrDefault() as AlternateTextRepresentationParamter;

        public TextListValue Resources { get; private set; } = new TextListValue();
        public IPropertyValue Value => Resources;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with RESOURCES
            if (!content.ToUpper().StartsWith(Markup)) { throw new ArgumentException($"Invalid resources detected! Component property needs to start with { Markup } keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();
            Resources = ObjectSerializer.Deserialize<TextListValue>(valueContent);

            // make sure that the language parameter only occurs once
            if (Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! Language parameter needs to be unique!"); }
            if (Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! AlternateTextRepresentation parameter needs to be unique!"); }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Resources.Serialize() }";
        }

        #endregion Methods
    }
}
