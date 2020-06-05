using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class ClassificationProperty : IComponentProperty
    {
        #region Constants

        public const string CLASSIFICATION_PUBLIC = "PUBLIC";
        public const string CLASSIFICATION_PRIVATE = "PRIVATE";
        public const string CLASSIFICATION_CONFIDENTIAL = "CONFIDENTIAL";

        #endregion Constants

        #region Constructor

        public ClassificationProperty() { }

        public ClassificationProperty(TextValue classification) { Classification = classification; }

        public ClassificationProperty(TextValue classification, IEnumerable<IPropertyParameter> parameters) { Classification = classification; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Classification;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public TextValue Classification { get; set; } = new TextValue(CLASSIFICATION_PUBLIC);

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CATEGORIES
            if (!content.ToUpper().StartsWith("CLASS")) { throw new ArgumentException("Invalid categories detected! Component property needs to start with CLASS keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("CLASS".Length, content.IndexOf(':') - "CLASS".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Classification = ObjectSerializer.Deserialize<TextValue>(valueContent);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"CLASS{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Classification }";
        }

        #endregion Methods
    }
}
