using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class SummaryProperty : IComponentProperty
    {
        #region Constructor

        public SummaryProperty() { }

        public SummaryProperty(TextValue summary) { Summary = summary; }

        public SummaryProperty(TextValue summary, IEnumerable<IPropertyParameter> parameters) { Summary = summary; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Summary;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public LanguageParameter Language => Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).FirstOrDefault() as LanguageParameter;

        public AlternateTextRepresentationParamter AltTextRep =>
            Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).FirstOrDefault() as AlternateTextRepresentationParamter;

        public TextValue Summary { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with SUMMARY
            if (!content.ToUpper().StartsWith("SUMMARY")) { throw new ArgumentException("Invalid summary detected! Component property needs to start with SUMMARY keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("SUMMARY".Length, content.IndexOf(':') - "SUMMARY".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Summary = ObjectSerializer.Deserialize<TextValue>(valueContent);

            // make sure that the language and altrep parameters only occur once
            if (Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! Language parameter needs to be unique!"); }
            if (Parameters.Where(x => x.GetType() == typeof(AlternateTextRepresentationParamter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! AlternateTextRepresentation parameter needs to be unique!"); }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"SUMMARY{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Summary.Serialize() }";
        }

        #endregion Methods
    }
}
